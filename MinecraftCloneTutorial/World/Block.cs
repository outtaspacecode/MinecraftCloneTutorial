using OpenTK.Mathematics;

namespace MinecraftCloneTutorial.World {
    internal class Block {
        public Vector3 Position;
        public BlockType type;
        private Dictionary<Faces, FaceData> _faces;

        private readonly List<Vector2> _uv = [
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(0.0f, 0.0f)
        ];

        public Block(Vector3 position, BlockType blockType = BlockType.Air) {
            Position = position;
            type = blockType;

            _faces = new Dictionary<Faces, FaceData> {
                {Faces.Front, new FaceData {
                    Vertices = AddTransformedVertices(RawFaceData.RawVertexData[Faces.Front]),
                    UV = _uv
                } },
                {Faces.Back, new FaceData {
                    Vertices = AddTransformedVertices(RawFaceData.RawVertexData[Faces.Back]),
                    UV = _uv
                } },
                {Faces.Right, new FaceData {
                    Vertices = AddTransformedVertices(RawFaceData.RawVertexData[Faces.Right]),
                    UV = _uv
                } },
                {Faces.Left, new FaceData {
                    Vertices = AddTransformedVertices(RawFaceData.RawVertexData[Faces.Left]),
                    UV = _uv
                } },
                {Faces.Top, new FaceData {
                    Vertices = AddTransformedVertices(RawFaceData.RawVertexData[Faces.Top]),
                    UV = _uv
                } },
                {Faces.Bottom, new FaceData {
                    Vertices = AddTransformedVertices(RawFaceData.RawVertexData[Faces.Bottom]),
                    UV = _uv
                } }
            };
        }

        public List<Vector3> AddTransformedVertices(List<Vector3> vertices) {
            List<Vector3> transformedVertices = new List<Vector3>();
            foreach (var vert in vertices) {
                transformedVertices.Add(vert + Position);
            }

            return transformedVertices;
        }

        public FaceData GetFace(Faces face) {
            return _faces[face];
        }
    }
}