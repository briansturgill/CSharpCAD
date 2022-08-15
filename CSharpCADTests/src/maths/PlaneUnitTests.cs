namespace CSharpCADTests;

[TestFixture]
public class PlaneTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestConstructors()
    {
        var plane1 = new Plane(new Vec3(0, 0, 0), 0);
        var ret1 = new Plane((plane1));
        Assert.IsTrue(plane1 == ret1);

        var plane2 = new Plane(new Vec3(1, 2, 3), 4);
        var ret2 = new Plane(plane2);
        Assert.IsTrue(plane2 == ret2);

        var plane3 = new Plane(new Vec3(-1, -2, -3), -4);
        var ret3 = new Plane(plane3);
        Assert.IsTrue(plane3 == ret3);
    }


    [Test]
    public void TestCopy()
    {
        var org1 = new Plane(new Vec3(), 0);
        var plane1 = new Plane();
        var ret1 = new Plane(plane1);
        Assert.IsTrue(org1 == plane1);
        Assert.IsTrue(ret1 == plane1);

        var plane2 = new Plane(new Vec3(1, 2, 3), 4);
        var ret2 = new Plane(plane2);
        Assert.IsTrue(ret2 == plane2);

        var plane3 = new Plane(new Vec3(-1, -2, -3), -4);
        var ret3 = new Plane(plane3);
        Assert.IsTrue(plane3 == ret3);
    }


    [Test]
    public void TestZeroConstructor()
    {
        var obs = new Plane();

        Assert.IsTrue(obs == new Plane(new Vec3(0, 0, 0), 0));
    }

    [Test]
    public void TestEqualityOps()
    {
        var plane0 = new Plane();
        var plane1 = new Plane();

        Assert.IsTrue(plane0 == new Plane(new Vec3(0, 0, 0), 0));
        Assert.IsTrue(plane0 == plane1);

        var plane2 = new Plane(new Vec3(1, 1, 1), 1);

        Assert.IsFalse(plane0.Equals(plane2));
        Assert.IsTrue(plane0 != plane2);

        var plane3 = new Plane(new Vec3(0, 1, 1), 0);

        Assert.IsFalse(plane0 == plane3);

        var plane4 = new Plane(new Vec3(0, 0, 1), 1);

        Assert.IsFalse(plane0 == plane4);
    }

    [Test]
    public void TestFlip()
    {
        var org1 = new Plane();
        var ret1 = org1.Flip();
        Assert.IsTrue(ret1 == new Plane(new Vec3(-0, -0, -0), -0));

        var ret2 = new Plane(new Vec3(1, 2, 3), 4).Flip();
        Assert.IsTrue(ret2 == new Plane(new Vec3(-1, -2, -3), -4));

        var ret3 = new Plane(new Vec3(-1, -2, -3), -4).Flip();
        Assert.IsTrue(ret3 == new Plane(new Vec3(1, 2, 3), 4));
    }


    [Test]
    public void TestFromNormalAndPointConstructor()
    {
        var obs1 = new Plane(new Vec3(5, 0, 0), new Vec3(0, 0, 0));
        Assert.IsTrue(obs1 == new Plane(new Vec3(1, 0, 0), 0));

        var obs2 = new Plane(new Vec3(0, 0, 5), new Vec3(5, 5, 5));
        Assert.IsTrue(obs2 == new Plane(new Vec3(0, 0, 1), 5));

    }

    [Test]
    public void TestConstructor3Points()
    {
        var obs1 = Plane.From3Points(new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(1, 1, 0));
        Assert.IsTrue(obs1 == new Plane(new Vec3(0, 0, 1), 0));

        var obs2 = Plane.From3Points(new Vec3(0, 6, 0), new Vec3(0, 2, 2), new Vec3(0, 6, 6));
        Assert.IsTrue(obs2 == new Plane(new Vec3(-1, 0, 0), 0));

        // planes created from the same points results in an invalid plane
#if TEST_SEEMS_FAULTY
        var obs3 = Plane.From3Points(new Vec3(0, 6, 0), new Vec3(0, 6, 0), new Vec3(0, 6, 0));
        Assert.IsTrue(obs3 == new Plane(new Vec3(double.NaN, double.NaN, double.NaN), double.NaN));
#endif

        // Apparently they handled more than three points too?
        // polygon with co-linear vertices
        //var obs4 = Plane.FromPoints(new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(2, 0, 0), new Vec3(0, 1, 0));
        //Assert.IsTrue(obs4 == new Plane(new Vec3(0, 0, 1), 0));
    }

    [Test]
    public void TestProjectionOfPoint()
    {
        var plane1 = new Plane(new Vec3(0, 0, 0), new Vec3(0, 0, 0));
        var point1 = plane1.ProjectionOfPoint(new Vec3(0, 0, 0));
        Assert.IsTrue(point1 == new Vec3(0, 0, 0));

        // axis aligned planes
        var plane2 = new Plane(new Vec3(0, 0, 1), new Vec3(0, 0, 0));
        var point2 = plane2.ProjectionOfPoint(new Vec3(1, 1, 1));
        Assert.IsTrue(point2 == new Vec3(1, 1, 0));

        var plane3 = new Plane(new Vec3(1, 0, 0), new Vec3(0, 0, 0));
        var point3 = plane3.ProjectionOfPoint(new Vec3(1, 1, 1));
        Assert.IsTrue(point3 == new Vec3(0, 1, 1));

        var plane4 = new Plane(new Vec3(0, 1, 0), new Vec3(0, 0, 0));
        var point4 = plane4.ProjectionOfPoint(new Vec3(1, 1, 1));
        Assert.IsTrue(point4 == new Vec3(1, 0, 1));

        // diagonal planes
        var plane5 = new Plane(new Vec3(1, 1, 1), new Vec3(0, 0, 0));
        var point5 = plane5.ProjectionOfPoint(new Vec3(0, 0, 0));
        Assert.IsTrue(point5 == new Vec3(0, 0, 0));

        var plane6 = new Plane(new Vec3(1, 1, 1), new Vec3(0, 0, 0));
        var point6 = plane6.ProjectionOfPoint(new Vec3(3, 3, 3));
        Assert.IsTrue(point6.IsNearlyEqual(new Vec3(0, 0, 0)));

        var plane7 = new Plane(new Vec3(1, 1, 1), new Vec3(0, 0, 0));
        var point7 = plane7.ProjectionOfPoint(new Vec3(-3, -3, -3));
        Assert.IsTrue(point7.IsNearlyEqual(new Vec3(0, 0, 0)));

        var plane8 = new Plane(new Vec3(1, 1, 1), new Vec3(0, 0, 0));
        var point8 = plane8.ProjectionOfPoint(new Vec3(0, 0, 0));
        Assert.IsTrue(point8 == new Vec3(0, 0, 0));
    }

    [Test]
    public void TestSignedDistanceToPoint()
    {
        var plane1 = new Plane(new Vec3(0, 0, 0), 0);
        var distance1 = plane1.SignedDistanceToPoint(new Vec3(0, 0, 0));
        Assert.IsTrue(distance1 == 0.0);

        var plane2 = new Plane(new Vec3(1, 1, 1), 1);
        var distance2 = plane2.SignedDistanceToPoint(new Vec3(-1, -1, -1));
        Assert.IsTrue(distance2 == (-3.0 - 1));

        var plane3 = new Plane(new Vec3(5, 5, 5), 5);
        var distance3 = plane3.SignedDistanceToPoint(new Vec3(5, 5, 5));
        Assert.IsTrue(distance3 == (75.0 - 5));

        var plane4 = new Plane(new Vec3(5, 5, 5), 5);
        var distance4 = plane4.SignedDistanceToPoint(new Vec3(-2, 3, -4));
        Assert.IsTrue(distance4 == (-15.0 - 5));
    }

    [Test]
    public void TestTransform()
    {
        var identityMatrix = new Mat4(
          1, 0, 0, 0,
          0, 1, 0, 0,
          0, 0, 1, 0,
          0, 0, 0, 1
        );
#if TEST_SEEMS_FAULTY
        var obs1 = new Plane(new Vec3(0, 0, 1), 0).Transform(identityMatrix);
        Assert.IsTrue(obs1.IsNearlyEqual(new Plane(new Vec3(0/0.0, 0/0.0, 0/0.0), 0/0.0)));
#endif

        var plane2 = new Plane(new Vec3(0, 0, -1), 0);
        var obs2 = plane2.Transform(identityMatrix);
        Assert.IsTrue(obs2 == new Plane(new Vec3(0, 0, -1), 0));

        var x = 1;
        var y = 5;
        var z = 7;
        var translationMatrix = new Mat4(
          1, 0, 0, 0,
          0, 1, 0, 0,
          0, 0, 1, 0,
          x, y, z, 1
        );

        var plane3 = new Plane(new Vec3(0, 0, 1), 0);
        var obs3 = plane3.Transform(translationMatrix);
        Assert.IsTrue(obs3 == new Plane(new Vec3(0, 0, 1), 7));

        var w = 1;
        var h = 3;
        var d = 5;
        var scaleMatrix = new Mat4(
          w, 0, 0, 0,
          0, h, 0, 0,
          0, 0, d, 0,
          0, 0, 0, 1
        );

        var plane4 = new Plane(new Vec3(0, -1, 0), 0);
        var obs4 = plane4.Transform(scaleMatrix);
        Assert.IsTrue(obs4 == new Plane(new Vec3(0, -1, 0), 0));

        var r = (90 * 0.017453292519943295);
        var rotateZMatrix = new Mat4(
          CosR(r), -SinR(r), 0, 0,
          SinR(r), CosR(r), 0, 0,
          0, 0, 1, 0,
          0, 0, 0, 1
        );

        var plane5 = new Plane(new Vec3(-1, 0, 0), 0);
        var obs5 = plane5.Transform(rotateZMatrix);
        Assert.IsTrue(obs5.IsNearlyEqual(new Plane(new Vec3(-0, 1, 0), 0)));

        var mirrorMatrix = new Mat4(
          -1, 0, 0, 0,
          0, 1, 0, 0,
          0, 0, 1, 0,
          0, 0, 0, 1
        );

        var plane6 = new Plane(new Vec3(1, 0, 0), 0);
        var obs6 = plane6.Transform(mirrorMatrix);
        Assert.IsTrue(obs6 == new Plane(new Vec3(-1, 0, 0), 0));
    }
}