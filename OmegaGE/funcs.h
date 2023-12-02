#pragma once

#define lib __declspec(dllexport)

#include "OmegaWindow.h"

extern "C" {
int lib Add(int a, int b);
void lib InitWindow(uint32_t width, uint32_t height, char* name);
void lib Start();
void lib Close();
int lib GetWindowCloseState();
void lib PollWindowEvents();
}
