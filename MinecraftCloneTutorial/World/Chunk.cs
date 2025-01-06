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
        
        private Block[,,] _chunkBlocks = new Block[Size, Size, Size];

        private uint _indexCount;

        private VAO _chunkVAO;
        private VBO _chunkVertexVBO;
        private VBO _chunkUVVBO;
        private IBO _chunkIBO;

        private Texture _texture;
        private string _texturePath;
        
        public Chunk(Vector3 position, string texturePath) {
            Position = position;
            _texturePath = texturePath;

            _chunkVertices = new List<Vector3>();
            _chunkUVs = new List<Vector2>();
            _chunkIndices = new List<uint>();
            
            var heightmap = GenChunk();
            GenBlocks(heightmap);
            GenFaces(heightmap);
            BuildChunk();
        }

        /// <summary>
        /// Generate the data
        /// </summary>
        public float[,] GenChunk() {
            float[,] heightmap = new float[Size, Size];

            Random rnd = new Random();
            SimplexNoise.Noise.Seed = rnd.Next();
            for (int x = 0; x < Size; x++) {
                for (int z = 0; z < Size; z++) {
                    heightmap[x, z] = SimplexNoise.Noise.CalcPixel2D(x, z, 0.01f);
                }
            }
            
            return heightmap;
        }

        /// <summary>
        /// Generates the appropriate block faces given the data
        /// </summary>
        private void GenBlocks(float [,] heightmap) {
            for (int x = 0; x < Size; x++) {
                for (int z = 0; z < Size; z++) {
                    int columnHeight = (int)(heightmap[x, z] / 10);
                    for (int y = 0; y < Size; y++) {

                        if (y < columnHeight) {
                            _chunkBlocks[x, y, z] = new Block(new Vector3(x, y, z), BlockType.Dirt);
                        } else {
                            _chunkBlocks[x, y, z] = new Block(new Vector3(x, y, z));
                        }
                    }
                }
            }
        }

        private void GenFaces(float [,] heightmap) {
            for (int x = 0; x < Size; x++) {
                for (int z = 0; z < Size; z++) {
                    var columnHeight = (int)(heightmap[x, z] / 10);
                    for (int y = 0; y < columnHeight; y++) {
                        var numFaces = 0;
                        // Left faces
                        // Qualifications: Block to left is empty or is the farthest left in chunk
                        if (x > 0) {
                            if (_chunkBlocks[x - 1, y, z].type == BlockType.Air) {
                                IntegrateFace(_chunkBlocks[x, y, z], Faces.Left);
                                numFaces++;
                            }
                        } else {
                           IntegrateFace(_chunkBlocks[x, y, z], Faces.Left);
                           numFaces++;
                        }
                        
                        // Right faces
                        // Qualifications: Block to right is empty or is the farthest right in chunk
                        if (x < Size - 1) {
                            if (_chunkBlocks[x + 1, y, z].type == BlockType.Air) {
                                IntegrateFace(_chunkBlocks[x, y, z], Faces.Right);
                                numFaces++;
                            }
                        } else {
                            IntegrateFace(_chunkBlocks[x, y, z], Faces.Right);
                            numFaces++;
                        }
                        
                        // Top faces
                        // Qualifications: Block above is empty or is the farthest up in chunk
                        if (y < Size - 1) {
                            if (_chunkBlocks[x, y + 1, z].type == BlockType.Air) {
                                IntegrateFace(_chunkBlocks[x, y, z], Faces.Top);
                                numFaces++;
                            }
                        } else {
                            IntegrateFace(_chunkBlocks[x, y, z], Faces.Top);
                            numFaces++;
                        }
                        
                        // Bottom faces
                        // Qualifications: Block below is empty or is the farthest down in chunk
                        if (y > 0) {
                            if (_chunkBlocks[x, y - 1, z].type == BlockType.Air) {
                                IntegrateFace(_chunkBlocks[x, y, z], Faces.Bottom);
                                numFaces++;
                            }
                        } else {
                            IntegrateFace(_chunkBlocks[x, y, z], Faces.Bottom);
                            numFaces++;
                        }
                        
                        // Front faces
                        // Qualifications: Block in front is empty or is the farthest front in chunk
                        if (z < Size - 1) {
                            if (_chunkBlocks[x, y, z + 1].type == BlockType.Air) {
                                IntegrateFace(_chunkBlocks[x, y, z], Faces.Front);
                                numFaces++;
                            }
                        } else {
                            IntegrateFace(_chunkBlocks[x, y, z], Faces.Front);
                            numFaces++;
                        }
                        
                        // Back faces
                        // Qualifications: Block behind is empty or is the farthest back in chunk
                        if (z > 0) {
                            if (_chunkBlocks[x, y, z - 1].type == BlockType.Air) {
                                IntegrateFace(_chunkBlocks[x, y, z], Faces.Back);
                                numFaces++;
                            }
                        } else {
                            IntegrateFace(_chunkBlocks[x, y, z], Faces.Back);
                            numFaces++;
                        }
                        
                        AddIndices(numFaces);
                    }
                }
            }
        }

        private void IntegrateFace(Block block, Faces face) {
            var faceData = block.GetFace(face);
            _chunkVertices.AddRange(faceData.Vertices);
            _chunkUVs.AddRange(faceData.UV);
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

            _texture = new Texture(_texturePath);
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