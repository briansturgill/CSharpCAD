namespace CSharpCADTests;

[TestFixture]
public class SliceTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestSliceCalculatePlane()
    {
        // do not do this... it's an error
        var slice1 = new Slice(new Geom2.Side[0]);
        Assert.Throws<ArgumentException>(() => { var plane1 = slice1.CalculatePlane(); });

        var slice2 = new Slice(new List<Vec2> { new Vec2(0, 0), new Vec2(1, 0), new Vec2(1, 1) });
        var plane2 = slice2.CalculatePlane();
        Assert.IsTrue(plane2 == new Plane(new Vec3(0, 0, 1), 0));

        var slice3 = slice2.Transform(Mat4.FromXRotation(Math.PI / 2));
        var plane3 = slice3.CalculatePlane();
        Assert.IsTrue(plane3.IsNearlyEqual(new Plane(new Vec3(0, -1, 0), 0)));

        var slice4 = slice3.Transform(Mat4.FromZRotation(Math.PI / 2));
        var plane4 = slice4.CalculatePlane();
        Assert.IsTrue(plane4.IsNearlyEqual(new Plane(new Vec3(1, 0, 0), 0)));

        // Issue #749
        var slice5 = new Slice(new List<Vec3>{new Vec3(-4, 0, 2), new Vec3(4, 0, 2), new Vec3(4, 5, 2),
          new Vec3(6, 5, 2), new Vec3(4, 7, 2), new Vec3(-4, 7, 2), new Vec3(-6, 5, 2), new Vec3(-4, 5, 2)});
        var plane5 = slice5.CalculatePlane();
        Assert.IsTrue(plane5 == new Plane(new Vec3(0, 0, 1), 2));

        var slice6 = new Slice(new List<Vec3>{new Vec3(4, 0, 0), new Vec3(-4, 0, 0), new Vec3(-4, 5, 0),
          new Vec3(-6, 5, 0), new Vec3(-4, 7, 0), new Vec3(4, 7, 0), new Vec3(6, 5, 0), new Vec3(4, 5, 0)});
        var plane6 = slice6.CalculatePlane();
        Assert.IsTrue(plane6 == new Plane(new Vec3(0, 0, -1), 0));
    }

    [Test]
    public void TestSliceConstructFromPoints()
    {
        var exp1 = new Slice.Edge[] {
          new Slice.Edge(new Vec3(1, 1, 0), new Vec3(0, 0, 0)),
          new Slice.Edge(new Vec3(0, 0, 0), new Vec3(1, 0, 0)),
          new Slice.Edge(new Vec3(1, 0, 0), new Vec3(1, 1, 0))
        };
        var obs1 = new Slice(new List<Vec2> { new Vec2(0, 0), new Vec2(1, 0), new Vec2(1, 1) });
        Assert.AreEqual(obs1.ToEdges(), exp1);
    }

    [Test]
    public void TestSliceConstructFromSides()
    {
        var exp1 = new Slice.Edge[] {
          new Slice.Edge(new Vec3(0, 0, 0), new Vec3(1, 0, 0)),
          new Slice.Edge(new Vec3(1, 0, 0), new Vec3(1, 1, 0)),
          new Slice.Edge(new Vec3(1, 1, 0), new Vec3(0, 0, 0))
      };
        var obs1 = new Slice(new Geom2.Side[] { new Geom2.Side(new Vec2(0, 0), new Vec2(1, 0)),
          new Geom2.Side(new Vec2(1, 0), new Vec2(1, 1)), new Geom2.Side(new Vec2(1, 1), new Vec2(0, 0))});
        Assert.AreEqual(obs1.ToEdges(), exp1);
    }

    [Test]
    public void TestSliceTransform()
    {
        var identityMatrix = new Mat4(
          1, 0, 0, 0,
          0, 1, 0, 0,
          0, 0, 1, 0,
          0, 0, 0, 1
        );

        var org1 = new Slice(new List<Vec2>{new Vec2(0, 0), new Vec2(1, 0), new Vec2(1, 1)});
        var ret1 = org1.Transform(identityMatrix);
        Assert.AreNotSame(org1, ret1);

        var edges1 = ret1.ToEdges();
        var exp1 = new Slice.Edge[] {
          new Slice.Edge(new Vec3(1, 1, 0), new Vec3(0, 0, 0)),
          new Slice.Edge(new Vec3(0, 0, 0), new Vec3(1, 0, 0)),
          new Slice.Edge(new Vec3(1, 0, 0), new Vec3(1, 1, 0))
        };
        Assert.AreEqual(edges1, exp1);

        var x = 1;
        var y = 5;
        var z = 7;
        var translationMatrix = new Mat4(
          1, 0, 0, 0,
          0, 1, 0, 0,
          0, 0, 1, 0,
          x, y, z, 1
        );

        var org2 = new Slice(new List<Vec2>{new Vec2(0, 0), new Vec2(1, 0), new Vec2(1, 1)});
        var ret2 = org2.Transform(translationMatrix);
        Assert.AreNotSame(org2, ret2);

        var edges2 = ret2.ToEdges();
        var exp2 = new Slice.Edge[] {
          new Slice.Edge(new Vec3(2, 6, 7), new Vec3(1, 5, 7)),
          new Slice.Edge(new Vec3(1, 5, 7), new Vec3(2, 5, 7)),
          new Slice.Edge(new Vec3(2, 5, 7), new Vec3(2, 6, 7))
        };
        Assert.AreNotSame(edges2, exp2);

        var r = (90 * 0.017453292519943295);
        var rotateZMatrix = new Mat4(
          Math.Cos(r), -Math.Sin(r), 0, 0,
          Math.Sin(r), Math.Cos(r), 0, 0,
          0, 0, 1, 0,
          0, 0, 0, 1
        );

        var org3 = new Slice(new List<Vec2>{new Vec2(0, 0), new Vec2(1, 0), new Vec2(1, 1)});
        var ret3 = org3.Transform(rotateZMatrix);
        Assert.AreNotSame(org3, ret3);

        var edges3 = ret3.ToEdges();
        var exp3 = new Slice.Edge[] {
          new Slice.Edge(new Vec3(1, -0.9999999999999999, 0), new Vec3(0, 0, 0)),
          new Slice.Edge(new Vec3(0, 0, 0), new Vec3(6.123233995736766e-17, -1, 0)),
          new Slice.Edge(new Vec3(6.123233995736766e-17, -1, 0), new Vec3(1, -0.9999999999999999, 0))
        };
        Assert.AreEqual(edges3, exp3);
    }
}