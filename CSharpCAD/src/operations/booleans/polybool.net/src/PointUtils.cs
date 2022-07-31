#nullable disable
namespace CSharpCAD.PolyboolDotNet;

internal static class PointUtils
{
    internal static bool PointAboveOrOnLine(Vec2 point, Vec2 left, Vec2 right)
    {
        return (right.X - left.X) * (point.Y - left.Y) - (right.Y - left.Y) * (point.X - left.X) >= -Epsilon.Eps;
    }

    internal static bool PointBetween(Vec2 point, Vec2 left, Vec2 right)
    {
        // p must be collinear with left->right
        // returns false if p == left, p == right, or left == right
        double dPyLy = point.Y - left.Y;
        double dRxLx = right.X - left.X;
        double dPxLx = point.X - left.X;
        double dRyLy = right.Y - left.Y;

        double dot = dPxLx * dRxLx + dPyLy * dRyLy;

        if (dot < Epsilon.Eps)
        {
            return false;
        }

        double sqlen = dRxLx * dRxLx + dRyLy * dRyLy;
        if (dot - sqlen > -Epsilon.Eps)
        {
            return false;
        }

        return true;
    }

    private static bool PointsSameX(Vec2 point1, Vec2 point2)
    {
        return Math.Abs(point1.X - point2.X) < Epsilon.Eps;
    }

    private static bool PointsSameY(Vec2 point1, Vec2 point2)
    {
        return Math.Abs(point1.Y - point2.Y) < Epsilon.Eps;
    }

    internal static bool PointsSame(Vec2 point1, Vec2 point2)
    {
        return PointsSameX(point1, point2) && PointsSameY(point1, point2);
    }

    internal static int PointsCompare(Vec2 point1, Vec2 point2)
    {
        if (PointsSameX(point1, point2))
        {
            return PointsSameY(point1, point2) ? 0 : (point1.Y < point2.Y ? -1 : 1);
        }
        return point1.X < point2.X ? -1 : 1;
    }

    internal static bool PointsCollinear(Vec2 pt1, Vec2 pt2, Vec2 pt3)
    {
        var dx1 = pt1.X - pt2.X;
        var dy1 = pt1.Y - pt2.Y;
        var dx2 = pt2.X - pt3.X;
        var dy2 = pt2.Y - pt3.Y;
        return Math.Abs(dx1 * dy2 - dx2 * dy1) < Epsilon.Eps;
    }

    internal static IntersectionPoint LinesIntersect(Vec2 a0, Vec2 a1, Vec2 b0, Vec2 b1)
    {
        double adx = a1.X - a0.X;
        double ady = a1.Y - a0.Y;
        double bdx = b1.X - b0.X;
        double bdy = b1.Y - b0.Y;

        double axb = adx * bdy - ady * bdx;

        if (Math.Abs(axb) < Epsilon.Eps)
        {
            return null;
        }

        double dx = a0.X - b0.X;
        double dy = a0.Y - b0.Y;

        double a = (bdx * dy - bdy * dx) / axb;
        double b = (adx * dy - ady * dx) / axb;

        return new IntersectionPoint(CalcAlongUsingValue(a), CalcAlongUsingValue(b), new Vec2(a0.X + a * adx, a0.Y + a * ady));
    }

    private static int CalcAlongUsingValue(double value)
    {
        if (value <= -Epsilon.Eps)
        {
            return -2;
        }
        else if (value < Epsilon.Eps)
        {
            return -1;
        }
        else if (value - 1 <= -Epsilon.Eps)
        {
            return 0;
        }
        else if (value - 1 < Epsilon.Eps)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }
}