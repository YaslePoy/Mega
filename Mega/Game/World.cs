using Mega.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Game
{
    internal class World
    {
        public Player Player;
        Window view;
        Chunk[] chunks;
        public readonly int RenderDistance;
        public World(Player player, Window view, int renderDistance)
        {
            Player = player;
            this.view = view;
            this.chunks = new Chunk[(renderDistance * renderDistance + renderDistance) * 4 + 1];
        }
        public void SetChunk(Chunk chunk, int index)
        {
            chunks[index] = chunk;
        }
        public void UpdateView()
        {

        }

    }
}
