#define PERFS

namespace PerformanceTests;

public struct PTest
{
    public delegate Geometry func_t();
    public string name;
    public string api;
    public string div;
    public func_t func;
};

public static class Program
{
    public static List<PTest> pTests = new List<PTest>();
    public static void Main()
    {
#if PERFS
        //Centers.Init();
        //Circles.Init();
        //Cylinders.Init();
        //Expands.Init();
        ExtrudeLinears.Init();
        ExtrudeTwists.Init();
        //ExtrudeRotates.Init();
        //Hulls.Init();
        Intersects.Init();
        //MeasurePolyBoundingSphere.Init();
        //Projects.Init();
        //RoundedCuboids.Init();
        //RoundedCylinders.Init();
        //RoundedRectangles.Init();
        //Spheres.Init();
        Subtracts.Init();
        //Transforms.Init();
        Unions.Init();
#endif
        var maxsamples = 1000;
        var watch = new Stopwatch();

        foreach (var pt in pTests)
        {
            watch.Start();
            for (var i = 0; i < maxsamples; i++)
            {
                pt.func();
            }
            watch.Stop();


            var meanms = watch.Elapsed.TotalMilliseconds;

            Console.WriteLine($"{pt.api},{pt.div},{maxsamples}: {meanms:F9}ms");
            watch.Reset();
        }
#if !PERFS
        UnionPerf.Run();
#endif
    }
}
