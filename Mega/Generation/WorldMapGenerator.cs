using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            Stopwatch timer = new Stopwatch();
            timer.Start();
            var Automaton = new ContinentAutimation(16);
            var total = Automaton.cells.Length;
            var land = 0;
            Automaton.ratio = 75;
            while (land / (double)total < 0.1)
            {
                Automaton.Next();
                land = Automaton.NotFreeCount();
            }
            //Automaton.ratio = 50;
            for (int i = 0; i < 4; i++)
            {
                Automaton.Scale2x();
                Automaton.Next();
                Automaton.Next();
                Automaton.Next();
                Automaton.ratio -= 10;
            }

            var stoves = new StoveAutimation(Automaton.cells);
            var stovesLast = 0;
            do
            {
                stovesLast = stoves.filled;
                stoves.Next();
            } while (stoves.total != stoves.filled && stovesLast != stoves.filled);
            timer.Stop();
            Console.WriteLine($"Generation time : {timer.Elapsed}");
            return stoves.cells;
        }
    }
}
