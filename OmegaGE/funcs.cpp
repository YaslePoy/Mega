﻿#include "funcs.h"
#include "OmegaWindow.h"


int Add(int a, int b)
{
    return a + b + 3;
}

void OpenWindow(uint32_t width, uint32_t height/*, char* name*/)
{
    // viewport.width = width;
    // viewport.height = height;
    // viewport.name = name;
    OmegaWindow vp(width, height, "Test win here");
    vp.Open();
    vp.mainLoop();
}

void Start()
{
    // viewport.mainLoop();
}
