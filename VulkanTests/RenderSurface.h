#pragma once
#include "Vertex.h"
#include "VertexCs.h"

class RenderSurface
{
public:
    VertexCs v1, v2, v3, v4;
    void CopyToArray(Vertex *vertices, int &offset)
    {
        vertices[offset++] = v1.ToCpp();
        vertices[offset++] = v1.ToCpp();
        vertices[offset++] = v1.ToCpp();
        vertices[offset++] = v1.ToCpp();
    }
};
