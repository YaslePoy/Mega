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
        public List<Vector3i> MembersList;
        public List<Vector3i> BorderMembersList;
        RenderSurface[] TotalSurface;
        public UnitedChunk(int chunks)
        {
            Chunks = new();
        }
        public void AddChunk(Chunk chunk)
        {
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
            chunk.Members.Set(path.block, true);
            chunk.MembersList.Add(path.block);

        }
        public Chunk GetChunkByLocation(Vector2i location)
        {
            Chunk cn = null;
            Chunks.TryGetValue(location, out cn);
            return cn;
        }
        public bool GetBorder(Vector3i position)
        {
            var path = position.ToWorldPath();
            if (!path.block.IsInChunk())
                return false;
            var chunk = GetChunkByLocation(path.chunk);
            if (chunk == null)
                return false;
            return chunk.Border.Get(path.block);
        }
        public void SetBorder(Vector3i position, bool value)
        {
            var path = position.ToWorldPath();
            var chunk = GetChunkByLocation(path.chunk);
            if (chunk == null)
                return;
            chunk.Border.Set(path.block, value);
        }
        public bool GetMember(Vector3i position)
        {
            var path = position.ToWorldPath();
            if (!path.block.IsInChunk())
                return false;
            var chunk = GetChunkByLocation(path.chunk);
            if (chunk == null)
                return false;
            return chunk.Members.Get(path.block);
        }
        public void SetMember(Vector3i position, bool value)
        {
            var path = position.ToWorldPath();
            var chunk = GetChunkByLocation(path.chunk);
            if (chunk == null)
                return;
            chunk.Members.Set(path.block, value);
        }
        public bool GetBorderMember(Vector3i position)
        {
            var path = position.ToWorldPath();
            if (!path.block.IsInChunk())
                return false;
            var chunk = GetChunkByLocation(path.chunk);
            if (chunk == null)
                return false;
            return chunk.BorderMembers.Get(path.block);
        }
        public void SetBorderMember(Vector3i position, bool value)
        {
            var path = position.ToWorldPath();
            var chunk = GetChunkByLocation(path.chunk);
            if (chunk == null)
                return;
            chunk.BorderMembers.Set(path.block, value);
        }
        //public void UpdateMemberList()
        //{
        //    MembersList = Chunks.Where(i => i != null).Select(i => i.MembersList).ToList().SumList();
        //}
        //public void UpdateBorderMembersList()
        //{
        //    BorderMembersList = Chunks.Where(i => i != null).Select(i => i.BorderMembersList).ToList().SumList();
        //}
        public void ClearMesh()
        {
            MembersList.Clear();
            BorderMembersList.Clear();
            foreach (var cn in Chunks)
            {
                cn.Value.ClearInternalData();
            }
        }
        void VerifyBlock(Vector3i border, Vector3i block, Chunk host)
        {
            if (border.Y != -1)
                if (!host.Members.Get(border))
                {
                    host.BorderMembers.Set(block, true);
                    if (host.BorderMembersList.Count == 0 || host.BorderMembersList.Last() != block)
                        host.BorderMembersList.Add(block);
                    host.Border.Set(border, true);
                }
        }

        public void BuildGlobalCoordinates()
        {
            foreach (var chunk in Chunks.Values)
            {
                chunk.UpdateGlobalCoords();
            }
            foreach (var chunk in Chunks.Values)
            {
                chunk.GenerateSurface();
            }
        }
        public void UpdateBorder()
        {
            foreach (var cn in Chunks.Values)
            {
                if (cn == null)
                    continue;
                foreach (var blockPos in cn.MembersList)
                {
                    bool skip = false;
                    var block = cn.Get(blockPos);
                    var nbs = block.Adjacent;
                    foreach (var verifyAdjacent in nbs)
                    {
                        var verify = verifyAdjacent.InChunk();
                        if (verify.IsInChunk())
                        {
                            VerifyBlock(verify, blockPos, cn);
                        }
                        else
                        {
                            if (blockPos.Y < 0 || blockPos.Y > Chunk.Size.Y)
                            {
                                cn.BorderMembers.Set(blockPos, true);
                                cn.BorderMembersList.Add(blockPos);
                                skip = true;
                                break;
                            }
                            var nextBlock = verify.InChunk();
                            var nextChunkLocation = cn.Location + new Vector2i(nextBlock.X == 0 ? 1 : -1, nextBlock.Y == 0 ? 1 : -1);
                            var nextChunk = GetChunkByLocation(nextChunkLocation);
                            if (nextChunk == null)
                                continue;
                            VerifyBlock(verify, nextBlock, nextChunk);
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
