using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class TestExtrudeFromSlices
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestExtrudeFSDefaults()
    {
        var geometry2 = new Geom2(new List<Vec2>{new Vec2(10, 10),
          new Vec2(-10, 10), new Vec2(-10, -10), new Vec2(10, -10)});

        var geometry3 = ExtrudeFromSlices(new Opts { }, new Slice(geometry2.ToSides()));
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        var exp = UnitTestData.ExtrudeFSDef1;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        var poly2 = new Poly3(new List<Vec3>{ new Vec3(10, 10, 0), new Vec3(-10, 10, 0),
          new Vec3(-10, -10, 0), new Vec3(10, -10, 0)});
        geometry3 = ExtrudeFromSlices(new Opts { }, new Slice(poly2.ToPoints()));
        Assert.DoesNotThrow(() => geometry3.Validate());
        pts = geometry3.ToPoints();

        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }

    [Test]
    public void TestExtrudeFSTorus()
    {
        var sqrt3 = Math.Sqrt(3) / 2;
        var radius = 10;

        var hex = new Poly3(new List<Vec3> {
          new Vec3(radius, 0, 0),
          new Vec3(radius / 2, radius * sqrt3, 0),
          new Vec3(-radius / 2, radius * sqrt3, 0),
          new Vec3(-radius, 0, 0),
          new Vec3(-radius / 2, -radius * sqrt3, 0),
          new Vec3(radius / 2, -radius * sqrt3, 0)
        });
        hex = hex.Transform(Mat4.FromTranslation(new Vec3(0, 20, 0)));
        var hexSlice = new Slice(hex.ToPoints());

        var angle = Math.PI / 4;
        Slice? callback(double progress, int index, object baseSlice)
        {
            return ((Slice)baseSlice).Transform(Mat4.FromXRotation(angle * index));
        }
        var geometry3 = ExtrudeFromSlices(new Opts {
          {"numberOfSlices", Floorish(Math.PI * 2 / angle)},
          {"capStart", false},
          {"capEnd", false},
          {"close", true}
        }, hexSlice, callback);
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        Assert.AreEqual(pts.Count, 96);
    }
    [Test]
    public void TestExtrudeFSSameShapeChangingDimensions()
    {
        Slice? callBack(double progress, int count, Slice baseSlice)
        {
            var newslice = baseSlice.Transform(Mat4.FromTranslation(new Vec3(0, 0, count * 2)));
            newslice = newslice.Transform(Mat4.FromScaling(new Vec3(1 + count, 1 + (count / 2), 1)));
            return newslice;
        }
        var baseSlice = new Slice(new List<Vec3> { new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(1, 1, 0), new Vec3(0, 1, 0) });
        var geometry3 = ExtrudeFromSlices(new Opts {
              {"numberOfSlices", 4},
              {"capStart", true},
              {"capEnd", false},
              }, baseSlice, callBack);
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        Assert.AreEqual(pts.Count, 26);
    }

    [Test]
    public void TestExtrudeFSChangingShapeChangingDimensions()
    {
        Slice? callBack(double progress, int count, Slice baseSlice)
        {
            var newshape = Circle(new Opts { { "radius", 5 + count }, { "segments", 4 + count } });
            var newslice = new Slice(newshape.ToSides());
            newslice = newslice.Transform(Mat4.FromTranslation(new Vec3(0, 0, count * 10)));
            return newslice;
        }
        var baseSlice = new Slice(Circle(new Opts { { "radius", 4 }, { "segments", 4 } }).ToSides());
        var geometry3 = ExtrudeFromSlices(new Opts { { "numberOfSlices", 5 } }, baseSlice, callBack);
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        Assert.AreEqual(pts.Count, 304);
    }

    [Test]
    public void TestExtrudeFSHoles()
    {
        var geometry2 = new Geom2(UnitTestData.ExtrudeFSHole1, new Mat4(), null);
        var geometry3 = ExtrudeFromSlices(new Opts { }, new Slice(geometry2.ToSides()));
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        var exp = UnitTestData.ExtrudeFSHoleExp;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }
}
