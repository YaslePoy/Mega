#include "pch.h"
#include "ChunkLocation.h"

ChunkLocation::ChunkLocation(Vector3i globalPosition)
{
	if (globalPosition.X >= 0)
		X = globalPosition.X % 32;
	else
		X = 32 + ((globalPosition.X + 1) % 32) - 1;
	if (globalPosition.Z >= 0)
		Z = globalPosition.Z % 32;
	else
		Z = 32 + ((globalPosition.Z + 1) % 32) - 1;
	Y = globalPosition.Y;
}

ChunkLocation::ChunkLocation(uint8_t x, uint16_t y, uint8_t z)
{
	X = x;
	Y = y;
	Z = z;
}

ChunkLocation::ChunkLocation()
{
	X = 0;
	Y = 0;
	Z = 0;
}
