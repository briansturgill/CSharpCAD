namespace CSharpCAD;
using CSharpCAD.Advanced.Algorithms.DataStructures;

public static partial class CSCAD
{
    internal static void MakePointsStable(string tag, Poly3[] polys)
    {
        var tree = new AVLTree<Vec3>();
        var totalPoints = 0;
        var pointsCorrected = 0;
        foreach (var poly in polys)
        {
            var plen = poly.Vertices.Length;
            for (var i = 0; i < plen; i++)
            {
                var v3 = poly.Vertices[i];
                var v3ret = tree.Insert(v3);
                if (v3 != v3ret)
                {
                    poly.Vertices[i] = v3ret;
                    pointsCorrected++;
                }
            }
            totalPoints += plen;
        }
        if (pointsCorrected > 0 && GlobalParams.DebugOutput)
        {
            var traceLines = Environment.StackTrace.Split('\n', '\r');
            var last = traceLines[traceLines.Length - 1].Trim();
            last = Regex.Replace(last, @":line ", ":") + ":1";
            Console.WriteLine($"MakePointsStable({tag}): {pointsCorrected} of {totalPoints} points corrected {last}");
        }
    }
}