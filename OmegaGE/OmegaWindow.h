#pragma once
#include <cstdint>
#include <cvt/wstring>
#include <GLFW/glfw3.h>


class OmegaWindow
{
public:
    uint32_t width, height;
    char* name;
    OmegaWindow(uint32_t width, uint32_t height, std::string name);
    void Open();
    void mainLoop() const;

private:
    static void framebufferResizeCallback(GLFWwindow* window, int width, int height);
    GLFWwindow* window;
    bool framebufferResized;
};

// inline OmegaWindow viewport{1024, 1024, "TestWindow"};
