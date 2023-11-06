#include "pch.h"
#include "WorldPath.h"
#include "Vector2i.h"
#include "Vector3i.h"
#include <math.h>

WorldPath::WorldPath(Vector3i inChunk, Vector2i chunk)
{
	InChunk = ChunkLocation(inChunk);
	Chunk = chunk;
}

WorldPath::WorldPath(Vector3i globalPosition)
{
	InChunk = 
        ChunkLocation(globalPosition);
	Chunk = Vector2i();
    if (globalPosition.X > 0)
    {
        Chunk.X = (int)floor((double)globalPosition.X / 32);
    }
    else
    {
        Chunk.X = -(int)fabs(floor(((double)globalPosition.X) / 32));
    }
    if (globalPosition.Z > 0)
    {
        Chunk.Y = (int)floor((double)globalPosition.Z / 32);
    }
    else
    {
        Chunk.Y = -(int)fabs(floor(((double)globalPosition.Z) / 32));
    }
}
