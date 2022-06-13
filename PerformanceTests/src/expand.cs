namespace PerformanceTests;

public static class Expands
{

    public static Geom2 circle_10; // 10 points
    public static Geom2 circle_100; // 100 points
    public static Geom2 circle_1000; // 1000 points

    public static Geom3 sphere_7; // 98 points
    public static Geom3 sphere_22; // 1012 points
    public static Geom3 sphere_70; // 9940 points
    public static Geom3 sphere_158; // 50244 points
    public static Geom3 sphere_224; // 99904 points

    static Expands()
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
            name = "expand(10)",
            api = "expand(circle)",
            div = "10",
            func = () =>
            {
                return Expand(circle_10, delta: 5, corners: Corners.Round, segments: 16);
            }
        });

        Program.pTests.Add(new PTest
        {
            name = "expand(100)",
            api = "expand(circle)",
            div = "100",
            func = () => { return Expand(circle_100, delta: 5, corners: Corners.Round, segments: 16); }
        });

        Program.pTests.Add(new PTest
        {
            name = "expand(1000)",
            api = "expand(circle)",
            div = "1000",
            func = () => { return Expand(circle_1000, delta: 5, corners: Corners.Round, segments: 16); }
        });

/* Currently not supported in ExtrudeRectangular
        Program.pTests.Add(new PTest
        {
            name = "expand(100)",
            api = "expand(sphere)",
            div = "100",
            func = () => { return Expand(sphere_7, delta: 5, corners: Corners.Round, segments: 12); }
        });

        Program.pTests.Add(new PTest
        {
            name = "expand(1000)",
            api = "expand(sphere)",
            div = "1000",
            func = () => { return Expand(sphere_22, delta: 5, corners: Corners.Round, segments: 12); }
        });
*/

    }
}