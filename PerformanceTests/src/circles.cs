namespace PerformanceTests;

public static class Circles
{
    public static void Init()
    {
        Program.pTests.Add(new PTest
        {
            name = "circle(segments: 12)",
            api = "circle",
            div = "12",
            func = () => { return Circle(radius: 100, segments: 12); }
        });

        Program.pTests.Add(new PTest
        {
            name = "circle(segments: 36)",
            api = "circle",
            div = "36",
            func = () => { return Circle(radius: 100, segments: 36); }
        });

        Program.pTests.Add(new PTest
        {
            name = "circle(segments: 72)",
            api = "circle",
            div = "72",
            func = () => { return Circle(radius: 100, segments: 72); }
        });

        Program.pTests.Add(new PTest
        {
            name = "circle(segments: 144)",
            api = "circle",
            div = "144",
            func = () => { return Circle(radius: 100, segments: 144); }
        });

        Program.pTests.Add(new PTest
        {
            name = "circle(segments: 360)",
            api = "circle",
            div = "360",
            func = () => { return Circle(radius: 100, segments: 360); }
        });

    }
}