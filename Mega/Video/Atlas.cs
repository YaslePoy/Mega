using System.Collections.Frozen;
using OpenTK.Mathematics;

namespace Mega.Video;

public class Atlas
{
    private List<Texture> _textures;
    public static Atlas Main;

    private FrozenDictionary<string, UVMap> maps;

    private Image data;

    public Image Image => data;

    public Atlas(List<Texture> textures)
    {
        _textures = textures;
    }

    public void Assemble()
    {
        var placedUnits = new List<Vector2i>();

        var sorted = _textures.OrderBy(i => i.Size.X * i.Size.Y).Reverse().ToList();

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

            bool outsidePlace(int x, int y)
            {
                tex.Location.X = x;
                tex.Location.Y = y;
                var moved = tex.GetUnits();
                selectedPos = (x, y);
                return moved.Any(i => placedUnits.Contains(i));
            }

            if (placedUnits.Count != 0)
            {
                if (placedUnits.Count != currentSize.X * currentSize.Y)
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
                else
                {
                    if (currentSize.X > currentSize.Y)
                        for (int i = 0; outsidePlace(0, i); i++)
                            ;
                    else
                        for (int i = 0; outsidePlace(i, 0); i++)
                            ;
                }
            }

            tex.Location.X = selectedPos.X;
            tex.Location.Y = selectedPos.Y;

            placedUnits.AddRange(tex.GetUnits());
        }

        var totalSize = GetOutSize(placedUnits);
        
        var preDict = new Dictionary<string, UVMap>();
        foreach (var texture in _textures)
        {
            preDict.Add(texture.Id, texture.GetTransformed(totalSize));
        }

        maps = preDict.ToFrozenDictionary();
        var size = (GetOutSize(placedUnits) * Texture.UnitSize).Yx;
        var rawAtlas = new byte[size.X, size.Y][];
        foreach (var tex in _textures)
        {
            tex.Load();
            tex.PlacePixels(rawAtlas);
        }

        var toSend = rawAtlas.Cast<byte[]>().ToList().ReplaceNulls([0, 0, 0, 255]).SumList();

        data = new Image { data = toSend, X = size.Y, Y = size.X };
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

    public Vector2[] this[string enity, int side] => maps[enity][side];
}

public struct Image
{
    public byte[] data;
    public int X, Y;
}