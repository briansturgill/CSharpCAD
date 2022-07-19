from clr_loader import get_coreclr
from pythonnet import set_runtime

rt = get_coreclr("./runtimeconfig.json")
set_runtime(rt)

import clr
from System import String
from System.Collections.Generic import List
clr.AddReference("CSharpCAD");
from CSharpCAD import CSCAD
from CSharpCAD import Geom2, Geom3, Vec2, Vec3

def circle(radius:float = 1.0, segments:int = 32, center:tuple[float, float] = (0,0)) -> Geom2:
    """Construct a circle in 2D space.

    Returns: Geom2

    Group: 2D Primitives
    """

    cent = Vec2(float(center[0]), float(center[1]))
    return CSCAD.Circle(float(radius), int(segments), cent)

def cutter2d(radius:float = 1.0, startAngle:float = 0, endAngle:float = 90, center:tuple[float, float] = (0,0)) -> Geom2:
    """Construct a "cutter".
    A negative geometry object, suitable to cut a "pie slice" out of a circlular 2d object of radius r.

    This object would normally be used as a second argument to "Subtract".

    To use with an elliptical object, specify radius as the largest radius of the elliptical.

    Returns: Geom2

    Group: 2D Primitives
    """

    cent = Vec2(float(center[0]), float(center[1]))
    return CSCAD.Cutter2D(float(radius), float(startAngle), float(endAngle), cent)

def ellipse(radius:tuple[float, float] = (1,1), segments:int = 32, center:tuple[float, float] = (0,0)) -> Geom2:
    """Construct an axis-aligned ellipse in two dimensional space.

    Returns: Geom2

    Group: 2D Primitives
    """

    rad = Vec2(float(radius[0]), float(radius[1]))
    cent = Vec2(float(center[0]), float(center[1]))
    return CSCAD.Ellipse(rad, int(segments), cent)

def polygon(points:list[tuple[float, float]], paths:list[list[int]] = None) -> Geom2:
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

def project(gobj:Geom3, axis:tuple[float, float, float] = (0, 0, 1), origin:tuple[float, float, float] = (0, 0, 0)) -> Geom2:
    """Project 3D object to 2D.

    Returns: Geom2

    Group: 2D Primitives
    """

    a = Vec3(float(axis[0]), float(axis[1]), float(axis[2]));
    o = Vec3(float(origin[0]), float(origin[1]), float(origin[2]));
    return CSCAD.Project(gobj, a, o)

def rectangle(size:tuple[float, float] = (2,2), center:tuple[float, float] = None):
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

def roundedRectangle(size:tuple[float, float] = (2,2), roundRadius:float=0.2, segments:int=32, center:tuple[float, float] = None):
    """Construct an axis-aligned rectangle in 2D space with rounded corners.

    The default center point is selected such that the bottom left corner of the rectangle is (0,0).

    (The rectangle is entirely in the first quadrant.)

    Returns: Geom2

    Group: 2D Primitives
    """

    s = Vec2(size[0], size[1]);
    if center == None:
        c = None
    else:
        c = Vec2(center[0], center[1])
    return CSCAD.RoundedRectangle(s, float(roundRadius), int(segments), c)

def semicircle(radius:float = 1.0, segments:int = 32, startAngle:float = 0, endAngle:float = 90, center:tuple[float, float] = (0,0)) -> Geom2:
    """Construct a partial circle in 2D space.

    Returns: Geom2

    Group: 2D Primitives
    """

    cent = Vec2(float(center[0]), float(center[1]))
    return CSCAD.Semicircle(float(radius), int(segments), float(startAngle), float(endAngle), cent)

def subtract(*gobjs) -> Geom2 | Geom3:
    return CSCAD.Subtract(gobjs)

def getColorNames() -> list[str]:
    l = []
    strArray = CSCAD.GetColorNames()
    for s in strArray:
        l.append(s)
    return l

def save(model_filename: str, gobj: Geom2 | Geom3):
    CSCAD.Save(model_filename, gobj)

def view(gobj: Geom2 | Geom3, title:str) -> Geom2 | Geom3:
    return CSCAD.View(gobj, title)

def waitForViewerTransfers():
    CSCAD.WaitForViewerTransfers()
