#include "OmegaWindow.h"

#include "funcs.h"

OmegaWindow::OmegaWindow(uint32_t width, uint32_t height, std::string name)
{
    this->height = height;
    this->width = width;
    this->name = const_cast<char*>(name.c_str());
}

void OmegaWindow::Open()
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
