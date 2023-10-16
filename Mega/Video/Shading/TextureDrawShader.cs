using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Video.Shading
{
    internal class TextureDrawShader : Shader
    {
        public Matrix4 View { set { base.SetMatrix4("view", value); } }
        public Matrix4 Projection { set { base.SetMatrix4("projection", value); } }
        public TextureDrawShader() : base("Shaders/shader.vert", "Shaders/shader.frag")
        {
        }
    }
}
