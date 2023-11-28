#pragma once
#include "Vertex.h"
#include "VertexCs.h"

class RenderSurface
{
public:
    VertexCs v1, v2, v3, v4;
    Vector3 normal;
    void CopyToArray(Vertex *vertices, int &offset)
    {
        vertices[offset++] = v1.ToCpp(normal);
        vertices[offset++] = v1.ToCpp(normal);
        vertices[offset++] = v1.ToCpp(normal);
        vertices[offset++] = v1.ToCpp(normal);
    }
};