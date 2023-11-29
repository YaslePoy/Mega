#pragma once

#define shared __declspec(dllexport)

extern "C" {
    int shared Add(int a, int b);
}
