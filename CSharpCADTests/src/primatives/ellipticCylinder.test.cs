using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class EllipticCylinderTests
{
    static bool WriteTests = true;

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

        Assert.AreEqual(pts.Count, 34);

        obs = SemiellipticCylinder();
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();

        Assert.AreEqual(pts.Count, 12);
    }

    [Test]
    public void TestEllipticCylinderOptions()
    {
        // test height
        var obs = EllipticCylinder(height: 10, segments: 12);
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();
        if(WriteTests) TestData.Make("EllipticCylinderOptsExp1", pts);
        var exp = UnitTestData.EllipticCylinderOptsExp1;

        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

#if LATER
        // test startRadius and endRadius
        obs = SemiellipticCylinder(startRadius: (1, 2), endRadius: (2, 1), segments: 12);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        exp = new List<List<Vec3>> {
          new List<Vec3> {new Vec3(0, 0, -1), new Vec3(0.8660254037844387, 0.9999999999999999, -1), new Vec3(1, 0, -1)},
          new List<Vec3> {new Vec3(1, 0, -1), new Vec3(0.8660254037844387, 0.9999999999999999, -1), new Vec3(2, 0, 1)},
          new List<Vec3> {new Vec3(0, 0, 1), new Vec3(2, 0, 1), new Vec3(1.7320508075688774, 0.49999999999999994, 1)},
          new List<Vec3> {new Vec3(2, 0, 1), new Vec3(0.8660254037844387, 0.9999999999999999, -1), new Vec3(1.7320508075688774, 0.49999999999999994, 1)},
          new List<Vec3> {new Vec3(0, 0, -1), new Vec3(0.5000000000000001, 1.7320508075688772, -1), new Vec3(0.8660254037844387, 0.9999999999999999, -1)},
          new List<Vec3> {new Vec3(0.8660254037844387, 0.9999999999999999, -1), new Vec3(0.5000000000000001, 1.7320508075688772, -1), new Vec3(1.7320508075688774, 0.49999999999999994, 1)},
          new List<Vec3> {new Vec3(0, 0, 1), new Vec3(1.7320508075688774, 0.49999999999999994, 1), new Vec3(1.0000000000000002, 0.8660254037844386, 1)},
          new List<Vec3> {new Vec3(1.7320508075688774, 0.49999999999999994, 1), new Vec3(0.5000000000000001, 1.7320508075688772, -1), new Vec3(1.0000000000000002, 0.8660254037844386, 1)},
          new List<Vec3> {new Vec3(0, 0, -1), new Vec3(6.123233995736766e-17, 2, -1), new Vec3(0.5000000000000001, 1.7320508075688772, -1)},
          new List<Vec3> {new Vec3(0.5000000000000001, 1.7320508075688772, -1), new Vec3(6.123233995736766e-17, 2, -1), new Vec3(1.0000000000000002, 0.8660254037844386, 1)},
          new List<Vec3> {new Vec3(0, 0, 1), new Vec3(1.0000000000000002, 0.8660254037844386, 1), new Vec3(1.2246467991473532e-16, 1, 1)},
          new List<Vec3> {new Vec3(1.0000000000000002, 0.8660254037844386, 1), new Vec3(6.123233995736766e-17, 2, -1), new Vec3(1.2246467991473532e-16, 1, 1)},
          new List<Vec3> {new Vec3(0, 0, -1), new Vec3(-0.4999999999999998, 1.7320508075688774, -1), new Vec3(6.123233995736766e-17, 2, -1)},
          new List<Vec3> {new Vec3(6.123233995736766e-17, 2, -1), new Vec3(-0.4999999999999998, 1.7320508075688774, -1), new Vec3(1.2246467991473532e-16, 1, 1)},
          new List<Vec3> {new Vec3(0, 0, 1), new Vec3(1.2246467991473532e-16, 1, 1), new Vec3(-0.9999999999999996, 0.8660254037844387, 1)},
          new List<Vec3> {new Vec3(1.2246467991473532e-16, 1, 1), new Vec3(-0.4999999999999998, 1.7320508075688774, -1), new Vec3(-0.9999999999999996, 0.8660254037844387, 1)},
          new List<Vec3> {new Vec3(0, 0, -1), new Vec3(-0.8660254037844387, 0.9999999999999999, -1), new Vec3(-0.4999999999999998, 1.7320508075688774, -1)},
          new List<Vec3> {new Vec3(-0.4999999999999998, 1.7320508075688774, -1), new Vec3(-0.8660254037844387, 0.9999999999999999, -1), new Vec3(-0.9999999999999996, 0.8660254037844387, 1)},
          new List<Vec3> {new Vec3(0, 0, 1), new Vec3(-0.9999999999999996, 0.8660254037844387, 1), new Vec3(-1.7320508075688774, 0.49999999999999994, 1)},
          new List<Vec3> {new Vec3(-0.9999999999999996, 0.8660254037844387, 1), new Vec3(-0.8660254037844387, 0.9999999999999999, -1), new Vec3(-1.7320508075688774, 0.49999999999999994, 1)},
          new List<Vec3> {new Vec3(0, 0, -1), new Vec3(-1, 2.4492935982947064e-16, -1), new Vec3(-0.8660254037844387, 0.9999999999999999, -1)},
          new List<Vec3> {new Vec3(-0.8660254037844387, 0.9999999999999999, -1), new Vec3(-1, 2.4492935982947064e-16, -1), new Vec3(-1.7320508075688774, 0.49999999999999994, 1)},
          new List<Vec3> {new Vec3(0, 0, 1), new Vec3(-1.7320508075688774, 0.49999999999999994, 1), new Vec3(-2, 1.2246467991473532e-16, 1)},
          new List<Vec3> {new Vec3(-1.7320508075688774, 0.49999999999999994, 1), new Vec3(-1, 2.4492935982947064e-16, -1), new Vec3(-2, 1.2246467991473532e-16, 1)},
          new List<Vec3> {new Vec3(0, 0, -1), new Vec3(-0.8660254037844386, -1.0000000000000002, -1), new Vec3(-1, 2.4492935982947064e-16, -1)},
          new List<Vec3> {new Vec3(-1, 2.4492935982947064e-16, -1), new Vec3(-0.8660254037844386, -1.0000000000000002, -1), new Vec3(-2, 1.2246467991473532e-16, 1)},
          new List<Vec3> {new Vec3(0, 0, 1), new Vec3(-2, 1.2246467991473532e-16, 1), new Vec3(-1.7320508075688772, -0.5000000000000001, 1)},
          new List<Vec3> {new Vec3(-2, 1.2246467991473532e-16, 1), new Vec3(-0.8660254037844386, -1.0000000000000002, -1), new Vec3(-1.7320508075688772, -0.5000000000000001, 1)},
          new List<Vec3> {new Vec3(0, 0, -1), new Vec3(-0.5000000000000004, -1.732050807568877, -1), new Vec3(-0.8660254037844386, -1.0000000000000002, -1)},
          new List<Vec3> {new Vec3(-0.8660254037844386, -1.0000000000000002, -1), new Vec3(-0.5000000000000004, -1.732050807568877, -1), new Vec3(-1.7320508075688772, -0.5000000000000001, 1)},
          new List<Vec3> {new Vec3(0, 0, 1), new Vec3(-1.7320508075688772, -0.5000000000000001, 1), new Vec3(-1.0000000000000009, -0.8660254037844385, 1)},
          new List<Vec3> {new Vec3(-1.7320508075688772, -0.5000000000000001, 1), new Vec3(-0.5000000000000004, -1.732050807568877, -1), new Vec3(-1.0000000000000009, -0.8660254037844385, 1)},
          new List<Vec3> {new Vec3(0, 0, -1), new Vec3(-1.8369701987210297e-16, -2, -1), new Vec3(-0.5000000000000004, -1.732050807568877, -1)},
          new List<Vec3> {new Vec3(-0.5000000000000004, -1.732050807568877, -1), new Vec3(-1.8369701987210297e-16, -2, -1), new Vec3(-1.0000000000000009, -0.8660254037844385, 1)},
          new List<Vec3> {new Vec3(0, 0, 1), new Vec3(-1.0000000000000009, -0.8660254037844385, 1), new Vec3(-3.6739403974420594e-16, -1, 1)},
          new List<Vec3> {new Vec3(-1.0000000000000009, -0.8660254037844385, 1), new Vec3(-1.8369701987210297e-16, -2, -1), new Vec3(-3.6739403974420594e-16, -1, 1)},
          new List<Vec3> {new Vec3(0, 0, -1), new Vec3(0.5000000000000001, -1.7320508075688772, -1), new Vec3(-1.8369701987210297e-16, -2, -1)},
          new List<Vec3> {new Vec3(-1.8369701987210297e-16, -2, -1), new Vec3(0.5000000000000001, -1.7320508075688772, -1), new Vec3(-3.6739403974420594e-16, -1, 1)},
          new List<Vec3> {new Vec3(0, 0, 1), new Vec3(-3.6739403974420594e-16, -1, 1), new Vec3(1.0000000000000002, -0.8660254037844386, 1)},
          new List<Vec3> {new Vec3(-3.6739403974420594e-16, -1, 1), new Vec3(0.5000000000000001, -1.7320508075688772, -1), new Vec3(1.0000000000000002, -0.8660254037844386, 1)},
          new List<Vec3> {new Vec3(0, 0, -1), new Vec3(0.8660254037844384, -1.0000000000000009, -1), new Vec3(0.5000000000000001, -1.7320508075688772, -1)},
          new List<Vec3> {new Vec3(0.5000000000000001, -1.7320508075688772, -1), new Vec3(0.8660254037844384, -1.0000000000000009, -1), new Vec3(1.0000000000000002, -0.8660254037844386, 1)},
          new List<Vec3> {new Vec3(0, 0, 1), new Vec3(1.0000000000000002, -0.8660254037844386, 1), new Vec3(1.7320508075688767, -0.5000000000000004, 1)},
          new List<Vec3> {new Vec3(1.0000000000000002, -0.8660254037844386, 1), new Vec3(0.8660254037844384, -1.0000000000000009, -1), new Vec3(1.7320508075688767, -0.5000000000000004, 1)},
          new List<Vec3> {new Vec3(0, 0, -1), new Vec3(1, -4.898587196589413e-16, -1), new Vec3(0.8660254037844384, -1.0000000000000009, -1)},
          new List<Vec3> {new Vec3(0.8660254037844384, -1.0000000000000009, -1), new Vec3(1, -4.898587196589413e-16, -1), new Vec3(1.7320508075688767, -0.5000000000000004, 1)},
          new List<Vec3> {new Vec3(0, 0, 1), new Vec3(1.7320508075688767, -0.5000000000000004, 1), new Vec3(2, -2.4492935982947064e-16, 1)},
          new List<Vec3> {new Vec3(1.7320508075688767, -0.5000000000000004, 1), new Vec3(1, -4.898587196589413e-16, -1), new Vec3(2, -2.4492935982947064e-16, 1)}
        };

        Assert.AreEqual(pts.Count, 48);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
#endif

        // test startAngle and endAngle
        obs = SemiellipticCylinder(radius: (1, 2), startAngle: 90, endAngle: 360*0.75, segments: 12);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        exp = new List<List<Vec3>> { };

        Assert.AreEqual(pts.Count, 10);
        // Assert.IsTrue(comparePolygonsAsPoints(pts, exp))

        // test segments
        obs = EllipticCylinder(segments: 8);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();

        Assert.AreEqual(pts.Count, 10);

        // test center
        obs = EllipticCylinder(center: (-5, -5, -5), height: 3, segments: 8);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        if(WriteTests) TestData.Make("EllpticCylinderOptsExp2", pts);
        exp = UnitTestData.EllpticCylinderOptsExp2;

        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }
}
