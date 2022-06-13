namespace PerformanceTests;

public static class Cylinders
{
    public static void Init()
    {
        Program.pTests.Add(new PTest
        {
            name = "cylinder(segments: 12)",
            api = "cylinder",
            div = "12",
            func = () => { return Cylinder(radius: 100, segments: 12); }
        });

        Program.pTests.Add(new PTest
        {
            name = "cylinder(segments: 36)",
            api = "cylinder",
            div = "36",
            func = () => { return Cylinder(radius: 100, segments: 36); }
        });

        Program.pTests.Add(new PTest
        {
            name = "cylinder(segments: 72)",
            api = "cylinder",
            div = "72",
            func = () => { return Cylinder(radius: 100, segments: 72); }
        });

        Program.pTests.Add(new PTest
        {
            name = "cylinder(segments: 144)",
            api = "cylinder",
            div = "144",
            func = () => { return Cylinder(radius: 100, segments: 144); }
        });

        Program.pTests.Add(new PTest
        {
            name = "cylinder(segments: 360)",
            api = "cylinder",
            div = "360",
            func = () => { return Cylinder(radius: 100, segments: 360); }
        });
    }
}