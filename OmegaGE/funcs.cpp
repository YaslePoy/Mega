#include "funcs.h"
#include "OmegaWindow.h"


int Add(int a, int b)
{
    return a + b + 3;
}

void InitWindow(uint32_t width, uint32_t height)
{
    viewport.width = width;
    viewport.height = height;
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

void SetViewSettings(Vector3 from, Vector3 to, Vector3 up, Vector3 sun)
{
    viewport.view.from = {from.X, from.Y, from.Z};
    viewport.view.to = {to.X, to.Y, to.Z};
    viewport.view.up = {up.X, up.Y, up.Z};
    viewport.view.sun = {sun.X, sun.Y, sun.Z};
}
