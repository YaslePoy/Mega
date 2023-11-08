using OpenTK.Mathematics;

namespace Mega.Video
{
    public class RenderSurface
    {
        //float[] rawPolygon;
        //float[] rawEdges;
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
            var t = new (Vector3, Vector2)[vertices.Length];
            for (var i = 0; i < vertices.Length; i++)
            {
                t[i] = (vertices[i], texture[i]);
            }
            _vts = t;
            TextureID = tx;
            this.Normal = normal;
        }

        public unsafe void GetRawPolygon(Span<float> span)
        {
            //if(rawPolygon != null) 
            //    return rawPolygon;
            //int offset = 0;
            //for (int i = 0; i < _vts.Length; i++)
            //{
            //    span[offset++] = _vts[i].Item1.X + move.X;
            //    span[offset++] = _vts[i].Item1.Y + move.Y;
            //    span[offset++] = _vts[i].Item1.Z + move.Z;
            //    span[offset++] = _vts[i].Item2.X;
            //    span[offset++] = _vts[i].Item2.Y;
            //    span[offset++] = Normal.X;
            //    span[offset++] = Normal.Y;
            //    span[offset++] = Normal.Z;
            //}
            System.Numerics.Vector3 fastMove = move.ToFastVector();
            fixed ((Vector3, Vector2)* v = _vts)
            {
                var current = v[0];
                var added = fastMove + new System.Numerics.Vector3(current.Item1.X, current.Item1.Y, current.Item1.Z);
                span[0] = added.X;
                span[1] = added.Y;
                span[2] = added.Z;
                span[3] = current.Item2.X;
                span[4] = current.Item2.Y;
                span[5] = Normal.X;
                span[6] = Normal.Y;
                span[7] = Normal.Z;

                current = v[1];
                span[8] = current.Item1.X + move.X;
                span[9] = current.Item1.Y + move.Y;
                span[10] = current.Item1.Z + move.Z;
                span[11] = current.Item2.X;
                span[12] = current.Item2.Y;
                span[13] = Normal.X;
                span[14] = Normal.Y;
                span[15] = Normal.Z;

                current = v[2];
                span[16] = current.Item1.X + move.X;
                span[17] = current.Item1.Y + move.Y;
                span[18] = current.Item1.Z + move.Z;
                span[19] = current.Item2.X;
                span[20] = current.Item2.Y;
                span[21] = Normal.X;
                span[22] = Normal.Y;
                span[23] = Normal.Z;

                current = v[3];
                span[24] = current.Item1.X + move.X;
                span[25] = current.Item1.Y + move.Y;
                span[26] = current.Item1.Z + move.Z;
                span[27] = current.Item2.X;
                span[28] = current.Item2.Y;
                span[29] = Normal.X;
                span[30] = Normal.Y;
                span[31] = Normal.Z;
            }

            //rawPolygon = ret;
            //return rawPolygon;
        }
        public float[] GetRawEdges()
        {
            //if (rawEdges != null)
            //    return rawEdges;
            var ret = new float[_vts.Length * 3];
            int offset = 0;
            for (int i = 0; i < _vts.Length; i++)
            {
                ret[offset++] = _vts[i].Item1.X + move.X;
                ret[offset++] = _vts[i].Item1.Y + move.Y;
                ret[offset++] = _vts[i].Item1.Z + move.Z;
            }
            //rawEdges = ret;
            //return rawEdges;
            return ret;
        }

    }
}
