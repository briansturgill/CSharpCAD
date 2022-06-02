namespace CSharpCAD;

public static partial class CSCAD
{

    /*
     * Original source from quickhull3d (https://github.com/mauriciopoppe/quickhull3d)
     * Copyright (c) 2015 Mauricio Poppe
     *
     * Adapted to JSCAD by Jeff Gay
     */

    internal static Vec3 GetPlaneNormal(Vec3 point1, Vec3 point2, Vec3 point3)
    {
        var outv = point1.Subtract(point2);
        outv = outv.Cross(point2.Subtract(point3));
        return outv.Normalize();
    }

}