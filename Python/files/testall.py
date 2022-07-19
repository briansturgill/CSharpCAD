from pycscad import *

g=circle(30)
view(g, "Circle")

g = subtract(ellipse((10, 20)), cutter2d(20, 90, 180))
view(g, "Subtract(Ellipse, Cutter2D)")

points = [
# roof
(10, 11), (0, 11), (5,20),
# wall
(0, 0), (10, 0), (10, 10), (0,10)
]
paths = [[0, 1, 2], [3,4,5,6]]
g = polygon(points, paths)
g.Validate()
view(g, "Polygon")

g = project(CSCAD.Cube(10))
view(g, "Project")

g = rectangle((5, 3))
view(g, "Rectangle")

g = roundedRectangle((5, 3), 1)
view(g, "RoundedRectangle")

g = semicircle(5, 50, startAngle =90, endAngle = 135)
view(g, "Semicircle")

print(getColorNames())


waitForViewerTransfers()