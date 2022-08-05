namespace CSharpCAD.Triangulator;
struct LineSegment
{
    public Vertex A;
    public Vertex B;

    public LineSegment(Vertex a, Vertex b)
    {
        A = a;
        B = b;
    }

    public double? IntersectsWithRay(Vec2 origin, Vec2 direction)
    {
        double largestDistance = Math.Max(A.Position.X - origin.X, B.Position.X - origin.X) * 2.0;
        LineSegment raySegment = new LineSegment(new Vertex(origin, 0),
			new Vertex(origin.Add(direction.Multiply(new Vec2(largestDistance, largestDistance))), 0));

        Vec2? intersection = FindIntersection(this, raySegment);
        double? value = null;

        if (intersection != null)
            value = origin.Distance(intersection.Value);

        return value;
    }

    public static Vec2? FindIntersection(LineSegment a, LineSegment b)
    {
        double x1 = a.A.Position.X;
        double y1 = a.A.Position.Y;
        double x2 = a.B.Position.X;
        double y2 = a.B.Position.Y;
        double x3 = b.A.Position.X;
        double y3 = b.A.Position.Y;
        double x4 = b.B.Position.X;
        double y4 = b.B.Position.Y;

        double denom = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);

        double uaNum = (x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3);
        double ubNum = (x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3);

        double ua = uaNum / denom;
        double ub = ubNum / denom;

        if (Math.Clamp(ua, 0.0, 1.0) != ua || Math.Clamp(ub, 0.0, 1.0) != ub)
            return null;

        return a.A.Position.Add(a.B.Position.Subtract(a.A.Position).Multiply(new Vec2(ua, ua)));
    }
}
