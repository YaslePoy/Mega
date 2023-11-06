#include <cstdint>
#include "Vector3i.h"

#pragma once
class ChunkLocation
{
public:
	uint8_t X, Z;
	uint16_t Y;
	ChunkLocation(Vector3i position);
	ChunkLocation(uint8_t x, uint16_t y, uint8_t z);
	ChunkLocation();
};

