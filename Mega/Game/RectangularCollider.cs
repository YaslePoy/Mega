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
        public Vector3[] originalVerteces;
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
            var localMove = move - new Vector3(0.5f, 0.5f, 0.5f);
            var startup = CubicCollider.OneCube;
            startup = startup.Select(i => i + localMove).ToArray();
            startup = startup.Select(i => i * size).ToArray();
            GenerateVertexes();
            originalVerteces = Vertexes;
            Vertexes = Vertexes.Select(i => i + position).ToArray();
            GenerateVertexes();
        }

    }
}
