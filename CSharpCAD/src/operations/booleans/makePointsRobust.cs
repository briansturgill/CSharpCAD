namespace CSharpCAD;
using KdTree;

public static partial class CSCAD
{
#if LATER
    internal class MakeRobustPoints
    {
        private KdTree<double, Vec3> kdtree;
        internal MakeRobustPoints()
        {
            kdtree = new KdTree<double, Vec3>(3, new KdTree.Math.DoubleMath());
        }
        internal void InsertOriginalPoints(Geom3 g3)
        {
            var polys = g3.ToPolygons();
            foreach (var poly in polys)
            {
                foreach (var v3 in poly.Vertices)
                {
                    var nearby = kdtree.RadialSearch(new[] { v3.X, v3.Y, v3.Z }, C.EPS);
                    if (nearby.Length > 1)
                    {
                        Console.WriteLine($"Multiple faulty input points around: {v3}");
                        continue;
                    }
                    if (nearby.Length == 1)
                    {
                        if (nearby[0].Value == v3)
                        {
                            continue;
                        }
                        Console.WriteLine($"Input points out of sync: {v3}, {nearby[0].Value}");
                        continue;
                    }
                    kdtree.Add(new[] { v3.X, v3.Y, v3.Z }, v3);
                }
            }
        }
        private struct PointsToFix
        {
            internal Vec3 point;
            internal Poly3 polygon;
            internal int pidx;
            internal PointsToFix(Vec3 point, Poly3 polygon, int pidx)
            {
                this.point = point;
                this.polygon = polygon;
                this.pidx = pidx;
            }
        };
        // Note, this function modifies directly the polygons inside the given Geom3.
        // This should be used only internally.
        internal void FixPoints(Geom3 g3)
        {
            var newptkdtree = new KdTree<double, List<PointsToFix>>(3, new KdTree.Math.DoubleMath());
            var newpts = new List<Vec3>(); // List of keys that point to clusters of nearby new points.
            var polys = g3.ToPolygons();
            foreach (var poly in polys)
            {
                var plen = poly.Vertices.Length;
                for (var i = 0; i < plen; i++)
                {
                    var v3 = poly.Vertices[i];
                    var v3Array = new[] { v3.X, v3.Y, v3.Z };
                    var nearby = kdtree.RadialSearch(v3Array, C.EPS);
                    if (nearby.Length != 0) continue; // We have an old point.
                    var ptf = new PointsToFix(v3, poly, i);
                    var newnearby = newptkdtree.RadialSearch(v3Array, C.EPS);
                    if (newnearby.Length == 0)
                    {
                        newptkdtree.Add(v3Array, new List<PointsToFix> { ptf });
                        newpts.Add(v3);
                    }
                    else
                    {
                        newnearby[0].Value.Add(ptf);
                    }
                }
            }
            foreach (var v3 in newpts)
            {
                var v3Array = new[] { v3.X, v3.Y, v3.Z };
                var nearby = newptkdtree.RadialSearch(v3Array, C.EPS);
                var ptList = nearby[0].Value;
                var len = ptList.Count;
                if (len < 2) continue;
                var avg = ptList[0].point;
                for (var i = 1; i < len; i++)
                {
                    avg = avg.Add(ptList[i].point);
                }
                avg = avg.Divide(new Vec3(len));
                var avgArray = new[] { avg.X, avg.Y, avg.Z };
                var nearbyOld = kdtree.RadialSearch(avgArray, C.EPS);
                if (nearbyOld.Length != 0)
                {
                    avg = nearbyOld[0].Value;
                }
                for (var i = 0; i < len; i++)
                {
                    var ptf = ptList[i];
                    ptf.polygon.Vertices[ptf.pidx] = avg;
                }
            }
        }
    }
#endif

    private struct PointsToFix
    {
        internal Vec3 point;
        internal Poly3 polygon;
        internal int pidx;
        internal PointsToFix(Vec3 point, Poly3 polygon, int pidx)
        {
            this.point = point;
            this.polygon = polygon;
            this.pidx = pidx;
        }
    };

    internal static void MakePointsRobust(string tag, Poly3[] polys)
    {
        var pointsCorrected = 0;
        var kdtree = new KdTree<double, List<PointsToFix>>(3, new KdTree.Math.DoubleMath());
        foreach (var poly in polys)
        {
            var plen = poly.Vertices.Length;
            for (var i = 0; i < plen; i++)
            {
                var v3 = poly.Vertices[i];
                var v3Array = new[] { v3.X, v3.Y, v3.Z };
                var nearby = kdtree.RadialSearch(v3Array, C.EPS);
                var ptf = new PointsToFix(v3, poly, i);
                if (nearby.Length == 0)
                {
                    kdtree.Add(v3Array, new List<PointsToFix> { ptf });
                }
                else
                {
                    nearby[0].Value.Add(ptf);
                }
            }
        }
        foreach (var node in kdtree)
        {
            var ptList = node.Value;
            var len = ptList.Count;
            if (len < 2) continue;
            var avg = ptList[0].point;
            for (var i = 1; i < len; i++)
            {
                avg = avg.Add(ptList[i].point);
            }
            avg = avg.Divide(new Vec3(len));
            for (var i = 0; i < len; i++)
            {
                var ptf = ptList[i];
                ptf.polygon.Vertices[ptf.pidx] = avg;
                pointsCorrected++;
            }
        }
        if (GlobalParams.DebugOutput)
            Console.WriteLine($"MakePointsRobust: {tag} {pointsCorrected} points corrected.");
    }
}