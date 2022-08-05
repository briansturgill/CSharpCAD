namespace CSharpCAD;

internal static partial class Earcut
{
    // Main entry point from CSharpCAD.
    // Perform EarCut on each Shape/Holes set and then add the resulting triangles to polys.
    internal static void DoEarcutCaps(List<(Vec2[], Vec2[][])> shapesAndHoles, List<Poly3> polys, double bottom_most_z, double top_most_z)
    {
        var bmz = bottom_most_z;
        var tmz = top_most_z;
        foreach (var (solid, holes) in shapesAndHoles)
        {
            // hole indices
            var index = solid.Length;
            var holesIndex = new List<int>();
            foreach (var hole in holes)
            {
                holesIndex.Add(index);
                index += hole.Length;
            }

            // compute earcut triangulation for each solid
            var len = solid.Length;
            foreach (var hole in holes)
            {
                len += hole.Length;
            }

            var vertices = new List<Vec2>(len);
            vertices.AddRange(solid);
            // WARNING... this next only works because the holes were sorted by Max.X in Geom2.ToEarcutNesting()
            foreach (var hole in holes)
            {
                vertices.AddRange(hole);
            }

            var data_index = 0;
            var data = new double[len * 2];
            foreach (var v in solid)
            {
                data[data_index++] = v.X;
                data[data_index++] = v.Y;
            }
            foreach (var hole in holes)
            {
                foreach (var v in hole)
                {
                    data[data_index++] = v.X;
                    data[data_index++] = v.Y;
                }
            }

            var indices = Earcut.Triangulate(data, holesIndex);
            for (var i = 0; i < indices.Count; i += 3)
            {
                var v0 = vertices[indices[i]];
                var v1 = vertices[indices[i+1]];
                var v2 = vertices[indices[i+2]];
                /*
                var op = data;
                var oi = indices;
                var top1 = new Vec3(data[indices[i]], data[indices[i + 1]], tmz);
                var top2 = new Vec3(data[indices[i + 2]], data[indices[i + 3]], tmz);
                var top3 = new Vec3(data[indices[i + 4]], data[indices[i + 5]], tmz);
                // Bottom needs to be reversed.
                var bot1 = new Vec3(data[indices[i + 4]], data[indices[i + 5]], bmz);
                var bot2 = new Vec3(data[indices[i + 2]], data[indices[i + 3]], bmz);
                var bot3 = new Vec3(data[indices[i]], data[indices[i + 1]], bmz);
                */
                // bottom -- needs to be reversed
                polys.Add(new Poly3(new Vec3[] { new Vec3(v2, bmz), new Vec3(v1, bmz), new Vec3(v0, bmz) }));
                // top
                polys.Add(new Poly3(new Vec3[] { new Vec3(v0, tmz), new Vec3(v1, tmz), new Vec3(v2, tmz) }));
            }
        }
    }
}