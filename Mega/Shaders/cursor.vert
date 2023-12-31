#version 460 core

layout(location = 0) in vec3 aPosition;

layout(location = 1) in vec2 aTexCoord;

uniform mat4 view;
uniform mat4 projection;

void main(void)
{
    vec4 pre = vec4(aPosition, 1.0) * view * projection;
    pre.z -= 0.01;
    gl_Position = pre;
}
