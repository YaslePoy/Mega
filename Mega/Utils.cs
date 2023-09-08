using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
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

    }
}
