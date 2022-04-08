namespace CSharpCADTests;

[TestFixture]
public class Geom3Tests
{
    private static Geom3 makeGeom3(Vec3[] points, Mat4? transforms = null)
    {
        var poly = new Poly3(points);
        var poly_array = new Poly3[] { poly };
        return new Geom3(poly_array, transforms);
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestCloneAndConstructors()
    {
        var points = new Vec3[] { new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(1, 0, 1) };
        var expected = makeGeom3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(1, 0, 1) });
        var geometry = makeGeom3(points);
        var another = geometry.Clone();
        Assert.IsFalse(Object.ReferenceEquals(another, geometry));
        Assert.IsTrue(geometry == another);
        Assert.IsTrue(geometry == expected);
        geometry = new Geom3();
        expected = new Geom3(new Poly3[0], null, null);
        Assert.IsTrue(geometry == expected);
    }

    [Test]
    public void TestApplyTransforms()
    {
        var points = new Vec3[] { new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(1, 0, 1) };
        var expected = makeGeom3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(1, 0, 1) });
        var geometry = makeGeom3(points);
        var updated = geometry.ApplyTransforms();
        Assert.IsTrue(Object.ReferenceEquals(geometry, updated));
        Assert.IsTrue(updated == expected);

        var updated2 = updated.ApplyTransforms();
        Assert.IsTrue(Object.ReferenceEquals(updated, updated2));
        Assert.IsTrue(updated2 == expected);
    }

    [Test]
    public void TestBoundingBox()
    {
        var g = makeGeom3(new Vec3[] { new Vec3(5, 10, 15), new Vec3(4, 11, 15), new Vec3(5, 11, 16) });
        var (min, max) = g.BoundingBox();
        Assert.IsTrue(min.x == 4);
        Assert.IsTrue(min.y == 10);
        Assert.IsTrue(min.z == 15);
        Assert.IsTrue(max.x == 5);
        Assert.IsTrue(max.y == 11);
        Assert.IsTrue(max.z == 16);
    }

    [Test]
    public void TestInvertOnEmpty()
    {
        var expected = new Geom3(new Poly3[0], null, null);
        var geometry = new Geom3();
        var another = geometry.Invert();
        Assert.IsFalse(Object.ReferenceEquals(another, geometry));
        Assert.IsTrue(another == expected);
    }

    [Test]
    public void TestInvertOnPopulated()
    {
        var points = new Vec3[] { new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(1, 0, 1) };
        var expected = makeGeom3(new Vec3[] { new Vec3(1, 0, 1), new Vec3(1, 0, 0), new Vec3(0, 0, 0) });
        var geometry = makeGeom3(points);
        var another = geometry.Invert();
        Assert.False(Object.ReferenceEquals(another, geometry));
        Assert.IsTrue(another == expected);
    }

    [Test]
    public void TestToPoints()
    {
        var points = new Vec3[] { new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(1, 0, 1) };
        var expected = makeGeom3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(1, 0, 1) });
        var geometry = makeGeom3(points);
        var pointarray = geometry.ToPoints();
        Assert.IsTrue(Helpers.CompareListOfLists(pointarray, expected.ToPoints()));
    }

    [Test]
    public void TestTransform()
    {
        var points = new Vec3[] { new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(1, 0, 1) };
        var rotation = 90 * 0.017453292519943295;
        var rotate90 = Mat4.FromZRotation(rotation);

        // continue with typical user scenario, several itterations of transforms and access

        // expect lazy transform, i.e. only the transforms change
        var expected = makeGeom3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(1, 0, 1) },
          new Mat4(0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1));
        var geometry = makeGeom3(points);
        var another = geometry.Transform(rotate90);
        Assert.IsFalse(Object.ReferenceEquals(geometry, another));
        Assert.IsTrue(another.IsNearlyEqual(expected));

        // expect lazy transform, i.e. only the transforms change
        expected = makeGeom3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(1, 0, 1) },
          new Mat4(6.123234262925839e-17, 1, 0, 0, -1, 6.123234262925839e-17, 0, 0, 0, 0, 1, 0, 5, 10, 15, 1));
        another = another.Transform(Mat4.FromTranslation(new Vec3(5, 10, 15)));
        Assert.IsTrue(another.IsNearlyEqual(expected));

        // expect application of the transforms to the polygons
        expected = makeGeom3(new Vec3[] { new Vec3(5, 10, 15), new Vec3(5, 11, 15), new Vec3(5, 11, 16) },
          new Mat4());
        another.ToPolygons();
        //Assert.IsTrue(another.Polygons == expected.Polygons);
        //Assert.IsTrue(another.Transforms == expected.Transforms);

        // expect lazy transform, i.e. only the transforms change
        expected = makeGeom3(new Vec3[] { new Vec3(5, 10, 15), new Vec3(5, 11, 15), new Vec3(5, 11, 16) },
          new Mat4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 5, 10, 15, 1));
        another = another.Transform(Mat4.FromTranslation(new Vec3(5, 10, 15)));
        Assert.IsTrue(another == expected);
    }
}