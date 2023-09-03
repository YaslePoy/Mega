using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Game
{
    internal class RenderSurface
    {
        Vector3 move;
        (Vector3, Vector2)[] _vts;


        public RenderSurface((Vector3, Vector2)[] vertexes, Vector3 move)
        {
            _vts = vertexes;
            this.move = move;
        }

        public RenderSurface((Vector3, Vector2)[] vertexes)
        {
            _vts = vertexes;

        }

        public RenderSurface(Vector3[] vertices, Vector2[] texture, Vector3 move)
        {
            this.move = move;
            var t = new List<(Vector3, Vector2)>();
            for (var i = 0; i < vertices.Length; i++)
            {
                t.Add((vertices[i], texture[i]));
            }
            _vts = t.ToArray();
        }

        public float[] GetRaw()
        {
            var ret = new float[_vts.Length * 5];
            int offset = 0;
            for (int i = 0; i < _vts.Length; i++)
            {
                
                ret[offset + 0] = _vts[i].Item1.X + move.X;
                ret[offset + 1] = _vts[i].Item1.Y + move.Y;
                ret[offset + 2] = _vts[i].Item1.Z + move.Z;
                ret[offset + 3] = _vts[i].Item2.X;
                ret[offset + 4] = _vts[i].Item2.Y;
                offset += 5;
            }
            return ret;
        }
    }
}
