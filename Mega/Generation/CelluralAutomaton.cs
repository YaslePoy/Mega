using Mega.Video.Shading;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Mega.Generation
{
    public class CelluralAutomaton<T>
    {
        protected int iteration;
        public RandomNumberGenerator rng;
        protected Random rand;
        public T[,] cells;
        public bool changed;
        public CelluralAutomaton(int size) : this(size, size)
        {
        }
        public CelluralAutomaton(int sizeX, int sizeY) : this(new T[sizeX, sizeY])
        {
            rand = new Random();
            rng = RandomNumberGenerator.Create();

        }
        public CelluralAutomaton(T[,] data)
        {
            cells = data;
            rand = new Random();
            rng = RandomNumberGenerator.Create();

        }
        public CelluralAutomaton(CelluralAutomaton<T> startup) { cells = startup.cells; }

        public void Clear()
        {
            cells = new T[cells.GetLength(0), cells.GetLength(1)];
            iteration = 0;
        }
        public void ResetIterations() => iteration = 0;
        public void SetRandom(int seed)
        {
            rand = new Random(seed);
        }
        protected virtual T Get(int x, int y)
        {
            int realX, realY;
            if (x < 0)
                realX = cells.GetLength(0) + x;
            else
                realX = x % cells.GetLength(0);
            if (y < 0)
                realY = cells.GetLength(1) + y;
            else
                realY = y % cells.GetLength(1);
            return cells[realX, realY];
        }

        protected virtual void Set(int x, int y, T value)
        {
            int realX, realY;
            if (x < 0)
                realX = cells.GetLength(0) + x;
            else
                realX = x % cells.GetLength(0);
            if (y < 0)
                realY = cells.GetLength(1) + y;
            else
                realY = y % cells.GetLength(1);
            if (value != null || cells[realX, realY] != null)
                if (value != null && !value.Equals(cells[realX, realY]))
                    changed = true;
                else if (!cells[realX, realY].Equals(value))
                    changed = true;
            cells[realX, realY] = value;
        }

        public virtual T[] GetAdjacentCrist(int x, int y)
        {
            var adj = new List<T>
            {
                Get(x + 1, y),
                Get(x - 1, y),
                Get(x, y + 1),
                Get(x, y - 1)
            };
            adj.RemoveAll(i => i is null);
            return adj.ToArray();
        }
        public virtual T[] GetAdjacentCircle(int x, int y)
        {
            var adj = new List<T>
            {
                Get(x + 1, y),
                Get(x - 1, y),
                Get(x, y + 1),
                Get(x, y - 1),
                Get(x + 1, y + 1),
                Get(x + 1, y - 1),
                Get(x - 1, y + 1),
                Get(x - 1, y - 1),
            };
            adj.RemoveAll(i => i is null);
            return adj.ToArray();
        }
        public virtual T Propogate(int x, int y)
        {
            return Get(x, y);
        }
        public virtual void Next()
        {
            changed = false;
            NextSpecial(Propogate);
        }

        public virtual void NextSpecial(Func<int, int, T> method)
        {
            if (iteration == 0)
                Start();
            var backup = new T[cells.GetLength(0), cells.GetLength(1)];
            Parallel.For(0, cells.GetLength(0), (i => {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    var nextVal = method(i, y);
                    backup[i, y] = nextVal;
                }
            }));
            //for (int x = 0; x < cells.GetLength(0); x++)
            //{
            //    for (int y = 0; y < cells.GetLength(1); y++)
            //    {
            //        var nextVal = method(x, y);
            //        backup[x, y] = nextVal;
            //    }
            //}
            cells = backup;
            iteration++;
        }
        public virtual void Start() { }

        public virtual Image GetImage()
        {
            var img = new Image() { Width = cells.GetLength(0), Height = cells.GetLength(1), Data = cells.Cast<T>().Select(GetPixel).SumList() };
            return img;
        }

        public virtual byte[] GetPixel(T cell)
        {
            return new byte[] { 0, 0, 0, 0 };
        }

        public virtual void Scale2x()
        {
            var next = new T[cells.GetLength(0) * 2, cells.GetLength(1) * 2];
            for (int i = 0; i < next.GetLength(0); i++)
            {
                for (int j = 0; j < next.GetLength(1); j++)
                {
                    next[i, j] = cells[i / 2, j / 2];
                }
            }
            cells = next;
            Console.WriteLine($"Scaled to {next.GetLength(0)}x{next.GetLength(1)}");
        }
        public virtual int NotFreeCount() => cells.Cast<T>().Count(i => i is not null);

    }
}
