namespace CSharpCAD;

public static partial class CSCAD
{
    /*
     * Determine if the given geometries overlap by comparing min and max bounds.
     * NOTE: This is used in union for performace gains.
     * @param {geom3} geometry1 - geometry for comparision
     * @param {geom3} geometry2 - geometry for comparision
     * @returns {boolean} true if the geometries overlap
     */
    internal static bool MayOverlap(Geom3 geometry1, Geom3 geometry2)
    {
        // CBS C# translation note... finally gave in here, though I did at least make it getter only.
        // FIXME accessing the data structure of the geometry should not be allowed
        if ((geometry1.Polygons.Length == 0) || (geometry2.Polygons.Length == 0))
        {
            return false;
        }

        var (min1, max1) = geometry1.BoundingBox();

        var (min2, max2) = geometry2.BoundingBox();
        if ((min2.X - max1.X) > C.EPS) return false;
        if ((min1.X - max2.X) > C.EPS) return false;
        if ((min2.Y - max1.Y) > C.EPS) return false;
        if ((min1.Y - max2.Y) > C.EPS) return false;
        if ((min2.Z - max1.Z) > C.EPS) return false;
        if ((min1.Z - max2.Z) > C.EPS) return false;
        return true;
    }
}