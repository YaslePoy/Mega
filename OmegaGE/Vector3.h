#pragma once

class Vector3
{
public:
    float X, Y, Z;
    Vector3 operator+( Vector3 &other )
    {
        return {X + other.X, Y + other.Y, Z + other.Z};
    }
};
