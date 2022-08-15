namespace CSharpCAD;

public static partial class CSCAD
{
    /// <summary>Save a 2D geometry object in a file suitable for printing, etc.</summary>
    /// <remarks>
    /// For formats that only support one output type, the binary flag is ignored.
    /// The output placed in "file" is determined by the files extention.
    /// Currently supported:
    ///     .svg    SVG - works for 2D geometry only.
    /// <example>
    /// var g = Circle(radius: 5);
    /// Save("circle.svg", g);
    /// </example>
    /// </remarks>
    /// <group>Miscellaneous</group>
    public static void Save(string file, Geom2 g, bool binary = true)
    {
        if (file.EndsWith(".svg"))
        {
            SerializeToSVG(file, g);
        }
        else
        {
            throw new ArgumentException($"Sorry but the output file type of \"{file}\" is not one we understand!");
        }
    }

    /// <summary>Save a geometry object in a file suitable for printing, etc.</summary>
    /// <remarks>
    /// For formats that only support one output type, the binary flag is ignored.
    /// The output placed in "file" is determined by the files extention.
    /// Currently supported:
    ///     .stl    STL - both Binary and ASCII supported. 3D only.
    ///     .amf    AMF - works for 3D geometry only.
    /// <example>
    /// var g = Cylinder(radius: 5);
    /// Save("cylinder.stl", g);
    /// </example>
    /// </remarks>
    /// <group>Miscellaneous</group>
    public static void Save(string file, Geom3 g, bool binary = true)
    {
        if (file.EndsWith(".amf"))
        {
            SerializeToAMF(file, g);
        }
        else if (file.EndsWith(".stl"))
        {
            if (binary)
            {
                SerializeToSTLBinary(file, g);
            }
            else
            {
                SerializeToSTLText(file, g);
            }
        }
        else
        {
            throw new ArgumentException($"Sorry but the output file type of \"{file}\" is not one we understand!");
        }
    }

    /// <summary>Points2 is used by Polyhedron for points.</summary>
    public class Points2 : List<Vec2>
    {
        ///
        public Points2() : base() { }
        ///
        public Points2(int s) : base(s) { }
    }

    /// <summary>Paths is used by Polygon for paths.</summary>
    public class Paths : List<Path>
    {
        ///
        public Paths() : base() { }
        ///
        public Paths(int s) : base(s) { }
    }

    /// <summary>Path is used by Polygon for paths.</summary>
    public class Path : List<int>
    {
        ///
        public Path() : base() { }
        ///
        public Path(int s) : base(s) { }
    }

    /// <summary>Points3 is used by Polyhedron for points.</summary>
    public class Points3 : List<Vec3>
    {
        ///
        public Points3() : base() { }
        ///
        public Points3(int s) : base(s) { }
    }

    /// <summary>Faces is used by Polyhedron for faces.</summary>
    public class Faces : List<Face>
    {
        ///
        public Faces() : base() { }
        ///
        public Faces(int s) : base(s) { }
    }

    /// <summary>Face is used by Polygon for faces.</summary>
    public class Face : List<int>
    {
        ///
        public Face() : base() { }
        ///
        public Face(int s) : base(s) { }
    }

    private static Dictionary<double, double> quickSin = new Dictionary<double, double>();
    private static Dictionary<double, double> quickCos = new Dictionary<double, double>();
    private static Dictionary<double, double> quickTan = new Dictionary<double, double>();
    private static void addVal(Dictionary<double, double> dict, double dtor, double rad, double trig)
    {
        if (dtor == rad)
        {
            dict[dtor] = trig;
        }
        else
        {
            dict[dtor] = trig;
            dict[rad] = trig;
        }
    }

