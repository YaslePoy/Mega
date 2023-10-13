using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Generation
{
    public class WorldMapGenerator
    {
        public WorldMapGenerator()
        {
        }
        public Area[,] Generate()
        {
            //for (int i = 0;i < 8; i++)
            //    Automaton.Next();
            //for (int i = 0; i < 3; i++)
            //{
            //    Automaton.Scale2x();
            //    for (int j = 0; j < 3; j++)
            //    Automaton.Next();
            //}
            var Automaton = new ContinentAutimation(16);
            var total = Automaton.cells.Length;
            var land = 0;
            while (land / (double)total < 0.2)
            {
                Automaton.Next();
                land = Automaton.NotFreeCount();
            }
            for (int i = 0; i < 4; i++)
            {
                if (i != 4) Automaton.Scale2x();
                Automaton.Next();
                Automaton.Next();
                Automaton.Next();
            }
            var stoves = new StoveAutimation(Automaton.cells);
            var stovesLast = 0;
            do
            {
                stovesLast = stoves.filled;
                stoves.Next();
                Console.WriteLine($"{stoves.filled} / {stoves.total}");
            } while (stoves.total != stoves.filled && stovesLast != stoves.filled);
            return stoves.cells;
        }
    }
}
