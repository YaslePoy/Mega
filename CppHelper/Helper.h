#include "WorldPath.h"
#include "AdjacentBlocks.h"
using namespace std;

extern "C" {
#define libOut __declspec(dllexport)
	int libOut Add(int a, int b);
	WorldPath libOut ToWorldPath(Vector3i globalPosition);
	Vector3i libOut InChunk(Vector3i position);
	void libOut GetAdjacent(Vector3i position, int* x, int size);
}

