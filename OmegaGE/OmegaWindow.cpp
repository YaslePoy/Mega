#include "OmegaWindow.h"

OmegaWindow::OmegaWindow(uint32_t width, uint32_t height, char* name)
{
    glfwInit();

    glfwWindowHint(GLFW_CLIENT_API, GLFW_NO_API);

    window = glfwCreateWindow(width, height, name, nullptr, nullptr);
    glfwSetWindowUserPointer(window, this);
    glfwSetFramebufferSizeCallback(window, framebufferResizeCallback);
}

void OmegaWindow::framebufferResizeCallback(GLFWwindow* window, int width, int height)
{
    auto app = reinterpret_cast<OmegaWindow*>(glfwGetWindowUserPointer(window));
    app->framebufferResized = true;
}

void OmegaWindow::mainLoop() const
{
    while (!glfwWindowShouldClose(window))
    {
        glfwPollEvents();
    }
}
