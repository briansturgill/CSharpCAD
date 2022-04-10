using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class SquareTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestSquareDefaults()
    {
        var geometry = Square(center: (0, 0)); // CSCAD changed the default center.
        Assert.DoesNotThrow(() => geometry.Validate());
        var obs = geometry.ToPoints();
        Assert.AreEqual(obs.Length, 4);
    }


    [Test]
    public void TestSquareOptions()
    {
        // test center
        var obs = Square(size: 7, center: (6.5, 6.5));
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();
        var exp = new Vec2[] {
          new Vec2(3, 3),
          new Vec2(10, 3),
          new Vec2(10, 10),
          new Vec2(3, 10)
        };

        Assert.AreEqual(pts.Length, 4);
        Assert.IsTrue(Helpers.CompareArrays(pts, exp));

        // test size
        obs = Square(size: 7, center: (0, 0));
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        exp = new Vec2[] {
          new Vec2(-3.5, -3.5),
          new Vec2(3.5, -3.5),
          new Vec2(3.5, 3.5),
          new Vec2(-3.5, 3.5)
        };

        Assert.AreEqual(pts.Length, 4);
        Assert.IsTrue(Helpers.CompareArrays(pts, exp));
    }

}
