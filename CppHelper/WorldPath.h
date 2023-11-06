#pragma once
#include "Vector3i.h"
#include "Vector2i.h"
#include "ChunkLocation.h"
class WorldPath
{
public:
	ChunkLocation InChunk;
	Vector2i Chunk;
	WorldPath(Vector3i inChunk, Vector2i chunk);
	WorldPath(Vector3i globalPosition);
};

