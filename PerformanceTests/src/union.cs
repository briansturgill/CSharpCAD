namespace PerformanceTests;

public static class Unions
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

    public static void Init()
    {

        circle_100 = centers.Select((center) => ((Translate(center, Circle(radius: 100, segments: 10))))).ToArray();
        circle_500 = centers.Select((center) => (Translate(center, Circle(radius: 100, segments: 50)))).ToArray();
        circle_1000 = centers.Select((center) => (Translate(center, Circle(radius: 100, segments: 100)))).ToArray();
        circle_5000 = centers.Select((center) => (Translate(center, Circle(radius: 100, segments: 500)))).ToArray();
        circle_10000 = centers.Select((center) => (Translate(center, Circle(radius: 100, segments: 1000)))).ToArray();

        sphere_600 = centers.Select((center) => (Translate(center, Sphere(radius: 100, segments: 6)))).ToArray();
        sphere_1000 = centers.Select((center) => (Translate(center, Sphere(radius: 100, segments: 7)))).ToArray();
        sphere_10000 = centers.Select((center) => (Translate(center, Sphere(radius: 100, segments: 22)))).ToArray();
        sphere_100000 = centers.Select((center) => (Translate(center, Sphere(radius: 100, segments: 70)))).ToArray();


        Program.pTests.Add(new PTest
        {

            name = "union(100)",
            api = "union(circle)",
            div = "100",
            func = () => { return Union(circle_100); }
        });


        Program.pTests.Add(new PTest
        {
            name = "union(500)",
            api = "union(circle)",
            div = "500",
            func = () => { return Union(circle_500); }
        });


        Program.pTests.Add(new PTest
        {
            name = "union(1000)",
            api = "union(circle)",
            div = "1000",
            func = () => { return Union(circle_1000); }
        });


        Program.pTests.Add(new PTest
        {
            name = "union(5000)",
            api = "union(circle)",
            div = "5000",
            func = () => { return Union(circle_5000); }
        });


        Program.pTests.Add(new PTest
        {
            name = "union(10000)",
            api = "union(circle)",
            div = "10000",
            func = () => { return Union(circle_10000); }
        });


        Program.pTests.Add(new PTest
        {
            name = "union(600)",
            api = "union(sphere)",
            div = "600",
            func = () => { return Union(sphere_600); }
        });


        Program.pTests.Add(new PTest
        {
            name = "union(1000)",
            api = "union(sphere)",
            div = "1000",
            func = () => { return Union(sphere_1000); }
        });


        Program.pTests.Add(new PTest
        {
            name = "union(10000)",
            api = "union(sphere)",
            div = "10000",
            func = () => { return Union(sphere_10000); }
        });

    }

}
