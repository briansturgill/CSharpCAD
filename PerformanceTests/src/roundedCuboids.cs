namespace PerformanceTests;

public static class RoundedCuboids
{
    public static void Init()
    {
        Program.pTests.Add(new PTest
        {
            name = "roundedCuboid(segments: 12)",
            api = "roundedCube",
            div = "12",
            func = () => { return RoundedCuboid(size: (100, 100, 100), roundRadius: 10, segments: 12); }
        });

        Program.pTests.Add(new PTest
        {
            name = "roundedCuboid(segments: 36)",
            api = "roundedCube",
            div = "36",
            func = () => { return RoundedCuboid(size: (100, 100, 100), roundRadius: 10, segments: 36); }
        });

        Program.pTests.Add(new PTest
        {
            name = "roundedCuboid(segments: 72)",
            api = "roundedCube",
            div = "72",
            func = () => { return RoundedCuboid(size: (100, 100, 100), roundRadius: 10, segments: 72); }
        });

        Program.pTests.Add(new PTest
        {
            name = "roundedCuboid(segments: 144)",
            api = "roundedCube",
            div = "144",
            func = () => { return RoundedCuboid(size: (100, 100, 100), roundRadius: 10, segments: 144); }
        });

        Program.pTests.Add(new PTest
        {
            name = "roundedCuboid(segments: 360)",
            api = "roundedCube",
            div = "360",
            func = () => { return RoundedCuboid(size: (100, 100, 100), roundRadius: 10, segments: 360); }
        });
    }
}