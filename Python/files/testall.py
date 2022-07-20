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

g = intersect(semicircle(5, 50, startAngle =90, endAngle = 135), semiellipse((3, 8), 50, 95, 140))
view(g, "Intersect(Semicircle, SemiEllipse)")

g = union(star(5, 10), square(8, (0,0)))
view(g, "Union(Star, Square)")

save("/tmp/test.svg", g)

g = colorize("red", star(5, 10))
view(g, "Red Star")

g = colorize((0,0,255), star(5, 10))
view(g, "Blue Star")

g = colorize((0,255, 0), CSCAD.Cube(4))
view(g, "Green Cube")

save("/tmp/test.stl", g)

assert(len(getColorNames()) == 147)

assert(version() == "0.5.0")

acos(0)

acosh(0)

asin(0)

asinh(0)

atan(0)

atan2(0, 0)

atanh(0)

cos(0)

cosh(0)

degToRad(0)

radToDeg(0)

sin(0)

sinh(0)

tan(0)

tanh(0)

waitForViewerTransfers()