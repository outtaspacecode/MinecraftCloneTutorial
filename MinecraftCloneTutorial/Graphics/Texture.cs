using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace MinecraftCloneTutorial.Graphics {
    internal class Texture {
        private readonly int _id;

        public Texture(string filepath) {
            _id = GL.GenTexture();
            
            // Activate the texture in the texture unit as texture 0
            GL.ActiveTexture(TextureUnit.Texture0);
            // Bind the texture so we can use it
            Bind();
            
            // Texture parameters
            // These first two make sure the texture repeats rather than stretching (S and T are the horizontal and vertical directions)
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            // These two set the mipmaps; this deals with scaling the image - it makes it so that the pixels aren't blurred
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            
            // Load texture image
            // If we don't do this, our textures will be flipped vertically (the 1 represents true)
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult dirtTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/" + filepath), ColorComponents.RedGreenBlueAlpha);
            
            // This just loads the texture for GL
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, dirtTexture.Width, dirtTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, dirtTexture.Data);
            
            // Unbind the texture
            Unbind();
        }
        
        public void Bind() { GL.BindTexture(TextureTarget.Texture2D, _id); }
        public void Unbind() { GL.BindTexture(TextureTarget.Texture2D, 0);}
        public void Delete() { GL.DeleteTexture(_id); }
    }
}

