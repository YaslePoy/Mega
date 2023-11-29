#pragma once
#include <cstdint>
#include <string>
#include <GLFW/glfw3.h>

class OmegaWindow
{
public:
    GLFWwindow* window;
    bool framebufferResized;
    OmegaWindow(uint32_t width, uint32_t height, char* name);
    static void framebufferResizeCallback(GLFWwindow* window, int width, int height);
    void mainLoop();
    
};
