#include "pch.h"
#include "ChunkLocation.h"

ChunkLocation::ChunkLocation(Vector3i globalPosition)
{
	if (globalPosition.X >= 0)
		X = globalPosition.X % 32;
	else
		X = 32 + ((globalPosition.X + 1) % 32) - 1;
	if (globalPosition.Y >= 0)
		Z = globalPosition.Y % 32;
	else
		Z = 32 + ((globalPosition.Y + 1) % 32) - 1;
	Z = globalPosition.Z;
}

ChunkLocation::ChunkLocation(uint8_t x, uint8_t y, uint16_t z)
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
