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
        public abstract VolumeMembership GetMembership(Vector3 point);
        public abstract bool MoveToPossible(Vector3 startPosition, Vector3 moveVector, out Vector3 nextPosition);
        public enum VolumeMembership
        {
            Out, Border, Into
        }
    }
}
