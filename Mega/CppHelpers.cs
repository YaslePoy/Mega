using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Mega.Game;
using OpenTK.Mathematics;

namespace Mega
{
    public static class CppHelpers
    {
        [DllImport(@"C:\Users\Mimm\Projects\VisualStudioProjects\Mega\x64\Release\CppHelper.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int Add(int a, int b);

        [DllImport(@"C:\Users\Mimm\Projects\VisualStudioProjects\Mega\x64\Release\CppHelper.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern Vector3i InChunk(Vector3i position);

        [DllImport(@"C:\Users\Mimm\Projects\VisualStudioProjects\Mega\x64\Release\CppHelper.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern WorldPath ToWorldPath(Vector3i globalPosition);
    }
}
