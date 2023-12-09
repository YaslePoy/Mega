#pragma once
#include <iostream>
#include <map>
#include <vector>
#include <GLFW/glfw3.h>

using namespace std;

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
        if (std::find(released.begin(), released.end(), key) != released.end())
            return KeyState::Released;
        if (!keys.contains(key))
            return KeyState::Up;
        if (keys[key] == frame)
            return KeyState::Pressed;
        if (keys[key] < frame)
            return KeyState::Down;
    }

    void press(int key)
    {
        keys[key] = frame;
        cout << key << " pressed" << endl;
        int a = GLFW_KEY_0
    }

    void release(int key)
    {
        keys.erase(key);
        released.push_back(key);
        cout << key << " released" << endl;
    }

    void next_frame()
    {
        frame++;
        released.clear();
    }

private:
    int frame;
};
