namespace PerformanceTests;

public static class ExtrudeRectangulars
{

    public static Geom2 circle_10; // 10 points
    public static Geom2 circle_100; // 100 points
    public static Geom2 circle_1000; // 1000 points

    static ExtrudeRectangulars()
    {
        circle_10 = Circle(radius: 100, segments: 10);
        circle_100 = Circle(radius: 100, segments: 100);
        circle_1000 = Circle(radius: 100, segments: 1000);
    }
    public static void Init()
    {
        Program.pTests.Add(new PTest
        {
            name = "extrudeRectangular(circle)",
            api = "extrudeRectangular(circle)",
            div = "10",
            func = () => { return ExtrudeRectangular(circle_10, size: 3, height: 10, corners: Corners.Round, segments: 16); }
        });

        Program.pTests.Add(new PTest
        {
            name = "extrudeRectangular(circle)",
            api = "extrudeRectangular(circle)",
            div = "100",
            func = () => { return ExtrudeRectangular(circle_100, size: 3, height: 10, corners: Corners.Round, segments: 16); }
        });

        Program.pTests.Add(new PTest
        {
            name = "extrudeRectangular(circle)",
            api = "extrudeRectangular(circle)",
            div = "1000",
            func = () => { return ExtrudeRectangular(circle_1000, size: 3, height: 10, corners: Corners.Round, segments: 16); }
        });

    }

}