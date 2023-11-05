﻿using OpenTK.Mathematics;

namespace Mega.Game
{
    public struct ChunkLocation
    {
        public byte X, Z;
        public ushort Y;
        public static implicit operator ChunkLocation(Vector3i from)
        {
            return new ChunkLocation() { X = (byte)from.X, Y = (ushort)from.Y, Z = (byte)from.Z };
        }
        public static implicit operator Vector3i(ChunkLocation from)
        {
            return new Vector3i() { X = from.X, Y = from.Y, Z = from.Z };
        }
    }
}
