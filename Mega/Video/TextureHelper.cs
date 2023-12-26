using OpenTK.Mathematics;

namespace Mega.Video
{
    public static class TextureHelper
    {
        public static Dictionary<string,UVMap> Maps = new ();
        public static List<Texture> Textures = new ();

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
        }

        static void LoadResoucePack(string packPath)
        {
            var text = File.ReadAllLines(packPath);
            var packId = packPath.Split(new[] { '/', '.' })[^2];
            for (int i = 0; i < text.Length;)
            {
                string line = text[i++];
                if (line.StartsWith("uvTemplate"))
                {
                    var splited = line.Split(' ');
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
                    var splited = line.Split(' ');
                    var key = splited[1];
                    var map = splited[2];
                    var img = splited[3];
                    var size = splited[4].Split('*');
                    var config = new Vector2i(int.Parse(size[0]), int.Parse(size[1]));
                    Maps.Add($"{packId}:{key}", Maps[map]);
                    Textures.Add(new Texture($"{string.Join('/', packPath.Split('/')[..^1])}/{img}",
                        Maps[map]){ Size = config});
                }
            }
        }

        public static byte[,][] AssemblevaАtlas()
        {
            var placedUnits = new List<Vector2i>();
            
            var sorted = Textures.OrderBy(i => i.Size.X * i.Size.Y).Reverse().ToList();

            foreach (var tex in sorted)
            {
                int maxSq = int.MaxValue;
                var selectedPos = new Vector2i();
                var currentSize = GetOutSize(placedUnits);
                void trySet(int i, int j)
                {
                    var tempTexture = new List<Vector2i>(placedUnits);
                    tex.Location.X = i;
                    tex.Location.Y = j;
                    var moved = tex.GetUnits();
                    bool next = false;
                    foreach (var unit in moved)
                    {
                        if (tempTexture.Contains(unit))
                        {
                            next = true;
                            break;
                        }
                    }

                    if (next)
                        return;

                    tempTexture.AddRange(moved);
                    var curSize = GetOutSize(tempTexture);
                    if (curSize.X * curSize.Y >= maxSq)
                        return;
                    maxSq = curSize.X * curSize.Y;
                    selectedPos = (i, j);
                }
                
                if (placedUnits.Count != 0)
                {
                    if (currentSize.X > currentSize.Y)
                        for (int i = 0; i < currentSize.X + 1; i++)
                        {
                            for (int j = 0; j < currentSize.Y + 1; j++)
                            {
                                trySet(i, j);
                            }
                        }
                    else
                        for (int j = 0; j < currentSize.Y + 1; j++)
                        {
                            for (int i = 0; i < currentSize.X + 1; i++)
                            {
                                trySet(i, j);
                            }
                        }
                }
                tex.Location.X = selectedPos.X;
                tex.Location.Y = selectedPos.Y;

                placedUnits.AddRange(tex.GetUnits());
            }

            var size = (GetOutSize(placedUnits) * Texture.UnitSize).Yx;
            var rawAtlas = new byte[size.X, size.Y][];
            foreach (var tex in Textures)
            {
                tex.Load();
                tex.PlacePixels(rawAtlas);
            }

            return rawAtlas;
        }
        static Vector2i GetOutSize(List<Vector2i> outTexture)
        {
            var res = new Vector2i();
            foreach (var unit in outTexture)
            {
                if (res.X < unit.X)
                    res.X = unit.X;
                if (res.Y < unit.Y)
                    res.Y = unit.Y;
            }

            return res + Vector2i.One;
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
    }
    
    public static class UVParser
    {
        static Vector2 parseVec(string vec)
        {
            var sp = vec.Split('*');
            return new Vector2(float.Parse(sp[0]), float.Parse(sp[1]));
        }

        public static Vector2[] parseArray(string array)
        {
            var sp = array.Split(" ");
            return sp.Select(parseVec).ToArray();
        }
    }
}