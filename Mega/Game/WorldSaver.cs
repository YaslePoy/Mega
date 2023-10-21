﻿using OpenTK.Mathematics;
using System.Collections;

namespace Mega.Game
{
    public static class WorldSaver
    {
        public readonly static string SavePath = "gw";
        public static void SaveWorld(UnitedChunk area)
        {
            foreach (Chunk chunk in area)
            {
                SmartBitArray sba = new SmartBitArray();
                byte streak = 0;
                BitArray ba = new BitArray(5);
                bool start = true;
                Block lastBlock = null;
                void appdendStreak()
                {
                    sba.Append(new BitArray(4));
                    sba.Append(new BinaryInt(streak, 8));
                }
                var stringed = chunk.data.Cast<Block>().Select(i => i is null ? new Block(OpenTK.Mathematics.Vector3i.One, -1, false) : i).ToArray();
                var count = stringed.Length;
                for (int i = 0; i < count;)
                {
                    var block = stringed[i++];
                    while (i < count && stringed[i].IDCode == block.IDCode && streak != byte.MaxValue)
                    {
                        streak++;
                        i++;
                    }
                    if (streak == 0)
                    {
                        sba.Append(block.GetSaveData());
                    }
                    else if (streak > 3)
                    {
                        sba.Append(new BitArray(4));
                        sba.Append(block.GetSaveData());
                        sba.Append(new BinaryInt(streak, 8));
                    }
                    else
                    {
                        Enumerable.Repeat(block.GetSaveData(), streak).ToList().ForEach(i => sba.Append(i));
                    }
                    streak = 0;
                }
                sba.Bake();
                var data = sba.ToByteArray();
                File.WriteAllBytes($"gw/{chunk.Location.X}x{chunk.Location.Y}.cd", data);
            }
        }

        public static Chunk LoadChunk(Vector2i position)
        {
            Chunk chunk = new Chunk();
            var file = Path.Combine(SavePath, $"{position.X}x{position.Y}.cd");
            var data = File.ReadAllBytes(file);
            int curBlock = 0;
            int lastBlock = Chunk.Size.X * Chunk.Size.Y * Chunk.Size.Z;
            SmartBitArray save = new SmartBitArray(new BitArray(data));
            Vector3i pos = Vector3i.Zero;
            void nextPos()
            {
                pos.X++;
                if (pos.X == Chunk.Size.X)
                {
                    pos.X = 0;
                    pos.Y++;
                }
                if (pos.Y == Chunk.Size.Y)
                {
                    pos.Y = 0;
                    pos.Z++;
                }
            }

            while (curBlock != lastBlock)
            {

                    int curID = save.GetInt(4) - 2;
                    if (curID == -2)
                    {
                        int block = save.GetInt(4);
                        int count = save.GetInt(8) + 1;
                        for (int i = 0; i < count; i++)
                        {
                            if (block != 1)
                                chunk.SetBlock(pos, block - 2);
                            curBlock++;
                            nextPos();
                        }
                    }
                    else
                    {
                        if (curID != -1)
                            chunk.SetBlock(pos, curID);
                        nextPos();
                        curBlock++;
                    }
            }
            return chunk;
        }
    }
    public struct SmartBitArray
    {
        int index;
        BitArray array;
        public List<BitArray> splited;
        public SmartBitArray(BitArray ba)
        {
            array = ba;
        }
        public SmartBitArray()
        {
            splited = new List<BitArray>();
        }
        public int GetInt(int len)
        {
            int ret = 0;
            for (int i = 0; i < len; i++)
            {
                if (array.Count < index)
                    break;
                if (array[index++])
                    ret |= 1 << i;
            }
            return ret;
        }
        public byte[] ToByteArray()
        {
            byte[] ret = new byte[(int)Math.Ceiling(array.Count / 8f)];
            array.CopyTo(ret, 0);
            return ret;
        }
        public void Append(BitArray i)
        {
            splited.Add(i);
        }
        public void Bake()
        {
            array = new BitArray(splited.Select(i => i.Count).Sum());
            int i = 0;
            foreach (var part in splited)
            {
                foreach (bool bit in part)
                    array[i++] = bit;
            }
        }
    }
    public struct BinaryInt
    {
        public BitArray data;
        public int Lenght => data.Count;
        public bool this[int index] => data[index];
        public BinaryInt(int number, int bits)
        {
            data = new BitArray(bits);
            for (int i = 0; i < bits; i++)
            {
                data[i] = number % 2 == 0 ? false : true;
                number /= 2;
            }
        }
        public static BinaryInt operator +(BinaryInt b1, BinaryInt b2)
        {
            var next = new BitArray(b1.Lenght + b2.Lenght);
            for (int i = 0; i < next.Count; i++)
            {
                next[i] = i < b1.Lenght ? b1[i] : b2[i - b1.Lenght];
            }
            return new BinaryInt() { data = next };
        }
        public static implicit operator BitArray(BinaryInt i) { return i.data; }

    }
}
