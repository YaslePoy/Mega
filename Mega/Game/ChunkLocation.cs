using OpenTK.Mathematics;

namespace Mega.Game
{
    public struct ChunkLocation
    {
        public byte X, Z;
        public ushort Y;

        public ChunkLocation(byte x, ushort y, byte z)
        {
            X = x; Y = y; Z = z;
        }

        public static implicit operator ChunkLocation(Vector3i from)
        {
            return new ChunkLocation() { X = (byte)from.X, Y = (ushort)from.Y, Z = (byte)from.Z };
        }
        public static implicit operator Vector3i(ChunkLocation from)
        {
            return new Vector3i() { X = from.X, Y = from.Y, Z = from.Z };
        }

        public static bool operator ==(ChunkLocation lhs, ChunkLocation rhs)
        {
            return lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z;
        }

        public static bool operator !=(ChunkLocation lhs, ChunkLocation rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            return $"<{X}; {Y}; {Z}>";
        }
    }
}
