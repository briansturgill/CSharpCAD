using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class RoundedRectangleTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestRoundedRectangleDefaults()
    {
        var geometry = RoundedRectangle(center: (0, 0)); // CSCAD changed the default center.
        Assert.DoesNotThrow(() => geometry.Validate());
        var obs = geometry.ToPoints();

        Assert.AreEqual(obs.Length, 36);
    }

    [Test]
    public void TestRoundedRectangleOptions()
    {
        // test center
        var geometry = RoundedRectangle(center: (4, 5), segments: 16);
        Assert.DoesNotThrow(() => geometry.Validate());
        var obs = geometry.ToPoints();
        var exp = new Vec2[] {
          new Vec2(5, 5.8),
          new Vec2(4.984775906502257, 5.876536686473018),
          new Vec2(4.941421356237309, 5.941421356237309),
          new Vec2(4.876536686473018, 5.984775906502257),
          new Vec2(4.8, 6),
          new Vec2(3.2, 6),
          new Vec2(3.1234633135269823, 5.984775906502257),
          new Vec2(3.0585786437626905, 5.941421356237309),
          new Vec2(3.015224093497743, 5.876536686473018),
          new Vec2(3, 5.8),
          new Vec2(3, 4.2),
          new Vec2(3.015224093497743, 4.123463313526982),
          new Vec2(3.0585786437626905, 4.058578643762691),
          new Vec2(3.1234633135269823, 4.015224093497743),
          new Vec2(3.2, 4),
          new Vec2(4.8, 4),
          new Vec2(4.876536686473018, 4.015224093497743),
          new Vec2(4.941421356237309, 4.058578643762691),
          new Vec2(4.984775906502257, 4.123463313526982),
          new Vec2(5, 4.2)
        };
        Assert.AreEqual(obs.Length, 20);
        Assert.IsTrue(Helpers.CompareArrays(obs, exp));

        // test size
        geometry = RoundedRectangle(size: (10, 6), segments: 16, center: (0, 0));
        Assert.DoesNotThrow(() => geometry.Validate());
        obs = geometry.ToPoints();
        exp = new Vec2[] {
          new Vec2(5, 2.8),
          new Vec2(4.984775906502257, 2.8765366864730177),
          new Vec2(4.941421356237309, 2.9414213562373095),
          new Vec2(4.876536686473018, 2.984775906502257),
          new Vec2(4.8, 3),
          new Vec2(-4.8, 3),
          new Vec2(-4.876536686473018, 2.984775906502257),
          new Vec2(-4.941421356237309, 2.9414213562373095),
          new Vec2(-4.984775906502257, 2.8765366864730177),
          new Vec2(-5, 2.8),
          new Vec2(-5, -2.8),
          new Vec2(-4.984775906502257, -2.8765366864730177),
          new Vec2(-4.941421356237309, -2.9414213562373095),
          new Vec2(-4.876536686473018, -2.984775906502257),
          new Vec2(-4.8, -3),
          new Vec2(4.8, -3),
          new Vec2(4.876536686473018, -2.984775906502257),
          new Vec2(4.941421356237309, -2.9414213562373095),
          new Vec2(4.984775906502257, -2.8765366864730177),
          new Vec2(5, -2.8)
        };
        Assert.AreEqual(obs.Length, 20);
        Assert.IsTrue(Helpers.CompareArrays(obs, exp));

        // test roundRadius
        geometry = RoundedRectangle(size: (10, 6), roundRadius: 2, segments: 16, center: (0, 0));
        Assert.DoesNotThrow(() => geometry.Validate());
        obs = geometry.ToPoints();
        exp = new Vec2[] {
          new Vec2(5, 1),
          new Vec2(4.847759065022574, 1.7653668647301797),
          new Vec2(4.414213562373095, 2.414213562373095),
          new Vec2(3.7653668647301797, 2.8477590650225735),
          new Vec2(3, 3),
          new Vec2(-3, 3),
          new Vec2(-3.7653668647301792, 2.8477590650225735),
          new Vec2(-4.414213562373095, 2.414213562373095),
          new Vec2(-4.847759065022574, 1.7653668647301797),
          new Vec2(-5, 1.0000000000000002),
          new Vec2(-5, -0.9999999999999998),
          new Vec2(-4.847759065022574, -1.7653668647301792),
          new Vec2(-4.414213562373095, -2.414213562373095),
          new Vec2(-3.76536686473018, -2.8477590650225735),
          new Vec2(-3.0000000000000004, -3),
          new Vec2(2.9999999999999996, -3),
          new Vec2(3.7653668647301792, -2.8477590650225735),
          new Vec2(4.414213562373095, -2.414213562373095),
          new Vec2(4.847759065022574, -1.7653668647301801),
          new Vec2(5, -1.0000000000000004)
        };
        Assert.AreEqual(obs.Length, 20);
        Assert.IsTrue(Helpers.CompareArrays(obs, exp));

        // test segments
        geometry = RoundedRectangle(size: (10, 6), roundRadius: 2, segments: 64, center: (0, 0));
        Assert.DoesNotThrow(() => geometry.Validate());
        obs = geometry.ToPoints();
        Assert.AreEqual(obs.Length, 68);
    }

}
