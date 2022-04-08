using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class TriangleTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestTriangleDefaults()
    {
        var geometry = Triangle(new Opts());
        Assert.DoesNotThrow(() => geometry.Validate());
        var obs = geometry.ToPoints();
        var exp = new Vec2[] {
          new Vec2(0, 0),
          new Vec2(1, 0),
          new Vec2(0.5000000000000002, 0.8660254037844387)
        };

        Assert.AreEqual(obs.Length, 3);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));
    }

    [Test]
    public void TestTriangleOptions()
    {
        // test SSS
        var geometry = Triangle(new Opts { { "type", "SSS" }, { "values", (7, 8, 6) } });
        Assert.DoesNotThrow(() => geometry.Validate());
        var obs = geometry.ToPoints();
        var exp = new Vec2[] {
          new Vec2(0, 0),
          new Vec2(7, 0),
          new Vec2(1.5, 5.809475019311125)
        };

        Assert.AreEqual(obs.Length, 3);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // test AAA
        geometry = Triangle(new Opts { { "type", "AAA" }, { "values", (Math.PI / 2, Math.PI / 4, Math.PI / 4) } });
        Assert.DoesNotThrow(() => geometry.Validate());
        obs = geometry.ToPoints();
        exp = new Vec2[] {
          new Vec2(0, 0),
          new Vec2(1, 0),
          new Vec2(0, 1.0000000000000002)
        };

        Assert.AreEqual(obs.Length, 3);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // test AAS
        geometry = Triangle(new Opts { { "type", "AAS" }, { "values", (Helpers.DegToRad(62), Helpers.DegToRad(35), 7) } });
        Assert.DoesNotThrow(() => geometry.Validate());
        obs = geometry.ToPoints();
        exp = new Vec2[] {
          new Vec2(0, 0),
          new Vec2(7.86889631692936, 0),
          new Vec2(2.1348320069064197, 4.015035054457325)
        };

        Assert.AreEqual(obs.Length, 3);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // test ASA
        geometry = Triangle(new Opts { { "type", "ASA" }, { "values", (Helpers.DegToRad(76), 9, Helpers.DegToRad(34)) } });
        Assert.DoesNotThrow(() => geometry.Validate());
        obs = geometry.ToPoints();
        exp = new Vec2[] {
          new Vec2(0, 0),
          new Vec2(9, 0),
          new Vec2(1.295667368233083, 5.196637976713814)
        };

        Assert.AreEqual(obs.Length, 3);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // test SAS
        geometry = Triangle(new Opts { { "type", "SAS" }, { "values", (5, Helpers.DegToRad(49), 7) } });
        Assert.DoesNotThrow(() => geometry.Validate());
        obs = geometry.ToPoints();
        exp = new Vec2[] {
          new Vec2(0, 0),
          new Vec2(5, 0),
          new Vec2(0.4075867970664495, 5.282967061559405)
        };

        Assert.AreEqual(obs.Length, 3);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // test SSA
        geometry = Triangle(new Opts { { "type", "SSA" }, { "values", (8, 13, Helpers.DegToRad(31)) } });
        Assert.DoesNotThrow(() => geometry.Validate());
        obs = geometry.ToPoints();
        exp = new Vec2[] {
          new Vec2(0, 0),
          new Vec2(8, 0),
          new Vec2(8.494946725906148, 12.990574573070846)
        };

        Assert.AreEqual(obs.Length, 3);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));
    }
}
