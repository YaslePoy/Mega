echo on
echo "compile&copy"
echo %1

if not exist %1\shaders mkdir %1\shaders
glslc shader.vert -o %1\shaders\vert.spv
echo "vert compiled"
glslc shader.frag -o %1\shaders\frag.spv
echo "frag compiled"

if not exist %1\textures mkdir %1\textures
copy texture.png %1\textures
