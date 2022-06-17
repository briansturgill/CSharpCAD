namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Create offset geometry from the given geometry.</summary>
     * <remarks>Offsets from internal and external space are created.</remarks>
     * <param name="gobj">2D geometry object to be offset.</param>
     * <param name="delta">Delta of offset (+ to exterior, - from interior).</param>
     * <param name="corners">Type of corner to create after offseting; edge, chamfer, round.</param>
     * <param name="segments">Number of segments when creating round corners.</param>
     * <group>Transformations</group>
     */
    public static Geometry Offset(Geometry gobj, double delta = 1, Corners corners = Corners.Edge, int segments = 16)
    {
        if (!gobj.Is2D)
        {
            throw new ArgumentException("Currently, only 2D objects are supported in Offset.");
        }
        return OffsetGeom2((Geom2)gobj, delta, corners, segments);

    }
}