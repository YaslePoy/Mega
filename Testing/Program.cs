﻿using Mega;
using Mega.Game;
using Mega.Video;
using OpenTK.Mathematics;
using System.Collections;

namespace Testing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //BinaryInt i = new BinaryInt(13, 4);
            //BinaryInt j = new BinaryInt(5, 3);
            //var x = i + j;
            Vector3i test = new Vector3i(-32, 5, -32);
            var ic = test.ToWorldPath();
        }
    }
}
