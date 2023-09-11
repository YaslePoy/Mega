using Mega.Video;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Game
{
    public class Player
    {
        World world;
        public Vector3i SelectedBlock;
        public Vector3i Cursor;
        public Vector3 Position => Cam.Position;
        public Vector3 View => Cam.Front;
        public Camera Cam;
        public Player(Camera camera, World world)
        {
            Cam = camera;
            this.world = world;
        }

        public void PlaceBlock()
        {
            try
            {
                Window.sw.Restart();
                world.SetBlock(Cursor, 1);
                Window.sw.Stop();
                Console.WriteLine("Change time: " + Window.sw.Elapsed.Microseconds);
            }
            catch { }
        }
    }
}
