echo on
echo "compile&copy"
echo %1

if not exist %1\shaders mkdir %1\shaders
glslc shaders\shader.vert -o %1\shaders\vert.spv
glslc shaders\shader.frag -o %1\shaders\frag.spv

glslc shaders\shaderAlt.vert -o %1\shaders\vertAlt.spv
glslc shaders\shaderAlt.frag -o %1\shaders\fragAlt.spv

if not exist %1\textures mkdir %1\textures
copy texture.png %1\textures
