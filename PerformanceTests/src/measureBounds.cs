namespace PerformanceTests;

public static class MeasureBounds
{

    public static Geom2 circle_10; // 10 points
    public static Geom2 circle_100; // 100 points
    public static Geom2 circle_1000; // 1000 points
    public static Geom2 circle_10000; // 10000 points

    public static Geom3 sphere_7; // 98 points
    public static Geom3 sphere_22; // 1012 points
    public static Geom3 sphere_70; // 9940 points
    public static Geom3 sphere_158; // 50244 points
    public static Geom3 sphere_224; // 99904 points

    static MeasureBounds()
    {
        circle_10 = Circle(radius: 100, segments: 10);
        circle_100 = Circle(radius: 100, segments: 100);
        circle_1000 = Circle(radius: 100, segments: 1000);
        circle_10000 = Circle(radius: 100, segments: 10000);

        sphere_7 = Sphere(radius: 100, segments: 7);
        sphere_22 = Sphere(radius: 100, segments: 22);
        sphere_70 = Sphere(radius: 100, segments: 70);
        sphere_158 = Sphere(radius: 100, segments: 158);
        sphere_224 = Sphere(radius: 100, segments: 224);
    }

    public static void Init()
    {
        Program.pTests.Add(new PTest
        {
            name = "measureBounds(10)",
            api = "measureBounds(circle)",
            div = "10",
            func = () => { var (min, max) = circle_10.BoundingBox(); return circle_10; }
        });

        Program.pTests.Add(new PTest
        {
            name = "measureBounds(100)",
            api = "measureBounds(circle)",
            div = "100",
            func = () => { var (min, max) = circle_100.BoundingBox(); return circle_100; }
        });

        Program.pTests.Add(new PTest
        {
            name = "measureBounds(1000)",
            api = "measureBounds(circle)",
            div = "1000",
            func = () => { var (min, max) = circle_1000.BoundingBox(); return circle_1000; }
        });

        Program.pTests.Add(new PTest
        {
            name = "measureBounds(10000)",
            api = "measureBounds(circle)",
            div = "10000",
            func = () => { var (min, max) = circle_10000.BoundingBox(); return circle_10000; }
        });

        Program.pTests.Add(new PTest
        {
            name = "measureBounds(100)",
            api = "measureBounds(sphere)",
            div = "100",
            func = () => { var (min, max) = sphere_7.BoundingBox(); return sphere_7; }
        });

        Program.pTests.Add(new PTest
        {
            name = "measureBounds(1000)",
            api = "measureBounds(sphere)",
            div = "1000",
            func = () => { var (min, max) = sphere_22.BoundingBox(); return sphere_22; }
        });

        Program.pTests.Add(new PTest
        {
            name = "measureBounds(10000)",
            api = "measureBounds(sphere)",
            div = "10000",
            func = () => { var (min, max) = sphere_70.BoundingBox(); return sphere_70; }
        });

        Program.pTests.Add(new PTest
        {
            name = "measureBounds(100000)",
            api = "measureBounds(sphere)",
            div = "100000",
            func = () => { var (min, max) = sphere_224.BoundingBox(); return sphere_224; }
        });
    }
}