namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Extrude the given geometry by following the outline(s) with a rectangle.</summary>
     * <param name="gobj">2D geometry object to be extruded.</param>
     * <param name="size">Size of the rectangle.</param>
     * <param name="height">Height of the extrusion.</param>
     * <param name="twistAngle">The final rotation (DEGREES) about the origin of the shape (if any).</param>
     * <param name="twistSteps">The resolution of the twist about the axis (if any).</param>
     * <param name="corners">Type of corner to create after expanding.</param>
     * <param name="segments">Number of segments when creating round corners.</param>
     * <group>3D Primitives</group>
     */
    public static Geom3 ExtrudeRectangular(Geom2 gobj, double size = 1, double height = 1,
            double twistAngle = 0, int twistSteps = 12, Corners corners = Corners.Edge, int segments = 16)
    {

        if (size <= 0) throw new ArgumentException("Argument \"size\" must be positive.");
        if (height <= 0) throw new ArgumentException("Argument \"height\" must be positive.");

        twistAngle = DegToRad(twistAngle);
        return ExtrudeRectangularGeom2(gobj, size, height, twistAngle, twistSteps, corners, segments);
    }
}