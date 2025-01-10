// using System.Globalization;
// using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using MinecraftCloneTutorial.Graphics;
using MinecraftCloneTutorial.World;

// using OpenTK.Windowing.GraphicsLibraryFramework;
// using StbImageSharp;

namespace MinecraftCloneTutorial {
    internal class Game : GameWindow {
        private Chunk _chunk;
        private ShaderProgram _shaderProgram;
        
        // Camera
        private Camera _camera = null!;
        private bool _isGrabbed = true;
        private bool _isFullscreen = true;
        private bool _isWireframe;
        
        // FPS
        private double _previousTime = 0.0;
        private double _currentTime = 0.0;
        private double _timeDifference;
        private uint _timeCounter = 0;
        
        // Width and Height of the game window
        private int _width, _height;
        
        // Constructor takes in a width and height and sets it
        public Game(int width, int height) : base(GameWindowSettings.Default, new NativeWindowSettings() {
            // This is needed to run on macOS
            Flags = ContextFlags.ForwardCompatible
        }) {

            _width = width;
            _height = height;
            // Center the window
            CenterWindow(new Vector2i(width, height));
            // Set the title in the titlebar
            Title = "Minecraft Clone Tutorial";
        }

        protected override void OnResize(ResizeEventArgs e) {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            _width = e.Width;
            _height = e.Height;
        }

        protected override void OnLoad() {
            base.OnLoad();

            _chunk = new Chunk(new Vector3(0.0f, 0.0f, 0.0f), "dirt.png");
            _shaderProgram = new ShaderProgram("Default.vert", "Default.frag");

            // Enable depth testing
            GL.Enable(EnableCap.DepthTest);
            
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            _camera = new Camera(_width, _height, Vector3.Zero);
            CursorState = CursorState.Grabbed;

            GL.Viewport(0, 0, 1920, 1080);
            _width = 1920;
            _height = 1080;
            WindowState = WindowState.Fullscreen;
        }

        protected override void OnUnload() {
            base.OnUnload();
        }

        protected override void OnRenderFrame(FrameEventArgs args) {
            GL.ClearColor(0.68f, 0.88f, 0.98f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            
            
            // Transformation matrices
            Matrix4 model = Matrix4.CreateScale(1.0f);
            Matrix4 view = _camera.GetViewMatrix();
            Matrix4 projection = _camera.GetProjectionMatrix();
            
            var modelLocation = GL.GetUniformLocation(_shaderProgram.ID, "model");
            var viewLocation = GL.GetUniformLocation(_shaderProgram.ID, "view");
            var projectionLocation = GL.GetUniformLocation(_shaderProgram.ID, "projection");
            
            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);
            
            _chunk.Render(_shaderProgram);
            
            SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args) {
            base.OnUpdateFrame(args);

            MouseState mouse = MouseState;
            KeyboardState input = KeyboardState;

            // Only update the camera if the mouse is grabbed
            if (_isGrabbed) {
                _camera.Update(input, mouse, args);
            }

            // Close the program if the escape key is pressed
            if (KeyboardState.IsKeyPressed(Keys.Escape)) { Close(); }

            // if (KeyboardState.IsKeyPressed(Keys.Enter)) { WindowState = WindowState.Fullscreen; }
            
            // Release the mouse if the E key has been pressed
            if (KeyboardState.IsKeyPressed(Keys.E)) {
                _isGrabbed = !_isGrabbed;
                CursorState = _isGrabbed ? CursorState.Grabbed : CursorState.Normal;
            }

            // Change between fullscreen and windowed when enter is pressed
            if (KeyboardState.IsKeyPressed(Keys.Enter)) {
                _isFullscreen = !_isFullscreen;
                if (_isFullscreen) {
                    GL.Viewport(0, 0, 1920, 1080);
                    _width = 1920;
                    _height = 1080;
                    WindowState = WindowState.Fullscreen;
                } else {
                    GL.Viewport(0, 0, 1000, 600);
                    _width = 1000;
                    _height = 600;
                    WindowState = WindowState.Normal;
                }
            }

            if (KeyboardState.IsKeyPressed(Keys.V)) {
                _isWireframe = !_isWireframe;
                if (_isWireframe) {
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                } else {
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                }
            }

            _currentTime = GLFW.GetTime();
            _timeDifference = _currentTime - _previousTime;
            _timeCounter++;
            if (_timeDifference >= 0.75) {
                double fps = (1.0 / _timeDifference) * _timeCounter;
                Title = "Minecraft Clone Tutorial | FPS: " + Math.Round(fps);
                _previousTime = _currentTime;
                _timeCounter = 0;
            }
        }
    }
}