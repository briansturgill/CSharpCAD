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

    internal static double AreaVec2(Vec2[] points)
    {
        var area = ((double)0.0);
        var len = points.Length;
        for (var i = 0; i < len; i++)
        {
            var j = (i + 1) % len;
            area += points[i].X * points[j].Y;
            area -= points[j].X * points[i].Y;
        }
        return (area / 2.0);
    }

    internal const int LOG_INFO = 1;
    internal static void Log(string message, int level = LOG_INFO)
    {
        var traceLines = Environment.StackTrace.Split('\n', '\r');
        var first = 0;
        for (var i = 0; i < traceLines.Length; i++)
        {
            traceLines[i] = traceLines[i].Trim();
            traceLines[i] = Regex.Replace(traceLines[i], @":line ", ":") + ":1";
            if (traceLines[i].Contains("at CSharpCAD.CSCAD.")) first = i;
        }
        var call = "";
        if (first != 0)
        {
            call = traceLines[first];
            var idx = call.IndexOf("(");
            if (idx != -1) call = call.Remove(idx);
            idx = call.LastIndexOf(".");
            if (idx != -1) call = call.Remove(0, idx+1);
            first++;
        }

        Console.WriteLine($"{message}: calling {call}");
        for (int i = first; i < traceLines.Length; i++)
        {
            Console.WriteLine($"    {traceLines[i]}");
        }
    }
}