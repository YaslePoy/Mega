using System.Diagnostics;
using System.Runtime.InteropServices;
using Mega.Game.Blocks;
using Mega.Video;
using OpenTK.Mathematics;

namespace OmegaTester;

static class Startup
{
    public static void Main(string[] args)
    {
        var rs = new RenderSurface(CubicBlock.MeshSides[0], new [] {Vector2.Zero, new (0, 1), new (1, 1), new (1, 0)}, Vector3.Zero, Vector3.UnitX, 1);
        var arr = new RenderSurface[] { rs };
        Console.WriteLine("Test");
        OmegaGE.InitWindow(900, 400, "Re test name");
        OmegaGE.Start();

        OmegaGE.SetMeshShaderData(arr, 1);
        OmegaGE.SetMainRenderTexture(new byte[] { 1, 2, 5, 3, 5 }, 5, 6);
       
        // Thread.Sleep(1000);
        var state = OmegaGE.GetWindowCloseState();
        Stopwatch time = new Stopwatch();
        while (state == 0)
        {
            OmegaGE.PollWindowEvents();
            OmegaGE.Draw();
            state = OmegaGE.GetWindowCloseState();
        }
    }
}

public static class OmegaGE
{
    private const string Library = @"C:\Users\Mimm\Projects\VisualStudioProjects\Mega\OmegaGE\bin\x64\Release\OmegaGE.dll";

    [DllImport(Library)]
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

    [DllImport(Library)]
    public static extern void Draw();

    [DllImport(Library)]
    public static extern void SetMainRenderTexture([In] [Out] byte[] data, int width, int height);
    
    [DllImport(Library)]
    public static extern void SetMeshShaderData([In] [Out] RenderSurface[] polygons, uint surfaceCount);
}