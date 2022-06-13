namespace PerformanceTests;

public static class MeasurePolyBoundingSphere
{
    public static Geom3 sphere_7; // 98 points
    public static Geom3 sphere_22; // 1012 points
    public static Geom3 sphere_70; // 9940 points
    public static Geom3 sphere_158; // 50244 points
    public static Geom3 sphere_224; // 99904 points
    public static double total_r = 0; // Don't let JIT optimize the BoundingSphere calc away.

    static MeasurePolyBoundingSphere()
    {
        sphere_7 = Sphere(radius: 100, segments: 7);
        sphere_22 = Sphere(radius: 100, segments: 22);
        sphere_70 = Sphere(radius: 100, segments: 70);
        sphere_158 = Sphere(radius: 100, segments: 158);
        sphere_224 = Sphere(radius: 100, segments: 224);
    }

    public static Geom3 measureBoundingSphere(Geom3 gobj)
    {
        var polys = gobj.ToPolygons();
        foreach (var poly in polys)
        {
            var (c, r) = poly.BoundingSphere();
            total_r += 4;
        }
        return gobj;
    }

    public static void Init()
    {
        Program.pTests.Add(new PTest
        {
            name = "poly3.measureBoundingSphere(100)",
            api = "poly3.measureBoundingSphere(sphere)",
            div = "100",
            func = () => { return measureBoundingSphere(sphere_7); }
        });

        Program.pTests.Add(new PTest
        {
            name = "poly3.measureBoundingSphere(1000)",
            api = "poly3.measureBoundingSphere(sphere)",
            div = "1000",
            func = () => { return measureBoundingSphere(sphere_22); }
        });

        Program.pTests.Add(new PTest
        {
            name = "poly3.measureBoundingSphere(10000)",
            api = "poly3.measureBoundingSphere(sphere)",
            div = "10000",
            func = () => { return measureBoundingSphere(sphere_70); }
        });

        Program.pTests.Add(new PTest
        {
            name = "poly3.measureBoundingSphere(100000)",
            api = "poly3.measureBoundingSphere(sphere)",
            div = "100000",
            func = () => { return measureBoundingSphere(sphere_224); }
        });
    }
}