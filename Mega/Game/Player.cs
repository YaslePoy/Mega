using Mega.Video;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Game
{
    public class Player
    {
        public const float Growth = 1.75f;
        readonly Vector3 growthAdd = new Vector3(0, Growth, 0);
        public const float WalkSpeed = 4f;
        public World world;
        public Vector3i? SelectedBlock;
        public Vector3i? Cursor;
        public Camera Cam;
        public Vector2 Moving;
        public float VerticalSpeed;
        public bool Jumping;
        public Vector3 Position;
        RectangularCollider Collider;

        public Vector3 LastUpdateMove;
        public Vector3 View => Cam.Front;
        public Vector3 ViewPoint => Position + growthAdd;
        public bool IsActs
        {
            get => acts; set
            {
                acts = value;
                if (acts)
                    IsActed = false;
            }
        }
        public bool IsActed { get; set; } = true;
        bool acts;
        public void UpdateCamPosition()
        {
            Cam.Position = Position + growthAdd;
        }
        public Player(Camera camera)
        {
            Cam = camera;
            Position = camera.Position;
            Collider = new RectangularCollider(Position, new Vector3(0.5f, 1.75f, 0.5f), -Vector3.UnitY);

        }
        public void PlaceBlock()
        {
            if (!Cursor.HasValue)
                return;
            var newBlock = new Block(Cursor.Value, 1);
            world.SetBlock(newBlock);
        }
        public RectangularCollider GetCollider()
        {
            if (Collider.Position == Position)
                return Collider;
            else
            {
                Collider.Position = Position;
                return Collider;
            }
        }

        public void MoveTo(Vector3 move)
        {
            Position += move;

        }
    }
}
