using System.Runtime.InteropServices;

namespace OmegaTester;

class Startup
{
    public static void Main(string[] args)
    {
        var win = OmegaGE.OpenWindow(900, 400, "Test");

        OmegaGE.Start(win); 
        Console.ReadKey();
    }
}

public static class OmegaGE
{
    [DllImport("OmegaGE.dll", CallingConvention = CallingConvention.StdCall)]
    public static extern int Add(int a, int b);

    [DllImport("OmegaGE.dll", CallingConvention = CallingConvention.StdCall)]
    public static extern nint OpenWindow(uint width, uint height, string name);

    [DllImport("OmegaGE.dll", CallingConvention = CallingConvention.StdCall)]
    public static extern void Start(nint window);
}