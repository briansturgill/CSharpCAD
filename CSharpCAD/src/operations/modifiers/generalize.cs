namespace CSharpCAD;

public static partial class Modifiers
{
    private static Geom2 generalizeGeom2(Opts opts, Geom2 geometry) => geometry;

    private static Geom3 generalizeGeom3(Opts opts, Geom3 geometry)
    {
        var snap = opts.GetBool("snap", false);
        var simplify = opts.GetBool("simplify", false);
        var triangulate = opts.GetBool("triangulate", false);
        var repair = opts.GetBool("repair", false);

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
            polygons = insertTjunctions(polygons);
            polygons = triangulatePolygons(epsilon, polygons);
        }

        // repair the polygons (possibly triangles) if requested
        if (repair)
        {
            // fix T junctions
            polygons = repairTjunctions(epsilon, polygons);
            // TODO fill holes
        }

        return new Geom3(polygons, geometry.Transforms, geometry.Color);
    }

    /**
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
    public static Geometry generalize(Opts opts, Geometry geometry)
    {
        switch (geometry)
        {
            case Geom2 g2:
                return generalizeGeom2(opts, g2);
            case Geom3 g3:
                return generalizeGeom3(opts, g3);
            default:
                throw new ArgumentException("Invalid geometry");
        }
    }
}
