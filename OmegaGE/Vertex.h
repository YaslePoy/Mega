#pragma once
#include "Vector2.h"
#include "Vector3.h"
#include "VertexRaw.h"

struct Vertex
{
    Vector2 uv;
    Vector3 pos;
    VertexRaw ToRaw(Vector3 nor, Vector3 move)
    {
        return {{pos.X + move.X, pos.Y + move.Y, pos.Z + move.Z}, {nor.X, nor.Y, nor.Z}, {uv.X, uv.Y}};
    }
};
