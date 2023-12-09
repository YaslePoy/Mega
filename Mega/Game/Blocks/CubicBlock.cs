using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mega.Video;
using OpenTK.Mathematics;

namespace Mega.Game.Blocks
{
    public abstract class CubicBlock : Block
    {
        public CubicBlock(Vector3i pos, int id) : base(pos, id)
        {
        }

        public override List<RenderSurface> GetDrawingMesh(UnitedChunk area)
        {
            view = new List<RenderSurface>(3);
            List<byte> sides = new List<byte>(3);
            for (byte i = 0; i < 6; i++)
            {
                if (area.IsMember(Adjacent[i]))
                    continue;
                var surface = new RenderSurface(MeshSides[i], TextureHelper.GetTextureCoords(/*IDCode*/0, i), Position,
                    Neibs[i], IDCode);
                view.Add(surface);
                sides.Add(i);
            }

            collider = new CubicCollider(Position, sides);
            return view;
        }
    }
}