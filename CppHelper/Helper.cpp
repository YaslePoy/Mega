#include "pch.h"
#include "Helper.h"
#include <iostream>

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

void shared GetAdjacent(Vector3i position, int* x, int size)
{


	for (int i = 0; i < size; i++)
	{
		std::cout << x[i] << " ";
		x[i] *= 2;
	}
}
