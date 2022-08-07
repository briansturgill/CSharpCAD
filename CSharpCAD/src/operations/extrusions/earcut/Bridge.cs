namespace CSharpCAD;

// Main entry points from CSharpCAD.

internal static partial class Earcut
{
    internal static List<Vec2[]> DoEarcutList(List<(Vec2[], Vec2[][])> shapesAndHoles)
    {
        var ret = new List<Vec2[]>(shapesAndHoles.Count);
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
                var v1 = vertices[indices[i + 1]];
                var v2 = vertices[indices[i + 2]];
                ret.Add(new Vec2[] { v0, v1, v2, });
            }
        }
        return ret;
    }
}