namespace CSharpCAD;

public static partial class CSCAD
{
    internal static Geom2 ExpandGeom2(Geom2 geometry, double delta, Corners corners, int segments)
    {
        // convert the geometry to outlines, and generate offsets from each
        var nrtree = new Geom2.NRTree();
        var outlines = geometry.ToOutlines();
        foreach (var outline in outlines)
        {
            var newOutline = OffsetFromPoints(outline, delta, corners, segments, closed: true);
            nrtree.Insert(newOutline.ToArray());
        }

        return new Geom2(nrtree, Color: geometry.Color);
    }
}