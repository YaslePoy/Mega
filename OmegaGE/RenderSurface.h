#pragma once
#include <iostream>

#include "VertexRaw.h"
#include "Vertex.h"


using namespace std;

struct RenderSurface
{
    Vector3 move;
    Vertex v1, v2, v3, v4;
    Vector3 normal;

    void CopyToArray(VertexRaw* vertices, uint32_t* indices, int offset)
    {
        int localOffset = offset * 4;
        const int startIndex = localOffset;
        vertices[localOffset++] = v1.ToRaw(normal, move);
        vertices[localOffset++] = v2.ToRaw(normal, move);
        vertices[localOffset++] = v3.ToRaw(normal, move);
        vertices[localOffset] = v4.ToRaw(normal, move);

        localOffset = offset * 6;
        indices[localOffset++] = startIndex + 0;
        indices[localOffset++] = startIndex + 1;
        indices[localOffset++] = startIndex + 2;
        indices[localOffset++] = startIndex + 2;
        indices[localOffset++] = startIndex + 3;
        indices[localOffset] = startIndex + 0;
    }

    void Show()
    {
        cout << "Move : ";
        cout << move.X << " " << move.Y << " " << move.Y << " ";
        cout << "Normal : ";
        cout << normal.X << " " << normal.Y << " " << normal.Y << " ";
    }
};
