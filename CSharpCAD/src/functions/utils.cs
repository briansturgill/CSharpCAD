namespace CSharpCAD;

public static partial class CSCAD
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

    public static int Floorish(double a, double epsilon = C.EPS)
    {
        var a_ceil = Math.Ceiling(a);
        if (Equalish(a, a_ceil, epsilon)) {
            return (int)a_ceil;
        }
        return (int)Math.Floor(a);
    }
}