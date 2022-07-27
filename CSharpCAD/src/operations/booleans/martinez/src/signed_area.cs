namespace CSharpCAD;

internal static partial class Geom2Booleans
{

    /*
     * Signed area of the triangle (p0, p1, p2)
     * @param  {Array.<Number>} p0
     * @param  {Array.<Number>} p1
     * @param  {Array.<Number>} p2
     * @return {Number}
     */
    internal static int signedArea(Vec2 p0, Vec2 p1, Vec2 p2)
    {
        var res = GeometricPredicates.Orient2D(p0, p1, p2);
        return (int)res;
    }
}