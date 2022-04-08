namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * Extrude the given geometry in an upward linear direction using the given options.
     * @param {Object} options - options for extrude
     * @param {Number} [options.height=1] the height of the extrusion
     * @param {Number} [options.twistAngle=0] the final rotation (RADIANS) about the origin of the shape (if any)
     * @param {Integer} [options.twistSteps=1] the resolution of the twist about the axis (if any)
     * @param {...Object} objects - the geometries to extrude
     * @return {Object|Array} the extruded geometry, or a list of extruded geometry
     * @alias module:modeling/extrusions.extrudeLinear
     *
     * @example
     * let myshape = extrudeLinear({height: 10}, rectangle({size: [20, 25]}))
     */
    public static Geom3 ExtrudeLinear(Geom2 obj, double height = 1, double twistAngle = 0, int twistSteps = 1, bool repair = true)
    {
        return ExtrudeLinearGeom2(obj, new Vec3(0, 0, height), twistAngle, twistSteps, repair);
    }
}