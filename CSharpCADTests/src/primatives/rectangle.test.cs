using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class RectangleTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestRectangleDefaults()
    {
        var geometry = Rectangle(new Opts());
        Assert.DoesNotThrow(() => geometry.Validate());
        var obs = geometry.ToPoints();
        var exp = new Vec2[] {
          new Vec2(-1, -1),
          new Vec2(1, -1),
          new Vec2(1, 1),
          new Vec2(-1, 1)
        };

        Assert.AreEqual(obs.Length, 4);
        Assert.IsTrue(Helpers.CompareArrays(obs, exp));
    }

    [Test]
    public void TestRectangleOptions()
    {
        // test center
        var geometry = Rectangle(new Opts { { "center", (-4, -4) } });
        Assert.DoesNotThrow(() => geometry.Validate());
        var obs = geometry.ToPoints();
        var exp = new Vec2[] {
          new Vec2(-5, -5),
          new Vec2(-3, -5),
          new Vec2(-3, -3),
          new Vec2(-5, -3)
        };

        Assert.AreEqual(obs.Length, 4);
        Assert.IsTrue(Helpers.CompareArrays(obs, exp));

        // test size
        geometry = Rectangle(new Opts { { "size", (6, 10) } });
        Assert.DoesNotThrow(() => geometry.Validate());
        obs = geometry.ToPoints();
        exp = new Vec2[] {
          new Vec2(-3, -5),
          new Vec2(3, -5),
          new Vec2(3, 5),
          new Vec2(-3, 5)
        };

        Assert.AreEqual(obs.Length, 4);
        Assert.IsTrue(Helpers.CompareArrays(obs, exp));
    }
}
