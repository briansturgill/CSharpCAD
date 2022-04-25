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

    public static bool GreaterThanOrEqualish(double a, double b, double epsilon = C.EPS) {
        if (Equalish(a, b, epsilon)) {
            return true;
        }
        return a >= b;
    }

    public static bool GreaterThanish(double a, double b, double epsilon = C.EPS) {
        if (Equalish(a, b, epsilon)) {
            return false;
        }
        return a > b;
    }

    public static bool LessThanOrEqualish(double a, double b, double epsilon = C.EPS) {
        if (Equalish(a, b, epsilon)) {
            return true;
        }
        return a <= b;
    }

    public static bool LessThanish(double a, double b, double epsilon = C.EPS) {
        if (Equalish(a, b, epsilon)) {
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
}