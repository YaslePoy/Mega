import os
import sys

for entry in os.listdir('Shaders'):
    if entry.endswith(('.vert', '.frag')):
        os.system(f'glslc Shaders/{entry} -o {sys.argv[1]}/shaders/{entry + ".spv"}')