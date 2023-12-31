using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using Mega.Game;
using Mega.Game.Blocks;
using Mega.Generation;
using Mega.Video.Shading;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Mega.Video;

public static class OmegaEngine
{
    private const string Library =
        @"C:\Users\Mimm\Projects\VisualStudioProjects\Mega\OmegaGE\bin\x64\Release\OmegaGE.dll";

    [DllImport(Library)]
    public static extern int Add(int a, int b);

    [DllImport(Library)]
    public static extern void InitWindow(uint width, uint height);

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
    public static extern void SetViewSettings(Vector3 from, Vector3 to, Vector3 up);

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

    //import end 

    private static Camera _camera;
    private static Player pl;
    public static World world;


    private static TextureDrawShader _meshRender;

    public static void Launch()
    {
        int coutner = 0;
        OnLoad();
        var fpsometer = Stopwatch.StartNew();
        while (GetWindowCloseState() == 0)
        {
            UpdateKeyboardState();
            PollWindowEvents();
            if (IsKeyDown((int)GLFWKeys.Escape))
            {
                Close();
                break;
            }

            OnUpdate();
            OnDraw();
            Draw();
            coutner++;
            if (coutner == 1000)
            {
                fpsometer.Stop();
                coutner = 0;
                var fps = 1000 / fpsometer.Elapsed.TotalSeconds;
                Console.WriteLine($"Framerate: {Math.Round(fps, 3),5} FPS");
                fpsometer.Restart();
            }
        }
    }

    private static void OnLoad()
    {
        //Base load 
        TextureHelper.Load();
        Atlas.Main.Assemble();
        var mainTex = Atlas.Main.Image;
        SetMainRenderTexture(mainTex.data, mainTex.X, mainTex.Y);

        _camera = new Camera(new Vector3(0, 0, 50));
        _camera.Apply();

        InitWindow(1920, 1080);
        Start();

        //Mega load
        pl = new Player(_camera);
        world = new World(pl, 1);

        _meshRender = new TextureDrawShader();

        var worldSize = 128 * 1;
        var Autimation = new HeightAutomation(worldSize);
        Console.WriteLine(worldSize);
        Autimation.Scale = 16;

        Autimation.SetRandom(30);
        TimeMeasurementService.Start("Base generation");
        Autimation.Next();
        TimeMeasurementService.Start("To image");
        Autimation.ToImage();
        TimeMeasurementService.Start("Hill");

        TimeMeasurementService.Start("Saving");
        Autimation.SaveTo(ref world.Area);


        TimeMeasurementService.Start("UpdateBorder");
        world.Area.UpdateBorder();

        TimeMeasurementService.Start("UpdateRenderSurface");
        world.Area.UpdateRenderSurface();
        TimeMeasurementService.Stop();

        world.Start(100);
    }

    private static void OnUpdate()
    {
        // if (IsKeyDown((int)GLFWKeys.D))
        //     _camera.Position.X += 0.0005f;
        // if (IsKeyDown((int)GLFWKeys.A))
        //     _camera.Position.X -= 0.0005f;
        // if (IsKeyDown((int)GLFWKeys.W))
        //     _camera.Position.Y += 0.0005f;
        // if (IsKeyDown((int)GLFWKeys.S))
        //     _camera.Position.Y -= 0.0005f;
        // if (IsKeyDown((int)GLFWKeys.E))
        //     _camera.Position.Z += 0.0005f;
        // if (IsKeyDown((int)GLFWKeys.Q))
        //     _camera.Position.Z -= 0.0005f;
        // if (IsKeyDown((int)GLFWKeys.Right))
        //     _camera.Yaw -= 0.01f;
        // if (IsKeyDown((int)GLFWKeys.Left))
        //     _camera.Yaw += 0.01f;
        // if (IsKeyDown((int)GLFWKeys.Up))
        //     _camera.Pitch += 0.01f;
        // if (IsKeyDown((int)GLFWKeys.Down))
        //     _camera.Pitch -= 0.01f;d

        const float sensitivity = 0.2f;


        var moveVec = new Vector2();
        if (IsKeyDown((int)GLFWKeys.W))
            moveVec.X = 1;
        if (IsKeyDown((int)GLFWKeys.S))
            moveVec.X = -1;
        if (IsKeyDown((int)GLFWKeys.A))
            moveVec.Y = 1;
        if (IsKeyDown((int)GLFWKeys.D))
            moveVec.Y = -1;

        if (IsKeyPressed((int)GLFWKeys.Num0))
        {
            DemoWriter.SaveDemo();
        }

        if (IsKeyPressed((int)GLFWKeys.Num1))
        {
            DemoWriter.Write = !DemoWriter.Write;
        }

        if (IsKeyPressed((int)GLFWKeys.Num2))
        {
            DemoWriter.Read = !DemoWriter.Read;
        }

        if (IsKeyPressed((int)GLFWKeys.Num3))
        {
            DemoWriter.LoadDemo();
        }

        if (IsKeyPressed((int)GLFWKeys.F))
            pl.Fly = !pl.Fly;
        pl.Fast = IsKeyDown((int)GLFWKeys.LeftShift);
        DemoWriter.Nexting = IsKeyDown((int)GLFWKeys.Num5);
        if (IsKeyReleased((int)GLFWKeys.R)) Debug.SaveEnable = !Debug.SaveEnable;

        if (IsKeyReleased((int)GLFWKeys.T)) Debug.SaveLogs();
        pl.Jumping = IsKeyDown((int)GLFWKeys.Space);

        if (IsKeyDown((int)GLFWKeys.Up))
            _camera.Pitch += 1 * sensitivity / 2;
        else if (IsKeyDown((int)GLFWKeys.Down))
            _camera.Pitch -= 1 * sensitivity / 2;
        if (IsKeyDown((int)GLFWKeys.Left))
            _camera.Yaw += 1 * sensitivity / 2;
        else if (IsKeyDown((int)GLFWKeys.Right))
            _camera.Yaw -= 1 * sensitivity / 2;

        pl.Moving = moveVec;

        _camera.Apply();
    }

    private static void OnDraw()
    {
        if (world.Redrawing)
        {
            TimeMeasurementService.Start("UpdateRenderSurface");
            world.Area.UpdateRenderSurface();
            TimeMeasurementService.Stop();
        }

        _meshRender.Run();
        world.Redrawing = false;
    }
}