    static void initMisc()
    {
        addVal(quickSin, DegToRad(0),   0,            0);
        addVal(quickSin, DegToRad(30),  Math.PI/6,    0.5);
        addVal(quickSin, DegToRad(45),  Math.PI/4,    Math.Sin(Math.PI/4));
        addVal(quickSin, DegToRad(60),  Math.PI/3,    Math.Sin(Math.PI/3));
        addVal(quickSin, DegToRad(90),  Math.PI/2,    1);
        addVal(quickSin, DegToRad(120), 2*Math.PI/3,  Math.Sin(2*Math.PI/3));
        addVal(quickSin, DegToRad(135), 3*Math.PI/4,  Math.Sin(3*Math.PI/4));
        addVal(quickSin, DegToRad(150), 5*Math.PI/6,  0.5);
        addVal(quickSin, DegToRad(180), Math.PI,      0);
        addVal(quickSin, DegToRad(210), 7*Math.PI/6,  -0.5);
        addVal(quickSin, DegToRad(225), 5*Math.PI/4,  Math.Sin(5*Math.PI/4));
        addVal(quickSin, DegToRad(240), 4*Math.PI/3,  Math.Sin(4*Math.PI/3));
        addVal(quickSin, DegToRad(270), 3*Math.PI/2,  -1);
        addVal(quickSin, DegToRad(300), 5*Math.PI/3,  Math.Sin(5*Math.PI/3));
        addVal(quickSin, DegToRad(315), 7*Math.PI/4,  Math.Sin(7*Math.PI/4));
        addVal(quickSin, DegToRad(330), 11*Math.PI/6, -0.5);
        addVal(quickSin, DegToRad(360), 2*Math.PI,    0);

        addVal(quickCos, DegToRad(0),   0,            1);
        addVal(quickCos, DegToRad(30),  Math.PI/6,    Math.Cos(Math.PI/6));
        addVal(quickCos, DegToRad(45),  Math.PI/4,    Math.Cos(Math.PI/4));
        addVal(quickCos, DegToRad(60),  Math.PI/3,    0.5);
        addVal(quickCos, DegToRad(90),  Math.PI/2,    0);
        addVal(quickCos, DegToRad(120), 2*Math.PI/3,  -0.5);
        addVal(quickCos, DegToRad(135), 3*Math.PI/4,  Math.Cos(3*Math.PI/4));
        addVal(quickCos, DegToRad(150), 5*Math.PI/6,  Math.Cos(5*Math.PI/6));
        addVal(quickCos, DegToRad(180), Math.PI,      -1);
        addVal(quickCos, DegToRad(210), 7*Math.PI/6,  Math.Cos(7*Math.PI/6));
        addVal(quickCos, DegToRad(225), 5*Math.PI/4,  Math.Cos(5*Math.PI/4));
        addVal(quickCos, DegToRad(240), 4*Math.PI/3,  -0.5);
        addVal(quickCos, DegToRad(270), 3*Math.PI/2,  0);
        addVal(quickCos, DegToRad(300), 5*Math.PI/3,  0.5);
        addVal(quickCos, DegToRad(315), 7*Math.PI/4,  Math.Cos(7*Math.PI/4));
        addVal(quickCos, DegToRad(330), 11*Math.PI/6, Math.Cos(11*Math.PI/6));
        addVal(quickCos, DegToRad(360), 2*Math.PI,    1);

        addVal(quickTan, DegToRad(0),   0,            0);
        addVal(quickTan, DegToRad(30),  Math.PI/6,    Math.Tan(Math.PI/6));
        addVal(quickTan, DegToRad(45),  Math.PI/4,    1);
        addVal(quickTan, DegToRad(60),  Math.PI/3,    Math.Tan(Math.PI/3));
        // Undefined addVal(quickTan, DegToRad(90),  Math.PI/2,    Math.Tan(Math.PI/2));
        addVal(quickTan, DegToRad(120), 2*Math.PI/3,  Math.Tan(2*Math.PI/3));
        addVal(quickTan, DegToRad(135), 3*Math.PI/4,  -1);
        addVal(quickTan, DegToRad(150), 5*Math.PI/6,  Math.Tan(5*Math.PI/6));
        addVal(quickTan, DegToRad(180), Math.PI,      0);
        addVal(quickTan, DegToRad(210), 7*Math.PI/6,  Math.Tan(7*Math.PI/6));
        addVal(quickTan, DegToRad(225), 5*Math.PI/4,  1);
        addVal(quickTan, DegToRad(240), 4*Math.PI/3,  Math.Tan(4*Math.PI/3));
        // Undefined addVal(quickTan, DegToRad(270), 3*Math.PI/2,  Math.Tan(3*Math.PI/2));
        addVal(quickTan, DegToRad(300), 5*Math.PI/3,  Math.Tan(5*Math.PI/3));
        addVal(quickTan, DegToRad(315), 7*Math.PI/4,  -1);
        addVal(quickTan, DegToRad(330), 11*Math.PI/6, Math.Tan(11*Math.PI/6));
        addVal(quickTan, DegToRad(360), 2*Math.PI,    0);
    }

    internal static double SinR(double angleInRadians)
    {
        var sin = 0.0;
        if(quickSin.TryGetValue(angleInRadians, out sin)) return sin;
        return Math.Sin(angleInRadians);
    }

    internal static double CosR(double angleInRadians)
    {
        var cos = 0.0;
        if(quickCos.TryGetValue(angleInRadians, out cos)) return cos;
        return Math.Cos(angleInRadians);
    }

    internal static double TanR(double angleInRadians)
    {
        var tan = 0.0;
        if(quickTan.TryGetValue(angleInRadians, out tan)) return tan;
        return Math.Tan(angleInRadians);
    }

