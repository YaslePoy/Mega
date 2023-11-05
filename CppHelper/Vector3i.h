#pragma once
class Vector3i
{
public:
	int X, Y, Z;
	Vector3i(int x, int y, int z);
	Vector3i();
	Vector3i InChunk();
};

