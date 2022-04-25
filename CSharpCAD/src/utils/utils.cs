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

    public static bool GreaterThanOrEqualish(double a, double b) {
        if (Equalish(a, b)) {
            return true;
        }
        return a >= b;
    }

    public static bool GreaterThanish(double a, double b) {
        if (Equalish(a, b)) {
            return false;
        }
        return a > b;
    }

    public static bool LessThanOrEqualish(double a, double b) {
        if (Equalish(a, b)) {
            return true;
        }
        return a <= b;
    }

    public static bool LessThanish(double a, double b) {
        if (Equalish(a, b)) {
            return false;
        }
        return a < b;
    }

    public static int Floorish(double a, double epsilon = C.EPS)
    {
        var a_ceil = Math.Ceiling(a);
        if (Equalish(a, a_ceil, epsilon)) {
            return (int)a_ceil;
        }
        return (int)Math.Floor(a);
    }

    private static double rezero(double v) => Math.Abs(v) < C.NEPS ? 0 : v;

    public static double xsin(double angle)
    {
        return rezero(Math.Sin(angle));
    }

    public static double xcos(double angle)
    {
        return rezero(Math.Cos(angle));
    }
}