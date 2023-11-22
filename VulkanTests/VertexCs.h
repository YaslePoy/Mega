#pragma once
#include "Vector2.h"
#include "Vector3.h"
#include "Vertex.h"

struct VertexCs
{
    Vector2 uv;
    Vector3 pos;
    Vertex ToCpp()
    {
        return {{pos.X, pos.X, pos.Z}, {0, 0, 0}, {uv.X, uv.Y}};
    }
};
