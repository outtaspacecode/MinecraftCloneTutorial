#version 330 core

// Vertex coordinates
layout (location = 0) in vec3 aPosition;
// Texture coordinates
layout (location = 1) in vec2 aTexCoord;

out vec2 texCoord;
out vec3 FragPos;

// Uniform variables
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main() {
    FragPos = (vec4(aPosition, 1.0) * model).xyz;
    gl_Position = vec4(FragPos, 1.0) * view * projection; // These *have* to be in this order
    texCoord = aTexCoord;
}