from clr_loader import get_coreclr
from pythonnet import set_runtime
rt = get_coreclr("./runtimeconfig.json")
set_runtime(rt)

import clr
from System import String
from System.Collections.Generic import List
clr.AddReference("CSharpCAD")
from CSharpCAD import CSCAD
from CSharpCAD import Geom2, Geom3, Vec2, Vec3, Mat4

def circle(radius: float = 1.0, segments: int = 32, center: tuple[float, float] = (0, 0)) -> Geom2:
    """Construct a circle in 2D space.

    Returns: Geom2

    Group: 2D Primitives
    """

    cent = Vec2(float(center[0]), float(center[1]))
    return CSCAD.Circle(float(radius), int(segments), cent)


def cutter2d(radius: float = 1.0, startAngle: float = 0, endAngle: float = 90, center: tuple[float, float] = (0, 0)) -> Geom2:
    """Construct a "cutter".
    A negative geometry object, suitable to cut a "pie slice" out of a circlular 2d object of radius r.

    This object would normally be used as a second argument to "Subtract".

    To use with an elliptical object, specify radius as the largest radius of the elliptical.

    Returns: Geom2

    Group: 2D Primitives
    """

    cent = Vec2(float(center[0]), float(center[1]))
    return CSCAD.Cutter2D(float(radius), float(startAngle), float(endAngle), cent)


def ellipse(radius: tuple[float, float] = (1, 1), segments: int = 32, center: tuple[float, float] = (0, 0)) -> Geom2:
    """Construct an axis-aligned ellipse in two dimensional space.

    Returns: Geom2

    Group: 2D Primitives
    """

    rad = Vec2(float(radius[0]), float(radius[1]))
    cent = Vec2(float(center[0]), float(center[1]))
    return CSCAD.Ellipse(rad, int(segments), cent)


def polygon(points: list[tuple[float, float]], paths: list[list[int]] = None) -> Geom2:
    """Construct a polygon in two dimensional space from a list of points, or a list of points and paths.

    NOTE: The ordering of points is VERY IMPORTANT.

    Polygon points must be in counter-clockwise order, all points coplanar.

    Holes inside the paths must be in clockwise order.

    Use the ".Validate()" method on the resulting Geom2 to check if the data is good.

    If "paths" is omitted, all points will be used in a single polygon in the order given.

    Returns: Geom2

    Group: 2D Primitives
    """

    pts2 = CSCAD.Points2()
    for pt in points:
        pts2.Add(Vec2(float(pt[0]), float(pt[1])))

    pths = CSCAD.Paths()
    for path in paths:
        pth = CSCAD.Path()
        for idx in path:
            pth.Add(idx)
        pths.Add(pth)
    return CSCAD.Polygon(pts2, pths)


def project(gobj: Geom3, axis: tuple[float, float, float] = (0, 0, 1), origin: tuple[float, float, float] = (0, 0, 0)) -> Geom2:
    """Project 3D object to 2D.

    Returns: Geom2

    Group: 2D Primitives
    """

    a = Vec3(float(axis[0]), float(axis[1]), float(axis[2]))
    o = Vec3(float(origin[0]), float(origin[1]), float(origin[2]))
    return CSCAD.Project(gobj, a, o)


def rectangle(size: tuple[float, float] = (2, 2), center: tuple[float, float] = None):
    """Construct an axis-aligned rectangle in 2D space with four sides at right angles.

    The default center point is selected such that the bottom left corner of the rectangle is (0,0).

    (The rectangle is entirely in the first quadrant.)

    Returns: Geom2

    Group: 2D Primitives
    """

    s = Vec2(size[0], size[1])
    if center == None:
        c = None
    else:
        c = Vec2(center[0], center[1])
    return CSCAD.Rectangle(s, c)


