#pragma once
#include <map>
#include <vector>

enum class KeyState
{
    Pressed,
    Released,
    Down,
    Up
};

class KeyboardInput
{
public:
    std::map<int, int> keys;
    std::vector<int> released;
    KeyState get_key(int key)
    {
        if(std::find(released.begin(), released.end(), key) != released.end())
            return KeyState::Released;
        if (!keys.contains(key))
            return KeyState::Up;
        if (keys[key] == frame)
            return KeyState::Pressed;
        if (keys[key] < frame)
            return KeyState::Down;
        
    }
    void next_frame()
    {
        frame++;
        released.clear();
    }
private:
    int frame;
};
