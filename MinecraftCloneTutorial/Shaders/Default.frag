#version 330 core

in vec2 texCoord;

// Send the color to the GPU
out vec4 FragColor;

uniform sampler2D texture0;

void main() {
    FragColor = texture(texture0, texCoord);
}