using System.ComponentModel;
using System.Diagnostics;
using Mega.Game;
using Mega.Generation;
using Mega.Video.Shading;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Mega.Video;
// We now have a rotating rectangle but how can we make the view move based on the users input?
// In this tutorial we will take a look at how you could implement a camera class
// and start responding to user input.
// You can move to the camera class to see a lot of the new code added.
// Otherwise you can move to Load to see how the camera is initialized.

// In reality, we can't move the camera but we actually move the rectangle.
// This will explained more in depth in the web version, however it pretty much gives us the same result
// as if the view itself was moved.
public class Window : GameWindow
{
    public static Stopwatch sw;

    // The view and projection matrices have been removed as we don't need them here anymore.
    // They can now be found in the new camera class.

    // We need an instance of the new camera class so it can manage the view and projection matrix code.
    // We also need a boolean set to true to detect whether or not the mouse has been moved for the first time.
    // Finally, we add the last position of the mouse so we can calculate the mouse offset easily.
    private Camera _camera;

    //CursorShader _cursor;
    private CursorShader _cursor;

    private bool _firstMove = true;

    private Vector2 _lastPos;
    private TextureDrawShader _meshRender;
    private UIShader _ui;
    public Shader edgeShader;
    private Player pl;
    private World world;


    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        TextureHelper.LoadUV();

        //this.Location = new Vector2i(100, 100);
        GLInit();

        // We initialize the camera so that it is 3 units back from where the rectangle is.
        // We also give it the proper aspect ratio.
        _camera = new Camera(new Vector3(0f, 50, 0f), Size.X / (float)Size.Y);

        // We make the mouse cursor invisible and captured so we can have proper FPS-camera movement.
        CursorState = CursorState.Grabbed;

        pl = new Player(_camera);
        world = new World(pl, this, 1);
        var worldSize = 128 * 2;
        var Autimation = new HeightAutomation(worldSize);
        Console.WriteLine(worldSize);
        Autimation.Scale = 16;

        Autimation.SetRandom(30);
        TimeMeasurementService.Start("Base generation");
        Autimation.Next();
        TimeMeasurementService.Start("To image");
        Autimation.ToImage();
        TimeMeasurementService.Start("Hill");
        Autimation.CreateHill();
        TimeMeasurementService.Start("Saving");
        Autimation.SaveTo(ref world.Area);
        ////Autimation.Save();
        //foreach (var item in Directory.GetFiles("gw").Where(i => i.EndsWith(".cd")))
        //{
        //    TimeMeasurementService.Start($"Loading {item}");
        //    WorldSaver.LoadFromFile(item, world.Area);
        //}
        TimeMeasurementService.Start("UpdateBorder");
        world.Area.UpdateBorder();
        int i = 0;

        TimeMeasurementService.Start(" UpdateRenderSurface");
        world.Area.UpdateRenderSurface();
        TimeMeasurementService.Stop();

        world.Start(100);

    }

    private void GLInit()
    {
        GL.ClearColor(0f, 0.5f, 0.5f, 1.0f);
        GL.Enable(EnableCap.DepthTest);
        _meshRender = new TextureDrawShader();
        _meshRender.Load();
        _cursor = new CursorShader();
        _cursor.Load();
        _ui = new UIShader();
        _ui.Load();

        //_cursor = new CursorShader();
        //_cursor.Use();
        //_cursor.Load();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        if (world.Redrawing)
            world.Area.UpdateRenderSurface();

        _meshRender.Run(world);
        _cursor.Run(world);
        _ui.Run(world);
        world.Redrawing = false;
        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        if (!IsFocused) // Check to see if the window is focused
            return;

        var input = KeyboardState;

        if (input.IsKeyDown(Keys.Escape))

            //Close();
            Process.GetCurrentProcess().Kill();

        const float cameraSpeed = 5f;
        const float sensitivity = 0.2f;
        var moveVec = new Vector2();
        if (input.IsKeyDown(Keys.W))

            //_camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
            moveVec.X = 1;
        if (input.IsKeyDown(Keys.S))

            //_camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
            moveVec.X = -1;
        if (input.IsKeyDown(Keys.A))

            //_camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
            moveVec.Y = 1;
        if (input.IsKeyDown(Keys.D))

            //_camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
            moveVec.Y = -1;
        if (input.IsKeyDown(Keys.LeftShift))
        {
            //_camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
        }

        if (input.IsKeyPressed(Keys.KeyPad0))
        {
            DemoWriter.SaveDemo();
        }
        if (input.IsKeyPressed(Keys.KeyPad1))
        {
            DemoWriter.Write = !DemoWriter.Write;
        }
        if (input.IsKeyPressed(Keys.KeyPad2))
        {
            DemoWriter.Read = !DemoWriter.Read;
        }
        if (input.IsKeyPressed(Keys.KeyPad3))
        {
            DemoWriter.LoadDemo();
        }

        DemoWriter.Nexting = input.IsKeyDown(Keys.KeyPad5);
        if (input.IsKeyReleased(Keys.R)) Debug.SaveEnable = !Debug.SaveEnable;

        if (input.IsKeyReleased(Keys.T)) Debug.SaveLogs();
        pl.Jumping = input.IsKeyDown(Keys.Space);

        // Get the mouse state
        var mouse = MouseState;
        pl.IsActs = mouse.IsButtonPressed(MouseButton.Left);

        //if (mouse.IsButtonPressed(MouseButton.Left))
        //    pl.PlaceBlock();

        if (_firstMove) // This bool variable is initially set to true.
        {
            _lastPos = new Vector2(0.5f, 0);
            _firstMove = false;
        }
        else
        {
            // Calculate the offset of the mouse position
            var deltaX = mouse.X - _lastPos.X;
            var deltaY = mouse.Y - _lastPos.Y;
            _lastPos = new Vector2(mouse.X, mouse.Y);

            // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
            _camera.Yaw += deltaX * sensitivity;
            _camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
        }

        pl.Moving = moveVec;
    }

    // In the mouse wheel function, we manage all the zooming of the camera.
    // This is simply done by changing the FOV of the camera.
    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

        _camera.Fov -= e.OffsetY;
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);

        // We need to update the aspect ratio once the window has been resized.
        _camera.AspectRatio = Size.X / (float)Size.Y;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        world.Stop();
        Console.ReadLine();
    }
}