    /// <summary>Cosine of angle in degrees.</summary>
    /// <group>Trigonometry</group>
    public static double Cos(double angleInDegrees)
    {
        return CosR(DegToRad(angleInDegrees));
    }

    /// <summary>Sine of angle in degrees.</summary>
    /// <group>Trigonometry</group>
    public static double Sin(double angleInDegrees)
    {
        return SinR(DegToRad(angleInDegrees));
    }

    /// <summary>Tangent of angle in degrees.</summary>
    /// <group>Trigonometry</group>
    public static double Tan(double angleInDegrees)
    {
        return TanR(DegToRad(angleInDegrees));
    }

    /// <summary>Hyperbolic cosine of angle in degrees.</summary>
    /// <group>Trigonometry</group>
    public static double Cosh(double angleInDegrees)
    {
        return Math.Cosh(DegToRad(angleInDegrees));
    }

    /// <summary>Hyperbolic sine of angle in degrees.</summary>
    /// <group>Trigonometry</group>
    public static double Sinh(double angleInDegrees)
    {
        return Math.Sinh(DegToRad(angleInDegrees));
    }

    /// <summary>Hyperbolic tangent of angle in degrees.</summary>
    /// <group>Trigonometry</group>
    public static double Tanh(double angleInDegrees)
    {
        return Math.Tanh(DegToRad(angleInDegrees));
    }

    /// <summary>ArcCosine of cosVal returning angle in degrees.</summary>
    /// <group>Trigonometry</group>
    public static double Acos(double cosVal)
    {
        return RadToDeg(Math.Acos(cosVal));
    }

    /// <summary>ArcSine of sinVal returning angle in degrees.</summary>
    /// <group>Trigonometry</group>
    public static double Asin(double sinVal)
    {
        return RadToDeg(Math.Asin(sinVal));
    }

    /// <summary>ArcTan of tanVal returning angle in degrees.</summary>
    /// <group>Trigonometry</group>
    public static double Atan(double tanVal)
    {
        return RadToDeg(Math.Atan(tanVal));
    }

    /// <summary>ArcTan of quotient of y and x returning angle in degrees.</summary>
    /// <group>Trigonometry</group>
    public static double Atan2(double y, double x)
    {
        return RadToDeg(Math.Atan2(y, x));
    }

    /// <summary>ArcCosine of hyperbolic cosVal returning angle in degrees.</summary>
    /// <group>Trigonometry</group>
    public static double Acosh(double cosVal)
    {
        return RadToDeg(Math.Acosh(cosVal));
    }

    /// <summary>ArcSine of hyperbolic sinVal returning angle in degrees.</summary>
    /// <group>Trigonometry</group>
    public static double Asinh(double sinVal)
    {
        return RadToDeg(Math.Asinh(sinVal));
    }

    /// <summary>ArcTan of hyperbolic tanVal returning angle in degrees.</summary>
    /// <group>Trigonometry</group>
    public static double Atanh(double tanVal)
    {
        return RadToDeg(Math.Atanh(tanVal));
    }

    /// <summary>Convert degrees to radians.</summary>
    /// <group>Trigonometry</group>
    public static double DegToRad(double angleInDegrees)
    {
        return angleInDegrees * (Math.PI / 180);
    }

    /// <summary>Convert radians to degrees.</summary>
    /// <group>Trigonometry</group>
    public static double RadToDeg(double angleInRadians)
    {
        return angleInRadians * (180 / Math.PI);
    }

    /// <summary>String description of winding of a 2D polygon.</summary>
    /// <group>Miscellaneous</group>
    public static string Winding(List<Vec2> lv)
    {
        string wstr(double winding)
        {
            if (winding > 0) return "cw";
            if (winding < 0) return "ccw";
            return "zero";
        }
        var len = lv.Count;
        if (len < 3) return "too small";
        var winding = 0.0;
        for (var i = 0; i < len; i++)
        {
            winding += (lv[(i + 1) % len].X - lv[i].X) * (lv[(i + 1) % len].Y + lv[i].Y);
        }
        return wstr(winding);
    }

    /// <summary>String description of winding of a 2D polygon.</summary>
    /// <group>Miscellaneous</group>
    public static string Winding(Vec2[] av)
    {
        string wstr(double winding)
        {
            if (winding > 0) return "cw";
            if (winding < 0) return "ccw";
            return "zero";
        }
        var len = av.Length;
        if (len < 3) return "too small";
        var winding = 0.0;
        for (var i = 0; i < len; i++)
        {
            winding += (av[(i + 1) % len].X - av[i].X) * (av[(i + 1) % len].Y + av[i].Y);
        }
        return wstr(winding);
    }
}