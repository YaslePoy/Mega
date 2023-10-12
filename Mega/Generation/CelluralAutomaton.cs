using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Generation
{
    public class CelluralAutomaton<T>
    {
        protected int iteration;
        protected Random rand;
        public T[,] cells;
        public CelluralAutomaton(int size) : this(size, size)
        {
        }
        public CelluralAutomaton(int sizeX, int sizeY) : this(new T[sizeX, sizeY])
        {
        }
        public CelluralAutomaton(T[,] data)
        {
            cells = data;
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
        public static T GetIn(int x, int y, T[,] array)
        {
            int realX, realY;
            if (x < 0)
                realX = array.GetLength(0) + x;
            else
                realX = x % array.GetLength(0);
            if (y < 0)
                realY = array.GetLength(1) + y;
            else
                realY = y % array.GetLength(1);
            return array[realX, realY];
        }
        public static void SetIn(int x, int y, T[,] array, T value)
        {
            int realX, realY;
            if (x < 0)
                realX = array.GetLength(0) + x;
            else
                realX = x % array.GetLength(0);
            if (y < 0)
                realY = array.GetLength(1) + y;
            else
                realY = y % array.GetLength(1);
             array[realX, realY] = value;
        }
        protected virtual T Get(int x, int y)
        {
            return GetIn(x, y, cells);
        }

        protected virtual void Set(int x, int y, T value)
        {
            SetIn(x, y, cells, value);
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
        [ScriptAction("N")]
        public virtual void Next()
        {
            //if (iteration == 0)
            //    Start();
            //var backup = new T[cells.GetLength(0), cells.GetLength(1)];
            //for (int x = 0; x < cells.GetLength(0); x++)
            //{
            //    for (int y = 0; y < cells.GetLength(1); y++)
            //    {
            //        backup[x, y] = Propogate(x, y);
            //    }
            //}
            //cells = backup;
            //iteration++;
            NextSpecial(Propogate);
        }

        public virtual void NextSpecial(Func<int, int, T> method)
        {
            if (iteration == 0)
                Start();
            var backup = new T[cells.GetLength(0), cells.GetLength(1)];
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    backup[x, y] = method(x, y);
                }
            }
            cells = backup;
            iteration++;
        }
        public virtual void Start() { }

        public virtual ImageResult GetImage()
        {
            var img = new ImageResult() { Width = cells.GetLength(0), Height = cells.GetLength(1), };
            var data = new byte[cells.Length * 4];
            var preRaw = cells.Cast<T>().ToList();
            for (int i = 0; i < preRaw.Count; i++)
            {
                GetPixel(preRaw[i]).CopyTo(data, i * 4);
            }
            img.Data = data;
            return img;
        }

        public virtual byte[] GetPixel(T cell)
        {
            return new byte[] { 0, 0, 0, 0 };
        }

        public virtual void Scale2x()
        {
            var next =  new T[cells.GetLength(0) * 2, cells.GetLength(1) * 2];
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
    }
}
