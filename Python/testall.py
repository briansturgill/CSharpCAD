from pycscad import *

g = circle(30)
view(g, "Circle")

g = subtract(ellipse((10, 20)), cutter2d(20, 90, 180))
view(g, "Subtract(Ellipse, Cutter2D)")

points = [
    # roof
    (10, 11), (0, 11), (5, 20),
    # wall
    (0, 0), (10, 0), (10, 10), (0, 10)
]
paths = [[0, 1, 2], [3, 4, 5, 6]]
g = polygon(points, paths)
# LATER g.Validate()
view(g, "Polygon")

g = project(cube(10))
view(g, "Project")

g = rectangle((5, 3))
view(g, "Rectangle")

g = roundedRectangle((5, 3), 1)
view(g, "RoundedRectangle")

g = semicircle(5, 50, startAngle=90, endAngle=135)
view(g, "Semicircle")

g = intersect(semicircle(5, 50, startAngle=90, endAngle=135),
              semiellipse((3, 8), 50, 95, 140))
view(g, "Intersect(Semicircle, SemiEllipse)")

g = union(star(5, 10), square(8, (0, 0)))
view(g, "Union(Star, Square)")

save("/tmp/test.svg", g)

g = colorize("red", star(6, 10))
view(g, "Red Star")

g = colorize((0, 0, 255), star(5, 10))
view(g, "Blue Star")

g = colorize((0, 255, 0), cube(4))
view(g, "Green Cube")

save("/tmp/test.stl", g)

assert(len(getColorNames()) == 147)

assert(version() == "0.8.0")

g = center(g, relativeTo=(10, 10, 10))
g = centerX(g)
g = centerY(g)
g = centerZ(g)
view(g, "GC centered on 10,10,10")

g = center(circle(), relativeTo=(10, 10, 10))
view(g, "Circle centered on 10,10,10")

g = expand(rectangle((10, 20)), 10, Corners.Round, segments=32)
view(g, "Expand rect")

g = cuboid((20, 25, 5), (0, 0, 15))
g = union(g, mirror(g, normal=(0, 0, 10)))
g = mirrorX(g)
g = mirrorY(g)
g = mirrorZ(g)
view(g, "Mirror around 0,0,10")

g = star(5)
save("/tmp/star.svg", g)
g = union(g, mirror(g, origin=(10, 10, 10), normal=(1, 0, 0)))
view(g, "Mirror around 1,0,0")


def round_rect(x, y, radius):
    return hull(
        translate((radius, radius), circle(radius)),
        translate((x-radius, radius), circle(radius)),
        translate((radius, y-radius), circle(radius)),
        translate((x-radius, y-radius), circle(radius))
    )


g = round_rect(20, 40, 4)
view(g, "round_rect")

g = hullChain(rectangle(center=(-5, -5)),
              circle(center=(0, 0)), rectangle(center=(5, 5)))
view(g, "HullChain")

g = offset(rectangle((10, 20)), 10, Corners.Round, segments=32)
view(g, "Offset rect")

m = [[1, 0, 0, 0], [0, 1, 0, 0], [0, 0, 1, 0], [0, 0, 0, 1]]
g = transform(m, g)
view(g, title="Transform by identity")

g = transform(m, cube(5))
g = rotate((45, 45, 45), g)
g = rotateX(45, g)
g = rotateY(45, g)
g = rotateZ(45, g)
view(g, title="Rotate by 45 twice")

g = transform(m, cube(5))
g = scale([0.5, 0.5, 0.5], g)
g = scaleX(0.5, g)
g = scaleY(0.5, g)
g = scaleZ(0.5, g)
view(g, title="Scale by 0.25")

g = translate((20, 20, 20), g)
view(g, title="Translate by 20")

g = translate((20, 20, 20), g)
g = translateX(10, g)
g = translateY(10, g)
g = translateZ(10, g)
view(g, title="Translate by 50")

g = cone()
view(g, title="Cone")

g = cube()
view(g, title="cube")

g = cuboid((2, 5, 2))
view(g, title="cuboid")

g = subtract(sphere(), cutter3d())
view(g, title="Subtract(Sphere, Cutter3D)")

g = cylinder()
view(g, title="Cylinder")

g = cuboid()
view(g, title="Cuboid")

g = ellipsoid()
view(g, title="Ellipsoid")

g = ellipticCylinder()
view(g, title="EllipticCylinder")

g = extrudeLinear(circle())
view(g, title="ExtrudeLinear(Circle)")

g = subtract(
    roundedRectangle((40, 40), roundRadius=10, center=(0, 0), segments=50),
    roundedRectangle((36, 36), roundRadius=8, center=(0, 0), segments=50))
view(g, "Twisty base")
g = extrudeTwist(g, 60, twistAngle=90, twistSteps=60)
view(g, "Twisty")

g = extrudeRotate(circle(radius = 3, center = (4, 0)), segments = 8, angle = 180)
view(g, "ExtrudeRotate(Circle)")
    
g = geodesicSphere()
view(g, "Geodesic Sphere")

points = [(10, 10, 0), (10, -10, 0), (-10, -10, 0), (-10, 10, 0), (0, 0, 10)]
faces = [[0, 1, 4],  [1, 2, 4 ], [2, 3, 4], [3, 0, 4], [1, 0, 3], [2, 1, 3]]
g = polyhedron(points, faces, False)
view(g, "Polyhedron")

g = roundedCuboid()
view(g, "RoundedCuboid")

g = roundedCylinder()
view(g, "roundedCylinder")

g = semicylinder()
view(g, "semicylinder")

g = semiellipticCylinder()
view(g, "semiellipticCylinder")

g = sphere()
view(g, title="Sphere")

g = torus()
view(g, title="Torus")

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
