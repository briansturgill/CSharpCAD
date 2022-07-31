using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class EllipticCylinderTests
{
    static bool WriteTests = false;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestEllipticCylinderDefaults()
    {
        var obs = EllipticCylinder();
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();

        Assert.AreEqual(pts.Count, 128);

        obs = SemiellipticCylinder();
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();

        Assert.AreEqual(pts.Count, 40);
    }

    [Test]
    public void TestEllipticCylinderOptions()
    {
        // test height
        var obs = EllipticCylinder(height: 10, segments: 12);
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();
        if (WriteTests) TestData.Make("EllipticCylinderOptsExp1", pts);
        var exp = UnitTestData.EllipticCylinderOptsExp1;

        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        // test startRadius and endRadius
        obs = Cone(top: 2, bottom: 20, segments: 12);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        if (WriteTests) TestData.Make("ConeOptsExp1", pts);
        exp = UnitTestData.ConeOptsExp1;

        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        // test startAngle and endAngle
        obs = SemiellipticCylinder(radius: (1, 2), startAngle: 90, endAngle: 360 * 0.75, segments: 12);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        exp = new List<List<Vec3>> { };

        Assert.AreEqual(pts.Count, 32);
        // Assert.IsTrue(comparePolygonsAsPoints(pts, exp))

        // test segments
        obs = EllipticCylinder(segments: 8);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();

        Assert.AreEqual(pts.Count, 32);

        // test center
        obs = EllipticCylinder(center: (-5, -5, -5), height: 3, segments: 8);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        if (WriteTests) TestData.Make("EllpticCylinderOptsExp2", pts);
        exp = UnitTestData.EllpticCylinderOptsExp2;

        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }
}
