#include "pch.h"
#include "Vector3i.h"


Vector3i::Vector3i(int x, int y, int z)
{
	X = x;
	Y = y;
	Z = z;
}

Vector3i::Vector3i()
{
	X = 0;
	Y = 0;
	Z = 0;
}

Vector3i Vector3i::InChunk()
{
	Vector3i ret = Vector3i(X, Y, Z);
    if (X >= 0)
    {
        ret.X %= 32;
    }
    else
    {
        ret.X = 32 + ((X + 1) % 32) - 1;
    }
    if (Y >= 0)
    {
        ret.Y %= 32;
    }
    else
    {
        ret.Y = 32 + ((Y + 1) % 32) - 1;
    }
    return ret;
}
