echo on
echo "compile&copy"
echo %1

if not exist %1\shaders mkdir %1\shaders
glslc Shaders\shader.vert -o %1\shaders\vert.spv
glslc Shaders\shader.frag -o %1\shaders\frag.spv

glslc Shaders\shaderAlt.vert -o %1\shaders\vertAlt.spv
glslc Shaders\shaderAlt.frag -o %1\shaders\fragAlt.spv

if not exist %1\textures mkdir %1\textures
copy texture.png %1\textures
