using Mega.Game;
using Mega.Game.Blocks;
using OpenTK.Mathematics;

namespace Mega.Video.Shading
{
    internal class CursorShader : Shader
    {
        private float[] _vertices;
        private uint[] _indices;

        
        public override void Run()
        {
            if (OmegaEngine.world.Player.SelectedBlock.HasValue)
            {
                var sel = OmegaEngine.world.Area.GetBlock(OmegaEngine.world.Player.SelectedBlock.Value);

                UpdateEdges(sel, OmegaEngine.world);
                
            }
        }
        void UpdateEdges(Block block, World w)
        {
            if (block is null)
            {
                _indices = new uint[0];
                _vertices = new float[0];
                return;
            }
            RenderSurface[] sides;
            //if (block.view is null)
            //    sides = block.GetDrawingMesh(w.Area);
            //else
            if(block.view is null)
                return;
            sides = block.view.Where(i => Vector3.Dot(i.Normal, w.Player.View) < 0).ToArray();
            uint offset = 0;
            _vertices = new float[sides.Length * 12];
            _indices = new uint[sides.Length * 4];
            uint[] adding;
            for (int i = 0; i < sides.Length; i++)
            {
                var side = sides[i];

                var v = side.GetRawEdges();
                v.CopyTo(_vertices, 4 * 3 * i);
                adding = new uint[] { offset, 1 + offset, 2 + offset, 3 + offset };
                adding.CopyTo(_indices, offset);

                offset += 4;
                //_indices[side.TextureID].AddRange(adding);
            }
        }
    }
}
