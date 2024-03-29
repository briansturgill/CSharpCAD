#nullable disable
namespace CSharpCAD;

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
    public static double AreaOfT(Node p, Node q, Node r)
    {
        return (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
    }
}