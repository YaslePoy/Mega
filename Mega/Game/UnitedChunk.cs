using Mega.Video;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return chunk.data.Get(path.block)  is not null;
        }
        void VerifyBlock(Vector3i border, Vector3i block, Chunk host)
        {
            if (border.Y != -1)
                if (host.data.Get(border) is null)
                {
                    if (host.BorderMembersList.Count == 0 || host.BorderMembersList.Last() != block)
                        host.BorderMembersList.Add(block);
                }
        }

        public void BuildGlobalCoordinates(bool coords = true)
        {
            if (coords)
                Chunks.Values.AsParallel().ForAll(i => i.UpdateGlobalCoords());
            Chunks.Values.AsParallel().ForAll(i => i.GenerateSurface());
        }
        public void UpdateBorder()
        {
            var testVec = new Vector3i(6, 6, -36);
            foreach (var cn in Chunks.Values)
            {
                if (cn == null)
                    continue;
                foreach (var blockPos in cn.MembersList)
                {

                    bool skip = false;
                    var block = cn.Get(blockPos);
                    if (block.Position == testVec)
                        Debug.Trap();
                    var nbs = block.Adjacent;
                    foreach (var verifyAdjacent in nbs)
                    {
                        //Vector3i verify = verifyAdjacent.InChunk();
                        //if (verify.IsInChunk())
                        //{
                        //    VerifyBlock(verify, blockPos, cn);
                        //}
                        //else
                        //{
                        //    if (blockPos.Y < 0 || blockPos.Y > Chunk.Size.Y)
                        //    {
                        //        cn.BorderMembersList.Add(blockPos);
                        //        skip = true;
                        //        break;
                        //    }
                        //    var nextBlock = verify.InChunk();
                        //    var nextChunkLocation = cn.Location + new Vector2i(nextBlock.X == 0 ? 1 : -1, nextBlock.Y == 0 ? 1 : -1);
                        //    var nextChunk = GetChunkByLocation(nextChunkLocation);
                        //    if (nextChunk == null)
                        //        continue;
                        //    VerifyBlock(verify, nextBlock, nextChunk);
                        //}
                        if (!GetMember(verifyAdjacent))
                        {
                            cn.BorderMembersList.Add(blockPos);
                            break;
                        }
                    }
                    if (skip)
                        break;
                }
            }
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
