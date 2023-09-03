using Mega.Video;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Game
{
    internal class Player
    {
        public Vector3 Position;
        public Camera Cam;
        public Player(Camera camera)
        {
            Cam = camera;
        }
    }
}
