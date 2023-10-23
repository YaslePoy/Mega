#version 460

out vec4 outputColor;

in vec2 texCoord;
in vec3 normal;

uniform sampler2D texture0;

void main()
{
    vec4 cl = texture(texture0, texCoord);
    if(cl.w < 0.5)
        discard;
    vec3 sun = normalize(vec3(1, -2, 2));
//    vec3 sun = normalize(vec3(0, -1, 0));
    vec3 mul = sun * normal;
    float x = mul.x + mul.y + mul.z;
    cl *= -x;
    outputColor = cl;
}