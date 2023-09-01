using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Game
{
    internal class World
    {
        Block[,,] worldData;
        public World()
        {
            worldData = new Block[32, 32, 64];

        }
    }
}
