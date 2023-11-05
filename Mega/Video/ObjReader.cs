using System.Globalization;

namespace Mega.Video
{
    internal static class ObjReader
    {
        public static void ReadFile(string path, out uint[] indices, out float[] vxs)
        {
            var text = File.ReadAllText(path);
            int readyPoly = 0;
            vxs = null;
            indices = null;
            List<float[]> vertexes = new List<float[]>();
            List<float[]> normals = new List<float[]>();
            List<float[]> texture = new List<float[]>();
            List<uint> ids = new List<uint>();

            List<int[]> pairs = new List<int[]>();

            float[] getFinalVertex(string id)
            {
                var p = id.Split("/").Select(i => int.Parse(i) - 1).ToArray();
                var r = new float[5];
                vertexes[p[0]].CopyTo(r, 0);
                texture[p[1]].CopyTo(r, 3);
                return r;
            }

            foreach (var line in text.Split("\n"))
            {
                var parts = line.Split(' ');
                bool br = false;
                float[] arr;
                switch (parts[0])
                {
                    case "v":
                        arr = ParceArray(parts);
                        vertexes.Add(arr);
                        break;
                    case "vt":
                        arr = ParceArray(parts);
                        texture.Add(arr);
                        break;
                    case "f":
                        br = true;
                        break;
                }
                if (br)
                    break;
            }
            var polsText = text.Substring(text.IndexOf('f'));
            polsText = polsText.Replace("f ", "");
            polsText = polsText.Replace("\n", " ");
            var polygonsText = polsText.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var unique = polygonsText.Distinct().ToList();
            var final = unique.Select(i => getFinalVertex(i)).ToList();
            vxs = SumFloatArray(final.ToList());
            foreach (var p in polygonsText)
            {
                var position = (uint)unique.IndexOf(p);
                ids.Add(position);
            }
            indices = ids.ToArray();

        }
        public static float[] ParceArray(string[] nums)
        {
            var ret = new float[nums.Length - 1];
            for (int i = 1; i < nums.Length; i++)
            {
                ret[i - 1] = float.Parse(nums[i], CultureInfo.InvariantCulture);
            }
            return ret;
        }

        public static float[] SumFloatArray(List<float[]> arr)
        {
            var ret = new float[arr.Sum(i => i.Length)];
            var offset = 0;
            foreach (var item in arr)
            {
                item.CopyTo(ret, offset);
                offset += item.Length;
            }
            return ret;
        }
    }
}
