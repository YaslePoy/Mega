#pragma once
#include "Vector2.h"
#include "Vector3.h"
#include "Vertex.h"

struct VertexCs
{
    Vector2 uv;
    Vector3 pos;
    Vertex ToCpp(Vector3 nor)
    {
        return {{pos.X, pos.X, pos.Z}, {nor.X, nor.Y, nor.Z}, {uv.X, uv.Y}};
    }
};
