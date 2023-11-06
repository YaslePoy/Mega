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

AdjacentBlocks shared GetAdjacent(Vector3i position)
{
	return AdjacentBlocks(position);
}

void shared XFunc()
{
	return void shared();
}

void shared FillArray(int* x, int n)
{
	x = new int[n];
	for (int i = 0; i < n; i++)
	{
		x[i] = i * i;
	}
}
