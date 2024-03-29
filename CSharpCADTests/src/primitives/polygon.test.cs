using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class PolygonTests
{
    static bool WriteTests = false;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestPolygonPoints()
    {
        var geometry = Polygon(points: new Points2 { new Vec2(0, 0), new Vec2(100, 0), new Vec2(130, 50), new Vec2(30, 50) });
        Assert.DoesNotThrow(() => geometry.Validate());

        var obs = geometry.ToPoints();
        var exp = new Vec2[] { new Vec2(0, 0), new Vec2(100, 0), new Vec2(130, 50), new Vec2(30, 50) };

        Assert.IsTrue(Helpers.CompareArrays(obs, exp));

        geometry = Polygon(points: new Points2 {new Vec2(0, 0), new Vec2(100, 0),new Vec2(0, 100),
          new Vec2(10, 10), new Vec2(80, 10), new Vec2(10, 80)});
        Assert.DoesNotThrow(() => geometry.Validate());

        obs = geometry.ToPoints();
        exp = new Vec2[] { new Vec2(0, 0), new Vec2(100, 0), new Vec2(0, 100), new Vec2(10, 10), new Vec2(80, 10), new Vec2(10, 80) };

        Assert.IsTrue(Helpers.CompareArrays(obs, exp));
    }

    [Test]
    public void TestPolygonAndPaths()
    {
        var geometry = Polygon(points: new Points2 { new Vec2(30, 50), new Vec2(130, 50), new Vec2(100, 0), new Vec2(0, 0) },
          paths: new Paths { new Path { 3, 2, 1, 0 } });
        Assert.DoesNotThrow(() => geometry.Validate());

        var obs = geometry.ToPoints();
        var exp = new Vec2[] {  new Vec2(0, 0), new Vec2(100, 0), new Vec2(130, 50), new Vec2(30, 50) };

        Assert.IsTrue(Helpers.CompareArrays(obs, exp));

        // multiple paths
        geometry = Polygon(points: new Points2 { new Vec2(0, 0), new Vec2(100, 0), new Vec2(0, 100), new Vec2(10, 80), new Vec2(80, 10), new Vec2(10, 10) },
          paths: new Paths { new Path { 0, 1, 2 }, new Path { 3, 4, 5 } });
        Assert.DoesNotThrow(() => geometry.Validate());

        obs = geometry.ToPoints();
        if(WriteTests) TestData.Make("PolygonMultiplePathsExp1", obs);
        exp = UnitTestData.PolygonMultiplePathsExp1;

        Assert.IsTrue(Helpers.CompareArrays(obs, exp));

        geometry = Polygon(points: new Points2 { new Vec2(0, 0), new Vec2(100, 0), new Vec2(0, 100), new Vec2(10, 10), new Vec2(80, 10), new Vec2(10, 80) },
          paths: new Paths { new Path { 0, 1, 5 }, new Path { 3, 4, 2 } });
        Assert.DoesNotThrow(() => geometry.Validate());

        obs = geometry.ToPoints();
        if(WriteTests) TestData.Make("PolygonMultiplePathsExp2", obs);
        exp = UnitTestData.PolygonMultiplePathsExp2;

        Assert.AreEqual(obs, exp);
    }
}
