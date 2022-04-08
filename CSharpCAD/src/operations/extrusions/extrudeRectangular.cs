namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Extrude the given geometry by following the outline(s) with a rectangle.</summary>
     * <param name="size">Size of the rectangle.</param>
     * <param name="height">Height of the extrusion.</param>
     * <see cref="ExtrudeLinear">For the "twistAngle" and "twistSteps" parameters.</see>
     * <see cref="Expand">For the "corners" and "segments" parameters.</see>
     */
    public static Geom3 ExtrudeRectangular(Geometry gobj, double size = 1, double height = 1,
            double twistAngle = 0, int twistSteps = 12, string corners = "edge", int segments = 16)
    {

        if (size <= 0) throw new ArgumentException("Argument \"size\" must be positive.");
        if (height <= 0) throw new ArgumentException("Argument \"height\" must be positive.");

        if (!gobj.Is2D)
        {
            throw new ArgumentException("Currently, only 2D objects are supported in ExtrudeRectangular.");
        }

        return ExtrudeRectangularGeom2((Geom2)gobj, size, height, twistAngle, twistSteps, corners, segments);
    }
}