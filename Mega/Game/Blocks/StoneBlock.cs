using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace Mega.Game.Blocks
{
    internal class StoneBlock : CubicBlock
    {
        public StoneBlock(Vector3i pos) : base(pos, 0)
        {
        }
    }
}
