using MinecraftCloneTutorial.Graphics;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace MinecraftCloneTutorial.World {
    internal class Chunk {
        private List<Vector3> _chunkVertices;
        private List<Vector2> _chunkUVs;
        private List<uint> _chunkIndices;

        private const int Size = 32;
        public Vector3 Position;

        private uint _indexCount;

        private VAO _chunkVAO;
        private VBO _chunkVertexVBO;
        private VBO _chunkUVVBO;
        private IBO _chunkIBO;

        private Texture _texture;
        
        public Chunk(Vector3 position) {
            Position = position;

            _chunkVertices = new List<Vector3>();
            _chunkUVs = new List<Vector2>();
            _chunkIndices = new List<uint>();
            
            GenBlocks();
            BuildChunk();
        }

        /// <summary>
        /// Generate the data
        /// </summary>
        public void GenChunk() {
            
        }

        /// <summary>
        /// Generates the appropriate block faces given the data
        /// </summary>
        private void GenBlocks() {
            for (int i = 0; i < Size * Size; i++) {
                for (int j = 0; j < Size; j++) {
                    Block block = new Block(new Vector3(i % Size - Size / 2, -2 - j, i / Size - Size / 2));

                    int faceCount = 0;

                    if (i % Size == 0) {
                        var leftFaceData = block.GetFace(Faces.Left);
                        _chunkVertices.AddRange(leftFaceData.Vertices);
                        _chunkUVs.AddRange(leftFaceData.UV);
                        faceCount++;
                    }
                    else if (i % Size == Size - 1) {
                        var rightFaceData = block.GetFace(Faces.Right);
                        _chunkVertices.AddRange(rightFaceData.Vertices);
                        _chunkUVs.AddRange(rightFaceData.UV);
                        faceCount++;
                    }

                    if (i / Size == 0) {
                        var backFaceData = block.GetFace(Faces.Back);
                        _chunkVertices.AddRange(backFaceData.Vertices);
                        _chunkUVs.AddRange(backFaceData.UV);
                        faceCount++;
                    }
                    else if (i / Size == Size - 1) {
                        var frontFaceData = block.GetFace(Faces.Front);
                        _chunkVertices.AddRange(frontFaceData.Vertices);
                        _chunkUVs.AddRange(frontFaceData.UV);
                        faceCount++;
                    }

                    if (j % Size == 0) {
                        var topFaceData = block.GetFace(Faces.Top);
                        _chunkVertices.AddRange(topFaceData.Vertices);
                        _chunkUVs.AddRange(topFaceData.UV);
                        faceCount++;
                    } else if (j % Size == Size - 1) {
                        var bottomFaceData = block.GetFace(Faces.Bottom);
                        _chunkVertices.AddRange(bottomFaceData.Vertices);
                        _chunkUVs.AddRange(bottomFaceData.UV);
                        faceCount++;
                    }

                    AddIndices(faceCount);
                }
            }
        }

        private void AddIndices(int range) {
            for (int i = 0; i < range; i++) {
                _chunkIndices.Add(0 + _indexCount);
                _chunkIndices.Add(1 + _indexCount);
                _chunkIndices.Add(2 + _indexCount);
                _chunkIndices.Add(2 + _indexCount);
                _chunkIndices.Add(3 + _indexCount);
                _chunkIndices.Add(0 + _indexCount);

                _indexCount += 4;
            }
        }

        /// <summary>
        /// Take data and process it for rendering
        /// </summary>
        public void BuildChunk() {
            _chunkVAO = new VAO();
            _chunkVAO.Bind();

            _chunkVertexVBO = new VBO(_chunkVertices);
            _chunkVertexVBO.Bind();
            _chunkVAO.LinkToVAO(0, 3, _chunkVertexVBO);

            _chunkUVVBO = new VBO(_chunkUVs);
            _chunkUVVBO.Bind();
            _chunkVAO.LinkToVAO(1, 2, _chunkUVVBO);

            _chunkIBO = new IBO(_chunkIndices);

            _texture = new Texture("dirt.png");
        }

        /// <summary>
        /// Drawing the chunk
        /// </summary>
        /// <param name="program">The shader program</param>
        public void Render(ShaderProgram program) {
            program.Bind();
            _chunkVAO.Bind();
            _chunkIBO.Bind();
            _texture.Bind();
            GL.DrawElements(PrimitiveType.Triangles, _chunkIndices.Count, DrawElementsType.UnsignedInt, 0);
        }

        public void Delete() {
            _chunkVAO.Delete();
            _chunkVertexVBO.Delete();
            _chunkUVVBO.Delete();
            _chunkIBO.Delete();
            _texture.Delete();
        }
    }
}