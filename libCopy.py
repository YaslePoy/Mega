import sys, shutil

print("copy", sys.argv)

shutil.copy(sys.argv[1], sys.argv[2])