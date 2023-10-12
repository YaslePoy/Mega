using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Game
{
    internal class RectangularCollider : Collider
    {
        public Vector3 size;
        public Vector3 position;
        public Vector3[,,] originalVerteces;
        public Vector3[,,] movedVerteces;
        public RectangularCollider(Vector3 position) : this(position, Vector3.One)
        {
        }

        public RectangularCollider(Vector3 position, Vector3 size) : this(position, size, Vector3.Zero)
        {
        }
        public RectangularCollider(Vector3 position, Vector3 size, Vector3 move)
        {
            this.position = position;
            this.size = size;
            GenerateSides(move);
        }
        void GenerateSides(Vector3 move)
        {
            originalVerteces = new Vector3[2, 2, 2];
            var half = size / 2;
            var add = half * move;
            int x = 0;
            var vtsList = new List<Vector3>();
            for (int i = -1; i < 2; i += 2)
            {
                int y = 0;
                for (int j = -1; j < 2; j += 2)
                {
                    int z = 0;
                    for (int k = -1; k < 2; k += 2)
                    {
                        var pt = half * new Vector3(i, j, k) + add;
                        originalVerteces[x, y, z] = pt;
                        pt += move;

                        z++;
                    }
                    y++;
                }
                x++;
            }
        }

    }
}
