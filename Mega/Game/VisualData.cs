using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Game
{
    public static class VisualData
    {
        public static Vector2[][][] TextureCoordiantes;

        public static void LoadVisualData()
        {
            TextureCoordiantes = new Vector2[2][][];
            var cubeCoords = new Vector2[2][];
            var vertical = new Vector2[] { Vector2.Zero, new Vector2(0.5f, 0), new Vector2(0.5f, 1), new Vector2(0, 1) };
            var horizontal = vertical.Select(i => i + Vector2.UnitX / 2).ToArray();
            cubeCoords[0] = vertical;
            cubeCoords[1] = horizontal;
            TextureCoordiantes[0] = cubeCoords;
            TextureCoordiantes[1] = new Vector2[][]{ vertical };
        }

        public static Vector2[] GetTextureCoords(string id, int side)
        {
            if (id == "birch")
            {
                switch (side)
                {
                    case 1:
                        return TextureCoordiantes[0][0];
                    case 4:
                        return TextureCoordiantes[0][0];
                    case 5:
                    case 0:
                    case 3:
                    case 2:
                        return TextureCoordiantes[0][1];


                }
            }
            if (id == "select")
                return TextureCoordiantes[1][0];
            return null;
        }
    }
}
