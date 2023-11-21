echo on
echo "compile&copy"
echo %1

if not exist %1\shaders mkdir %1\shaders
glslc shader.vert -o %1\shaders\vert.spv
glslc shader.frag -o %1\shaders\frag.spv

glslc shaderAlt.vert -o %1\shaders\vertAlt.spv
glslc shaderAlt.frag -o %1\shaders\fragAlt.spv

if not exist %1\textures mkdir %1\textures
copy texture.png %1\textures