def roundedRectangle(size: tuple[float, float] = (2, 2), roundRadius: float = 0.2, segments: int = 32, center: tuple[float, float] = None):
    """Construct an axis-aligned rectangle in 2D space with rounded corners.

    The default center point is selected such that the bottom left corner of the rectangle is (0,0).

    (The rectangle is entirely in the first quadrant.)

    Returns: Geom2

    Group: 2D Primitives
    """

    s = Vec2(size[0], size[1])
    if center == None:
        c = None
    else:
        c = Vec2(center[0], center[1])
    return CSCAD.RoundedRectangle(s, float(roundRadius), int(segments), c)


def semicircle(radius: float = 1.0, segments: int = 32, startAngle: float = 0, endAngle: float = 90, center: tuple[float, float] = (0, 0)) -> Geom2:
    """Construct a partial circle in 2D space.

    Returns: Geom2

    Group: 2D Primitives
    """

    cent = Vec2(float(center[0]), float(center[1]))
    return CSCAD.Semicircle(float(radius), int(segments), float(startAngle), float(endAngle), cent)


def semiellipse(radius: tuple[float, float] = (1, 1), segments: int = 32, startAngle: float = 0, endAngle: float = 90, center: tuple[float, float] = (0, 0)) -> Geom2:
    """Construct an axis-aligned ellipse in two dimensional space.

    Returns: Geom2

    Group: 2D Primitives
    """

    rad = Vec2(float(radius[0]), float(radius[1]))
    cent = Vec2(float(center[0]), float(center[1]))
    return CSCAD.Semiellipse(rad, segments, startAngle, endAngle, cent)


def square(size: float = 2, center: tuple[float, float] = None) -> Geom2:
    """A Rectangle with the same x and y dimensions.

    The default center point is selected such that the bottom left corner of the square is (0,0).

    (The square is entirely in the first quadrant.)

    Returns: Geom2

    Group: 2D Primitives
    """

    if center == None:
        c = None
    else:
        c = Vec2(center[0], center[1])

    return CSCAD.Square(size, c)


def star(vertices: int = 5, outerRadius: float = 1, innerRadius: float = 0, density: int = 2, startAngle: float = 0, center: tuple[float, float] = (0, 0)) -> Geom2:
    """Construct a star in two dimensional space.

    Returns: Geom2

    Group: 2D Primitives
    """

    cent = Vec2(float(center[0]), float(center[1]))
    return CSCAD.Star(vertices, outerRadius, innerRadius, density, startAngle, cent)

def cone(top:float = 2, bottom:float = 10, height:float = 20, segments:int = 32, center:tuple[float, float, float] = None) -> Geom3:
    """Construct a Z axis-aligned cone in three dimensional space.

    The default center places the cone's base at (0,0,0).

    Returns: Geom3

    Group: 3D Primitives
    """

    if center == None:
        center = (0,0,height/2)

    cent = Vec3(float(center[0]), float(center[1]), float(center[2]))
    return CSCAD.Cone(top, bottom, height, segments, cent)

def cube(size:float = 2, center:tuple[float,float,float] = None) -> Geom3:
    """Construct an axis-aligned solid cube in three dimensional space with six square faces.

    You may have actually wanted to use a Cuboid.

    To be clear, this makes a "square" cube.
    
    The default center point is selected such that the bottom left corner of the cube is (0,0,0).
    
    (The cube is entirely in the first quadrant.)

    Returns: Geom3

    Group: 3D Primitives
    """

    if center == None:
        center = (size/2,size/2,size/2)
    cent = Vec3(float(center[0]), float(center[1]), float(center[2]))
    return CSCAD.Cube(size, cent)

def cuboid(size:tuple[float,float,float] = (2,2,2), center:tuple[float,float,float] = None) -> Geom3:
    """Construct an axis-aligned solid cuboid in three dimensional space.

    The default center point is selected such that the bottom left corner of the cuboid is (0,0,0).
    
    (The cuboid is entirely in the first quadrant.)

    Returns: Geom3

    Group: 3D Primitives
    """

    if center == None:
        center = (size[0]/2,size[1]/2,size[2]/2)
    sz = Vec3(float(size[0]), float(size[1]), float(size[2]))
    cent = Vec3(float(center[0]), float(center[1]), float(center[2]))
    return CSCAD.Cuboid(sz, cent)

