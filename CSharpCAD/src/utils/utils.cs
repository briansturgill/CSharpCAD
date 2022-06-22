namespace CSharpCAD;

internal static partial class CSharpCADInternals
{
    public static bool Equalish(double a, double b, double epsilon = C.EPS)
    {
        if (a == b)
        { // shortcut, also handles infinities
            return true;
        }

        var absA = Math.Abs(a);
        var absB = Math.Abs(b);
        var diff = Math.Abs(a - b);
        if (double.IsNaN(diff))
        {
            throw new ValidationException("Should never have NaN.");
        }
        return diff <= epsilon;
    }

    public static bool GreaterThanOrEqualish(double a, double b, double epsilon = C.EPS)
    {
        if (Equalish(a, b, epsilon))
        {
            return true;
        }
        return a >= b;
    }

    public static bool GreaterThanish(double a, double b, double epsilon = C.EPS)
    {
        if (Equalish(a, b, epsilon))
        {
            return false;
        }
        return a > b;
    }

    public static bool LessThanOrEqualish(double a, double b, double epsilon = C.EPS)
    {
        if (Equalish(a, b, epsilon))
        {
            return true;
        }
        return a <= b;
    }

    public static bool LessThanish(double a, double b, double epsilon = C.EPS)
    {
        if (Equalish(a, b, epsilon))
        {
            return false;
        }
        return a < b;
    }

    public static int Floorish(double a, double epsilon = C.EPS)
    {
        var a_ceil = Math.Ceiling(a);
        if (Equalish(a, a_ceil, epsilon))
        {
            return (int)a_ceil;
        }
        return (int)Math.Floor(a);
    }

    internal static double AreaVec2(List<Vec2> points)
    {
        var area = ((double)0.0);
        var len = points.Count;
        for (var i = 0; i < len; i++)
        {
            var j = (i + 1) % len;
            area += points[i].X * points[j].Y;
            area -= points[j].X * points[i].Y;
        }
        return (area / 2.0);
    }

    // This is used to pull Sin and Cos back to zero
    // I don't approve of its usage, but JSCAD uses it to fix flaws in various places.
    internal static double Rezero(double val) => Math.Abs(val) < C.NEPS ? 0 : val;
}