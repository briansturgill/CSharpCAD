using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class EllipseTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestEllipseDefaults()
    {
        var geometry = Ellipse();
        Assert.DoesNotThrow(() => geometry.Validate());
        var obs = geometry.ToPoints();

        Assert.AreEqual(obs.Length, 32);
    }


    [Test]
    public void TestEllipseOptions()
    {
        // test center
        var geometry = Ellipse(center: (3, 5));
        Assert.DoesNotThrow(() => geometry.Validate());
        var obs = geometry.ToPoints();
        var exp = new Vec2[] {
          new Vec2(4, 5),
          new Vec2(3.9807852804032304, 5.195090322016128),
          new Vec2(3.923879532511287, 5.38268343236509),
          new Vec2(3.8314696123025453, 5.555570233019602),
          new Vec2(3.7071067811865475, 5.707106781186548),
          new Vec2(3.555570233019602, 5.831469612302545),
          new Vec2(3.3826834323650896, 5.923879532511287),
          new Vec2(3.1950903220161284, 5.98078528040323),
          new Vec2(3, 6),
          new Vec2(2.804909677983872, 5.98078528040323),
          new Vec2(2.6173165676349104, 5.923879532511287),
          new Vec2(2.444429766980398, 5.831469612302545),
          new Vec2(2.2928932188134525, 5.707106781186548),
          new Vec2(2.1685303876974547, 5.555570233019602),
          new Vec2(2.076120467488713, 5.38268343236509),
          new Vec2(2.0192147195967696, 5.195090322016129),
          new Vec2(2, 5),
          new Vec2(2.0192147195967696, 4.804909677983872),
          new Vec2(2.076120467488713, 4.61731656763491),
          new Vec2(2.1685303876974547, 4.444429766980398),
          new Vec2(2.292893218813452, 4.292893218813452),
          new Vec2(2.444429766980398, 4.168530387697455),
          new Vec2(2.6173165676349095, 4.076120467488714),
          new Vec2(2.804909677983871, 4.01921471959677),
          new Vec2(3, 4),
          new Vec2(3.1950903220161284, 4.01921471959677),
          new Vec2(3.38268343236509, 4.076120467488713),
          new Vec2(3.5555702330196017, 4.168530387697454),
          new Vec2(3.7071067811865475, 4.292893218813452),
          new Vec2(3.8314696123025453, 4.444429766980398),
          new Vec2(3.9238795325112865, 4.6173165676349095),
          new Vec2(3.9807852804032304, 4.804909677983871)
        };

        Assert.AreEqual(obs.Length, 32);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // test radius
        geometry = Ellipse(radius: (3, 5), segments: 16);
        Assert.DoesNotThrow(() => geometry.Validate());
        obs = geometry.ToPoints();
        exp = new Vec2[] {
          new Vec2(3, 0),
          new Vec2(2.77163859753386, 1.913417161825449),
          new Vec2(2.121320343559643, 3.5355339059327373),
          new Vec2(1.1480502970952695, 4.619397662556434),
          new Vec2(1.8369701987210297e-16, 5),
          new Vec2(-1.1480502970952693, 4.619397662556434),
          new Vec2(-2.1213203435596424, 3.5355339059327378),
          new Vec2(-2.77163859753386, 1.9134171618254494),
          new Vec2(-3, 6.123233995736766e-16),
          new Vec2(-2.7716385975338604, -1.9134171618254483),
          new Vec2(-2.121320343559643, -3.5355339059327373),
          new Vec2(-1.148050297095271, -4.619397662556432),
          new Vec2(-5.51091059616309e-16, -5),
          new Vec2(1.14805029709527, -4.619397662556433),
          new Vec2(2.121320343559642, -3.5355339059327386),
          new Vec2(2.7716385975338595, -1.913417161825452)
        };

        Assert.AreEqual(obs.Length, 16);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // test startAngle
        geometry = Ellipse(radius: (3, 5), startAngle: Math.PI / 2, segments: 16);
        Assert.DoesNotThrow(() => geometry.Validate());
        obs = geometry.ToPoints();
        exp = new Vec2[] {
          new Vec2(1.8369701987210297e-16, 5),
          new Vec2(-1.1480502970952693, 4.619397662556434),
          new Vec2(-2.1213203435596424, 3.5355339059327378),
          new Vec2(-2.77163859753386, 1.9134171618254494),
          new Vec2(-3, 6.123233995736766e-16),
          new Vec2(-2.7716385975338604, -1.9134171618254483),
          new Vec2(-2.121320343559643, -3.5355339059327373),
          new Vec2(-1.148050297095271, -4.619397662556432),
          new Vec2(-5.51091059616309e-16, -5),
          new Vec2(1.14805029709527, -4.619397662556433),
          new Vec2(2.121320343559642, -3.5355339059327386),
          new Vec2(2.7716385975338595, -1.913417161825452),
          new Vec2(3, -1.2246467991473533e-15),
          new Vec2(0, 0)
        };

        Assert.AreEqual(obs.Length, 14);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // test endAngle
        geometry = Ellipse(radius: (3, 5), endAngle: Math.PI / 2, segments: 16);
        Assert.DoesNotThrow(() => geometry.Validate());
        obs = geometry.ToPoints();
        exp = new Vec2[] {
          new Vec2(3, 0),
          new Vec2(2.77163859753386, 1.913417161825449),
          new Vec2(2.121320343559643, 3.5355339059327373),
          new Vec2(1.1480502970952695, 4.619397662556434),
          new Vec2(1.8369701987210297e-16, 5),
          new Vec2(0, 0)
        };

        Assert.AreEqual(obs.Length, 6);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // test segments
        geometry = Ellipse(segments: 72);
        Assert.DoesNotThrow(() => geometry.Validate());
        obs = geometry.ToPoints();
        Assert.AreEqual(obs.Length, 72);
    }

}
