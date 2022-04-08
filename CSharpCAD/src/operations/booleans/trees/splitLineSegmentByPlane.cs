namespace CSharpCAD;

public static partial class CSCAD
{

    private static Vec3 SplitLineSegmentByPlane(Plane plane, Vec3 p1, Vec3 p2)
    {
        var direction = p2.Subtract(p1);
        var lambda = (plane.w - plane.normal.Dot(p1)) / plane.normal.Dot(direction);
        if (double.IsNaN(lambda)) lambda = 0;
        if (lambda > 1) lambda = 1;
        if (lambda < 0) lambda = 0;

        direction = direction.Scale(lambda);
        direction = p1.Add(direction);
        return direction;
    }
}