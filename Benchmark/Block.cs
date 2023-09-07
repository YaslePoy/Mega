using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark
{
    public class Block
    {
        Vector3i[] localNeibs;
        public Vector3i[] LocalNeibs => localNeibs == null ? GenerateNeibs() : localNeibs;
        public static readonly Vector3i[] Neibs = {
            Vector3i.UnitX, Vector3i.UnitY, Vector3i.UnitZ,
            -Vector3i.UnitX, -Vector3i.UnitY, -Vector3i.UnitZ
        };
        public static readonly Vector3[][] MeshSides = {
            new[] { new (1, 0, 0), new (1, 0, 1), new (1, 1, 1), new Vector3(1, 1, 0) },
            new[] { new (0, 1, 0), new (0, 1, 1), new (1, 1, 1), new Vector3(1, 1, 0) },
            new[] { new (0, 0, 1), new (1, 0, 1), new (1, 1, 1), new Vector3(0, 1, 1) },
            new[] { new (0, 0, 0), new (0, 0, 1), new (0, 1, 1), new Vector3(0, 1, 0) },
            new[] { new (0, 0, 0), new (1, 0, 0), new (1, 0, 1), new Vector3(0, 0, 1) },
            new[] { new (0, 0, 0), new (1, 0, 0), new (1, 1, 0), new Vector3(0, 1, 0) }
        };

        public string ID;
        public Vector3i Position;

        //RenderSurface[] totalSurface;
        //public Block()
        //{
        //    Type = BlockType.Air;
        //    totalSurface = new RenderSurface[0];
        //}

        public Block(Vector3i pos, string id)
        {
            Position = pos;
            ID = id;
        }

        Vector3i[] GenerateNeibs()
        {
            var pos = Position;
            localNeibs = Neibs.Select(i => i + pos).Where(i => (i.X is > 0 and < 32) && (i.Y is > 0 and < 32) && (i.Z is > 0 and < 32)).ToArray();
            return localNeibs;
        }

        public Vector3i[] GenerateNeisByCode()
        {
            List<Vector3i> result = new List<Vector3i>();
            var neib = Position + Neibs[0];
            if ((neib.X is > 0 and < 32) && (neib.Y is > 0 and < 32) && (neib.Z is > 0 and < 32))
                result.Add(neib);
            neib = Position + Neibs[1];
            if ((neib.X is > 0 and < 32) && (neib.Y is > 0 and < 32) && (neib.Z is > 0 and < 32))
                result.Add(neib);
            neib = Position + Neibs[2];
            if ((neib.X is > 0 and < 32) && (neib.Y is > 0 and < 32) && (neib.Z is > 0 and < 32))
                result.Add(neib);
            neib = Position + Neibs[3];
            if ((neib.X is > 0 and < 32) && (neib.Y is > 0 and < 32) && (neib.Z is > 0 and < 32))
                result.Add(neib);
            neib = Position + Neibs[4];
            if ((neib.X is > 0 and < 32) && (neib.Y is > 0 and < 32) && (neib.Z is > 0 and < 32))
                result.Add(neib);
            neib = Position + Neibs[5];
            if ((neib.X is > 0 and < 32) && (neib.Y is > 0 and < 32) && (neib.Z is > 0 and < 32))
                result.Add(neib);
            return result.ToArray();
        }
    }
}
