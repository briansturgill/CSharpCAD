namespace PerformanceTests;

public static class Spheres
{
    public static void Init()
    {
        Program.pTests.Add(new PTest
        {
            name = "sphere(segments: 12)",
            api = "sphere",
            div = "12",
            func = () =>
            {
                return Sphere(radius: 100, segments: 12);
            }
        });

        Program.pTests.Add(new PTest
        {
            name = "sphere(segments: 36)",
            api = "sphere",
            div = "36",
            func = () => { return Sphere(radius: 100, segments: 36); }
        });

        Program.pTests.Add(new PTest
        {
            name = "sphere(segments: 72)",
            api = "sphere",
            div = "72",
            func = () => { return Sphere(radius: 100, segments: 72); }
        });

        Program.pTests.Add(new PTest
        {
            name = "sphere(segments: 144)",
            api = "sphere",
            div = "144",
            func = () => { return Sphere(radius: 100, segments: 144); }
        });

        Program.pTests.Add(new PTest
        {
            name = "sphere(segments: 360)",
            api = "sphere",
            div = "360",
            func = () => { return Sphere(radius: 100, segments: 360); }
        });

    }
}
