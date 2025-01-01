using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
// using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;

namespace MinecraftCloneTutorial {
    internal class Game : GameWindow {
        
        // Vertices for a triangle
        private readonly float[] _vertices = [
            -0.5f,  0.5f, 0.0f,     // Top-left vertex
             0.5f,  0.5f, 0.0f,     // Top-right vertex
             0.5f, -0.5f, 0.0f,     // Bottom-right vertex
            -0.5f, -0.5f, 0.0f      // Bottom-left vertex
        ];

        // Texture coordinates range from 0.0 to 1.0. Since we flipped the texture vertically, the
        // bottom left corner is (0,0)
        private readonly float[] _texCoords = [
            0.0f, 1.0f,
            1.0f, 1.0f,
            1.0f, 0.0f,
            0.0f, 0.0f
        ];

        private readonly uint[] _indices = [
            // Top triangle
            0, 1, 2,
            // Bottom triangle
            2, 3, 0
        ];
        
        // Render pipeline variables
        private int _vao;
        private int _shaderProgram;
        private int _vbo;
        private int _ibo;
        private int _textureID;
        private int _textureVBO;
        
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
            // Just like we bound the VBO, we have to bind the VAO as well, so let's do that now
            GL.BindVertexArray(_vao);
            
            // -- Vertices VBO -- //
            
            // Generate and bind the VBO (Vertex Buffer Object)
            _vbo = GL.GenBuffer();
            // "Binding" the VBO simply means that we are currently using it
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            
            // Now that we've bound (are using) the VBO, we can write to it by using BufferData()
            // This takes in the Buffer Target (which has been bound to the VBO, the size of the data (in memory,
            // which is why we have to multiply the length of the vertex array by the size of a float), the data itself,
            // and finally we are going to use StaticDraw (I'm not exactly sure what this means yet)
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
            
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
            
            // --- Texture VBO --- //
            _textureVBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _textureVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, _texCoords.Length * sizeof(float), _texCoords, BufferUsageHint.StaticDraw);
            
            // Here is the code for the texture VBO, the same as above but for slot 1 instead of 0
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(_vao, 1);
            
            // Since we've finished with what we were doing, we should unbind the VBO and the VAO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            
            
            // -- IBO -- //
            
            // Implementation of the IBO (Index Buffer Object)
            // Just like everything else, we generate it, bind it, and assign the data
            _ibo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ibo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
            
            // Unbind the IBO
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            
            
            // -- Shader -- //
            
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
            
            
            // --- Textures --- //

            // Generate the texture ID
            _textureID = GL.GenTexture();
            // Activate the texture in the texture unit as texture 0
            GL.ActiveTexture(TextureUnit.Texture0);
            // Bind the texture so we can use it
            GL.BindTexture(TextureTarget.Texture2D, _textureID);
            
            // Texture parameters
            // These first two make sure the texture repeats rather than stretching (S and T are the horizontal and vertical directions)
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            // These two set the mipmaps; this deals with scaling the image - it makes it so that the pixels aren't blurred
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            // Load texture image
            // If we don't do this, our textures will be flipped vertically (the 1 represents true)
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult dirtTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/dirt.png"), ColorComponents.RedGreenBlueAlpha);
            
            // This just loads the texture for GL
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, dirtTexture.Width, dirtTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, dirtTexture.Data);
            
            // Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);

        }

        protected override void OnUnload() {
            base.OnUnload();
            
            // Cleanup after drawing the triangle
            GL.DeleteVertexArray(_vao);
            GL.DeleteBuffer(_vbo);
            GL.DeleteBuffer(_ibo);
            GL.DeleteTexture(_textureID);
            GL.DeleteProgram(_shaderProgram);
        }

        protected override void OnRenderFrame(FrameEventArgs args) {
            GL.ClearColor(0.68f, 0.88f, 0.98f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            
            // Draw the triangle
            // First, let GL know that we're using the program
            GL.UseProgram(_shaderProgram);
            
            // Bind the texture
            GL.BindTexture(TextureTarget.Texture2D, _textureID);
            
            // Bind the VAO so we can use it
            GL.BindVertexArray(_vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ibo);
            // Draw the triangle, with 'count' being the number of vertices
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
            // GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            
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