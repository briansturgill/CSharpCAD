namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Create offset geometry from the given geometry.</summary>
     * <remarks>Offsets from internal and external space are created.</remarks>
     * <param name="delta=1">Delta of offset (+ to exterior, - from interior).</param>
     * <param name="corners">Type of corner to create after offseting; edge, chamfer, round.</param>
     * <param name="segments">Number of segments when creating round corners.</param>
     */
    public static Geometry Offset(Geometry gobj, double delta = 1, string corners = "edge", int segments = 16)
    {
        if (!(corners == "edge" || corners == "chamfer" || corners == "round"))
        {
            throw new ArgumentException("corners must be \"edge\", \"chamfer\", or \"round\"");
        }


        if (!gobj.Is2D)
        {
            throw new ArgumentException("Currently, only 2D objects are supported in ExtrudeRectangular.");
        }
        return OffsetGeom2((Geom2)gobj, delta, corners, segments);

    }
}