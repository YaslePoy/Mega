using Mega.Game;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Video
{
    public class UnitedChunk
    {
        public Chunk[] Chunks;
        public List<Vector3i> MembersList;
        public List<Vector3i> BorderMembersList;
        public UnitedChunk(int chunks)
        {
            Chunks = new Chunk[chunks];
        }
        public Block GetBlock(Vector3i position)
        {
            var path = position.ToWorldPath();
            var chunk = Chunks.FirstOrDefault(i => i.Location == path.chunk);
            if (chunk == null)
                return null;
            return chunk.data.Get(path.block);
        }

        public void SetBlock(Vector3i position, Block block)
        {
            var path = position.ToWorldPath();
            var chunk = Chunks.FirstOrDefault(i => i.Location == path.chunk);
            if (chunk == null)
                return;
            chunk.data.Set(path.block, block);
        }
        public bool GetBorder(Vector3i position)
        {
            var path = position.ToWorldPath();
            var chunk = Chunks.FirstOrDefault(i => i.Location == path.chunk);
            if (chunk == null)
                return false;
            return chunk.Border.Get(path.block);
        }
        public void SetBorder(Vector3i position, bool value)
        {
            var path = position.ToWorldPath();
            var chunk = Chunks.FirstOrDefault(i => i.Location == path.chunk);
            if (chunk == null)
                return;
            chunk.Border.Set(path.block, value);
        }
        public bool GetMember(Vector3i position)
        {
            var path = position.ToWorldPath();
            var chunk = Chunks.FirstOrDefault(i => i.Location == path.chunk);
            if (chunk == null)
                return false;
            return chunk.Members.Get(path.block);
        }
        public void SetMember(Vector3i position, bool value)
        {
            var path = position.ToWorldPath();
            var chunk = Chunks.FirstOrDefault(i => i.Location == path.chunk);
            if (chunk == null)
                return;
            chunk.Members.Set(path.block, value);
        }
        public bool GetBorderMember(Vector3i position)
        {
            var path = position.ToWorldPath();
            var chunk = Chunks.FirstOrDefault(i => i.Location == path.chunk);
            if (chunk == null)
                return false;
            return chunk.BorderMembers.Get(path.block);
        }
        public void SetBorderMember(Vector3i position, bool value)
        {
            var path = position.ToWorldPath();
            var chunk = Chunks.FirstOrDefault(i => i.Location == path.chunk);
            if (chunk == null)
                return;
            chunk.BorderMembers.Set(path.block, value);
        }
        public void UpdateMemberList()
        {
            MembersList = Chunks.Where(i => i != null).Select(i => i.MembersList).ToList().SumList();
        }
        public void UpdateBorderMembersList()
        {
            BorderMembersList = Chunks.Where(i => i != null).Select(i => i.BorderMembersList).ToList().SumList();
        }
    }
}
