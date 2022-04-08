namespace CSharpCAD;

public static partial class CSCAD
{
    internal static Geom2 ExpandGeom2(Geom2 geometry, double delta, string corners, int segments)
    {
        if (!(corners == "edge" || corners == "chamfer" || corners == "round"))
        {
            throw new ArgumentException("Corners must be one of: \"edge\", \"chamfer\", or \"round\".");
        }

        // convert the geometry to outlines, and generate offsets from each
        var outlines = geometry.ToOutlines();
        var allSides = new List<Geom2.Side>();
        foreach (var outline in outlines)
        {
            var newOutline = OffsetFromPoints(outline, delta, corners, segments, closed: true);
            var newSides = new Geom2(newOutline).ToSides();
            allSides.AddRange(newSides);
        }

        return new Geom2(allSides.ToArray(), Color: geometry.Color);
    }
}