def cutter3d(radius:float = 1, height:float = None, startAngle:float = 0, endAngle:float = 90,
    center:tuple[float,float,float] = (0,0,0)) -> Geom3:
    """Construct a "cutter", a negative geometry object, suitable to cut a "pie slice" out of a circlular 3D object of radius r.

    Argument "height" defaults to (radius*2+2) and usually will not need to be changed.

    This object would normally be used as a second argument to "Subtract".

    To use with an elliptical object, specify radius as the largest radius of the elliptical.

    Returns: Geom3

    Group: 3D Primitives
    """

    if height == None:
        height = radius*2+2
    cent = Vec3(float(center[0]), float(center[1]), float(center[2]))
    return CSCAD.Cutter3D(radius, height, startAngle, endAngle, cent)

def cylinder(radius:float = 1, height:float = 2, segments:int = 32, center:tuple[float,float,float] = None) -> Geom3:
    """Construct a Z axis-aligned cylinder in three dimensional space.

    The default center places the cylinder's base at (0,0,0).

    Returns: Geom3

    Group: 3D Primitives
    """

    if center == None:
        center = (0,0,height/2)
    cent = Vec3(float(center[0]), float(center[1]), float(center[2]))
    return CSCAD.Cylinder(radius, height, segments, cent)

def ellipsoid(radius:tuple[float,float,float] = (1,1,1), segments:int = 32,
    axes_x:tuple[float,float,float] = (1,0,0), axes_y:tuple[float,float,float] = (0,-1,0),
    axes_z:tuple[float,float,float] = (0,0,1), center:tuple[float,float,float] = (0,0,0)) -> Geom3:
    """Construct an axis-aligned ellipsoid in three dimensional space.

    Returns: Geom3

    Group: 3D Primitives
    """

    rad = Vec3(float(radius[0]), float(radius[1]), float(radius[2]))
    ax = Vec3(float(axes_x[0]), float(axes_x[1]), float(axes_x[2]))
    ay = Vec3(float(axes_y[0]), float(axes_y[1]), float(axes_y[2]))
    az = Vec3(float(axes_z[0]), float(axes_z[1]), float(axes_z[2]))
    cent = Vec3(float(center[0]), float(center[1]), float(center[2]))
    return CSCAD.Ellipsoid(rad, segments, ax, ay, az, cent)

def ellipticCylinder(radius:tuple[float,float] = (1,1), height:float = 2, segments:int = 32, center:tuple[float,float,float] = None) -> Geom3:
    """Construct a Z axis-aligned elliptic cylinder in three dimensional space.

    The default center places the elliptic cylinder's base at (0,0,0).

    Returns: Geom3

    Group: 3D Primitives
    """

    if center == None:
        center = (0,0,height/2)
    rad = Vec2(float(radius[0]), float(radius[1]))
    cent = Vec3(float(center[0]), float(center[1]), float(center[2]))
    return CSCAD.EllipticCylinder(rad, height, segments, cent)

def extrudeLinear(gobj:Geom2, height:float = 1, twistAngle:float = 0, twistSteps:int = 1, repair:bool = True) -> Geom3:
    """Extrude the given geometry in an upward linear direction using the given options.

    Returns: Geom3

    Group: 3D Primitives
    """

    return CSCAD.ExtrudeLinear(gobj, height, twistAngle, twistSteps, repair)

def extrudeRotate(gobj: Geom2, segments:int = 12, startAngle:float = 0, angle:float = 360) -> Geom3:
    """Rotate extrude the given geometry using the given options.

    Returns: Geom3

    Group: 3D Primitives
    """

    return CSCAD.ExtrudeRotate(gobj, segments, startAngle, angle)

def geodesicSphere(radius:float = 1, frequency:int = 6) -> Geom3:
    """Construct a geodesic sphere based on icosahedron symmetry.

    Returns: Geom3

    Group: 3D Primitives
    """

    return CSCAD.GeodesicSphere(radius, frequency)

