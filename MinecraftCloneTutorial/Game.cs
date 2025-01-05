using System.Globalization;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using MinecraftCloneTutorial.Graphics;
// using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;

namespace MinecraftCloneTutorial {
    internal class Game : GameWindow {
        
        // Vertices for a cube
        private readonly List<Vector3> _vertices = [
            // Front face
            new Vector3(-0.5f,  0.5f, 0.5f),    // Top-left vertex
            new Vector3( 0.5f,  0.5f, 0.5f),    // Top-right vertex
            new Vector3( 0.5f, -0.5f, 0.5f),    // Bottom-right vertex
            new Vector3(-0.5f, -0.5f, 0.5f),    // Bottom-left vertex
            
            // Right face
            new Vector3( 0.5f,  0.5f,  0.5f),   // Top-left vertex
            new Vector3( 0.5f,  0.5f, -0.5f),   // Top-right vertex
            new Vector3( 0.5f, -0.5f, -0.5f),   // Bottom-right vertex
            new Vector3( 0.5f, -0.5f,  0.5f),   // Bottom-left vertex
            
            // Back face
            new Vector3( 0.5f,  0.5f, -0.5f),   // Top-left vertex
            new Vector3(-0.5f,  0.5f, -0.5f),   // Top-right vertex
            new Vector3(-0.5f, -0.5f, -0.5f),   // Bottom-right vertex
            new Vector3( 0.5f, -0.5f, -0.5f),   // Bottom-left vertex
            
            // Left face
            new Vector3(-0.5f,  0.5f, -0.5f),   // Top-left vertex
            new Vector3(-0.5f,  0.5f,  0.5f),   // Top-right vertex
            new Vector3(-0.5f, -0.5f,  0.5f),   // Bottom-right vertex
            new Vector3(-0.5f, -0.5f, -0.5f),   // Bottom-left vertex
            
            // Top face
            new Vector3(-0.5f,  0.5f, -0.5f),   // Top-left vertex
            new Vector3( 0.5f,  0.5f, -0.5f),   // Top-right vertex
            new Vector3( 0.5f,  0.5f,  0.5f),   // Bottom-right vertex
            new Vector3(-0.5f,  0.5f,  0.5f),   // Bottom-left vertex
            
            // Bottom face
            new Vector3(-0.5f, -0.5f,  0.5f),   // Top-left vertex
            new Vector3( 0.5f, -0.5f,  0.5f),   // Top-right vertex
            new Vector3( 0.5f, -0.5f, -0.5f),   // Bottom-right vertex
            new Vector3(-0.5f, -0.5f, -0.5f)    // Bottom-left vertex
            
        ];

        // Texture coordinates range from 0.0 to 1.0. Since we flipped the texture vertically, the
        // bottom left corner is (0,0)
        // These are the same for every face
        private readonly List<Vector2> _texCoords = [
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(0.0f, 0.0f),
            
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(0.0f, 0.0f),
            
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(0.0f, 0.0f),
            
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(0.0f, 0.0f),
            
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(0.0f, 0.0f),
            
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(0.0f, 0.0f)
        ];

        // The indices of each triangle, with two triangles per face
        private readonly List<uint> _indices = [
            0, 1, 2,
            2, 3, 0,
            
            4, 5, 6,
            6, 7, 4,
            
            8, 9, 10,
            10, 11, 8,
            
            12, 13, 14,
            14, 15, 12,
            
            16, 17, 18,
            18, 19, 16,
            
            20, 21, 22,
            22, 23, 20
        ];
        
        // Render pipeline variables
        private VAO _vao;
        private IBO _ibo;
        private ShaderProgram _shaderProgram;
        private Texture _texture;
        
        
        // Camera
        private Camera _camera;
        private bool _isGrabbed = true;
        
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

            _vao = new VAO();
            VBO vbo = new VBO(_vertices);
            _vao.LinkToVAO(0, 3, vbo);
            VBO uv = new VBO(_texCoords);
            _vao.LinkToVAO(1, 2, uv);
            
            _ibo = new IBO(_indices);

            _shaderProgram = new ShaderProgram("Default.vert", "Default.frag");
            _texture = new Texture("stone.png");

            // Enable depth testing
            GL.Enable(EnableCap.DepthTest);

            _camera = new Camera(_width, _height, Vector3.Zero);
            CursorState = CursorState.Grabbed;

            WindowState = WindowState.Fullscreen;
        }

        protected override void OnUnload() {
            base.OnUnload();

            _vao.Delete();
            _ibo.Delete();
            _texture.Delete();
            _shaderProgram.Delete();
        }

        protected override void OnRenderFrame(FrameEventArgs args) {
            GL.ClearColor(0.68f, 0.88f, 0.98f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            _shaderProgram.Bind();
            _vao.Bind();
            _ibo.Bind();
            _texture.Bind();
            
            // Transformation matrices
            Matrix4 model = Matrix4.CreateScale(1.0f);
            Matrix4 view = _camera.GetViewMatrix();
            Matrix4 projection = _camera.GetProjectionMatrix();
            
            model *= Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            
            var modelLocation = GL.GetUniformLocation(_shaderProgram.ID, "model");
            var viewLocation = GL.GetUniformLocation(_shaderProgram.ID, "view");
            var projectionLocation = GL.GetUniformLocation(_shaderProgram.ID, "projection");
            
            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);
            
            // Draw the triangle, with 'count' being the number of vertices
            GL.DrawElements(PrimitiveType.Triangles, _indices.Count, DrawElementsType.UnsignedInt, 0);
            // GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            
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
            }

            if (_isGrabbed) {
                CursorState = CursorState.Grabbed;
            } else {
                CursorState = CursorState.Normal;
            }
        }
    }
}