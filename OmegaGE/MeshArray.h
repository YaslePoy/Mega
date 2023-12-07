#pragma once
#include "VertexRaw.h"

struct MeshArray
{
    VertexRaw *vertices;
    uint32_t vertices_count;
    uint32_t *indices;
    uint32_t indices_count;
};
