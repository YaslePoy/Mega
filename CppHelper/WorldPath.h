#pragma once
#include "Vector3i.h"
#include "Vector2i.h"
class WorldPath
{
public:
	Vector3i InChunk;
	Vector2i Chunk;
	WorldPath(Vector3i inChunk, Vector2i chunk);
	WorldPath(Vector3i globalPosition);
};

