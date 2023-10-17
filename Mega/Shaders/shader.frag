#version 460

out vec4 outputColor;

in vec2 texCoord;

uniform sampler2D texture0;

void main()
{

    
    vec4 cl = texture(texture0, texCoord);
    if(cl.w < 0.5)
        discard;
    outputColor = cl;
}