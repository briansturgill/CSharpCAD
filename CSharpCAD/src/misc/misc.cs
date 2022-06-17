namespace CSharpCAD;

public static partial class CSCAD
{
    /// <summary>Print string representations of the listed arguments.</summary>
    /// <example>
    /// var a = "test";
    /// Echo(1.2, a, new Vec3(1, 2, 3));
    /// </example>
    /// <group>Miscellaneous</group>
    public static void Echo(params object[] args)
    {
        var first = true;
        foreach (var o in args)
        {
            if (!first)
            {
                Console.Write(" ");
            }
            Console.Write(o.ToString());
            first = false;
        }
        Console.WriteLine("");
    }

    /// <summary>Save a geometry object in a file suitable for printing, etc.</summary>
    /// <remarks>
    /// For formats that only support one output type, the binary flag is ignored.
    /// The output placed in "file" is determined by the files extention.
    /// Currently supported:
    ///     .stl    STL - both Binary and ASCII supported. 3D only.
    ///     .svg    SVG - works for 2D geometry only.
    ///     .amf    AMF - works for 3D geometry only.
    /// <example>
    /// var g = Circle(radius: 5);
    /// Save("circle.svg", g);
    /// </example>
    /// </remarks>
    /// <group>Miscellaneous</group>
    public static void Save(string file, Geometry g, bool binary = true)
    {
        if (file.EndsWith(".svg"))
        {
            SerializeToSVG(file, g);
        }
        else if (file.EndsWith(".amf"))
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

    /// <summary>Cosine of angle in degrees.</summary>
    /// <group>Trigonometry</group>
    public static double Cos(double angleInDegrees)
    {
        return Math.Cos(DegToRad(angleInDegrees));
    }

    /// <summary>Sine of angle in degrees.</summary>
    /// <group>Trigonometry</group>
    public static double Sin(double angleInDegrees)
    {
        return Math.Sin(DegToRad(angleInDegrees));
    }

    /// <summary>Tangent of angle in degrees.</summary>
    /// <group>Trigonometry</group>
    public static double Tan(double angleInDegrees)
    {
        return Math.Tan(DegToRad(angleInDegrees));
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
        return angleInRadians * (180/Math.PI);
    }
}