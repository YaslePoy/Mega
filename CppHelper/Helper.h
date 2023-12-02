#include "WorldPath.h"
#include "AdjacentBlocks.h"
using namespace std;

extern "C" {
#define lib __declspec(dllexport)
	int lib Add(int a, int b);
	WorldPath lib ToWorldPath(Vector3i globalPosition);
	Vector3i lib InChunk(Vector3i position);
	void lib GetAdjacent(Vector3i position, int* x, int size);
}

