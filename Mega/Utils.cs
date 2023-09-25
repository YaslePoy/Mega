using Mega.Game;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mega
{
    public static class Utils
    {
        public static T Get<T>(this T[,,] array, Vector3i index) => array[index.X, index.Y, index.Z];
        public static void Set<T>(this T[,,] array, Vector3i index, T value) => array[index.X, index.Y, index.Z] = value;
        public static bool IsNegative(this Vector3i vec) => vec.X < 0 || vec.Y < 0 || vec.Z < 0;
        public static bool IsLessThan(this Vector3i vec, int n) => vec.X < n && vec.Y < n && vec.Z < n;
        public static bool IsInRange(this Vector3i vec, int s, int e) => vec.X >= s && vec.X < e && vec.Y >= s && vec.Y < e && vec.Z >= s && vec.Z < e;
        public static Vector3i Round(this Vector3 vec) => new Vector3i((int)vec.X, (int)vec.Y, (int)vec.Z);
        public static List<T> SumList<T>(this IEnumerable<IEnumerable<T>> lists)
        {
            var result = new List<T>();
            foreach (var list in lists)
                result.AddRange(list);
            return result;
        }

        public const float G = 20f;
        public static (Vector2i chunk, Vector3i block) ToWorldPath(this Vector3i vec) => (new Vector2i(vec.X / Chunk.Size.X - vec.X > 0 ? 0  : 1, vec.Z / Chunk.Size.Z - vec.Z > 0 ? 0 : 1), InChunk(vec));
        public static Vector3i InChunk(this Vector3i globalPosition) => new Vector3i(globalPosition.X % Chunk.Size.X, globalPosition.Y, globalPosition.Z % Chunk.Size.Z);
        public static bool IsInChunk(this Vector3i position) => position.X >= 0 && position.X < Chunk.Size.X &&
                                                                position.Y >= 0 && position.Y < Chunk.Size.Y &&
                                                                position.Z >= 0 && position.Z < Chunk.Size.Z;
    }
}
