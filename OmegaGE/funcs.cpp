#include "funcs.h"
#include "OmegaWindow.h"


int Add(int a, int b)
{
    return a + b + 3;
}
 
void InitWindow(uint32_t width, uint32_t height, char* name)
{
    viewport.width = width;
    viewport.height = height;
    viewport.name = name;
}

void Start()
{
    viewport.Open();
}

void Close()
{
    viewport.Close();
}

int GetWindowCloseState()
{
    return glfwWindowShouldClose(viewport.window);
}

void PollWindowEvents()
{
    glfwPollEvents();
}
