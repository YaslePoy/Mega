#pragma once
#include "VertexRaw.h"
#include "Vertex.h"

class RenderSurface
{
public:
    Vertex v1, v2, v3, v4;
    Vector3 normal;
    void CopyToArray(VertexRaw *vertices, int &offset)
    {
        vertices[offset++] = v1.ToRaw(normal);
        vertices[offset++] = v1.ToRaw(normal);
        vertices[offset++] = v1.ToRaw(normal);
        vertices[offset++] = v1.ToRaw(normal);
    }
};