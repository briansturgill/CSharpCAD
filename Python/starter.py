from pycscad import *

g = circle(10)
view(g, "Circle")
save("/tmp/test.stl", extrudeLinear(g, 25))

waitForViewerTransfers()
