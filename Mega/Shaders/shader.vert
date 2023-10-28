#version 460 core

layout(location = 0) in vec3 aPosition;

layout(location = 1) in vec2 aTexCoord;

layout(location = 2) in vec3 nor;
out vec2 texCoord;
out vec3 normal;
out vec3 normalRot;

uniform mat4 view;
uniform mat4 projection;


void main(void)
{
    texCoord = aTexCoord;
    normal = nor;
    vec4 pre = vec4(nor, 0);
    pre = pre * view * projection;
    normalRot = pre.xyz;
    
    gl_Position = vec4(aPosition, 1.0) * view * projection;
}
