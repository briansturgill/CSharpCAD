namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Create offset geometry from the given geometry.</summary>
     * <remarks>
     *   Offset is different from "expand" in that expand, "expands" both contours and holes.
     *   If "offset" is used, solids are expanded and holes are contracted (or vice-versa if delta &lt; 0).
     *   If the Geom2 has no holes, then the two functions behave exactly the same.
     * </remarks>
     * <param name="gobj">2D geometry object to be offset.</param>
     * <param name="delta">Delta of offset (+ to exterior, - from interior).</param>
     * <param name="corners">Type of corner to create after offseting; edge, chamfer, round.</param>
     * <param name="segments">Number of segments when creating round corners.</param>
     * <group>Transformations</group>
     */
    public static Geom2 Offset(Geom2 gobj, double delta = 1, Corners corners = Corners.Edge, int segments = 16)
    {
        return OffsetGeom2(gobj, delta, corners, segments);

    }
}