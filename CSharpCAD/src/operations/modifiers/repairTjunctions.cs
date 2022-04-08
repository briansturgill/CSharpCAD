namespace CSharpCAD;

public static partial class Modifiers
{
    public static Poly3[] repairTjunctions(double epsilon, Poly3[] polygons)
    {
        var edges = polygonsToEdges(polygons);
        var openedges = cullOpenEdges(edges);
        if (openedges.Count == 0) return polygons;

        // split open edges until no longer possible
        var splitting = true;
        while (splitting)
        {
            var splitcount = 0;
            for (var i = 0; i < openedges.Count; i++)
            {
                var edge = openedges[i];
                if (edge is not null && edge.polygons.Count == 1)
                {
                    var newpolygons = splitEdge(openedges, edge, epsilon);
                    if (newpolygons is not null)
                    {
                        var (newpoly0, newpoly1) = ((Poly3, Poly3))newpolygons;
                        openedges.RemoveAt(i);
                        i--; // NOTE! this list just got smaller.
                        addPolygon(openedges, newpoly0);
                        addPolygon(openedges, newpoly1);

                        // adjust the master list as well
                        removePolygons(edges, edge);
                        // add edges for each new polygon
                        addPolygon(edges, newpoly0);
                        addPolygon(edges, newpoly1);

                        splitcount++;
                        break; // start again
                    }
                }
            }
            splitting = (splitcount > 0);
        }
        
        var remaining_openedges = new List<Edge>();
        foreach(var edge in openedges) {
            if (edge is not null && edge.polygons is not null && edge.polygons.Count == 1) {
                remaining_openedges.Add(edge);
            }
        }
        if (remaining_openedges.Count > 0) Debug.Print($"Repair of all T-junctions failed: {remaining_openedges.Count}");

        // rebuild the list of polygons from the edges
        return edgesToPolygons(edges).ToArray();
    }
}