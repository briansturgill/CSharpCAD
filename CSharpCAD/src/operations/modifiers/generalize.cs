namespace CSharpCAD;

internal static partial class Modifiers
{
    private static Geom3 generalizeGeom3(Geom3 geometry, bool snap, bool simplify, bool triangulate, bool repair)
    {

        var epsilon = geometry.MeasureEpsilon();
        var polygons = geometry.ToPolygons();

        // snap the given geometry if requested
        if (snap)
        {
            polygons = snapPolygons(epsilon, polygons);
        }

        // simplify the polygons if requested
        if (simplify)
        {
            // TODO implement some mesh decimations
            polygons = mergePolygons(epsilon, polygons);
        }

        // triangulate the polygons if requested
        if (triangulate)
        {
            polygons = InsertTjunctions(polygons);
            polygons = TriangulatePolygons(epsilon, polygons);
        }

        return new Geom3(polygons, geometry.Transforms, geometry.Color);
    }

    /*
     * <summary>Apply various modifications in proper order to produce a generalized geometry.</summary>
     * <remarks>
     * @param {Object} options - options for modifications
     * @param {Boolean} [options.snap=false] the geometries should be snapped to epsilons
     * @param {Boolean} [options.simplify=false] the geometries should be simplified
     * @param {Boolean} [options.triangulate=false] the geometries should be triangulated
     * @param {Boolean} [options.repair=false] the geometries should be repaired
     * @param {...Object} geometries - the geometries to generalize
     * @return {Object|Array} the modified geometry, or a list of modified geometries
     * </remarks>
     */
    internal static Geom3 generalize(Geom3 g3, bool snap=false, bool simplify = false,
        bool triangulate = false, bool repair = false)
    {
        return generalizeGeom3(g3, snap, simplify, triangulate, repair);
    }
}
