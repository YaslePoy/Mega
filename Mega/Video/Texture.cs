using Mega.Video.Shading;
using OpenTK.Mathematics;
using StbImageSharp;

namespace Mega.Video
{
    // A helper class, much like Shader, meant to simplify loading textures.
    public class Texture
    {
        public const int UnitSize = 16;
        public readonly int Handle;
        public int width, height;
        public byte[] pixels;
        public readonly string FilePath;
        public UVMap map;
        public Vector2i Size;
        public Vector2i Location;
        public readonly string Id;

        public Vector2i[] GetUnits()
        {
            var result = new Vector2i[Size.X * Size.Y];
            int offset = 0;
            for (int i = 0; i < Size.X; i++)
            {
                for (int j = 0; j < Size.Y; j++)
                {
                    result[offset++] = new Vector2i(i, j) + Location;
                }
            }

            return result;
        }

        public Texture(int glHandle)
        {
            Handle = glHandle;
        }

        public void Load()
        {
            ImageResult image;
            using (Stream stream = File.OpenRead(FilePath))
            {
                image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
            }

            pixels = image.Data;
            width = image.Width;
            height = image.Height;
        }

        public Texture(string path, UVMap map, string id)
        {
            FilePath = path;
            this.map = map;
            this.Id = id;
        }

        public void PlacePixels(byte[,][] rawImg)
        {
            var startPosition = Location * UnitSize;
            var chunked = pixels.Chunk(4).ToList();
            for (int h = 0, c = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++, c++)
                {
                    var coords = (new Vector2i(w, h) + startPosition).Yx;
                    rawImg[coords.X, coords.Y] = chunked[c];
                }
            }
        }

        public UVMap GetTransformed(Vector2i total)
        {
            return map.Transform(Location, Size, total);
        }
    }
}