using OpenTK.Mathematics;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Generation
{
    public class ContinentAutimation : CelluralAutomaton<Area>
    {
        public ContinentAutimation(int size) : base(size)
        {
        }

        public ContinentAutimation(Area[,] data) : base(data)
        {
        }

        public ContinentAutimation(CelluralAutomaton<Area> startup) : base(startup)
        {
        }

        public ContinentAutimation(int sizeX, int sizeY) : base(sizeX, sizeY)
        {
        }

        public override void Start()
        {
            base.Start();
        }

        public override Area Propogate(int x, int y)
        {
            var around = GetAdjacentCircle(x, y);
            if (around.Count() == 0)
                return Get(x, y);
            if (around.Any(i => i != around.First()))
                return Get(x, y);

            if (around.Count() == 1 && iteration > 1)
                return Get(x, y);
            //if (around.Count() >= 7 && iteration > 5)
            //    return around[0];
            if (rand.NextDouble() > 0.9 * around[0].K)
                return around[0];
            return Get(x, y);
        }
        public void BuildContinentsPoints(int n)
        {
            for (int i = 0; i < n; i++)
            {
                while (true)
                {
                    var ptX = rand.Next(0, cells.GetLength(0));
                    var ptY = rand.Next(0, cells.GetLength(1));
                    if (GetAdjacentCircle(ptX, ptY).Count() > 0)
                        continue;
                    var gen = new Area {K = rand.NextDouble(), ID = i, Color = new Vector4((float)(rand.NextDouble() * byte.MaxValue), (float)(rand.NextDouble() * byte.MaxValue), (float)(rand.NextDouble() * byte.MaxValue), 255) };
                    Set(ptX, ptY, gen);
                    break;
                }
            }
        }
        public override byte[] GetPixel(Area cell)
        {
            if (cell is null)
                return new byte[4];
            return new byte[] { Math.Min(byte.MaxValue, (byte)cell.Color.X), Math.Min(byte.MaxValue, (byte)cell.Color.Y), Math.Min(byte.MaxValue, (byte)cell.Color.Z), Math.Min(byte.MaxValue, (byte)cell.Color.W), };
        }
    }

    public class Area
    {
        public int ID;
        public double K;
        public Vector4 Color;
        public static bool operator ==(Area c1, Area c2) => c1.ID == c2.ID;
        public static bool operator !=(Area c1, Area c2) => c1.ID != c2.ID;

    }
}
