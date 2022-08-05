namespace PerformanceTests;

public static class ExtrudeTwists
{

    public static Geom2 circle_10; // 10 points
    public static Geom2 circle_50; // 50 points
    public static Geom2 circle_100; // 100 points

    static ExtrudeTwists()
    {

        circle_10 = Circle(radius: 100, segments: 10);
        circle_50 = Circle(radius: 100, segments: 50);
        circle_100 = Circle(radius: 100, segments: 100);
    }
    public static void Init()
    {
        Program.pTests.Add(new PTest
        {
            name = "extrudeTwist",
            api = "extrudeTwist",
            div = "100",
            func = () => { return ExtrudeTwist(circle_10, height: 100, twistAngle: RadToDeg(5), twistSteps: 10); }
        });

        Program.pTests.Add(new PTest
        {
            name = "extrudeTwist",
            api = "extrudeTwist",
            div = "500",
            func = () => { return ExtrudeTwist(circle_50, height: 100, twistAngle: RadToDeg(5), twistSteps: 10); }
        });

        Program.pTests.Add(new PTest
        {
            name = "extrudeTwist",
            api = "extrudeTwist",
            div = "1000",
            func = () => { return ExtrudeTwist(circle_100, height: 100, twistAngle: RadToDeg(5), twistSteps: 10); }
        });

        Program.pTests.Add(new PTest
        {
            name = "extrudeTwist",
            api = "extrudeTwist",
            div = "5000",
            func = () => { return ExtrudeTwist(circle_100, height: 100, twistAngle: RadToDeg(5), twistSteps: 50); }
        });

        Program.pTests.Add(new PTest
        {
            name = "extrudeTwist",
            api = "extrudeTwist",
            div = "10000",
            func = () => { return ExtrudeTwist(circle_100, height: 100, twistAngle: RadToDeg(5), twistSteps: 100); }
        });
    }
}
