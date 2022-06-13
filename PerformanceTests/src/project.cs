namespace PerformanceTests;

public static class Projects
{
    public static Geom3 sphere_7; // 98 points
    public static Geom3 sphere_22; // 1012 points
    public static Geom3 sphere_70; // 9940 points
    public static Geom3 sphere_158; // 50244 points
    public static Geom3 sphere_224; // 99904 points

    static Projects()
    {
        sphere_7 = Sphere(radius: 100, segments: 7);
        sphere_22 = Sphere(radius: 100, segments: 22);
        sphere_70 = Sphere(radius: 100, segments: 70);
        sphere_158 = Sphere(radius: 100, segments: 158);
        sphere_224 = Sphere(radius: 100, segments: 224);
    }

    public static void Init()
    {
        Program.pTests.Add(new PTest
        {
            name = "project(100)",
            api = "project(sphere)",
            div = "100",
            func = () => { return Project(sphere_7); }
        });

        Program.pTests.Add(new PTest
        {
            name = "project(1000)",
            api = "project(sphere)",
            div = "1000",
            func = () => { return Project(sphere_22); }
        });

        Program.pTests.Add(new PTest
        {
            name = "project(10000)",
            api = "project(sphere)",
            div = "10000",
            func = () => { return Project(sphere_70); }
        });

/* Commented out in JSCAD version.
        Program.pTests.Add(new PTest
        {
            name = "project(100000)",
            api = "project(sphere)",
            div = "100000",
            func = () => { return Project(sphere_224); }
        });
*/
    }
}