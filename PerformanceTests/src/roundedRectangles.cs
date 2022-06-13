namespace PerformanceTests;

public static class RoundedRectangles
{
    public static void Init()
    {
        Program.pTests.Add(new PTest
        {
            name = "roundedRectangle",
            api = "roundedRectangle",
            div = "12",
            func = () => { return RoundedRectangle(size: (100, 100), roundRadius: 10, segments: 12); }
        });

        Program.pTests.Add(new PTest
        {
            name = "roundedRectangle",
            api = "roundedRectangle",
            div = "36",
            func = () => { return RoundedRectangle(size: (100, 100), roundRadius: 10, segments: 36); }
        });

        Program.pTests.Add(new PTest
        {
            name = "roundedRectangle",
            api = "roundedRectangle",
            div = "72",
            func = () => { return RoundedRectangle(size: (100, 100), roundRadius: 10, segments: 72); }
        });

        Program.pTests.Add(new PTest
        {
            name = "roundedRectangle",
            api = "roundedRectangle",
            div = "144",
            func = () => { return RoundedRectangle(size: (100, 100), roundRadius: 10, segments: 144); }
        });

        Program.pTests.Add(new PTest
        {
            name = "roundedRectangle",
            api = "roundedRectangle",
            div = "360",
            func = () => { return RoundedRectangle(size: (100, 100), roundRadius: 10, segments: 360); }
        });
    }
}