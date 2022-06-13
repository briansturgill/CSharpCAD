namespace PerformanceTests;

public static class Centers
{

    public static Geom2 circle_10; // 10 points
    public static Geom2 circle_100; // 100 points
    public static Geom2 circle_1000; // 1000 points

    public static Geom3 sphere_7; // 98 points
    public static Geom3 sphere_22; // 1012 points
    public static Geom3 sphere_70; // 9940 points
    public static Geom3 sphere_158; // 50244 points
    public static Geom3 sphere_224; // 99904 points

    static Centers()
    {
        circle_10 = Circle(radius: 100, segments: 10);
        circle_100 = Circle(radius: 100, segments: 100);
        circle_1000 = Circle(radius: 100, segments: 1000);

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
            name = "center(10)",
            api = "center(circle)",
            div = "10",
            func = () => { return Center(circle_10, axisX: true, axisY: true, axisZ: true); }
        });

        Program.pTests.Add(new PTest
        {
            name = "center(100)",
            api = "center(circle)",
            div = "100",
            func = () => { return Center(circle_100, axisX: true, axisY: true, axisZ: true); }
        });

        Program.pTests.Add(new PTest
        {
            name = "center(1000)",
            api = "center(circle)",
            div = "1000",
            func = () => { return Center(circle_1000, axisX: true, axisY: true, axisZ: true); }
        });

        Program.pTests.Add(new PTest
        {
            name = "center(100)",
            api = "center(sphere)",
            div = "100",
            func = () => { return Center(sphere_7, axisX: true, axisY: true, axisZ: true); }
        });

        Program.pTests.Add(new PTest
        {
            name = "center(1000)",
            api = "center(sphere)",
            div = "1000",
            func = () => { return Center(sphere_22, axisX: true, axisY: true, axisZ: true); }
        });

        Program.pTests.Add(new PTest
        {
            name = "center(10000)",
            api = "center(sphere)",
            div = "10000",
            func = () => { return Center(sphere_70, axisX: true, axisY: true, axisZ: true); }
        });
    }
}