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

    }
}