def polyhedron(points:list[list[float]],faces:list[list[list[int]]], orientationOutward:bool = True) -> Geom3:
    """Construct a polyhedron in 3D space from the given set of 3D points and faces.

    Returns: Geom3

    Group: 3D Primitives
    """

    pts3 = CSCAD.Points3()
    for pt in points:
        pts3.Add(Vec3(float(pt[0]), float(pt[1]), float(pt[2])))

    fces = CSCAD.Faces()
    for face in faces:
        fce = CSCAD.Face()
        for idx in face:
            fce.Add(idx)
        fces.Add(fce)
    
    return CSCAD.Polyhedron(pts3, fces, None, orientationOutward)

def roundedCuboid(size = (2,2,2), roundRadius:float = 0.2, segments:int = 32, center = None) -> Geom3:
    """Construct an axis-aligned solid cuboid in three dimensional space with rounded corners.

    The default center point is selected such that the bottom left corner of the cuboid is (0,0,0).
    
    (The cuboid is entirely in the first quadrant.)

    Returns: Geom3

    Group: 3D Primitives
    """

    if center == None:
        center = (size[0]/2,size[1]/2,size[2]/2)
    sz = Vec3(float(size[0]), float(size[1]), float(size[2]))
    cent = Vec3(float(center[0]), float(center[1]), float(center[2]))
    return CSCAD.RoundedCuboid(sz, roundRadius, segments, cent)

def roundedCylinder(radius:float = 1, height:float = 2, roundRadius:float = 0.2, segments:int = 32,
    center:tuple[float,float,float] = None) -> Geom3:
    """Construct a Z axis-aligned cylinder in three dimensional space with rounded top and bottom.

    The default center places the rounded cylinder's base at (0,0,0).

    Returns: Geom3

    Group: 3D Primitives
    """

    if center == None:
        center = (0,0,height/2)
    cent = Vec3(float(center[0]), float(center[1]), float(center[2]))
    return CSCAD.RoundedCylinder(radius, height, roundRadius, segments, cent)

def semicylinder(radius:float = 1, height:float = 2, segments:int = 32,
    startAngle:float = 0, endAngle:float = 90, center:tuple[float,float,float] = None) -> Geom3:
    """Construct a Z axis-aligned semicylinder in three dimensional space.

    The default center places the semicylinder's base at (0,0,0).

    Returns: Geom3

    Group: 3D Primitives
    """

    if center == None:
        center = (0,0,height/2)
    cent = Vec3(float(center[0]), float(center[1]), float(center[2]))
    return CSCAD.Semicylinder(radius, height, segments, startAngle, endAngle, cent)

def semiellipticCylinder(radius:tuple[float,float] = (1,1), height:float = 2, segments:int = 32,
    startAngle:float = 0, endAngle:float = 90, center:tuple[float,float,float] = None) -> Geom3:
    """Construct a Z axis-aligned semiellipticCylinder in three dimensional space.

    The default center places the semiellipticCylinder's base at (0,0,0).

    Returns: Geom3

    Group: 3D Primitives
    """

    rad = Vec2(float(radius[0]), float(radius[1]))
    if center == None:
        center = (0,0,height/2)
    cent = Vec3(float(center[0]), float(center[1]), float(center[2]))
    return CSCAD.SemiellipticCylinder(rad, height, segments, startAngle, endAngle, cent)

def sphere(radius:float = 1, segments:int = 32,
    axes_x:tuple[float,float,float] = (1,0,0), axes_y:tuple[float,float,float] = (0,-1,0),
    axes_z:tuple[float,float,float] = (0,0,1), center:tuple[float,float,float] = (0,0,0)) -> Geom3:
    """Construct a sphere in 3D space where all points are at the same distance from the center.

    Returns: Geom3

    Group: 3D Primitives
    """

    ax = Vec3(float(axes_x[0]), float(axes_x[1]), float(axes_x[2]))
    ay = Vec3(float(axes_y[0]), float(axes_y[1]), float(axes_y[2]))
    az = Vec3(float(axes_z[0]), float(axes_z[1]), float(axes_z[2]))
    cent = Vec3(float(center[0]), float(center[1]), float(center[2]))
    return CSCAD.Sphere(radius, segments, ax, ay, az, cent)

