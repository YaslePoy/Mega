using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Game
{
    public class RectangularCollider : Collider
    {
        public Vector3 size;
        Vector3 position;
        public Vector3 Position
        {
            get => position;
            set
            {
                position = value;
                UpdateDynamicMesh();
            }
        }
        public Vector3[] originalVerteces;
        public LimitedPlane[] originalPlanes;
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
            var localMove = (move  + Vector3.One) / -2;
            sides = CubicCollider.OneCube.Select(i => i + localMove).ToArray();
            sides = sides.Select(i => i * size).ToArray();
            GenerateVertexes();
            originalVerteces = Vertexes;
            originalPlanes = sides;
            Vertexes = Vertexes.Select(i => i + position).ToArray();
            sides = sides.Select(i => i + position).ToArray();
        }
        void UpdateDynamicMesh()
        {
            Vertexes = originalVerteces.Select(i => i + position).ToArray();
            sides = originalPlanes.Select(i => i + position).ToArray();
        }
    }
}
