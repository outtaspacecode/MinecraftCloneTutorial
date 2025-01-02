using OpenTK.Graphics.OpenGL4;

namespace MinecraftCloneTutorial {
    internal class IBO {

        private readonly int _id;
        
        public IBO(List<uint> data) {
            _id = GL.GenBuffer();
            Bind();
            GL.BufferData(BufferTarget.ElementArrayBuffer, data.Count * sizeof(uint), data.ToArray(), BufferUsageHint.StaticDraw);
        }
        
        public void Bind() { GL.BindBuffer(BufferTarget.ElementArrayBuffer, _id); }
        public void Unbind() { GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);}
        public void Delete() { GL.DeleteBuffer(_id); }
    }
}