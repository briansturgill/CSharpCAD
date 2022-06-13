namespace PerformanceTests;

public static class UnionPerf
{

    const int segments = 100;

    public static void Run()
    {
        test(4, 1, "warmup1");
        test(2, 1, "warmup2");
        test(4, 1, "disjoint");
        test(2, 1, "touching");
        test(1, 1, "overlap");
        test(0, 1, "same");
        test(0, 0.5, "inside");
    }

    static Geometry test(double x, double radius, string name)
    {
        var s1 = Sphere(segments: segments);
        var s2 = Sphere(segments: segments, center: (x, 0, 0), radius: radius);
        var watch = new System.Diagnostics.Stopwatch();

        watch.Restart();
        var ret = Union(s1, s2);
        watch.Stop();
        Console.WriteLine($"Union {name} {watch.ElapsedMilliseconds}ms");
        watch.Restart();
        ret = Intersect(s1, s2);
        watch.Stop();
        Console.WriteLine($"Intersect {name} {watch.ElapsedMilliseconds}ms");
        watch.Restart();
        ret = Subtract(s1, s2);
        watch.Stop();
        Console.WriteLine($"Subtract {name} {watch.ElapsedMilliseconds}ms");
        return ret;
    }
}