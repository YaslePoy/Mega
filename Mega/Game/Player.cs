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
        public const float Growth = 1.75f;
        readonly Vector3 growthAdd = new Vector3(0, Growth, 0);
        public const float WalkSpeed = 6f;
        public World world;
        public Vector3i SelectedBlock;
        public Vector3i Cursor;
        public Camera Cam;
        public Vector2 Moving;
        public float VerticalSpeed;
        public bool Jumping;
        public Vector3 Position;

        public Vector3 View => Cam.Front;
        public Vector3 ViewPoint => Position + growthAdd;
        public void UpdateCamPosition()
        {
            Cam.Position = Position + growthAdd;
        }
        public Player(Camera camera)
        {
            Cam = camera;
            Position = camera.Position;
        }
        public void PlaceBlock()
        {
            try
            {
                Window.sw.Restart();
                var newBlock = new Block(Cursor, 1);
                world.SetBlock(newBlock);
                Window.sw.Stop();
                Console.WriteLine("Change time: " + Window.sw.Elapsed.Microseconds);
            }
            catch { }
        }
    }
}
