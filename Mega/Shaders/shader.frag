#version 460

out vec4 outputColor;

in vec2 texCoord;
in vec3 normal;
in vec3 normalRot;

uniform sampler2D texture0;


void main()
{
    vec4 cl = texture(texture0, texCoord);
    if(cl.w < 0.5)
        discard;
    vec3 sun = normalize(vec3(1, -2, 2));
    vec3 mul = sun * normal;
    float x = -(mul.x + mul.y + mul.z);
    if(x < 0)
    x = 0;
    x = (x + 1) / 2;
    cl *= x;
    outputColor = vec4(normalRot, 1);
}