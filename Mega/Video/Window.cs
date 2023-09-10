using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using Mega.Game;
using System.Diagnostics;

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
        Stopwatch sw;
        Player pl;
        public Vector3i newBlock = new Vector3i(3, 3, 3);
        private World world;
        private float[] _vertices;

        private Dictionary<int, List<uint>> _indices;

        private int _elementBufferObject;

        private int _vertexBufferObject;

        private int _vertexArrayObject;

        private Shader _shader;

        private Texture _texture;


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
            //ObjReader.ReadFile("Resources\\birch.obj", out
            //    _indices, out _vertices);

        }

        protected override void OnLoad()
        {
            base.OnLoad();
            TextureHelper.LoadUV();

            GLInit();

            sw = new Stopwatch();

            // We initialize the camera so that it is 3 units back from where the rectangle is.
            // We also give it the proper aspect ratio.
            _camera = new Camera(new Vector3(0, 3, 0), Size.X / (float)Size.Y);

            // We make the mouse cursor invisible and captured so we can have proper FPS-camera movement.
            CursorState = CursorState.Grabbed;

            world = World.GenerateFlat(1, this);

            pl = new Player(_camera, world);
            world.player = pl;
        }

        void GLInit()
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.DynamicDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.DynamicDraw);


            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            _shader.Use();

            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            _texture = Texture.LoadFromFile("Resources/0.png");

            _texture.Use(TextureUnit.Texture0);

            _shader.SetInt("texture0", 0);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            world.OnRender();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.BindVertexArray(_vertexArrayObject);

            //_texture.Use(TextureUnit.Texture0);
            //_shader.Use();

            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        public void UpdateMesh(float[] vertexes, Dictionary<int,  List<uint>> path)
        {

            _vertices = vertexes;
            _indices = path;
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.DynamicDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.DynamicDraw);


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
                Close();
            }

            const float cameraSpeed = 5f;
            const float sensitivity = 0.2f;

            if (input.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward

            }

            if (input.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards


            }
            if (input.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left


            }
            if (input.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right


            }
            if (input.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up


            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down

            }
            if (input.IsKeyReleased(Keys.KeyPad7))
            {
                newBlock += Vector3i.UnitY;
                world.SetBlock(newBlock);
            }
            if (input.IsKeyReleased(Keys.KeyPad1))
            {
                newBlock -= Vector3i.UnitY;
                world.SetBlock(newBlock);
            }
            if (input.IsKeyReleased(Keys.KeyPad8))
            {
                newBlock += Vector3i.UnitX;
                world.SetBlock(newBlock);
            }
            if (input.IsKeyReleased(Keys.KeyPad2))
            {
                newBlock -= Vector3i.UnitX;
                world.SetBlock(newBlock);
            }
            if (input.IsKeyReleased(Keys.KeyPad6))
            {
                newBlock += Vector3i.UnitZ;
                world.SetBlock(newBlock);
            }
            if (input.IsKeyReleased(Keys.KeyPad4))
            {
                newBlock -= Vector3i.UnitZ;
                world.SetBlock(newBlock);
            }
            if (input.IsKeyReleased(Keys.R))
            {
                _camera.Position = new Vector3(MathF.Round(_camera.Position.X), MathF.Round(_camera.Position.Y), MathF.Round(_camera.Position.Z));
            }

            if (input.IsKeyReleased(Keys.I))
            {
                Console.WriteLine(
                    $"Position: {_camera.Position}\n" +
                    $"Surfaces: {world.Surface.Length}({world.MembersList.Count * 6})\n" +
                    $"Blocks: {world.MembersList.Count}");
            }
            // Get the mouse state
            var mouse = MouseState;
            if(mouse.IsButtonPressed(MouseButton.Left))
            {
                pl.PlaceBlock();
            }
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
            Title = _camera.Position.ToString() + " " + (1 / e.Time).ToString();

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
    }
}