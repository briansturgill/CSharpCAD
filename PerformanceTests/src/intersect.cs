namespace PerformanceTests;

public static class Intersects
{
    public static Vec3[] centers = new Vec3[]
    {
        (0, 0, 0),
        (10, 10, 10),
        (20, 20, 20),
        (30, 30, 30),
        (40, 40, 40),
        (50, 50, 50),
        (60, 60, 60),
        (70, 70, 70),
        (80, 80, 80),
        (90, 90, 90),
    };

    public static Geom2[] circle_100 = new Geom2[centers.Length]; // 10 points X centers
    public static Geom2[] circle_500 = new Geom2[centers.Length];     // 50 points X centers
    public static Geom2[] circle_1000 = new Geom2[centers.Length]; // 100 points X centers
    public static Geom2[] circle_5000 = new Geom2[centers.Length];     // 500 points X centers
    public static Geom2[] circle_10000 = new Geom2[centers.Length];     // 1000 points X centers

    public static Geom3[] sphere_600 = new Geom3[centers.Length];     // 60 points X centers
    public static Geom3[] sphere_1000 = new Geom3[centers.Length];     // 98 points X centers
    public static Geom3[] sphere_10000 = new Geom3[centers.Length];     // 1012 points X centers
    public static Geom3[] sphere_100000 = new Geom3[centers.Length];     // 9940 points X centers

    static Intersects()
    {

        circle_100 = centers.Select((center) => ((Geom2)(Translate(center, Circle(radius: 100, segments: 10))))).ToArray();
        circle_500 = centers.Select((center) => ((Geom2)Translate(center, Circle(radius: 100, segments: 50)))).ToArray();
        circle_1000 = centers.Select((center) => ((Geom2)Translate(center, Circle(radius: 100, segments: 100)))).ToArray();
        circle_5000 = centers.Select((center) => ((Geom2)Translate(center, Circle(radius: 100, segments: 500)))).ToArray();
        circle_10000 = centers.Select((center) => ((Geom2)Translate(center, Circle(radius: 100, segments: 1000)))).ToArray();

        sphere_600 = centers.Select((center) => ((Geom3)Translate(center, Sphere(radius: 100, segments: 6)))).ToArray();
        sphere_1000 = centers.Select((center) => ((Geom3)Translate(center, Sphere(radius: 100, segments: 7)))).ToArray();
        sphere_10000 = centers.Select((center) => ((Geom3)Translate(center, Sphere(radius: 100, segments: 22)))).ToArray();
        sphere_100000 = centers.Select((center) => ((Geom3)Translate(center, Sphere(radius: 100, segments: 70)))).ToArray();

    }

    public static void Init()
    {
        Program.pTests.Add(new PTest
        {
            name = "intersect(100)",
            api = "intersect(circle)",
            div = "100",
            func = () => { return Intersect(circle_100); }
        });

        Program.pTests.Add(new PTest
        {
            name = "intersect(500)",
            api = "intersect(circle)",
            div = "500",
            func = () => { return Intersect(circle_500); }
        });

        Program.pTests.Add(new PTest
        {
            name = "intersect(1000)",
            api = "intersect(circle)",
            div = "1000",
            func = () => { return Intersect(circle_1000); }
        });

        Program.pTests.Add(new PTest
        {
            name = "intersect(5000)",
            api = "intersect(circle)",
            div = "5000",
            func = () => { return Intersect(circle_5000); }
        });

        Program.pTests.Add(new PTest
        {
            name = "intersect(10000)",
            api = "intersect(circle)",
            div = "10000",
            func = () => { return Intersect(circle_10000); }
        });

        Program.pTests.Add(new PTest
        {
            name = "intersect(600)",
            api = "intersect(sphere)",
            div = "600",
            func = () => { return Intersect(sphere_600); }
        });

        Program.pTests.Add(new PTest
        {
            name = "intersect(1000)",
            api = "intersect(sphere)",
            div = "1000",
            func = () => { return Intersect(sphere_1000); }
        });

        Program.pTests.Add(new PTest
        {
            name = "intersect(10000)",
            api = "intersect(sphere)",
            div = "10000",
            func = () => { return Intersect(sphere_10000); }
        });
    }
}