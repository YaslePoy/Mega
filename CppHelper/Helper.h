#include "WorldPath.h"
#include "AdjacentBlocks.h"
using namespace std;

extern "C" {
#define shared __declspec(dllexport)
	int shared Add(int a, int b);
	WorldPath shared ToWorldPath(Vector3i globalPosition);
	Vector3i shared InChunk(Vector3i position);
	void shared GetAdjacent(Vector3i position, int* x, int size);
}

