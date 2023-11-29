#include "funcs.h"

#include "OmegaWindow.h"


int Add(int a, int b)
{
    return  a + b + 3;
}

OmegaWindow OpenWindow(uint32_t width, uint32_t height, char* name)
{
    OmegaWindow win(width, height, name);
    return win;
}
// void Start(OmegaWindow win)
// {
//     win.mainLoop();
// }
