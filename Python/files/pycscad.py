import atexit
from clr_loader import get_coreclr
from pythonnet import set_runtime

rt = get_coreclr("./runtimeconfig.json")
set_runtime(rt)

import clr
from System import String
from System.Collections import *
clr.AddReference("CSharpCAD");
from CSharpCAD import CSCAD
from CSharpCAD import Geom2, Geom3

atexit.register(CSCAD.WaitForViewer)

def circle(radius:int = 5) -> Geom2:
    return CSCAD.Circle(radius)

def save(model_filename: str, geom: Geom2 | Geom3):
    return CSCAD.Save(model_filename, geom)
