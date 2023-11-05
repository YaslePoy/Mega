using Mega.Game;
using OpenTK.Mathematics;
using Sys = System.Numerics;

namespace Mega
{
    public static class Utils
    {
        public static T Get<T>(this T[,,] array, ChunkLocation index) => array[index.X, index.Y, index.Z];
        public static void Set<T>(this T[,,] array, ChunkLocation index, T value) => array[index.X, index.Y, index.Z] = value;
        public static bool IsNegative(this Vector3i vec) => vec.X < 0 || vec.Y < 0 || vec.Z < 0;
        public static bool IsLessThan(this Vector3i vec, int n) => vec.X < n && vec.Y < n && vec.Z < n;
        public static bool IsInRange(this Vector3i vec, int s, int e) => vec.X >= s && vec.X < e && vec.Y >= s && vec.Y < e && vec.Z >= s && vec.Z < e;
        public static bool IsInRange(this Vector3 vec, Vector3 s, Vector3 e) => vec.X >= s.X && vec.X <= e.X && vec.Y >= s.Y && vec.Y <= e.Y && vec.Z >= s.Z && vec.Z <= e.Z;

        public static Vector3i Round(this Vector3 vec) => new Vector3i((int)vec.X, (int)vec.Y, (int)vec.Z);
        public static Vector3 Round(this Vector3 vec, int digit = 0) => new Vector3(MathF.Round(vec.X, digit), MathF.Round(vec.Y, digit), MathF.Round(vec.Z, digit));

        public static T[] SumList<T>(this IEnumerable<IEnumerable<T>> lists)
        {
            var len = lists.Sum(i => i.Count());
            var temp = lists.ToArray();
            var ret = new T[len];
            int offset = 0;
            for (int i = 0; i < lists.Count(); i++)
            {
                temp[i].ToArray().CopyTo(ret, offset);
                offset += temp[i].Count();
            }
            return ret;
        }
        public const float G = 30f;
        public static (Vector2i chunk, Vector3i block) ToWorldPath(this Vector3i vec)
        {
            Vector2i cn = new Vector2i(0, 0);
            if (vec.X > 0)
            {
                cn.X = (int)Math.Floor((double)vec.X / Chunk.Size.X);
            }
            else
            {
                cn.X = -(int)Math.Abs(Math.Floor(((double)vec.X) / Chunk.Size.X));
            }
            if (vec.Z > 0)
            {
                cn.Y = (int)Math.Floor((double)vec.Z / Chunk.Size.Z);
            }
            else
            {
                cn.Y = -(int)Math.Abs(Math.Floor(((double)vec.Z) / Chunk.Size.Z));
            }
            return (cn, vec.InChunk());
        }
        public static Vector3i InChunk(this Vector3i globalPosition)
        {
            Vector3i ret = new Vector3i(0, globalPosition.Y, 0);
            if (globalPosition.X >= 0)
            {
                ret.X = globalPosition.X % Chunk.Size.X;
            }
            else
            {
                ret.X = Chunk.Size.X + ((globalPosition.X + 1) % Chunk.Size.X) - 1;
            }
            if (globalPosition.Z >= 0)
            {
                ret.Z = globalPosition.Z % Chunk.Size.Z;
            }
            else
            {
                ret.Z = Chunk.Size.Z + ((globalPosition.Z + 1) % Chunk.Size.Z) - 1;
            }
            return ret;
        }

        public static bool IsInChunk(this Vector3i position) => position.X >= 0 && position.X < Chunk.Size.X &&
                                                                position.Y >= 0 && position.Y < Chunk.Size.Y &&
                                                                position.Z >= 0 && position.Z < Chunk.Size.Z;
        public static Sys.Vector3 ToSys(this Vector3 otk) => new Sys.Vector3(otk.X, otk.Y, otk.Z);

        public static float Sum(this Vector3 vector) => vector.X + vector.Y + vector.Z;
        public static float Sum(this Vector4 vector) => vector.X + vector.Y + vector.Z + vector.W;
    }
}
