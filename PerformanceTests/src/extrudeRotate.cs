namespace PerformanceTests;

public static class ExtrudeRotates
{

    public static Geom2 circle_10; // 10 points
    public static Geom2 circle_50; // 50 points
    public static Geom2 circle_100; // 100 points

    static ExtrudeRotates()
    {
        circle_10 = Translate((0, 500, 0), Circle(radius: 100, segments: 10));
        circle_50 = Translate((0, 500, 0), Circle(radius: 100, segments: 50));
        circle_100 = Translate((0, 500, 0), Circle(radius: 100, segments: 100));
    }
    public static void Init()
    {
        Program.pTests.Add(new PTest
        {
            name = "extrudeRotate",
            api = "extrudeRotate",
            div = "100",
            func = () => { return ExtrudeRotate(circle_10, segments: 10); }
        });

        Program.pTests.Add(new PTest
        {
            name = "extrudeRotate",
            api = "extrudeRotate",
            div = "500",
            func = () => { return ExtrudeRotate(circle_10, segments: 50); }
        });

        Program.pTests.Add(new PTest
        {
            name = "extrudeRotate",
            api = "extrudeRotate",
            div = "1000",
            func = () => { return ExtrudeRotate(circle_50, segments: 20); }
        });

        Program.pTests.Add(new PTest
        {
            name = "extrudeRotate",
            api = "extrudeRotate",
            div = "5000",
            func = () => { return ExtrudeRotate(circle_50, segments: 100); }
        });

        Program.pTests.Add(new PTest
        {
            name = "extrudeRotate",
            api = "extrudeRotate",
            div = "10000",
            func = () => { return ExtrudeRotate(circle_50, segments: 200); }
        });
    }
}