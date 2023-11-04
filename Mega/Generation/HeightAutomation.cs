using Mega.Game;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Generation
{
    public class HeightAutomation : CelluralAutomaton<Height>
    {
        public int Scale = 128;
        UnitedChunk world;
        public HeightAutomation(int size) : base(size)
        {
        }
        public override void Start()
        {
            for (int i = 0; i < 32; i++)
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
            //var cell = Get(x, y);
            //if (cell.isStatic)
            //    return cell;

            //cell.h = GetAdjacentCrist(x, y).Select(i => i.h).Max();
            //cell.h -= 0.01;
            //cell.h *= cell.h;
            //return cell;
            var cell = new Height();
            var central = ((x - cells.GetLength(0) / 2d) / Scale, (y - cells.GetLength(1) / 2d) / Scale);
            //double dist = 1 / MathHelper.InverseSqrtFast(central.Item1 * central.Item1 + central.Item2 * central.Item2);
            //var h = (Math.Cos(central.Item2) * Math.Sin(central.Item1) * (2 * dist) + 1) / 2;
            var h = (Math.Sin(central.Item1) * Math.Cos(central.Item2) + 1) / 2;
            //var h = (Math.Sin(central.Item2) * central.Item1 * (2 / MathHelper.InverseSqrtFast(central.Item1 * central.Item1 + central.Item2 * central.Item2)) + 1) / 2;
            //var h = (Math.Cos(Math.Sqrt(central.Item1 * central.Item1 * central.Item2 * central.Item2)) + 1) / 2;
            //h *= (16 - dist) / 16 /** 0.1*/;
            cell.h = h;

            return cell;
        }
        public Height ToImgFormat(int x, int y)
        {
            var cell = Get(x, y);
            cell.h = (byte)(cell.h * byte.MaxValue);
            return cell;
        }
        public void ToImage()
        {
            NextSpecial(ToImgFormat);
        }
        public Height Minimize(int x, int y)
        {
            var cell = Get(x, y);
            var central = ((x - cells.GetLength(0) / 2d) / Scale, (y - cells.GetLength(1) / 2d) / Scale);
            double dist = MathHelper.InverseSqrtFast(central.Item1 * central.Item1 + central.Item2 * central.Item2);
            cell.h *= dist * 4;
            cell.h = Math.Min(cell.h, byte.MaxValue);
            return cell;
        }

        public void CreateHill()
        {
            NextSpecial(Minimize);
        }
        public Height SavePix(int x, int y)
        {
            var cell = Get(x, y);
            var half = cells.GetLength(0) / 2;
            var real = (x - half, y - half);
            var h = (int)Math.Truncate(cell.h / 8) + 1;
            for (int i = 0; i < h; i++)
            {
                world.SetBlock(new Block(new Vector3i(real.Item1, i, real.Item2), rand.Next(0, 2), true));
            }
            return cell;
        }
        public void Save()
        {
            world = new UnitedChunk();
            var n = cells.GetLength(0) / 64;
            for (int i = -n; i < n; i++)
            {
                for (int j = -n; j < n; j++)
                {
                    world.AddChunk(new Chunk(new Vector2i(i, j), world));
                }
            }
            NextSpecial(SavePix, false);
            WorldSaver.SaveWorld(world);
        }
        public override byte[] GetPixel(Height cell)
        {
            byte d = (byte)(/*cell.h * byte.MaxValue*/cell.h);
            return new byte[] { d, d, d, byte.MaxValue };
        }

        internal void SaveTo(ref UnitedChunk area)
        {

            this.world = new UnitedChunk();
            
            var n = cells.GetLength(0) / 64;
            for (int i = -n; i < n; i++)
            {
                for (int j = -n; j < n; j++)
                {
                    world.AddChunk(new Chunk(new Vector2i(i, j), world));
                }
            }
            NextSpecial(SavePix, false);
            area = world;
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
        public Height(double h)
        {
            this.h = h;
            isStatic = true;
        }
    }
}