def torus(innerRadius:float = 1, innerSegments:int = 32, outerRadius:float = 4, outerSegments:int = 32,
    innerRotation:float = 0, startAngle:float = 0, outerRotation:float = 360) -> Geom3:
    """Construct a torus by revolving a small circle (inner) about the circumference of a large (outer) circle.

    Returns: Geom3

    Group: 3D Primitives
    """

    return CSCAD.Torus(innerRadius, innerSegments, outerRadius, outerSegments, innerRotation, startAngle, outerRotation)



def intersect(*gobjs) -> Geom2 | Geom3:
    """Return a new geometry representing space in both the first geometry and all subsequent geometries.

    Returns: Geom2 | Geom3

    Group: Booleans
    """

    return CSCAD.Intersect(gobjs)

def subtract(*gobjs) -> Geom2 | Geom3:
    """Return a new geometry representing space in the first geometry but not in all subsequent geometries.

    Returns: Geom2 | Geom3

    Group: Booleans
    """

    return CSCAD.Subtract(gobjs)

def union(*gobjs) -> Geom2 | Geom3:
    """Return a new geometry representing the total space in the given geometries.

    Returns: Geom2 | Geom3

    Group: Booleans
    """

    return CSCAD.Union(gobjs)

def colorize(color: str | tuple[int, int, int] | tuple[int, int, int, int], gobj: Geom2 | Geom3) -> Geom2 | Geom3:
    """Assign the given color to the given objects.

    Color has 3 formats:
    * A tuple of RGB or RGBA color values, where each value is between 0 and 255.
    * A string beginning with "#" followed by 6 hex digits representing RGB.
    * A string that is one of the extended CSS color names.

    Returns: Geom2 | Geom3

    Group: Miscellaneous
    """

    if type(color) is str:
        color = CSCAD.Color(color)
    elif len(color) == 3:
        color = CSCAD.Color(color[0], color[1], color[2])
    elif len(color) == 4:
        color = CSCAD.Color(color[0], color[1], color[2], color[3])
    return CSCAD.Colorize(color, gobj)

def getColorNames() -> list[str]:
    """

    Returns: Geom2 | Geom3

    Group: Miscellaneous
    """
    l = []
    strArray = CSCAD.GetColorNames()
    for s in strArray:
        l.append(s)
    return l

def save(model_filename: str, gobj: Geom2 | Geom3):
    """Save a geometry object in an external format.

    Group: Miscellaneous
    """

    CSCAD.Save(model_filename, gobj)

def version() -> str:
    """Returns version string for CSCAD.

    Returns: str

    Group: Miscellaneous
    """

    return CSCAD.Version();

def view(gobj: Geom2 | Geom3, title: str = "") -> Geom2 | Geom3:
    """Use CADView protocol to view the given object, labeled with "title".

    This function returns the gobj unchanged.

    This is useful for when you need to see a returned value:

    return circle(...)

    Just add:

    return view(circle(...))

    Returns: Geom2 | Geom3

    Group: Miscellaneous
    """

    return CSCAD.View(gobj, title)

def waitForViewerTransfers(verbose:bool = True):
    """Wait until all "view" transfers have completed.

    To make things work faster, Views are queued and usually have not
    all transfered by the time you are finished calculating your models.

    Place a call to waitForViewerTransfers() at the end of your program, otherwise
    not all that you "viewed" will reach your CADViewer.

    By default you will recieve status messages about the pending transfers.
    (Pass verbose as False if you don't like that behavior.)

    Group: Miscellaneous
    """

    CSCAD.WaitForViewerTransfers(verbose)


