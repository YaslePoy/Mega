using Mega.Game.Blocks;
using Mega.Video;
using OpenTK.Mathematics;

namespace Mega.Game
{
    public class UnitedChunk
    {
        public Dictionary<Vector2i, Chunk> Chunks;
        private HashSet<Vector2i> remeshRequest;
        RenderSurface[] TotalSurface;
        public UnitedChunk()
        {
            Chunks = new();
            remeshRequest = new HashSet<Vector2i>();
        }
        public void AddChunk(Chunk chunk)
        {
            chunk.Root = this;
            Chunks.Add(chunk.Location, chunk);
        }
        public Block GetBlock(Vector3i position)
        {
            var path = position.ToWorldPath();
            if ((path.InChunk.Y > Chunk.Size.Y))
                return null;
            if (!Chunks.TryGetValue(path.Chunk, out var chunk))
                return null;
            return chunk.data[path.InChunk.X, path.InChunk.Y, path.InChunk.Z];
        }

        public void SetBlock(Block block)
        {
            var path = block.Position.ToWorldPath();
            var chunk = GetChunkByLocation(path.Chunk);
            if (chunk == null)
                return;
            chunk.data.Set(path.InChunk, block);
            chunk.MembersList.Add(path.InChunk);
            remeshRequest.Add(path.Chunk);

        }
        public Chunk GetChunkByLocation(Vector2i location)
        {
            if (Chunks.TryGetValue(location, out var cn))
                return cn;
            return null;
        }
        public bool GetMember(Vector3i position)
        {

            return GetBlock(position) is { } block;
        }

        public void UpdateBorder()
        {

            var nn = Chunks.Values.ToList().Where(i => remeshRequest.Contains(i.Location));
            nn.AsParallel().ForAll(cn =>
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
            nn.Where(i => remeshRequest.Contains(i.Location)).ToList().ForEach(i => i.RebuildMesh());
            TotalSurface = nn.Select(i => i.Surface).ToList().SumList().ToArray();
            remeshRequest.Clear();
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
