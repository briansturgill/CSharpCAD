#nullable disable
namespace CSharpCAD;

internal static partial class Earcut
{
    /*
     * Constructs a polygon hierarchy of solids and holes.
     * The hierarchy is represented as a forest of trees. All trees shall be depth at most 2.
     * If a solid exists inside the hole of another solid, it will be split out as its own root.
     *
     * @param {geom2} geometry
     * @returns {Array} an array of polygons with associated holes
     * @alias module:modeling/geometries/geom2.toTree
     *
     * @example
     * var geometry = subtract(rectangle({size: [5, 5]}), rectangle({size: [3, 3]}))
     * console.log(assignHoles(geometry))
     * [{
     *   "solid": [[-2.5,-2.5],[2.5,-2.5],[2.5,2.5],[-2.5,2.5]],
     *   "holes": [[[-1.5,1.5],[1.5,1.5],[1.5,-1.5],[-1.5,-1.5]]]
     * }]
     */
    internal static List<(Vec2[], Vec2[][])> AssignHoles(Geom2 geometry)
    {
        return geometry.ToEarcutNesting();
    }
}