#pragma once

struct Vector3
{
    float X, Y, Z;

    Vector3 operator+(Vector3& other)
    {
        return {X + other.X, Y + other.Y, Z + other.Z};
    }
};
