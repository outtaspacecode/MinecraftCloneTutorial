#version 330 core

in vec2 texCoord;
in vec3 FragPos;

// Send the color to the GPU
out vec4 FragColor;

uniform sampler2D texture0;

void main() {
    // Calculate the normal of the fragment
    vec3 dx = dFdx(FragPos);
    vec3 dy = dFdy(FragPos);
    vec3 normal = normalize(cross(dx, dy));
    
    // Calculate the per-face shading. The x, y, and z components of faceLight represent the strength
    // of the light on the fragments facing in their respective axes (i.e. the x component only affects 
    // the x-facing fragments
    vec3 faceLight = vec3(0.65, 1.0, 0.8);
    float shading = abs(dot(normal, faceLight));
    
    FragColor = texture(texture0, texCoord) * shading;
}