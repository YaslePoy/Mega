using OpenTK.Mathematics;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Generation
{
    public class StoveAutimation : ContinentAutimation
    {
        Dictionary<Area, int> areas;
        int maxContID;

        public StoveAutimation(Area[,] data) : base(data)
        {
            SetRandom(0);
        }
        //public static StoveAutimation FromContinental(ContinentAutimation continentAutimation)
        //{
        //    var 
        //}
        public override void Start()
        {
            areas = new Dictionary<Area, int>();
            foreach (Area continentCell in cells)
            {
                if(continentCell is null) continue;
                areas.TryAdd(continentCell, 0);
                areas[continentCell] += 1;
            }
            maxContID = areas.Count();
            CreateStartupCells(10);
        }
        public void CreateStartupCells(int count)
        {
            for (int i = 0; i < count; i++)
            {
                while (true)
                {
                    var ptX = rand.Next(0, cells.GetLength(0));
                    var ptY = rand.Next(0, cells.GetLength(1));
                    var adj = GetAdjacentCircle(ptX, ptY);
                    if (adj.Count() == 0)
                        continue;
                    var gen = new Area { K = rand.NextDouble(), ID = i + 1 + maxContID, Color = new Vector4((float)(rand.NextDouble() * byte.MaxValue), (float)(rand.NextDouble() * byte.MaxValue), (float)(rand.NextDouble() * byte.MaxValue), 255) };
                    Set(ptX, ptY, gen);
                    break;
                }
            }
        }

        public override Area Propogate(int x, int y)
        {
            var currCell = Get(x, y);

            if (currCell is null)
                return currCell;
            if (currCell.ID >= maxContID)
                return currCell;
            var adj = GetAdjacentCircle(x, y);
            if(!adj.Any(i => i.ID >= maxContID))
                return currCell;
            if (rand.NextDouble() > 0.7)
                return adj.FirstOrDefault(i => i.ID >= maxContID);
            return currCell;
        }
        public Area RoundBorder(int x, int y)
        {
            var adj = GetAdjacentCircle(x, y);
            if (adj.Count() == 0)
                return null;
            var disincted = adj.Distinct().ToList();
            if (disincted.Count() == 1)
                return adj[0];
            else
            {
                var counts = new Dictionary<Area, int>(disincted.Select(i => new KeyValuePair<Area, int>(i, 0)));
                foreach ( var i in adj )
                    counts[i] += 1;
                var cur = counts.MaxBy(i => i.Value).Key;
                return cur;
            }
        }

        public void RoundBorders()
        {
            NextSpecial(RoundBorder);
        }
    }
}
