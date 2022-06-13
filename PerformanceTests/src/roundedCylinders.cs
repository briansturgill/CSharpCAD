namespace PerformanceTests;

public static class RoundedCylinders
{
    public static void Init()
    {
        Program.pTests.Add(new PTest
        {
            name = "roundedCylinder(segments: 12)",
            api = "roundedCylinder",
            div = "12",
            func = () => { return RoundedCylinder(radius: 100, segments: 12); }
        });

        Program.pTests.Add(new PTest
        {
            name = "roundedCylinder(segments: 36)",
            api = "roundedCylinder",
            div = "36",
            func = () => { return RoundedCylinder(radius: 100, segments: 36); }
        });

        Program.pTests.Add(new PTest
        {
            name = "roundedCylinder(segments: 72)",
            api = "roundedCylinder",
            div = "72",
            func = () => { return RoundedCylinder(radius: 100, segments: 72); }
        });

        Program.pTests.Add(new PTest
        {
            name = "roundedCylinder(segments: 144)",
            api = "roundedCylinder",
            div = "144",
            func = () => { return RoundedCylinder(radius: 100, segments: 144); }
        });

        Program.pTests.Add(new PTest
        {
            name = "roundedCylinder(segments: 360)",
            api = "roundedCylinder",
            div = "360",
            func = () => { return RoundedCylinder(radius: 100, segments: 360); }
        });
    }
}