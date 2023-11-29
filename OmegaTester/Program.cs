using System.Runtime.InteropServices;

namespace OmegaTester;

class Startup
{
    public static void Main(string[] args)
    {
        OmegaGE.OpenWindow(900, 400, "Test");
        // OmegaGE.Start(win);
        Console.ReadKey();
    }
}

public static class OmegaGE
{
    private const string Library = "OmegaGE.dll";
    [DllImport(Library, CallingConvention = CallingConvention.StdCall)]
    public static extern int Add(int a, int b);

    [DllImport(Library, CallingConvention = CallingConvention.StdCall)]
    public static extern void OpenWindow(uint width, uint height, string name);

    [DllImport(Library, CallingConvention = CallingConvention.StdCall)]
    public static extern void Start(nint window);
}