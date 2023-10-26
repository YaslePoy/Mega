using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using Mega.Game;
using System.Diagnostics;
using System.ComponentModel;
using Mega.Video.Shading;
using Mega.Generation;
using System.Drawing;

namespace Mega.Video
{
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
        public Shader edgeShader;
        public static Stopwatch sw;
        Player pl;
        private World world;
        //CursorShader _cursor;
        CursorShader _cursor;
        TextureDrawShader _meshRender;
        UIShader _ui;

        // The view and projection matrices have been removed as we don't need them here anymore.
        // They can now be found in the new camera class.

        // We need an instance of the new camera class so it can manage the view and projection matrix code.
        // We also need a boolean set to true to detect whether or not the mouse has been moved for the first time.
        // Finally, we add the last position of the mouse so we can calculate the mouse offset easily.
        private Camera _camera;

        private bool _firstMove = true;

        private Vector2 _lastPos;


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
            var Autimation = new HeightAutomation(128*8);
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

            foreach (var item in Directory.GetFiles("gw").Where(i => i.EndsWith(".cd")))
            {
                Console.WriteLine($"Loading {item}");
                WorldSaver.LoadFromFile(item, world.Area);
            }
            TimeMeasurementService.Start("BuildGlobalCoordinates");
            world.Area.BuildGlobalCoordinates(false);
            TimeMeasurementService.Start("UpdateBorder");
            world.Area.UpdateBorder();
            TimeMeasurementService.Start("UpdateRenderSurface");
            world.Area.UpdateRenderSurface();
            TimeMeasurementService.Stop();
            world.Start(100);
        }

        void GLInit()
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
            //_cursor.Run(world);
            _ui.Run(world);
            world.Redrawing = false;
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (!IsFocused) // Check to see if the window is focused
            {
                return;
            }

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                //Close();
                Process.GetCurrentProcess().Kill();
            }

            const float cameraSpeed = 5f;
            const float sensitivity = 0.2f;
            var moveVec = new Vector2();
            if (input.IsKeyDown(Keys.W))
            {
                //_camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
                moveVec.X = 1;

            }
            if (input.IsKeyDown(Keys.S))
            {
                //_camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
                moveVec.X = -1;


            }
            if (input.IsKeyDown(Keys.A))
            {
                //_camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left

                moveVec.Y = 1;
            }
            if (input.IsKeyDown(Keys.D))
            {
                //_camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
                moveVec.Y = -1;

            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                //_camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
            }
            if (input.IsKeyReleased(Keys.R))
            {
                _camera.Position = new Vector3(MathF.Round(_camera.Position.X), MathF.Round(_camera.Position.Y), MathF.Round(_camera.Position.Z));
            }
            if (input.IsKeyReleased(Keys.T))
            {
                WorldSaver.SaveWorld(world.Area);
            }
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
}