def center(gobj:Geom2 | Geom3, axisX: bool = True, axisY:bool = True, axisZ:bool = True, relativeTo: tuple[float, float, float] = (0,0,0)) -> Geom2 | Geom3:
    """Center the given object about the specified axes.
    For Geom2 objects, axisZ and the Z point in relativeTo can be omitted and will be ignored.

    Returns: Geom2 | Geom3

    Group: Transformations
    """

    if type(gobj) is Geom2:
        rt = Vec2(float(relativeTo[0]), float(relativeTo[1]))
        return CSCAD.Center(gobj, axisX, axisY, rt)
    else:
        rt = Vec3(float(relativeTo[0]), float(relativeTo[1]), float(relativeTo[2]))
        return CSCAD.Center(gobj, axisX, axisY, axisZ, rt)


def centerX(obj: Geom2 | Geom3) -> Geom2 | Geom3:
    """Center the given object about the X axis.

    Returns: Geom2 | Geom3

    Group: Transformations
    """

    return CSCAD.CenterX(obj)


def centerY(obj: Geom2 | Geom3) -> Geom2 | Geom3:
    """Center the given object about the Y axis.

    Returns: Geom2 | Geom3

    Group: Transformations
    """

    return CSCAD.CenterY(obj)


def centerZ(obj: Geom3) -> Geom3:
    """Center the given object about the Z axis.

    Returns: Geom2 | Geom3

    Group: Transformations
    """

    return CSCAD.CenterZ(obj)

class Corners:
    Edge = 0
    Chamfer = 1
    Round = 2

def expand(gobj:Geom2, delta:float = 1, corners:Corners = Corners.Edge, segments:int = 16) -> Geom2:
    """Expand (delta < 0, contract) the given geometry using the given options.

    Returns: Geom2

    Group: Transformations
    """

    return CSCAD.Expand(gobj, delta, CSCAD.__int_to_Corners(corners), segments)

def hull(*geometries: Geom2 | Geom3) -> Geom2 | Geom3:
    """Create a convex hull of the given geometries.

    Returns: Geom2 | Geom3

    Group: Transformations
    """

    return CSCAD.Hull(geometries)

def hullChain(*gobjs:Geom2 | Geom3) -> Geom2 | Geom3:
    """Create a chain of hulled geometries from the given geometries.

    Essentially hull A+B, B+C, C+D, etc., then union the results.

    Returns: Geom2 | Geom3

    Group: Transformations
    """

    return CSCAD.HullChain(gobjs)

def mirror(g:Geom2 | Geom3, origin:tuple[float,float,float] = (0,0,0), normal:tuple[float,float,float] = (0,0,1)) -> Geom2 | Geom3:
    """Mirror the given object using the given options.

    Returns: Geom2 | Geom3

    Group: Transformations
    """

    o = Vec3(float(origin[0]), float(origin[1]), float(origin[2]))
    n = Vec3(float(normal[0]), float(normal[1]), float(normal[2]))
    return CSCAD.Mirror(g, o, n)

def mirrorX(g:Geom2 | Geom3, origin:tuple[float,float,float] = (0,0,0)) -> Geom2 | Geom3:
    """Mirror the given object around the X axis using the given options.

    Returns: Geom2 | Geom3

    Group: Transformations
    """

    o = Vec3(float(origin[0]), float(origin[1]), float(origin[2]))
    return CSCAD.MirrorX(g, o)

def mirrorY(g:Geom2 | Geom3, origin:tuple[float,float,float] = (0,0,0)) -> Geom2 | Geom3:
    """Mirror the given object around the Y axis using the given options.

    Returns: Geom2 | Geom3

    Group: Transformations
    """

    o = Vec3(float(origin[0]), float(origin[1]), float(origin[2]))
    return CSCAD.MirrorY(g, o)

def mirrorZ(g:Geom2 | Geom3, origin:tuple[float,float,float] = (0,0,0)) -> Geom2 | Geom3:
    """Mirror the given object around the Z axis using the given options.

    Returns: Geom2 | Geom3

    Group: Transformations
    """

    o = Vec3(float(origin[0]), float(origin[1]), float(origin[2]))
    return CSCAD.MirrorZ(g, o)

