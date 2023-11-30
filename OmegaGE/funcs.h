#pragma once

#define shared __declspec(dllexport)
#include <string>

#include "OmegaWindow.h"

extern "C" {
int shared Add(int a, int b);
void shared OpenWindow(uint32_t width, uint32_t height, char* name);
void shared Start();
}
