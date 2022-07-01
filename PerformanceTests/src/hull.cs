namespace PerformanceTests;

public static class Hulls
{
    public static Vec2[] centers2D = new Vec2[]
    {
        (0, 0),
        (10, 10),
        (20, 20),
        (30, 30),
        (40, 40),
        (50, 50),
        (60, 60),
        (70, 70),
        (80, 80),
        (90, 90),
    };
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

    public static Geom2[] circle_100 = new Geom2[centers2D.Length]; // 10 points X centers
    public static Geom2[] circle_1000 = new Geom2[centers2D.Length]; // 100 points X centers
    public static Geom2[] circle_10000 = new Geom2[centers2D.Length]; // 1000 points X centers

    public static Geom3[] sphere_1000 = new Geom3[centers.Length]; // 98 points X centers
    public static Geom3[] sphere_10000 = new Geom3[centers.Length]; // 1012 points X centers
    public static Geom3[] sphere_100000 = new Geom3[centers.Length]; // 9940 points X centers

    // number of points ["100", "1000", "10000", "100000", "200000", "400000"]

    public static void Init()
    {

        circle_100 = centers2D.Select((center) => (Translate(center, Circle(radius: 100, segments: 10)))).ToArray();
        circle_1000 = centers2D.Select((center) => (Translate(center, Circle(radius: 100, segments: 100)))).ToArray();
        circle_10000 = centers2D.Select((center) => (Translate(center, Circle(radius: 100, segments: 1000)))).ToArray();

        sphere_1000 = centers.Select((center) => (Translate(center, Sphere(radius: 100, segments: 7)))).ToArray();
        sphere_10000 = centers.Select((center) => (Translate(center, Sphere(radius: 100, segments: 22)))).ToArray();
        sphere_100000 = centers.Select((center) => (Translate(center, Sphere(radius: 100, segments: 70)))).ToArray();


        Program.pTests.Add(new PTest
        {
            name = "hull(100)",
            api = "hull(circle)",
            div = "100",
            func = () => { return Hull(circle_100); }
        });

        Program.pTests.Add(new PTest
        {
            name = "hull(1000)",
            api = "hull(circle)",
            div = "1000",
            func = () => { return Hull(circle_1000); }
        });

        Program.pTests.Add(new PTest
        {
            name = "hull(10000)",
            api = "hull(circle)",
            div = "10000",
            func = () => { return Hull(circle_10000); }
        });

        Program.pTests.Add(new PTest
        {
            name = "hull(1000)",
            api = "hull(sphere)",
            div = "1000",
            func = () => { return Hull(sphere_1000); }
        });

        Program.pTests.Add(new PTest
        {
            name = "hull(10000)",
            api = "hull(sphere)",
            div = "10000",
            func = () => { return Hull(sphere_10000); }
        });

        Program.pTests.Add(new PTest
        {
            name = "hull(100000)",
            api = "hull(sphere)",
            div = "100000",
            func = () => { return Hull(sphere_100000); }
        });
    }
}