using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class TestExtrudeFromSlices
{
    static bool WriteTests = false;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestExtrudeFSDefaults()
    {
        var geometry2 = new Geom2(new List<Vec2>{new Vec2(10, 10),
          new Vec2(-10, 10), new Vec2(-10, -10), new Vec2(10, -10)});

        var geometry3 = ExtrudeFromSlices(new Slice(geometry2.ToOutlines()));
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        if (WriteTests) TestData.Make("ExtrudeFSDef1", pts);
        var exp = UnitTestData.ExtrudeFSDef1;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        var poly2 = new Poly3(new List<Vec3>{ new Vec3(10, 10, 0), new Vec3(-10, 10, 0),
          new Vec3(-10, -10, 0), new Vec3(10, -10, 0)});
        geometry3 = ExtrudeFromSlices(new Slice(poly2.ToPoints()));
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
        var geometry3 = ExtrudeFromSlices(hexSlice, callback,
          numberOfSlices: Floorish(Math.PI * 2 / angle), capStart: false, capEnd: false, close: true);
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
        var geometry3 = ExtrudeFromSlices(baseSlice, callBack, numberOfSlices: 4, capStart: true, capEnd: false);
        // LATER JSCAD Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        Assert.AreEqual(pts.Count, 26);
    }

    [Test]
    public void TestExtrudeFSChangingShapeChangingDimensions()
    {
        Slice? callBack(double progress, int count, Slice baseSlice)
        {
            var newshape = Circle(radius: 5 + count, segments: 4 + count);
            var newslice = new Slice(newshape.ToOutlines());
            newslice = newslice.Transform(Mat4.FromTranslation(new Vec3(0, 0, count * 10)));
            return newslice;
        }
        var baseSlice = new Slice(Circle(radius: 4, segments: 4).ToOutlines());
        var geometry3 = ExtrudeFromSlices(baseSlice, callBack, numberOfSlices: 5);
        // LATER JSCAD Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        Assert.AreEqual(pts.Count, 304);
    }

    [Test]
    public void TestExtrudeFSHoles()
    {
        var nrtree = new Geom2.NRTree();
        nrtree.Insert(new Vec2[] {
            new Vec2((double)(-10), (double)(-10)),
            new Vec2((double)(10), (double)(-10)),
            new Vec2((double)(10), (double)(10)),
            new Vec2((double)(-10), (double)(10)),
        });
        nrtree.Insert(new Vec2[] {
            new Vec2((double)(5), (double) (5)),
            new Vec2((double)(5), (double) (-5)),
            new Vec2((double)(-5), (double) (-5)),
            new Vec2((double)(-5), (double) (5)),
        });
        var geometry2 = new Geom2(nrtree);
        geometry2.Validate();
        var geometry3 = ExtrudeFromSlices(new Slice(geometry2.ToOutlines()));
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        if(WriteTests) TestData.Make("ExtrudeFSHoleExp", pts);
        var exp = UnitTestData.ExtrudeFSHoleExp;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }
}
