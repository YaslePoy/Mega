using OpenTK.Mathematics;

namespace Mega.Video
{
    public static class TextureHelper
    {
        public static Dictionary<string, UVMap> Maps = new();
        public static List<Texture> _textures = new();

        public static void Load()
        {
            LoadResoucePack("Resources/Mega.rp");

            // BaseMaps = new Dictionary<string, UVMap>();
            // var dir = Directory.GetFiles("Resources");
            // List<string> textureFiles = new List<string>();
            // foreach (var f in dir)
            // {
            //     var name = f.Substring(10);
            //     if (name.EndsWith(".png"))
            //     {
            //         textureFiles.Add(f);
            //         continue;
            //     }
            //
            //     if (!char.IsDigit(name[0]))
            //     {
            //         var currentMap = UVParser.ParseFile(f);
            //         BaseMaps.Add(name.Substring(5, name.Length - 8), currentMap);
            //     }
            // }
            //
            // TotalUVMaps = new List<(Texture, UVMap)>();
            // foreach (var texture in textureFiles)
            // {
            //     var loadTexture = Texture.LoadFromFile(texture);
            //     var map = UVParser.ParseFile(texture.Replace(".png", ".uv"));
            //     TotalUVMaps.Add((loadTexture, map));
            // }

            Atlas.Main = new Atlas(_textures);
        }

        static void LoadResoucePack(string packPath)
        {
            var text = File.ReadAllLines(packPath).Where(i => !string.IsNullOrWhiteSpace(i)).ToArray();
            var packId = packPath.Split(new[] { '/', '.' })[^2];
            for (int i = 0; i < text.Length;)
            {
                string line = text[i++];
                if (line.StartsWith("uvTemplate"))
                {
                    var splited = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var localMap = new UVMap() { Name = splited[1] };

                    line = text[i++];
                    List<Vector2[]> sides = new List<Vector2[]>();
                    while (line != "uvTemplateEnd")
                    {
                        Vector2[] points;
                        if (!line.StartsWith("link"))
                            points = UVParser.parseArray(line);
                        else
                            points = sides[int.Parse(line.Substring(5))];
                        sides.Add(points);
                        line = text[i++];
                    }

                    localMap.Sides = sides.ToArray();
                    Maps.Add(localMap.Name, localMap);
                }
                else if (line.StartsWith("bind"))
                {
                    var splited = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var key = splited[1];
                    var map = splited[2];
                    var img = splited[3];
                    var size = splited[4].Split('*', StringSplitOptions.RemoveEmptyEntries);
                    var config = new Vector2i(int.Parse(size[0]), int.Parse(size[1]));
                    Maps.Add($"{packId}:{key}", Maps[map]);
                    _textures.Add(new Texture($"{string.Join('/', packPath.Split('/', StringSplitOptions.RemoveEmptyEntries)[..^1])}/{img}",
                        Maps[map], $"{packId}:{key}") { Size = config });
                }
            }
        }
        

        public static Vector2[] GetTextureCoords(int id, int side)
        {
            // var map = TotalUVMaps[id];
            // var pts = map.uv.GetSide(side);
            // return pts;
            return new Vector2[0];
        }
    }

    public class UVMap
    {
        public string Name;
        public Vector2[][] Sides;
        public Vector2[] this[int side] => Sides[side];

        public UVMap Transform(Vector2i location, Vector2i size, Vector2i total)
        {
            Vector2 temp;
            Vector2[][] data = new Vector2[Sides.Length][];
            for (int i = 0; i < Sides.Length; i++)
            {
                data[i] = new Vector2[Sides[i].Length]; 
                for (int j = 0; j < Sides[i].Length; j++)
                {
                    temp = (Sides[i][j] * size + location) / total;
                    data[i][j] = temp;
                }
            }

            return new UVMap() { Sides = data };
        }
    }

    public static class UVParser
    {
        static Vector2 parseVec(string vec)
        {
            var sp = vec.Split('*', StringSplitOptions.RemoveEmptyEntries);
            return new Vector2(float.Parse(sp[0]), float.Parse(sp[1]));
        }

        public static Vector2[] parseArray(string array)
        {
            var sp = array.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            return sp.Select(parseVec).ToArray();
        }
    }
}