using Mega.Video;
using OpenTK.Mathematics;

namespace Mega.Game
{
    public class UnitedChunk
    {
        public Dictionary<Vector2i, Chunk> Chunks;

        RenderSurface[] TotalSurface;
        public UnitedChunk()
        {
            Chunks = new();
        }
        public void AddChunk(Chunk chunk)
        {
            chunk.Root = this;
            Chunks.Add(chunk.Location, chunk);
        }
        public Block GetBlock(Vector3i position)
        {
            var path = position.ToWorldPath();
            if (!path.block.IsInChunk())
                return null;
            var chunk = GetChunkByLocation(path.chunk);
            if (chunk == null)
                return null;
            return chunk.data.Get(path.block);
        }

        public void SetBlock(Block block)
        {
            var path = block.Position.ToWorldPath();
            var chunk = GetChunkByLocation(path.chunk);
            if (chunk == null)
                return;
            chunk.data.Set(path.block, block);
            chunk.MembersList.Add(path.block);

        }
        public Chunk GetChunkByLocation(Vector2i location)
        {
            Chunk cn = null;
            Chunks.TryGetValue(location, out cn);
            return cn;
        }
        public bool GetMember(Vector3i position)
        {
            var path = position.ToWorldPath();
            if (!path.block.IsInChunk())
                return false;
            var chunk = GetChunkByLocation(path.chunk);
            if (chunk == null)
                return false;
            return chunk.data.Get(path.block) is not null;
        }

        public void UpdateBorder()
        {
            Chunks.Values.AsParallel().ForAll(cn =>
            {
                foreach (var blockPos in cn.MembersList)
                {

                    bool skip = false;
                    var block = cn.Get(blockPos);
                    var nbs = block.Adjacent;
                    foreach (var verifyAdjacent in nbs)
                    {
                        if (!GetMember(verifyAdjacent))
                        {
                            cn.BorderMembersList.Add(blockPos);
                            break;
                        }
                    }
                    if (skip)
                        break;
                }
            });
        }
        public void UpdateRenderSurface()
        {
            var nn = Chunks.Values.ToList();
            nn.ForEach(i => i.RebuildMesh());
            TotalSurface = nn.Select(i => i.Surface).ToList().SumList().ToArray();
        }

        public IEnumerator<Chunk> GetEnumerator()
        {
            foreach (var chunk in Chunks.Values)
            {
                yield return chunk;
            }
        }
    }
}
