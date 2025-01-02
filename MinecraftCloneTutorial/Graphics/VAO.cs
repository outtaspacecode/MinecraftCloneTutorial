using OpenTK.Graphics.OpenGL4;

namespace MinecraftCloneTutorial.Graphics {
    internal class VAO {

        private readonly int _id;

        public VAO() {
            _id = GL.GenVertexArray();
            Bind();
        }

        public void LinkToVAO(int location, int size, VBO vbo) {
            Bind();
            vbo.Bind();
            GL.VertexAttribPointer(location, size, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(location);
            Unbind();
        }
        
        public void Bind() { GL.BindVertexArray(_id); }
        public void Unbind() { GL.BindVertexArray(0); }
        public void Delete() { GL.DeleteVertexArray(_id); }
    }
}