#pragma once

class Vector2
{
public:
    float X, Y;

    Vector2 operator+(Vector2& other)
    {
        return {X + other.X, Y + other.Y};
    }
};
