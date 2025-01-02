using OpenTK.Graphics.OpenGL4;

namespace MinecraftCloneTutorial.Graphics {
    internal class ShaderProgram {

        public readonly int ID;
        
        public ShaderProgram(string vertexShaderFilepath, string fragmentShaderFilepath) {
            // -- Shader -- //
            
            // Now we can create the shader program
            ID = GL.CreateProgram();

            // Create and compile the vertex shader
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, LoadShaderSource(vertexShaderFilepath));
            GL.CompileShader(vertexShader);
            
            // Create and compile the fragment shader
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, LoadShaderSource(fragmentShaderFilepath));
            GL.CompileShader(fragmentShader);
            
            // Attach the vertex and fragment shaders to the program
            GL.AttachShader(ID, vertexShader);
            GL.AttachShader(ID, fragmentShader);
            
            // Now that the shaders are attached, we have to link the program
            GL.LinkProgram(ID);
            
            // It's good practice to delete the shaders once we're done with them
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public void Bind() { GL.UseProgram(ID); }
        public void Unbind() { GL.UseProgram(0); }
        public void Delete() { GL.DeleteProgram(ID); }
        
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