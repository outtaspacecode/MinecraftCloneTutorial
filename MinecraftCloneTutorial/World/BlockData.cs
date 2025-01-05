using OpenTK.Mathematics;

namespace MinecraftCloneTutorial.World {
    public enum Faces {
        Front,
        Back,
        Left,
        Right,
        Top,
        Bottom
    }

    public struct FaceData {
        public List<Vector3> Vertices;
        public List<Vector2> UV;
    }

    public struct RawFaceData {
        public static readonly Dictionary<Faces, List<Vector3>> RawVertexData = new Dictionary<Faces, List<Vector3>>()
        {
            {Faces.Front,  [new Vector3(-0.5f,  0.5f,  0.5f),       // Top-left vertex
                            new Vector3( 0.5f,  0.5f,  0.5f),       // Top-right vertex
                            new Vector3( 0.5f, -0.5f,  0.5f),       // Bottom-right vertex
                            new Vector3(-0.5f, -0.5f,  0.5f)]},     // Bottom-left vertex
            
            {Faces.Back,   [new Vector3( 0.5f,  0.5f, -0.5f),       // Top-left vertex
                            new Vector3(-0.5f,  0.5f, -0.5f),       // Top-right vertex
                            new Vector3(-0.5f, -0.5f, -0.5f),       // Bottom-right vertex
                            new Vector3( 0.5f, -0.5f, -0.5f)]},     // Bottom-left vertex
            
            {Faces.Right,  [new Vector3( 0.5f,  0.5f,  0.5f),       // Top-left vertex
                            new Vector3( 0.5f,  0.5f, -0.5f),       // Top-right vertex
                            new Vector3( 0.5f, -0.5f, -0.5f),       // Bottom-right vertex
                            new Vector3( 0.5f, -0.5f,  0.5f)]},     // Bottom-left vertex
            
            {Faces.Left,   [new Vector3(-0.5f,  0.5f, -0.5f),       // Top-left vertex
                            new Vector3(-0.5f,  0.5f,  0.5f),       // Top-right vertex
                            new Vector3(-0.5f, -0.5f,  0.5f),       // Bottom-right vertex
                            new Vector3(-0.5f, -0.5f, -0.5f)]},     // Bottom-left vertex
            
            {Faces.Top,    [new Vector3(-0.5f,  0.5f, -0.5f),       // Top-left vertex
                            new Vector3( 0.5f,  0.5f, -0.5f),       // Top-right vertex
                            new Vector3( 0.5f,  0.5f,  0.5f),       // Bottom-right vertex
                            new Vector3(-0.5f,  0.5f,  0.5f)]},     // Bottom-left vertex
            
            {Faces.Bottom, [new Vector3(-0.5f, -0.5f,  0.5f),       // Top-left vertex
                            new Vector3( 0.5f, -0.5f,  0.5f),       // Top-right vertex
                            new Vector3( 0.5f, -0.5f, -0.5f),       // Bottom-right vertex
                            new Vector3(-0.5f, -0.5f, -0.5f)]}      // Bottom-left vertex
            
        };
    }
}