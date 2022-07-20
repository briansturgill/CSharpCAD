from clr_loader import get_coreclr
from pythonnet import set_runtime
rt = get_coreclr("./runtimeconfig.json")
set_runtime(rt)

import clr
from System import String
from System.Collections.Generic import List
clr.AddReference("CSharpCAD")
from CSharpCAD import CSCAD
from CSharpCAD import Geom2, Geom3, Vec2, Vec3

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

def view(gobj: Geom2 | Geom3, title: str) -> Geom2 | Geom3:
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

    return CSCAD.Atan2(x, y)

def atanh(tanVal: float) -> float:
    """Atanh for degrees

    Returns: degrees

    Group: Trigonometry
    """

    return CSCAD.Atanh(tanVal)

def cos(angleInDegrees: float) -> float:
    """Cosine for angle specified in degrees

    Returns: cos 

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