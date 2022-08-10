namespace CSharpCAD;

public static partial class CSCAD
{
    internal static Geom2 OffsetGeom2(Geom2 gobj, double delta = 1, Corners corners = Corners.Edge, int segments = 16)
    {
        var nrtree = new Geom2.NRTree();
        var shapesAndHoles = gobj.ToShapesAndHoles();

        foreach (var (shape, holes) in shapesAndHoles)
        {
            // Note - +delta
            var newShape = OffsetFromPoints(shape, delta: delta, corners: corners, segments: segments, closed: true);
            nrtree.Insert(newShape.ToArray());
            foreach (var hole in holes)
            { // Note -delta
                var newHole = OffsetFromPoints(hole, delta: -delta, corners: corners, segments: segments, closed: true);
                nrtree.Insert(newHole.ToArray());
            }
        }

        return new Geom2(nrtree);
    }
}