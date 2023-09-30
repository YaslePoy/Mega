using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Game
{
    public abstract class Collider
    {
        public abstract bool IsContains(Vector3 point);
        public abstract bool MoveToPossible(ref Vector3 nextPt, Vector3 startPt);
    }
}
