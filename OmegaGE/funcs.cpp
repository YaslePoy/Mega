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

void SetMeshShaderData(RenderSurface* surfaces, uint32_t count)
{
    viewport.SetMainMesh(surfaces, count);
}

void Draw()
{
    viewport.drawFrame();
}

void SetMainRenderTexture(stbi_uc* data, int x, int y)
{
    viewport.mainTexture = {data, x, y};
}

void UpdateKeyboardState()
{
    viewport.keyboard.next_frame();
}

bool IsKeyPressed(int key)
{
    return viewport.keyboard.get_key(key) == Pressed
}

bool IsKeyReleased(int key)
{
    return viewport.keyboard.get_key(key) == Released
}

bool IsKeyDown(int key)
{
    return viewport.keyboard.get_key(key) == Down
}

bool IsKeyUp(int key)
{
    return viewport.keyboard.get_key(key) == Up
}

void UpdateMainRenderTexture(stbi_uc* data, int x, int y)
{
    viewport.mainTexture = {data, x, y};
    viewport.UpdateMainTexture();
}
