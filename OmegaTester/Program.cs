using System.Runtime.InteropServices;

namespace OmegaTester;

class Startup
{
    public static void Main(string[] args)
    {
        var s = OmegaGE.Add(1, 2); 
        Console.WriteLine(s);
    }
}

public static class OmegaGE
{
    [DllImport("OmegaGE.dll", CallingConvention = CallingConvention.StdCall)]
    public static extern int Add(int a, int b);
}