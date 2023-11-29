using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace OmegaTester;

static class Startup
{
    public static void Main(string[] args)
    {
        var x = OmegaGE.Add(5, 10);
        OmegaGE.OpenWindow(900, 400);
        OmegaGE.Start();
        Console.ReadKey();
    }
}

public static class OmegaGE
{
    private const string Library = "OmegaGE.dll";
    [DllImport(Library, CallingConvention = CallingConvention.StdCall)]
    public static extern int Add(int a, int b);

    [DllImport(Library, CharSet = CharSet.Ansi)]
    public static extern void OpenWindow(uint width, uint height/*, string name*/);

    [DllImport(Library, CallingConvention = CallingConvention.StdCall)]
    public static extern void Start();
}