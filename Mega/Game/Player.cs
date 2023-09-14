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
        public const float WalkSpeed = 1.5f;
        World world;
        public Vector3i SelectedBlock;
        public Vector3i Cursor;
        public Camera Cam;
        public Vector2 Moving;
        public float VerticalSpeed;
        public bool Jumping;
        public Vector3 Position { get => Cam.Position; set => Cam.Position = value; }
        public Vector3 View => Cam.Front;

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
