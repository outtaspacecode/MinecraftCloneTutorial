using Microsoft.VisualBasic.FileIO;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace MinecraftCloneTutorial.Graphics {
    internal class VBO {
        private readonly int _id;

        public VBO(List<Vector3> data) {
            _id = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _id);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Count * Vector3.SizeInBytes, data.ToArray(), BufferUsageHint.StaticDraw);
        }

        public VBO(List<Vector2> data) {
            _id = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _id);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Count * Vector2.SizeInBytes, data.ToArray(), BufferUsageHint.StaticDraw);
        }

        public void Bind() { GL.BindBuffer(BufferTarget.ArrayBuffer, _id); }
        public void Unbind() { GL.BindBuffer(BufferTarget.ArrayBuffer, 0); }
        public void Delete() { GL.DeleteBuffer(_id); }
    }
}