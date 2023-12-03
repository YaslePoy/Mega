#include "pch.h"
#include "Helper.h"
#include <iostream>

int libOut Add(int a, int b)
{
    return a + b;
}

WorldPath libOut ToWorldPath(Vector3i globalPosition)
{
    return WorldPath(globalPosition);
}

Vector3i libOut InChunk(Vector3i position)
{
    return position.InChunk();
}

void libOut GetAdjacent(Vector3i position, int* x, int size)
{
    for (int i = 0; i < size; i++)
    {
        std::cout << x[i] << " ";
        x[i] *= 2;
    }
}
