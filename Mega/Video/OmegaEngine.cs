using System.Runtime.InteropServices;

namespace Mega.Video;

public static class OmegaEngine
{
    private const string Library =
        @"C:\Users\Mimm\Projects\VisualStudioProjects\Mega\OmegaGE\bin\x64\Release\OmegaGE.dll";

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
    public static extern void UpdateMainRenderTexture([In] [Out] byte[] data, int width, int height);

    [DllImport(Library)]
    public static extern void SetMeshShaderData([In] [Out] RenderSurface[] polygons, uint surfaceCount);

    [DllImport(Library)]
    public static extern void UpdateKeyboardState();

    [DllImport(Library)]
    public static extern bool IsKeyPressed(int key);

    [DllImport(Library)]
    public static extern bool IsKeyReleased(int key);

    [DllImport(Library)]
    public static extern bool IsKeyDown(int key);

    [DllImport(Library)]
    public static extern bool IsKeyUp(int key);
}