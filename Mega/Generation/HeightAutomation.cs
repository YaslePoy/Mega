using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Generation
{
    public class HeightAutomation : CelluralAutomaton<Height>
    {
        public HeightAutomation(int size) : base(size)
        {
        }
        public override void Start()
        {
            for (int i = 0; i < 20; i++)
            {

                while (true)
                {
                    var ptX = rand.Next(0, cells.GetLength(0));
                    var ptY = rand.Next(0, cells.GetLength(1));
                    if (Get(ptX, ptY).isStatic)
                        continue;
                    var gen = new Height(1);
                    Set(ptX, ptY, gen);
                    break;
                }

            }
        }
        public override Height Propogate(int x, int y)
        {
            var cell = Get(x, y);
            if(cell.isStatic)
            return cell;
            cell.h += GetAdjacentCircle(x, y).Select(i => i.h).Sum();
            cell.h /= 9;
            return cell;

        }
        public override byte[] GetPixel(Height cell)
        {
            byte d = (byte)(cell.h * byte.MaxValue);
            return new byte[] { d, d, d, byte.MaxValue };
        }
    }
    public struct Height
    {
        public double h;
        public bool isStatic;
        public Height()
        {
            h = 0;
            isStatic = false;
        }
        public Height(double h) {
            this.h = h;
            isStatic = true;
        }
    }
}