def offset(gobj:Geom2, delta:float = 1, corners:Corners = Corners.Edge, segments:int = 16) -> Geom2:
    """Offset (delta < 0, inset) the given geometry using the given options.

    Offset is different from "expand" in that expand, "expands" both contours and holes.
    If "offset" is used, solids are expanded and holes are contracted (or vice-versa if delta < 0).
    If the Geom2 has no holes, then the two functions behave exactly the same.

    Returns: Geom2

    Group: Transformations
    """

    return CSCAD.Offset(gobj, delta, CSCAD.__int_to_Corners(corners), segments)

def rotate(angles:tuple[float,float,float], gobj:Geom2 | Geom3) -> Geom2 | Geom3:
    """Rotate the given object using the given angles (in DEGREES).

    Returns: Geom2 | Geom3

    Group: Transformations
    """

    a = Vec3(float(angles[0]), float(angles[1]), float(angles[2]))
    return CSCAD.Rotate(a, gobj)

def rotateX(angle:float, gobj:Geom3) -> Geom3:
    """Rotate the given object along the X axis using the given angle (in DEGREES).

    Returns: Geom3

    Group: Transformations
    """

    return CSCAD.RotateX(float(angle), gobj)

def rotateY(angle:float, gobj:Geom3) -> Geom3:
    """Rotate the given object along the Y axis using the given angle (in DEGREES).

    Returns: Geom3

    Group: Transformations
    """

    return CSCAD.RotateY(float(angle), gobj)

def rotateZ(angle:float, gobj:Geom2 | Geom3) -> Geom2 | Geom3:
    """Rotate the given object along the Z axis using the given angle (in DEGREES).

    Returns: Geom2 | Geom3

    Group: Transformations
    """

    return CSCAD.RotateZ(float(angle), gobj)

def scale(factors:tuple[float,float,float], gobj:Geom2 | Geom3) -> Geom2 | Geom3:
    """Scale the given geometry object using the given factors.

    For Geom2, the third "Z" factor is unnecessary and is ignored.

    Returns: Geom2 | Geom3

    Group: Transformations
    """

    if type(gobj) is Geom2:
        f = Vec2(float(factors[0]), float(factors[1]))
    else:
        f = Vec3(float(factors[0]), float(factors[1]), float(factors[2]))
    return CSCAD.Scale(f, gobj)

def scaleX(factor:float, gobj:Geom2 | Geom3) -> Geom2 | Geom3:
    """Scale the given geometry object along the X axis using the given factor.

    Returns: Geom2 | Geom3

    Group: Transformations
    """

    return CSCAD.ScaleX(float(factor), gobj)

def scaleY(factor:float, gobj:Geom2 | Geom3) -> Geom2 | Geom3:
    """Scale the given geometry object along the Y axis using the given factor.

    Returns: Geom2 | Geom3

    Group: Transformations
    """

    return CSCAD.ScaleY(float(factor), gobj)

def scaleZ(factor:float, gobj:Geom3) -> Geom3:
    """Scale the given geometry object along the Z axis using the given factor.

    Returns: Geom2

    Group: Transformations
    """

    return CSCAD.ScaleZ(float(factor), gobj)

def transform(matrix:list[list[float]], gobj:Geom2|Geom3) -> Geom2 | Geom3:
    """Transform the geometry object using the given matrix.

    Returns: Geom2

    Group: Transformations
    """

    m = matrix
    if len(m) != 4 or len(m[0]) != 4 or len(m[1]) != 4 or len(m[2]) != 4 or len(m[3]) != 4:
        raise Exception("Matrix argument must be a 4 by 4 array of float.")
    mat4 = Mat4(
            m[0][0], m[0][1], m[0][2], m[0][3],
            m[1][0], m[1][1], m[1][2], m[1][3],
            m[2][0], m[2][1], m[2][2], m[2][3],
            m[3][0], m[3][1], m[3][2], m[3][3]
        )
    return CSCAD.Transform(mat4, gobj)

