#pragma once


#include <stb_image.h>

#include "OmegaWindow.h"
#include "RenderSurface.h"
#define lib __declspec(dllexport)

extern "C" {
int lib Add(int a, int b);
void lib InitWindow(uint32_t width, uint32_t height, char* name);
void lib Start();
void lib Close();
int lib GetWindowCloseState();
void lib PollWindowEvents();
void lib SetMeshShaderData(RenderSurface* surfaces, uint32_t count);
void lib Draw();
void lib SetMainRenderTexture(stbi_uc* data, int x, int y);
void lib UpdateMainRenderTexture(stbi_uc* data, int x, int y);

void lib UpdateKeyboardState();
bool lib IsKeyPressed(int key);
bool lib IsKeyReleased(int key);
bool lib IsKeyDown(int key);
bool lib IsKeyUp(int key);
}
