namespace CSharpCAD;

internal static partial class CSharpCADInternals
{

    internal static partial class Earcut
    {
        /*
         * check if a point lies within a convex triangle
         */
        public static bool PointInTriangle(double ax, double ay, double bx, double by,
          double cx, double cy, double px, double py) => (
          (cx - px) * (ay - py) - (ax - px) * (cy - py) >= 0 &&
              (ax - px) * (by - py) - (bx - px) * (ay - py) >= 0 &&
              (bx - px) * (cy - py) - (cx - px) * (by - py) >= 0
        );

        /*
         * signed area of a triangle
         */
        public static double AreaOfT(Node? p, Node? q, Node? r)
        {
            if (p is null || q is null || r is null) return 0;
            return (q.y - p.y) * (r.x - q.x) - (q.x - p.x) * (r.y - q.y);
        }
    }
}