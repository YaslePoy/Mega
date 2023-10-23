#version 460

out vec4 outputColor;

in vec2 texCoord;
in vec3 normal;

uniform sampler2D texture0;
uniform vec3 sun;

void main()
{
    vec4 cl = texture(texture0, texCoord);
    if(cl.w < 0.5)
        discard;
    cl *= 0.5;
    outputColor = cl;
}