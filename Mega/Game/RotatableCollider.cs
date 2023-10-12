using OpenTK.Mathematics;

namespace Mega.Game
{
    public class RotatableCollider
    {
        public Vector3 size;
        public Vector3 position;
        public Vector3[,,] originalVerteces;
        public Quaternion rotation;
        public RotatableCollider(Vector3 position) : this(position, Vector3.One)
        {
        }

        public RotatableCollider(Vector3 position, Vector3 size) : this(position, size, Vector3.Zero)
        {
        }
        public RotatableCollider(Vector3 position, Vector3 size, Vector3 move)
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
            int x = 0, y = 0, z = 0;
            for (int i = -1; i < 2; i += 2)
            {
                y = 0;
                for (int j = -1; j < 2; j += 2)
                {
                    z = 0;
                    for (int k = -1; k < 2; k += 2)
                    {
                        originalVerteces[x, y, z] = half * new Vector3(i, j, k);
                        z++;
                    }
                    y++;
                }
                x++;
            }
        }
       
    }
}