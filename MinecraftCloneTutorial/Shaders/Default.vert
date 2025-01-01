#version 330 core

// Vertex coordinates
layout (location = 0) in vec3 aPosition;
// Texture coordinates
layout (location = 1) in vec2 aTexCoord;

out vec2 texCoord;

void main() {
    gl_Position = vec4(aPosition, 1.0); // Coordinates
    texCoord = aTexCoord;
}