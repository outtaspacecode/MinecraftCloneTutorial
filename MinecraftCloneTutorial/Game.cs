using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
// using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MinecraftCloneTutorial {
    internal class Game : GameWindow {
        
        // Vertices for a triangle
        private readonly float[] _vertices = [
            0.0f,  0.5f, 0.0f,     // Top vertex
            -0.5f, -0.5f, 0.0f,     // Bottom-left vertex
             0.5f, -0.5f, 0.0f      // Bottom-right vertex
        ];
        
        // Render pipeline variables
        private int _vao;
        private int _shaderProgram;
        
        // Width and Height of the game window
        private int _width, _height;
        
        // Constructor takes in a width and height and sets it
        public Game(int width, int height) : base(GameWindowSettings.Default, new NativeWindowSettings() {
            // This is needed to run on macOS
            // It doesn't seem to be working though. continuing work solely on Windows for now
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

            // Generate the VAO (Vertex Array Object)
            _vao = GL.GenVertexArray();
            
            // Generate and bind the VBO (Vertex Buffer Object)
            var vbo = GL.GenBuffer();
            // "Binding" the VBO simply means that we are currently using it
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            
            // Now that we've bound (are using) the VBO, we can write to it by using BufferData()
            // This takes in the Buffer Target (which has been bound to the VBO, the size of the data (in memory,
            // which is why we have to multiply the length of the vertex array by the size of a float), the data itself,
            // and finally we are going to use StaticDraw (I'm not exactly sure what this means yet)
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
            
            // Just like we bound the VBO, we have to bind the VAO as well, so let's do that now
            GL.BindVertexArray(_vao);
            
            // Now we can put the data in the VBO into a slot of the VAO using VertexAttribPointer()
            // The first parameter represents which slot the data is entering. The second parameter is more complicated
            // and I don't really understand it, but basically since we are using points in a three-dimensional space
            // we have to put 3 here, to represent that the data comes in clusters of three. Our next parameter lets
            // GL know that we're using floats. I'm not sure what the purpose of normalized is yet, but it's false here.
            // The last two parameters are for stride and offset, which are for packaging other data into the array,
            // which we aren't using here, so they're both zero.
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            
            // Now we have to enable the slot, which is pretty straightforward
            GL.EnableVertexArrayAttrib(_vao, 0);
            
            // Since we've finished with what we were doing, we should unbind the VBO and the VAO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            
            ////////////
            // SHADER //
            ////////////
            
            // Now we can create the shader program
            _shaderProgram = GL.CreateProgram();

            // Create and compile the vertex shader
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, LoadShaderSource("Default.vert"));
            GL.CompileShader(vertexShader);
            
            // Create and compile the fragment shader
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, LoadShaderSource("Default.frag"));
            GL.CompileShader(fragmentShader);
            
            // Attach the vertex and fragment shaders to the program
            GL.AttachShader(_shaderProgram, vertexShader);
            GL.AttachShader(_shaderProgram, fragmentShader);
            
            // Now that the shaders are attached, we have to link the program
            GL.LinkProgram(_shaderProgram);
            
            // It's good practice to delete the shaders once we're done with them
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        protected override void OnUnload() {
            base.OnUnload();
            
            // Cleanup after drawing the triangle
            GL.DeleteVertexArray(_vao);
            GL.DeleteProgram(_shaderProgram);
        }

        protected override void OnRenderFrame(FrameEventArgs args) {
            GL.ClearColor(0.68f, 0.88f, 0.98f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            
            // Draw the triangle
            // First, let GL know that we're using the program
            GL.UseProgram(_shaderProgram);
            // Bind the VAO so we can use it
            GL.BindVertexArray(_vao);
            // Draw the triangle, letting it know that we bundled the data in cluster of three
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            
            SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args) {
            base.OnUpdateFrame(args);
        }

        /// <summary>
        /// Opens a file in the Shaders directory
        /// </summary>
        /// <param name="filePath">The path to the file to be opened</param>
        /// <returns>An opened shader file, as a string</returns>
        private static string LoadShaderSource(string filePath) {
            var shaderSource = "";

            try {
                using var reader = new StreamReader("../../../Shaders/" + filePath);
                shaderSource = reader.ReadToEnd();
            } catch (Exception e) {
                Console.WriteLine("ERROR: Failed to load shader source file: " + e.Message);
            }
            
            return shaderSource;
        }
    }
}