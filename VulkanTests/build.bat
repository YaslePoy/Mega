echo on
echo "compile&copy"
echo %1
if not exist %1 mkdir %1
glslc shader.vert -o %1\vert.spv
echo "vert compiled"
glslc shader.frag -o %1\frag.spv
echo "frag compiled"