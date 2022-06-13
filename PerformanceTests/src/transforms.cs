namespace PerformanceTests;

public static class Transforms
{

    public static Geom2 circle_10; // 10 points
    public static Geom2 circle_100; // 100 points
    public static Geom2 circle_1000; // 1000 points

    public static Geom3 sphere_7; // 98 points
    public static Geom3 sphere_22; // 1012 points
    public static Geom3 sphere_70; // 9940 points
    public static Geom3 sphere_158; // 50244 points
    public static Geom3 sphere_224; // 99904 points

    static Transforms()
    {
        circle_10 = Circle(radius: 100, segments: 10);
        circle_100 = Circle(radius: 100, segments: 100);
        circle_1000 = Circle(radius: 100, segments: 1000);

        sphere_7 = Sphere(radius: 100, segments: 7);
        sphere_22 = Sphere(radius: 100, segments: 22);
        sphere_70 = Sphere(radius: 100, segments: 70);
        sphere_158 = Sphere(radius: 100, segments: 158);
        sphere_224 = Sphere(radius: 100, segments: 224);
    }

    public static void Init()
    {
        var newfactors = (5, 5, 5);
        var newangles = (Math.PI / 2, Math.PI / 2, Math.PI / 2);
        var newoffsets = (-10, -10, -10);

        Program.pTests.Add(new PTest
        {
            name = "transform(10)",
            api = "transform(circle)",
            div = "10",
            func = () => { return ((Geom2)Translate(newoffsets, Rotate(newangles, Scale(newfactors, circle_10)))).ApplyTransforms(); }
        });

        Program.pTests.Add(new PTest
        {
            name = "transform(100)",
            api = "transform(circle)",
            div = "100",
            func = () => { return ((Geom2)Translate(newoffsets, Rotate(newangles, Scale(newfactors, circle_100)))).ApplyTransforms(); }
        });

        Program.pTests.Add(new PTest
        {
            name = "transform(1000)",
            api = "transform(circle)",
            div = "1000",
            func = () => { return ((Geom2)Translate(newoffsets, Rotate(newangles, Scale(newfactors, circle_1000)))).ApplyTransforms(); }
        });

        Program.pTests.Add(new PTest
        {
            name = "transform(100)",
            api = "transform(sphere)",
            div = "100",
            func = () => { return ((Geom3)Translate(newoffsets, Rotate(newangles, Scale(newfactors, sphere_7)))).ApplyTransforms(); }
        });

        Program.pTests.Add(new PTest
        {
            name = "transform(1000)",
            api = "transform(sphere)",
            div = "1000",
            func = () => { return ((Geom3)Translate(newoffsets, Rotate(newangles, Scale(newfactors, sphere_22)))).ApplyTransforms(); }
        });

        Program.pTests.Add(new PTest
        {
            name = "transform(10000)",
            api = "transform(sphere)",
            div = "10000",
            func = () => { return ((Geom3)Translate(newoffsets, Rotate(newangles, Scale(newfactors, sphere_70)))).ApplyTransforms(); }
        });
    }
}