using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class PrimitivesTests
{
    static bool WriteTests = false;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestApplyTransforms()
    {
        var points = new List<Vec2>(3);
        points.Add(new Vec2(0, 0));
        points.Add(new Vec2(1, 0));
        points.Add(new Vec2(0, 1));
        var ex_tree = new Geom2.NRTree();
        ex_tree.Insert(new Vec2[] { new Vec2(0, 0), new Vec2(1, 0), new Vec2(0, 1) });
        var ex_transforms = new Mat4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
        var expected = new Geom2(ex_tree, ex_transforms);
        Assert.DoesNotThrow(() => expected.Validate());

        var geometry = new Geom2(points);
        Assert.DoesNotThrow(() => geometry.Validate());
        var updated = geometry.ApplyTransforms();
        Assert.DoesNotThrow(() => updated.Validate());
        Assert.IsTrue(Object.ReferenceEquals(geometry, updated));
        Assert.IsTrue(updated == expected);

        var updated2 = updated.ApplyTransforms();
        Assert.IsTrue(Object.ReferenceEquals(updated, updated2));
        Assert.IsTrue(updated == expected);
    }

    [Test]
    public void TestCloneComplete()
    {
        var points = new List<Vec2>(3);
        points.Add(new Vec2(0, 0));
        points.Add(new Vec2(1, 0));
        points.Add(new Vec2(0, 1));
        var ex_tree = new Geom2.NRTree();
        ex_tree.Insert(new Vec2[] { new Vec2(0, 0), new Vec2(1, 0), new Vec2(0, 1) });
        var ex_transforms = new Mat4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
        var expected = new Geom2(ex_tree, ex_transforms);
        Assert.DoesNotThrow(() => expected.Validate());
        var geometry = new Geom2(points);
        Assert.DoesNotThrow(() => geometry.Validate());
        var another = geometry.Clone();
        Assert.DoesNotThrow(() => another.Validate());
        Assert.IsFalse(Object.ReferenceEquals(another, geometry));
        Assert.IsTrue(another == expected);
    }

    [Test]
    public void TestToOutlinesSimple()
    {
        var shp1_tree = new Geom2.NRTree();
        shp1_tree.Insert(new Vec2[] { new Vec2(1, -1), new Vec2(1, 1), new Vec2(-1, -1) });
        var shp1 = new Geom2(shp1_tree);
        Assert.DoesNotThrow(() => shp1.Validate());
        var ret1 = shp1.ToOutlinesLLV();
        var exp1 = new List<List<Vec2>>(1);
        var plist1 = new List<Vec2>(3);
        plist1.Add(new Vec2(1, -1));
        plist1.Add(new Vec2(1, 1));
        plist1.Add(new Vec2(-1, -1));
        exp1.Add(plist1);
        Assert.IsTrue(Helpers.CompareListOfLists<Vec2>(ret1, exp1));


        var shp2_tree = new Geom2.NRTree();
        shp2_tree.Insert(new Vec2[] {
          new Vec2(1, -1),
          new Vec2(1, 1),
          new Vec2(-1, -1),
        });
        shp2_tree.Insert(new Vec2[] {
          new Vec2(6, 4),
          new Vec2(6, 6),
          new Vec2(4, 4)
        });
        var shp2 = new Geom2(shp2_tree, new Mat4());
        Assert.DoesNotThrow(() => shp2.Validate());
        var ret2 = shp2.ToOutlinesLLV();
        var exp2 = new List<List<Vec2>>(1);
        var plist2a = new List<Vec2>(3);
        plist2a.Add(new Vec2(1, -1));
        plist2a.Add(new Vec2(1, 1));
        plist2a.Add(new Vec2(-1, -1));
        var plist2b = new List<Vec2>(3);
        plist2b.Add(new Vec2(6, 4));
        plist2b.Add(new Vec2(6, 6));
        plist2b.Add(new Vec2(4, 4));
        exp2.Add(plist2a);
        exp2.Add(plist2b);
        Assert.IsTrue(Helpers.CompareListOfLists<Vec2>(ret2, exp2));
    }


    [Test]
    public void TestToOulinesForHoles()
    {
        var shp1_tree = new Geom2.NRTree();
        shp1_tree.Insert(new Vec2[] {
          new Vec2(-10, -10),
          new Vec2(10, -10),
          new Vec2(10, 10)
        });
        shp1_tree.Insert(new Vec2[] {
          new Vec2(6, -5),
          new Vec2(5, -5),
          new Vec2(6, -4),
        });
        var shp1 = new Geom2(shp1_tree, new Mat4());
        Assert.DoesNotThrow(() => shp1.Validate());
        var ret1 = shp1.ToOutlines();
        if (WriteTests) TestData.Make("ToOutlinesForHoles", ret1);
        var exp1 = UnitTestData.ToOutlinesForHoles;

        Assert.AreEqual(ret1, exp1);

        var shp2_tree = new Geom2.NRTree();
        shp2_tree.Insert(new Vec2[] {
          new Vec2(-10, -10),
          new Vec2(10, -10),
          new Vec2(10, 10),
        });
        shp2_tree.Insert(new Vec2[] {
          new Vec2(8, -8),
          new Vec2(-6, -8),
          new Vec2(8, 6),
        });
        shp2_tree.Insert(new Vec2[] {
          new Vec2(5, -5),
          new Vec2(6, -5),
          new Vec2(6, -4),
        });
        var shp2 = new Geom2(shp2_tree, new Mat4());
        Assert.DoesNotThrow(() => shp2.Validate());
        var ret2 = shp2.ToOutlines();
        if (WriteTests) TestData.Make("ToOutlinesForHolesExp2", ret2);
        var exp2 = UnitTestData.ToOutlinesForHolesExp2;

        Assert.AreEqual(ret2, exp2);
    }


    [Test]
    public void TestToOutlinesForTouchingEdges()
    {
        var nrtree = new Geom2.NRTree();
        nrtree.Insert(new Vec2[] {
          new Vec2(5, 5),
          new Vec2(15, 5),
          new Vec2(15, 15),
          new Vec2(5, 15),
        });
        nrtree.Insert(new Vec2[] {
          new Vec2(-5, -5),
          new Vec2(5, -5),
          new Vec2(5, 5),
          new Vec2(-5, 5)
        });
        var shp1 = new Geom2(nrtree, new Mat4());
        Assert.DoesNotThrow(() => shp1.Validate());
        var ret1 = shp1.ToOutlinesLLV();
        var exp1 = new List<List<Vec2>>(2);
        var plist1a = new List<Vec2>(4);
        plist1a.Add(new Vec2(5, 5));
        plist1a.Add(new Vec2(15, 5));
        plist1a.Add(new Vec2(15, 15));
        plist1a.Add(new Vec2(5, 15));
        exp1.Add(plist1a);
        var plist1b = new List<Vec2>(4);
        plist1b.Add(new Vec2(-5, -5));
        plist1b.Add(new Vec2(5, -5));
        plist1b.Add(new Vec2(5, 5));
        plist1b.Add(new Vec2(-5, 5));
        exp1.Add(plist1b);
        Assert.IsTrue(Helpers.CompareListOfLists<Vec2>(ret1, exp1));
    }


    [Test]
    public void TestToOutlinesWithHolesThatTouch()
    {
        var nrtree = new Geom2.NRTree();
        nrtree.Insert(new Vec2[] {
          new Vec2(-20, -20),
          new Vec2(20, -20),
          new Vec2(20, 20),
          new Vec2(-20, 20)
        });
        nrtree.Insert(new Vec2[] {
          new Vec2(15, 15),
          new Vec2(15, 5),
          new Vec2(5, 5),
          new Vec2(5, 15),
        });
        nrtree.Insert(new Vec2[] {
          new Vec2(5, 5),
          new Vec2(5, -5),
          new Vec2(-5, -5),
          new Vec2(-5, 5),
        });
        var shp1 = new Geom2(nrtree);
        Assert.DoesNotThrow(() => shp1.Validate());
        var ret1 = shp1.ToOutlinesLLV();
        var exp1 = new List<List<Vec2>>(3);
        var plist1a = new List<Vec2>(4);
        plist1a.Add(new Vec2(-20, -20));
        plist1a.Add(new Vec2(20, -20));
        plist1a.Add(new Vec2(20, 20));
        plist1a.Add(new Vec2(-20, 20));
        exp1.Add(plist1a);
        var plist1b = new List<Vec2>(4);
        plist1b.Add(new Vec2(15, 15));
        plist1b.Add(new Vec2(15, 5));
        plist1b.Add(new Vec2(5, 5));
        plist1b.Add(new Vec2(5, 15));
        exp1.Add(plist1b);
        var plist1c = new List<Vec2>(4);
        plist1c.Add(new Vec2(5, 5));
        plist1c.Add(new Vec2(5, -5));
        plist1c.Add(new Vec2(-5, -5));
        plist1c.Add(new Vec2(-5, 5));
        exp1.Add(plist1c);

        Assert.IsTrue(Helpers.CompareListOfLists<Vec2>(ret1, exp1));
    }


    [Test]
    public void TestReverse()
    {
        var points = new List<Vec2> { new Vec2(0, 0), new Vec2(1, 0), new Vec2(0, 1) };
        var geometry = new Geom2(points);
        Assert.DoesNotThrow(() => geometry.Validate());
        points.Reverse();
        var geometry2 = new Geom2(points);
        // LATER surely this failure should happen! Assert.DoesNotThrow(() => geometry2.Validate());
        // LATER does a reverse of the geometry EVER make sense?
        var another = geometry.Reverse();
        Assert.AreNotSame(geometry, another);
        Assert.AreEqual(another.ToPoints(), geometry2.ToPoints());
    }

    [Test]
    public void TestToPoints()
    {
        var points = new List<Vec2>(3);
        points.Add(new Vec2(0, 0));
        points.Add(new Vec2(1, 0));
        points.Add(new Vec2(0, 1));
        var geometry = new Geom2(points);
        Assert.DoesNotThrow(() => geometry.Validate());

        var expected = new List<Vec2>(3);
        expected.Add(new Vec2(0, 0));
        expected.Add(new Vec2(1, 0));
        expected.Add(new Vec2(0, 1));
        var pointarray = geometry.ToPoints();
        Assert.IsTrue(Helpers.CompareArrays(pointarray, expected.ToArray()));
    }

    [Test]
    public void TestTransforms()
    {
        var points = new List<Vec2>(3);
        points.Add(new Vec2(0, 0));
        points.Add(new Vec2(1, 0));
        points.Add(new Vec2(0, 1));
        var ex_tree = new Geom2.NRTree();
        ex_tree.Insert(new Vec2[] { new Vec2(0, 0), new Vec2(1, 0), new Vec2(0, 1) });
        var ex_transforms = new Mat4(0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
        var expected = new Geom2(ex_tree, ex_transforms);
        Assert.DoesNotThrow(() => expected.Validate());

        var rotation = 90 * 0.017453292519943295;
        var rotate90 = Mat4.FromZRotation(rotation);

        // continue with typical user scenario, several itterations of transforms and access

        // expect lazy transform, i.e. only the transforms change
        var geometry = new Geom2(points);
        Assert.DoesNotThrow(() => geometry.Validate());
        var another = geometry.Transform(rotate90);
        Assert.False(Object.ReferenceEquals(geometry, another));

        // expect lazy transform, i.e. only the transforms change
        expected = new Geom2(ex_tree, new Mat4(0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 1, 0, 5, 10, 15, 1));
        Assert.DoesNotThrow(() => expected.Validate());
        another = another.Transform(Mat4.FromTranslation(new Vec3(5, 10, 15)));
        Assert.DoesNotThrow(() => another.Validate());

        // expect application of the transforms to the sides
        expected = new Geom2(new Vec2[] {
          new Vec2(5, 11),
          new Vec2(4, 10),
          new Vec2(5, 10),
        });
        Assert.DoesNotThrow(() => expected.Validate());
        var nrtree = new Geom2.NRTree();
        nrtree.Insert(expected.ToPoints());
        another = new Geom2(nrtree, new Mat4());
        Assert.DoesNotThrow(() => another.Validate());
        Assert.IsTrue(another == expected);

        // expect lazy transform, i.e. only the transforms change
        nrtree = new Geom2.NRTree();
        nrtree.Insert(expected.ToPoints());
        expected = new Geom2(nrtree, new Mat4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 5, 10, 15, 1));
        Assert.DoesNotThrow(() => expected.Validate());
        another = another.Transform(Mat4.FromTranslation(new Vec3(5, 10, 15)));
        Assert.DoesNotThrow(() => another.Validate());
        Assert.IsTrue(another == expected);
    }
}