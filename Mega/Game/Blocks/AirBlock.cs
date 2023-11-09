using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mega.Video;
using OpenTK.Mathematics;

namespace Mega.Game.Blocks
{
    internal class AirBlock : Block
    {
        public AirBlock(Vector3i pos) : base(pos, -1)
        {

        }

        public override List<RenderSurface> GetDrawingMesh(UnitedChunk area)
        {
            view = new List<RenderSurface>();
            return view;
        }
    }
}