def translate(offset:tuple[float,float,float], gobj:Geom2 | Geom3) -> Geom2 | Geom3:
    """Translate (move) the given geometry.

    Returns: Geom2

    Group: Transformations
    """

    if type(gobj) is Geom2:
        o = Vec2(float(offset[0]), float(offset[1]))
    else:
        o = Vec3(float(offset[0]), float(offset[1]), float(offset[2]))
    return CSCAD.Translate(o, gobj)

def translateX(offset:float, gobj:Geom2 | Geom3) -> Geom2 | Geom3:
    """Translate (move) the given geometry along the X axis.

    Returns: Geom2

    Group: Transformations
    """

    return CSCAD.TranslateX(float(offset), gobj)

def translateY(offset:float, gobj:Geom2 | Geom3) -> Geom2 | Geom3:
    """Translate (move) the given geometry along the Y axis.

    Returns: Geom2

    Group: Transformations
    """

    return CSCAD.TranslateY(float(offset), gobj)

def translateZ(offset:float, gobj:Geom3) -> Geom3:
    """Translate (move) the given geometry along the Z axis.

    Returns: Geom2

    Group: Transformations
    """

    return CSCAD.TranslateZ(float(offset), gobj)

def degToRad(angleInDegrees: float) -> float:
    """Converts degrees to radians.

    Returns: radians

    Group: Trigonometry
    """

    return CSCAD.DegToRad(angleInDegrees)

def radToDeg(angleInRadians: float) -> float:
    """Converts radians to degrees.

    Returns: degrees

    Group: Trigonometry
    """

    return CSCAD.RadToDeg(angleInRadians)

def acos(cosVal: float) -> float:
    """Acos for degrees

    Returns: degrees

    Group: Trigonometry
    """

    return CSCAD.Acos(cosVal)

def acosh(cosVal: float) -> float:
    """Acosh for degrees

    Returns: degrees

    Group: Trigonometry
    """

    return CSCAD.Acosh(cosVal)

def asin(sinVal: float) -> float:
    """Asin for degrees

    Returns: degrees

    Group: Trigonometry
    """

    return CSCAD.Asin(sinVal)

def asinh(sinVal: float) -> float:
    """Asinh for degrees

    Returns: degrees

    Group: Trigonometry
    """

    return CSCAD.Asinh(sinVal)

def atan(tanVal: float) -> float:
    """Atan for degrees

    Returns: degrees

    Group: Trigonometry
    """

    return CSCAD.Atan(tanVal)

def atan2(y: float, x: float) -> float:
    """Atan2 for degrees

    Returns: degrees

    Group: Trigonometry
    """

    return CSCAD.Atan2(y, x)

def atanh(tanVal: float) -> float:
    """Atanh for degrees

    Returns: degrees

    Group: Trigonometry
    """

    return CSCAD.Atanh(tanVal)

def cos(angleInDegrees: float) -> float:
    """Cosine for angle specified in degrees

    Returns: cosine

    Group: Trigonometry
    """

    return CSCAD.Cos(angleInDegrees)

def cosh(angleInDegrees: float) -> float:
    """Cosh for angle specified in degrees

    Returns: cosh

    Group: Trigonometry
    """

    return CSCAD.Cosh(angleInDegrees)

def sin(angleInDegrees: float) -> float:
    """Sine for angle specified in degrees

    Returns: sine

    Group: Trigonometry
    """

    return CSCAD.Sin(angleInDegrees)

def sinh(angleInDegrees: float) -> float:
    """Sinh for angle specified in degrees

    Returns: sinh

    Group: Trigonometry
    """

    return CSCAD.Sinh(angleInDegrees)

def tan(angleInDegrees: float) -> float:
    """Tangent for angle specified in degrees

    Returns: tan

    Group: Trigonometry
    """

    return CSCAD.Tan(angleInDegrees)

def tanh(angleInDegrees: float) -> float:
    """Tanh for angle specified in degrees

    Returns: tanh

    Group: Trigonometry
    """

    return CSCAD.Tanh(angleInDegrees)