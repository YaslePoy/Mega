using Mega.Generation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Game
{
    public static class WorldSaver
    {
        public readonly static string Path = "gw";
        public static void SaveWorld(UnitedChunk area)
        {
            foreach (Chunk chunk in area)
            {
                BitArray ba = new BitArray(5);
                bool start = true;
                Block lastBlock = null;
                foreach (var block in chunk)
                {
                    if (!start)
                    {
                        if (lastBlock == block)
                        {

                        }
                    }
                }
            }
        }
    }
    public struct SmartBitArray
    {
        int index;
        BitArray array;
        public SmartBitArray(BitArray ba)
        {
            array = ba;
        }
        public int GetInt(int len)
        {
            int ret = 0;
            for (int i = 0;i < len; i++)
            {
                if (array.Count < index)
                    break;
                if (array[index++])
                    ret |= 1 << i;
            }
            return ret;
        }
    }
}
