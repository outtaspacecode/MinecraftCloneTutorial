using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MinecraftCloneTutorial {
    internal class Camera {
        
        // Constants
        private float SPEED = 5.0f;
        private float SCREENWIDTH;
        private float SCREENHEIGHT;
        private float SENSITIVITY = 120.0f;
        
        // Position variables
        public Vector3 position;
        private Vector3 _up = Vector3.UnitY;
        private Vector3 _headTilt = -Vector3.UnitZ;
        private Vector3 _right = Vector3.UnitX;
        private Vector3 _forward = Vector3.UnitZ;
        
        // View rotations
        private float _pitch;
        private float _yaw = -90.0f;

        private bool _firstMove = true;
        private Vector2 _lastPos;
        

        public Camera(float width, float height, Vector3 position) {
            SCREENWIDTH = width;
            SCREENHEIGHT = height;
            this.position = position;
        }

        public Matrix4 GetViewMatrix() {
            return Matrix4.LookAt(position, position + _headTilt, _up); }
        
        public Matrix4 GetProjectionMatrix() {
            return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), SCREENWIDTH / SCREENHEIGHT, 0.1f, 500.0f);
        }

        private void UpdateVectors() {
            if (_pitch > 89.0f) {
                _pitch = 89.0f;
            }

            if (_pitch < -89.0) {
                _pitch = -89.0f;
            }
            
            _headTilt.X = MathF.Cos(MathHelper.DegreesToRadians(_pitch)) * MathF.Cos(MathHelper.DegreesToRadians(_yaw));
            _headTilt.Y = MathF.Sin(MathHelper.DegreesToRadians(_pitch));
            _headTilt.Z = MathF.Cos(MathHelper.DegreesToRadians(_pitch)) * MathF.Sin(MathHelper.DegreesToRadians(_yaw));

            _headTilt = Vector3.Normalize(_headTilt);

            _right = Vector3.Normalize(Vector3.Cross(_headTilt, Vector3.UnitY));
            _forward = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, _right));
        }

        public void InputController(KeyboardState input, MouseState mouse, FrameEventArgs e) {

            if (input.IsKeyDown(Keys.W)) {
                position += _forward * SPEED * (float)e.Time;
            }
            
            if (input.IsKeyDown(Keys.A)) {
                position -= _right * SPEED * (float)e.Time;
            }
            
            if (input.IsKeyDown(Keys.S)) {
                position -= _forward * SPEED * (float)e.Time;
            }
            
            if (input.IsKeyDown(Keys.D)) {
                position += _right * SPEED * (float)e.Time;
            }
            
            if (input.IsKeyDown(Keys.Space)) {
                position.Y += SPEED * (float)e.Time;
            }
            
            if (input.IsKeyDown(Keys.LeftShift)) {
                position.Y -= SPEED * (float)e.Time;
            }

            _yaw += mouse.Delta.X * SENSITIVITY * (float)e.Time; 
            _pitch -= mouse.Delta.Y * SENSITIVITY * (float)e.Time;

            if (input.IsKeyDown(Keys.LeftControl)) {
                SPEED = 18.0f;
            } else {
                SPEED = 5.0f;
            }
            UpdateVectors();
        }

        public void Update(KeyboardState input, MouseState mouse, FrameEventArgs e) {
            InputController(input, mouse, e);
        }
    }
}