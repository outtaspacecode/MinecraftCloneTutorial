using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MinecraftCloneTutorial {
    internal class Camera {
        
        // Constants
        private float SPEED = 4.2f;
        private float SCREENWIDTH;
        private float SCREENHEIGHT;
        private float SENSITIVITY = 120.0f;
        
        // Position variables
        public Vector3 position;
        private Vector3 _up = Vector3.UnitY;
        private Vector3 _front = -Vector3.UnitZ;
        private Vector3 _right = Vector3.UnitX;
        
        // View rotations
        private float _pitch;
        private float _yaw = -90.0f;

        private bool _firstMove = true;
        public Vector2 lastPos;
        

        public Camera(float width, float height, Vector3 position) {
            SCREENWIDTH = width;
            SCREENHEIGHT = height;
            this.position = position;
        }

        public Matrix4 GetViewMatrix() {
            return Matrix4.LookAt(position, position + _front, _up); }
        
        public Matrix4 GetProjectionMatrix() {
            return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), SCREENWIDTH / SCREENHEIGHT, 0.1f, 100.0f);
        }

        private void UpdateVectors() {
            if (_pitch > 89.0f) {
                _pitch = 89.0f;
            }

            if (_pitch < -89.0) {
                _pitch = -89.0f;
            }
            
            _front.X = MathF.Cos(MathHelper.DegreesToRadians(_pitch)) * MathF.Cos(MathHelper.DegreesToRadians(_yaw));
            _front.Y = MathF.Sin(MathHelper.DegreesToRadians(_pitch));
            _front.Z = MathF.Cos(MathHelper.DegreesToRadians(_pitch)) * MathF.Sin(MathHelper.DegreesToRadians(_yaw));

            _front = Vector3.Normalize(_front);

            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            // _up = Vector3.Normalize(Vector3.Cross(_right, _front));
        }

        public void InputController(KeyboardState input, MouseState mouse, FrameEventArgs e) {

            if (input.IsKeyDown(Keys.W)) {
                position += _front * SPEED * (float)e.Time;
            }
            
            if (input.IsKeyDown(Keys.A)) {
                position -= _right * SPEED * (float)e.Time;
            }
            
            if (input.IsKeyDown(Keys.S)) {
                position -= _front * SPEED * (float)e.Time;
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

            if (_firstMove) {
                lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            } else {
                var deltaX = mouse.X - lastPos.X;
                var deltaY = mouse.Y - lastPos.Y;
                lastPos = new Vector2(mouse.X, mouse.Y);

                _yaw += deltaX * SENSITIVITY * (float)e.Time;
                _pitch -= deltaY * SENSITIVITY * (float)e.Time;
            }

            UpdateVectors();
        }

        public void Update(KeyboardState input, MouseState mouse, FrameEventArgs e) {
            InputController(input, mouse, e);
        }
    }
}