using System.Runtime.InteropServices;

namespace OmegaTester;

static class Startup
{
    public static void Main(string[] args)
    {
        var x = OmegaGE.Add(5, 10);
        OmegaGE.InitWindow(900, 400, "Re test name");
        OmegaGE.Start();
        // Thread.Sleep(1000);
        var state = OmegaGE.GetWindowCloseState();
        while (state == 0)
        {
            OmegaGE.PollWindowEvents();
            state = OmegaGE.GetWindowCloseState();
        }
        // OmegaGE.Close();
    }
}

public static class OmegaGE
{
    private const string Library = "OmegaGE.dll";

    [DllImport(Library, CallingConvention = CallingConvention.StdCall)]
    public static extern int Add(int a, int b);

    [DllImport(Library, CharSet = CharSet.Ansi)]
    public static extern void InitWindow(uint width, uint height, string name);

    [DllImport(Library)]
    public static extern void Start();
    [DllImport(Library)]
    public static extern void Close();
    [DllImport(Library)]
    public static extern int GetWindowCloseState();
    [DllImport(Library)]
    public static extern void PollWindowEvents();
    
}