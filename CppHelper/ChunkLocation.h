#include <cstdint>
#include "Vector3i.h"

#pragma once
class ChunkLocation
{
public:
	uint8_t X, Y;
	uint16_t Z;
	ChunkLocation(Vector3i position);
	ChunkLocation(uint8_t x, uint8_t y, uint16_t z);
	ChunkLocation();
};

