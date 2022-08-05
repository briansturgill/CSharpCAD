namespace PerformanceTests;

public static class ExtrudeLinears
{

    public static Geom2 circle_10; // 10 points
    public static Geom2 circle_50; // 50 points
    public static Geom2 circle_100; // 100 points

    static ExtrudeLinears()
    {

        circle_10 = Circle(radius: 100, segments: 10);
        circle_50 = Circle(radius: 100, segments: 50);
        circle_100 = Circle(radius: 100, segments: 100);
    }
    public static void Init()
    {
        Program.pTests.Add(new PTest
        {
            name = "extrudeLinear",
            api = "extrudeLinear",
            div = "100",
            func = () => { return ExtrudeLinear(circle_10, height: 100); }
        });

        Program.pTests.Add(new PTest
        {
            name = "extrudeLinear",
            api = "extrudeLinear",
            div = "500",
            func = () => { return ExtrudeLinear(circle_50, height: 100); }
        });

        Program.pTests.Add(new PTest
        {
            name = "extrudeLinear",
            api = "extrudeLinear",
            div = "1000",
            func = () => { return ExtrudeLinear(circle_100, height: 100); }
        });

        Program.pTests.Add(new PTest
        {
            name = "extrudeLinear",
            api = "extrudeLinear",
            div = "5000",
            func = () => { return ExtrudeLinear(circle_100, height: 100); }
        });

        Program.pTests.Add(new PTest
        {
            name = "extrudeLinear",
            api = "extrudeLinear",
            div = "10000",
            func = () => { return ExtrudeLinear(circle_100, height: 100); }
        });
        Program.pTests.Add(new PTest
        {
            name = "extrudeLinearOld",
            api = "extrudeLinearOld",
            div = "100",
            func = () => { return ExtrudeTwist(circle_10, height: 100); }
        });

        Program.pTests.Add(new PTest
        {
            name = "extrudeLinearOld",
            api = "extrudeLinearOld",
            div = "500",
            func = () => { return ExtrudeTwist(circle_50, height: 100); }
        });

        Program.pTests.Add(new PTest
        {
            name = "extrudeLinearOld",
            api = "extrudeLinearOld",
            div = "1000",
            func = () => { return ExtrudeTwist(circle_100, height: 100); }
        });

        Program.pTests.Add(new PTest
        {
            name = "extrudeLinearOld",
            api = "extrudeLinearOld",
            div = "5000",
            func = () => { return ExtrudeTwist(circle_100, height: 100); }
        });

        Program.pTests.Add(new PTest
        {
            name = "extrudeLinearOld",
            api = "extrudeLinearOld",
            div = "10000",
            func = () => { return ExtrudeTwist(circle_100, height: 100); }
        });
    }
}
