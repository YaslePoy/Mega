using Mega.Game.Blocks;
using Mega.Video;
using OpenTK.Mathematics;

namespace Mega.Game
{
    public class Chunk
    {
        public static readonly Vector3i Size = new Vector3i(32, 256, 32);
        public Block[,,] data;
        public List<ChunkLocation> MembersList;
        public List<ChunkLocation> BorderMembersList;
        public readonly Vector2i Location;
        public UnitedChunk Root;

        public RenderSurface[] Surface;
        public Chunk(UnitedChunk root)
        {

            data = new Block[Size.X, Size.Y, Size.Z];
            MembersList = new List<ChunkLocation>();
            BorderMembersList = new List<ChunkLocation>();
            Root = root;
        }

        public Chunk(Vector2i location, UnitedChunk root) : this(root)
        {
            this.Location = location;
        }

        public void SetBlock(Block block, ChunkLocation location)
        {
            data.Set(location, block);
            MembersList.Add(location);
        }
        public void RebuildMesh()
        {
            int id = 0;
            var chunked = BorderMembersList.Chunk(1024).ToList().Select(j => (id++, j)).ToList();
            var ready = new List<RenderSurface>[chunked.Count];
            Parallel.ForEach(chunked, chunk =>
            {
                List<RenderSurface> surfaces = new List<RenderSurface>(chunk.j.Length * 3);
                foreach (var member in chunk.j)
                {
                    surfaces.AddRange(data.Get(member).GetDrawingMesh(Root));
                }

                ready[chunk.Item1] = surfaces;
            });
            
            Surface = ready.SumList();
        }
        public Block Get(Vector3i pos)
        {
            return data.Get(pos);
        }
        public IEnumerator<Block> GetEnumerator()
        {
            foreach (var block in data)
                yield return block;
        }
        public override string ToString()
        {
            return Location.ToString();
        }
        public Vector3i BlockOffset()
        {
            int xOffset, yOffset;
            if (Location.X >= 0)
            {
                xOffset = Location.X * Size.X;
            }
            else
            {
                xOffset = (Location.X * Size.X);
            }
            if (Location.Y >= 0)
            {
                yOffset = Location.Y * Size.Z;
            }
            else
            {
                yOffset = (Location.Y * Size.Z);
            }
            return new Vector3i(xOffset, 0, yOffset);
        }
    }
}
