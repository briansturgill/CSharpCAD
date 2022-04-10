namespace CSharpCAD;

public static partial class CSCAD
{
    // returns angle C
    private static double solveAngleFromSSS(double a, double b, double c) => Math.Acos(((a * a) + (b * b) - (c * c)) / (2 * a * b));

    // returns side c
    private static double solveSideFromSAS(double a, double C, double b)
    {
        if (C > CSharpCAD.C.NEPS)
        {
            return Math.Sqrt(a * a + b * b - 2 * a * b * Math.Cos(C));
        }

        // Explained in https://www.nayuki.io/page/numerically-stable-law-of-cosines
        return Math.Sqrt((a - b) * (a - b) + a * b * C * C * (1 - C * C / 12));
    }

    // AAA is when three angles of a triangle, but no sides
    private static Geom2 solveAAA(Vec3 angles)
    {
        var eps = Math.Abs(angles.x + angles.y + angles.z - Math.PI);
        if (eps > CSharpCAD.C.NEPS) throw new ArgumentException("AAA triangles require angles that sum to PI");

        var A = angles.x;
        var B = angles.y;
        var C = Math.PI - A - B;

        // Note: This is not 100% proper but...
        // default the side c length to 1
        // solve the other lengths
        var c = 1;
        var a = (c / Math.Sin(C)) * Math.Sin(A);
        var b = (c / Math.Sin(C)) * Math.Sin(B);
        return createTriangle(A, B, C, a, b, c);
    }

    // AAS is when two angles and one side are known, and the side is not between the angles
    private static Geom2 solveAAS(Vec3 values)
    {
        var A = values.x;
        var B = values.y;
        var C = Math.PI + CSharpCAD.C.NEPS - A - B;

        if (C < CSharpCAD.C.NEPS) throw new ArgumentException("AAS triangles require angles that sum to PI");

        var a = values.z;
        var b = (a / Math.Sin(A)) * Math.Sin(B);
        var c = (a / Math.Sin(A)) * Math.Sin(C);
        return createTriangle(A, B, C, a, b, c);
    }

    // ASA is when two angles and the side between the angles are known
    private static Geom2 solveASA(Vec3 values)
    {
        var A = values.x;
        var B = values.z;
        var C = Math.PI + CSharpCAD.C.NEPS - A - B;

        if (C < CSharpCAD.C.NEPS) throw new ArgumentException("ASA triangles require angles that sum to PI");

        var c = values.y;
        var a = (c / Math.Sin(C)) * Math.Sin(A);
        var b = (c / Math.Sin(C)) * Math.Sin(B);
        return createTriangle(A, B, C, a, b, c);
    }

    // SAS is when two sides and the angle between them are known
    private static Geom2 solveSAS(Vec3 values)
    {
        var c = values.x;
        var B = values.y;
        var a = values.z;

        var b = solveSideFromSAS(c, B, a);

        var A = solveAngleFromSSS(b, c, a); // solve for A
        var C = Math.PI - A - B;
        return createTriangle(A, B, C, a, b, c);
    }

    // SSA is when two sides and an angle that is not the angle between the sides are known
    private static Geom2 solveSSA(Vec3 values)
    {
        var c = values.x;
        var a = values.y;
        var C = values.z;

        var A = Math.Asin(a * Math.Sin(C) / c);
        var B = Math.PI - A - C;

        var b = (c / Math.Sin(C)) * Math.Sin(B);
        return createTriangle(A, B, C, a, b, c);
    }

    // SSS is when we know three sides of the triangle
    private static Geom2 solveSSS(Vec3 lengths)
    {
        var a = lengths.y;
        var b = lengths.z;
        var c = lengths.x;
        if (((a + b) <= c) || ((b + c) <= a) || ((c + a) <= b))
        {
            throw new ArgumentException("SSS triangle is incorrect, as the longest side is longer than the sum of the other sides");
        }

        var A = solveAngleFromSSS(b, c, a); // solve for A
        var B = solveAngleFromSSS(c, a, b); // solve for B
        var C = Math.PI - A - B;
        return createTriangle(A, B, C, a, b, c);
    }

    private static Geom2 createTriangle(double A, double B, double C, double a, double b, double c)
    {
        var p0 = new Vec2(0, 0); // everything starts from 0, 0
        var p1 = new Vec2(c, 0);
        var p2 = new Vec2(a, 0);
        p2 = p2.Rotate(new Vec2(0, 0), Math.PI - B).Add(p1);
        return new Geom2(new List<Vec2> { p0, p1, p2 });
    }

    /**
     * <summary>Construct a triangle in two dimensional space from the given options.</summary>
     * The triangle is always constructed CCW from the origin, [0, 0, 0].
     * @see https://www.mathsisfun.com/algebra/trig-solving-triangles.html
     * @param {Object} [options] - options for construction
     * @param {String} [options.type="SSS" - type of triangle to construct; A ~ angle, S ~ side
     * @param {Array} [options.values=[1,1,1]] - angle (radians) of corners or length of sides
     * @returns {geom2} new 2D geometry
     * @alias module:modeling/primitives.triangle
     *
     * @example
     * var myshape = triangle({type: "AAS", values: [degToRad(62), degToRad(35), 7]})
     */
    public static Geom2 Triangle(string type = "SSS", Vec3? values = null)
    {
        var _values = values ?? new Vec3(1, 1, 1);

        type = type.ToUpper();
        if (!((type[0] == 'A' || type[0] == 'S') &&
              (type[1] == 'A' || type[1] == 'S') &&
              (type[2] == 'A' || type[2] == 'S'))) throw new ArgumentException("Triangle type must contain three letters: A or S");

        if (_values.x < 0 || _values.y < 0 || _values.z < 0) throw new ArgumentException("Triangle values must be greater than zero.");

        switch (type)
        {
            case "AAA":
                return solveAAA(_values);
            case "AAS":
                return solveAAS(_values);
            case "ASA":
                return solveASA(_values);
            case "SAS":
                return solveSAS(_values);
            case "SSA":
                return solveSSA(_values);
            case "SSS":
                return solveSSS(_values);
            default:
                throw new ArgumentException("Invalid triangle type");
        }
    }
}
