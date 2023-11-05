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
    if (Z >= 0)
    {
        ret.Z %= 32;
    }
    else
    {
        ret.Z = 32 + ((Z + 1) % 32) - 1;
    }
    return ret;
}
