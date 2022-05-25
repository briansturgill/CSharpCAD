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
    public static double Cos(double angleInDegrees)
    {
        return RadToDeg(Math.Cos(DegToRad(angleInDegrees)));
    }

    /// <summary>Sine of angle in degrees.</summary>
    public static double Sin(double angleInDegrees)
    {
        return RadToDeg(Math.Sin(DegToRad(angleInDegrees)));
    }
}