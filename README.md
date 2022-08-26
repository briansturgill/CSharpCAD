# CSharpCAD

This is a port (with a somewhat different API) from [JSCAD](https://github.com/jscad/OpenJSCAD.org) to C#.

We are deeply grateful to the JSCAD team for creating such a great package.

# THIS PACKAGE IS NOT READY FOR USE YET!!!

(But it is getting closer.)

If you want to poke around, I suggest you start in the CADViewer and Python
directories. The READMEs there will help you quickly get started.
You of course need to have the "dotnet" sdk installed on your system: [DotNet Install](https://docs.microsoft.com/en-us/dotnet/core/install/)

The API is still in flux, so please don't use it for major projects.

I have placed it here to facility cooperation with the JSCAD team for
when I'm point to improvements or bugs (amazingly few) out to them.

If you just want to play with it, it has mostly been created on Linux.
I have done profiling of it under Windows (using Visual Studio).
I have not tried it on a Mac yet, but it is expect to work there.
I would advise VSCode with the C# extension from Microsoft installed.
You must have .NET 6.0 installed!
Later, I'll have instructions for all of that, but if you know how to
do it, I suggest you override the contents of cscad/Program.cs with
what you'd like to try.

Documentation:
[Main CSharpCAD Documentation](https://briansturgill.github.io/CSharpCAD/CSharpCADDocs.html)


Remember the API WILL CHANGE!

## CREDITS

CSharpCAD contains integrated code from numerous places, if you want the originals see the web addresses below. We are grateful that the following have contributed code under liberal licensing (See [LICENSE](https://github.com/briansturgill/CSharpCAD/blob/main/LICENSE.md) for the list of licenses.)

First and foremost: [JSCAD](https://github.com/jscad/OpenJSCAD.org) JSCAD Organization

The origin of JSCAD: [CSG](https://github.com/evanw/csg.js) Joost Nieuwenhuijse and Evan Wallace.

Portions of glMatrix Library: [glMatrix](https://github.com/toji/gl-matrix) Brandon Jones and Colin MacKenzie IV

Quickhull Library: [QuickHull3D](https://github.com/mauriciopoppe/quickhull3dopyright) Mauricio Poppe

Earcut Triangulation: [Earcut](https://github.com/mapbox/earcut) Mapbox

2D Boolean Operations [NPolyBool](https://github.com/pchalamet/NPolyBool) Pierre Chalamet

2D Boolean Operations [PolyBool.Net](https://github.com/idormenco/PolyBool.Net) idormenco

2D Boolean Operations [polybooljs](https://github.com/velipso/polybooljs) Sean Connelly

AVLTree (for stability): [AVLTree](https://github.com/justcoding121/advanced-algorithms) Jehonathan

Triangle Intersection: [Möller](https://fileadmin.cs.lth.se/cs/Personal/Tomas_Akenine-Moller/code/tritri_isectline.txt)

While no code from them was directly used, Microsoft Corporation provided the wonderful open source .Net tools that underly the development of CSharpCAD. Amazingly, it too is under the MIT license.

Finally I'd like to point out three special members of the JSCAD team, @hrgdavor, @platypii, and @z3dev for their assistance, comments and discussions.
