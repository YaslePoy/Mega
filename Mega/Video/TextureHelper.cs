using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Video
{
    public static class TextureHelper
    {
        public static Vector2[][][] TextureCoordiantes;
        public static Dictionary<string, UVMap> BaseMaps;
        public static List<(Texture tex, UVMap uv)> TotalUVMaps;
        public static void LoadUV()
        {
            //TextureCoordiantes = new Vector2[2][][];
            //var cubeCoords = new Vector2[2][];
            //var vertical = new Vector2[] { Vector2.Zero, new Vector2(0.5f, 0), new Vector2(0.5f, 1), new Vector2(0, 1) };
            //var horizontal = vertical.Select(i => i + Vector2.UnitX / 2).ToArray();
            //cubeCoords[0] = vertical;
            //cubeCoords[1] = horizontal;
            //TextureCoordiantes[0] = cubeCoords;
            //TextureCoordiantes[1] = new Vector2[][]{ vertical };
            BaseMaps = new Dictionary<string, UVMap>();
            var dir = Directory.GetFiles("Resources");
            List<string> textureFiles = new List<string>();
            foreach (var f in dir)
            {
                var name = f.Substring(10);
                if (name.EndsWith(".png"))
                {
                    textureFiles.Add(f);
                    continue;
                }
                if (!char.IsDigit(name[0]))
                {
                    var currentMap = UVParser.ParseFile(f);
                    BaseMaps.Add(name.Substring(5, name.Length - 8), currentMap);
                }
            }
            TotalUVMaps = new List<(Texture, UVMap)>();
            foreach (var texture in textureFiles)
            {
                var loadTexture = Texture.LoadFromFile(texture);
                var map = UVParser.ParseFile(texture.Replace(".png", ".uv"));
                TotalUVMaps.Add((loadTexture, map));
            }
        }

        public static Vector2[] GetTextureCoords(string id, int side)
        {
            if (id == "birch")
            {
                switch (side)
                {
                    case 1:
                    case 4:
                        return TextureCoordiantes[0][0];
                    case 0:
                    case 2:
                    case 3:
                    case 5:
                        return TextureCoordiantes[0][1];


                }
            }
            if (id == "select")
                return TextureCoordiantes[1][0];
            return null;
        }
    }
    public class UVMap
    {
        UVSide[] sides;
        public void SetSides(UVSide[] sides) => this.sides = sides;
        public virtual Vector2[] GetSide(int side)
        {
            return sides[side].Get();
        }
    }

    public class LinkMap : UVMap
    {
        string link;
        public LinkMap() : base()
        {
            link = "all";
        }
        public LinkMap(string link) : base()
        {
            this.link = link;
        }
        public override Vector2[] GetSide(int side)
        {
            return TextureHelper.BaseMaps[link].GetSide(side);
        }
    }
    public class UVSide
    {
        public UVSide(UVMap map)
        {
            this.map = map;
        }
        public void SetPoints(Vector2[] points)
        {
            Points = points;
        }
        public Vector2[] Points;
        protected UVMap map;
        public virtual Vector2[] Get()
        {
            return Points;
        }
    }

    public class LinkSide : UVSide
    {
        int link;
        public LinkSide(UVMap map) : base(map)
        {
        }

        public LinkSide(UVMap map, int link) : base(map)
        {
            this.link = link;
        }

        public override Vector2[] Get()
        {
            return map.GetSide(link);
        }
    }
    public static class UVParser
    {
        public static UVMap ParseFile(string path)
        {
            var map = new UVMap();
            var text = File.ReadAllLines(path);
            if (text.Length == 1 && text[0].StartsWith("use"))
                return new LinkMap(text[0].Substring(4));
                var sides = new List<UVSide>();
            foreach (var line in text)
            {
                if (line.StartsWith("link"))
                {
                    sides.Add(new LinkSide(map, int.Parse(line.Substring(5))));
                }
                else
                {
                    var pts = parseArray(line);
                    sides.Add(new UVSide(map) { Points = pts });
                }
            }
            map.SetSides(sides.ToArray());
            return map;
        }
        static Vector2 parseVec(string vec)
        {
            var sp = vec.Split('*');
            return new Vector2(float.Parse(sp[0]), float.Parse(sp[1]));
        }

        static Vector2[] parseArray(string array)
        {
            var sp = array.Split(" ");
            return sp.Select(parseVec).ToArray();
        }
    }
}
