namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Expand the given geometry using the given options.</summary
     * Both internal and external space is expanded for 2D and 3D shapes.
     *
     * Note: Contract is expand using a negative delta.
     * <param name="gobj">The geometry to expand.</param>
     * <param name="delta">Delta (+/-) of expansion.</param>
     * <param name="corners">Type of corner to create after expanding; edge, chamfer, round</param>
     * <param name="segments">Number of segments when creating round corners.</param>
     */
    public static Geometry Expand(Geometry gobj, double delta = 1, string corners = "edge", int segments = 16)
    {
        if (!gobj.Is2D)
        {
            throw new ArgumentException("Currently, only 2D objects are supported in ExtrudeRectangular.");
        }
        return ExpandGeom2((Geom2)gobj, delta, corners, segments);
    }
}