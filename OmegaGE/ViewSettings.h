#pragma once
#include <glm/vec3.hpp>

#include "Vector3.h"

struct ViewSettings
{
public:
    glm::vec3 from, to, up, sun;
};
