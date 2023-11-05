#include "pch.h"
#include "Helper.h"

int shared Add(int a, int b)
{
	return a + b;
}

WorldPath shared ToWorldPath(Vector3i globalPosition)
{
	return WorldPath(globalPosition);
}

Vector3i shared InChunk(Vector3i position)
{
	return position.InChunk();
}
