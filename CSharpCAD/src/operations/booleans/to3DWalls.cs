namespace CSharpCAD;

public static partial class CSCAD
{

    /*
     * Create a polygon (wall) from the given Z values and side.
     */
    private static Poly3 To3DWall(double z0, double z1, Geom2.Side side, Color? color)
    {
        var points = new List<Vec3> {
        new Vec3(side.v0.x, side.v0.y, z0),
    new Vec3(side.v1.x, side.v1.y, z0),
    new Vec3(side.v1.x, side.v1.y, z1),
    new Vec3(side.v0.x, side.v0.y, z1)
  };
        return new Poly3(points, color);
    }

    /*
     * Create a 3D geometry with walls, as constructed from the given options and geometry.
     *
     * @param {Object} options - options with Z offsets
     * @param {geom2} geometry - geometry used as base of walls
     * @return {geom3} the new geometry
     */
    private static Geom3 To3DWalls(double z0, double z1, Geom2 geometry)
    {
        var sides = geometry.ToSides();

        var polygons = new List<Poly3>(sides.Length);
        foreach (var side in sides)
        {
            polygons.Add(To3DWall(z0, z1, side, geometry.Color));
        }

        return new Geom3(polygons.ToArray(), new Mat4(), geometry.Color);
    }
}