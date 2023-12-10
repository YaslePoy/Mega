#pragma once
#include <iostream>
#include <map>
#include <vector>
#include <GLFW/glfw3.h>


#define Released 0;
#define Up 1;
#define Down 2;
#define Pressed 0;
using namespace std;

class KeyboardInput
{
public:
    std::map<int, int> keys;
    std::vector<int> released;

    int get_key(int key)
    {
        if (ranges::find(released, key) != released.end())
            return Released
        if (!keys.contains(key))
            return Up
        if (keys[key] == frame && keys[key] != -1)
            return Pressed
        if (keys[key] < frame)
            return Down
        return -1;
    }

    void press(int key)
    {
        keys[key] = frame;
    }

    void release(int key)
    {
        keys[key] = -1;
        keys.erase(key);
        released.push_back(key);
    }

    void next_frame()
    {
        frame++;
        released.clear();
    }

private:
    int frame;
};
