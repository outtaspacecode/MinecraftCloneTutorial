#version 330 core

// Vertex coordinates
layout (location = 0) in vec3 aPosition;
// Texture coordinates
layout (location = 1) in vec2 aTexCoord;

out vec2 texCoord;

// Uniform variables
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main() {
    gl_Position = vec4(aPosition, 1.0) * model * view * projection; // These *have* to be in this order
    texCoord = aTexCoord;
}