using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Video
{
    public class RenderSurface
    {
        float[] rawPolygon;
        float[] rawEdges;
        Vector3 move;
        (Vector3, Vector2)[] _vts;
        public readonly Vector3 Normal;
        public int TextureID;

        public RenderSurface((Vector3, Vector2)[] vertexes, Vector3 move)
        {
            _vts = vertexes;
            this.move = move;
        }

        public RenderSurface((Vector3, Vector2)[] vertexes)
        {
            _vts = vertexes;

        }

        public RenderSurface(Vector3[] vertices, Vector2[] texture, Vector3 move, Vector3 normal, int tx)
        {
            this.move = move;
            var t = new List<(Vector3, Vector2)>();
            for (var i = 0; i < vertices.Length; i++)
            {
                t.Add((vertices[i], texture[i]));
            }
            _vts = t.ToArray();
            TextureID = tx;
            this.Normal = normal;
        }

        public float[] GetRawPolygon()
        {
            if(rawPolygon != null) 
                return rawPolygon;
            var ret = new float[_vts.Length * (2 + 3 + 3)];
            int offset = 0;
            for (int i = 0; i < _vts.Length; i++)
            {

                ret[offset++] = _vts[i].Item1.X + move.X;
                ret[offset++] = _vts[i].Item1.Y + move.Y;
                ret[offset++] = _vts[i].Item1.Z + move.Z;
                ret[offset++] = _vts[i].Item2.X;
                ret[offset++] = _vts[i].Item2.Y;
                ret[offset++] = Normal.X;
                ret[offset++] = Normal.Y;
                ret[offset++] = Normal.Z;
            }
            rawPolygon = ret;
            return rawPolygon;
        }
        public float[] GetRawEdges()
        {
            if (rawEdges != null)
                return rawEdges;
            var ret = new float[_vts.Length * 3];
            int offset = 0;
            for (int i = 0; i < _vts.Length; i++)
            {
                ret[offset++] = _vts[i].Item1.X + move.X;
                ret[offset++] = _vts[i].Item1.Y + move.Y;
                ret[offset++] = _vts[i].Item1.Z + move.Z;
            }
            rawEdges = ret;
            return rawEdges;
        }

    }
}
