namespace CSharpCADTests;

[TestFixture]
public class Poly3Tests
{
    public const double EPSILON = C.EPSILON; //2.220446049250313e-16f;
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestCloneAndConstructors()
    {
        var a = new Vec3[] { new Vec3(1, 1, 0), new Vec3(-1, 1, 0), new Vec3(-1, -1, 0), new Vec3(1, -1, 0) };
        var org1 = new Poly3(a);
        var ret1 = org1.Clone();
        Assert.IsTrue(ret1 == org1);
        Assert.IsFalse(Object.ReferenceEquals(ret1, org1));

        var l = new List<Vec3>(a.Length);
        foreach (var v in a)
        {
            l.Add(v);
        }
        var org2 = new Poly3(l);
        var ret2 = org2.Clone();
        Assert.IsTrue(ret2 == org2);
        Assert.IsFalse(Object.ReferenceEquals(ret2, org2));
    }


    [Test]
    public void TestInvert()
    {
        var exp1 = new Poly3(new Vec3[] { new Vec3(1, 1, 0), new Vec3(1, 0, 0), new Vec3(0, 0, 0) });
        var org1 = new Poly3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(1, 1, 0) });
        var ret1 = org1.Invert();
        Assert.IsTrue(ret1 == exp1);

        var exp2 = new Poly3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(1, 1, 0) });
        var org2 = new Poly3(new Vec3[] { new Vec3(1, 1, 0), new Vec3(1, 0, 0), new Vec3(0, 0, 0) });
        var ret2 = org2.Invert();
        Assert.IsTrue(ret2 == exp2);
        Assert.IsFalse(Object.ReferenceEquals(ret2, org2)); // the original should NOT change
    }

    [Test]
    public void TestIsConvex()
    {
        var ply2 = new Poly3(new Vec3[] { new Vec3(1, 1, 0), new Vec3(1, 0, 0), new Vec3(0, 0, 0) });
        Assert.IsTrue(ply2.IsConvex());

        var points2ccw = new Poly3(new Vec3[] { new Vec3(0, 0, 3), new Vec3(10, 10, 3), new Vec3(0, 5, 3) });
        Assert.IsTrue(points2ccw.IsConvex());

        var points2cw = new Poly3(new Vec3[] { new Vec3(0, 0, 3), new Vec3(-10, 10, 3), new Vec3(0, 5, 3) });
        Assert.IsTrue(points2cw.IsConvex());

        // V-shape
        var pointsV = new Poly3(new Vec3[] { new Vec3(0, 0, 3), new Vec3(-10, 10, 3), new Vec3(0, 5, 3), new Vec3(10, 10, 3) });
        Assert.IsFalse(pointsV.IsConvex());
    }


    [Test]
    public void TestArea()
    {
        var ply1 = new Poly3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(0, 0, 0), new Vec3(0, 0, 0) });
        var ret1 = ply1.Area();
        Assert.IsTrue(ret1 == 0.0);

        // simple triangle
        var ply2 = new Poly3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(0, 10, 0), new Vec3(0, 10, 10) });
        var ret2 = ply2.Area();
        Assert.IsTrue(ret2 == 50.0);

        // simple square
        var ply3 = new Poly3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(0, 10, 0), new Vec3(0, 10, 10), new Vec3(0, 0, 10) });
        var ret3 = ply3.Area();
        Assert.IsTrue(ret3 == 100.0);

        // V-shape
        var points = new Vec3[] {
          new Vec3(0, 3, 0),
          new Vec3(0, 5, 0),
          new Vec3(0, 8, 2),
          new Vec3(0, 6, 5),
          new Vec3(0, 8, 6),
          new Vec3(0, 5, 6),
          new Vec3(0, 5, 2),
          new Vec3(0, 2, 5),
          new Vec3(0, 1, 3),
          new Vec3(0, 3, 3)
        };
        var ply4 = new Poly3(points);
        var ret4 = ply4.Area();
        Assert.IsTrue(ret4 == 19.5);

        // colinear vertices non-zero area
        var ply5 = new Poly3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(2, 0, 0), new Vec3(0, 1, 0) });
        var ret5 = ply5.Area();
        Assert.IsTrue(Helpers.NearlyEqual(ret5, 1, EPSILON));

        // colinear vertices empty area
        var ply6 = new Poly3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(2, 0, 0) });
        var ret6 = ply6.Area();
        Assert.IsTrue(ret6 == 0);

        // duplicate vertices empty area
        var ply7 = new Poly3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(0, 0, 0), new Vec3(0, 0, 0) });
        var ret7 = ply7.Area();
        Assert.IsTrue(ret7 == 0);

        // rotated to various angles
        var rotation = Mat4.FromZRotation(45 * 0.017453292519943295);
        ply1 = ply1.Transform(rotation);
        ply2 = ply2.Transform(rotation);
        ply3 = ply3.Transform(rotation);
        ply4 = ply4.Transform(rotation);
        ret1 = ply1.Area();
        ret2 = ply2.Area();
        ret3 = ply3.Area();
        ret4 = ply4.Area();
        Helpers.NearlyEqual(ret1, 0.0, EPSILON);
        Helpers.NearlyEqual(ret2, 50.0, EPSILON);
        Helpers.NearlyEqual(ret3, 100.0, EPSILON);
        Helpers.NearlyEqual(ret4, 19.5, EPSILON);

        rotation = Mat4.FromYRotation(45 * 0.017453292519943295);
        ply1 = ply1.Transform(rotation);
        ply2 = ply2.Transform(rotation);
        ply3 = ply3.Transform(rotation);
        ply4 = ply4.Transform(rotation);
        ret1 = ply1.Area();
        ret2 = ply2.Area();
        ret3 = ply3.Area();
        ret4 = ply4.Area();
        Helpers.NearlyEqual(ret1, 0.0, EPSILON);
        Helpers.NearlyEqual(ret2, 50.0, EPSILON);
        Helpers.NearlyEqual(ret3, 100.0, EPSILON);
        Helpers.NearlyEqual(ret4, 19.5, EPSILON);

        rotation = Mat4.FromXRotation(45 * 0.017453292519943295);
        ply1 = ply1.Transform(rotation);
        ply2 = ply2.Transform(rotation);
        ply3 = ply3.Transform(rotation);
        ply4 = ply4.Transform(rotation);
        ret1 = ply1.Area();
        ret2 = ply2.Area();
        ret3 = ply3.Area();
        ret4 = ply4.Area();
        Helpers.NearlyEqual(ret1, 0.0, EPSILON);
        Helpers.NearlyEqual(ret2, 50.0, EPSILON);
        Helpers.NearlyEqual(ret3, 100.0, EPSILON);
        Helpers.NearlyEqual(ret4, 19.5, EPSILON);

        // inverted
        ply1 = ply1.Invert();
        ply2 = ply2.Invert();
        ply3 = ply3.Invert();
        ply4 = ply4.Invert();
        ret1 = ply1.Area();
        ret2 = ply2.Area();
        ret3 = ply3.Area();
        ret4 = ply4.Area();
        Helpers.NearlyEqual(ret1, 0.0, EPSILON);
        Helpers.NearlyEqual(ret2, 50.0, EPSILON);
        Helpers.NearlyEqual(ret3, 100.0, EPSILON);
        Helpers.NearlyEqual(ret4, 19.5, EPSILON);
    }


    [Test]
    public void TestBoundingBox()
    {
        var ply1 = new Poly3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(0, 0, 0) });
        var exp1a = new Vec3(0, 0, 0);
        var exp1b = new Vec3(0, 0, 0);
        var (ret1a, ret1b) = ply1.BoundingBox();
        Assert.IsTrue(ret1a == exp1a);
        Assert.IsTrue(ret1b == exp1b);

        // simple triangle
        var ply2 = new Poly3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(0, 10, 0), new Vec3(0, 10, 10) });
        var exp2a = new Vec3(0, 0, 0);
        var exp2b = new Vec3(0, 10, 10);
        var (ret2a, ret2b) = ply2.BoundingBox();
        Assert.IsTrue(ret2a == exp2a);
        Assert.IsTrue(ret2b == exp2b);

        // simple square
        var ply3 = new Poly3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(0, 10, 0), new Vec3(0, 10, 10), new Vec3(0, 0, 10) });
        var exp3a = new Vec3(0, 0, 0);
        var exp3b = new Vec3(0, 10, 10);
        var (ret3a, ret3b) = ply3.BoundingBox();
        Assert.IsTrue(ret3a == exp3a);
        Assert.IsTrue(ret3b == exp3b);

        // V-shape
        var points = new Vec3[] {
          new Vec3(0, 3, 0),
          new Vec3(0, 5, 0),
          new Vec3(0, 8, 2),
          new Vec3(0, 6, 5),
          new Vec3(0, 8, 6),
          new Vec3(0, 5, 6),
          new Vec3(0, 5, 2),
          new Vec3(0, 2, 5),
          new Vec3(0, 1, 3),
          new Vec3(0, 3, 3)
        };
        var ply4 = new Poly3(points);
        var exp4a = new Vec3(0, 1, 0);
        var exp4b = new Vec3(0, 8, 6);
        var (ret4a, ret4b) = ply4.BoundingBox();
        Assert.IsTrue(ret4a == exp4a);
        Assert.IsTrue(ret4b == exp4b);

        // rotated to various angles
        var rotation = Mat4.FromZRotation(45 * 0.017453292519943295);
        ply1 = ply1.Transform(rotation);
        ply2 = ply2.Transform(rotation);
        ply3 = ply3.Transform(rotation);
        ply4 = ply4.Transform(rotation);
        (ret1a, ret1b) = ply1.BoundingBox();
        (ret2a, ret2b) = ply2.BoundingBox();
        (ret3a, ret3b) = ply3.BoundingBox();
        (ret4a, ret4b) = ply4.BoundingBox();
        exp1a = new Vec3(0, 0, 0);
        exp1b = new Vec3(0, 0, 0);
        Assert.IsTrue(ret1a == exp1a);
        Assert.IsTrue(ret1b == exp1b);
        exp2a = new Vec3(-7.071067811865475, 0, 0);
        exp2b = new Vec3(0, 7.0710678118654755, 10);
        Assert.IsTrue(ret2a == exp2a);
        Assert.IsTrue(ret2b == exp2b);
        exp3a = new Vec3(-7.071067811865475, 0, 0);
        exp3b = new Vec3(0, 7.0710678118654755, 10);
        Assert.IsTrue(ret3a == exp3a);
        Assert.IsTrue(ret3b == exp3b);
        exp4a = new Vec3(-5.65685424949238, 0.7071067811865476, 0);
        exp4b = new Vec3(-0.7071067811865475, 5.656854249492381, 6);
        Assert.IsTrue(ret4a == exp4a);
        Assert.IsTrue(ret4b == exp4b);
    }


    [Test]
    public void TestBoundingSphere()
    {
        var ply1 = new Poly3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(0, 0, 0), new Vec3(0, 0, 0) });
        var exp1a = new Vec3(0, 0, 0);
        var exp1b = 0.0;
        var (ret1a, ret1b) = ply1.BoundingSphere();
        Assert.IsTrue(ret1a == exp1a);
        Helpers.NearlyEqual(ret1b, exp1b, EPSILON);

        // simple triangle
        var ply2 = new Poly3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(0, 10, 0), new Vec3(0, 10, 10) });
        var exp2a = new Vec3(0, 5, 5);
        var exp2b = 7.0710678118654755;
        var (ret2a, ret2b) = ply2.BoundingSphere();
        Assert.IsTrue(ret2a == exp2a);
        Helpers.NearlyEqual(ret2b, exp2b, EPSILON);

        // simple square
        var ply3 = new Poly3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(0, 10, 0), new Vec3(0, 10, 10), new Vec3(0, 0, 10) });
        var exp3a = new Vec3(0, 5, 5);
        var exp3b = 7.0710678118654755;
        var (ret3a, ret3b) = ply3.BoundingSphere();
        Assert.IsTrue(ret3a == exp3a);
        Helpers.NearlyEqual(ret3b, exp3b, EPSILON);

        // V-shape
        var points = new Vec3[] {
          new Vec3(0, 3, 0),
          new Vec3(0, 5, 0),
          new Vec3(0, 8, 2),
          new Vec3(0, 6, 5),
          new Vec3(0, 8, 6),
          new Vec3(0, 5, 6),
          new Vec3(0, 5, 2),
          new Vec3(0, 2, 5),
          new Vec3(0, 1, 3),
          new Vec3(0, 3, 3)
        };
        var ply4 = new Poly3(points);
        var exp4a = new Vec3(0, 4.5, 3);
        var exp4b = 4.6097722286464435;
        var (ret4a, ret4b) = ply4.BoundingSphere();
        Assert.IsTrue(ret4a == exp4a);
        Helpers.NearlyEqual(ret4b, exp4b, EPSILON);

        // rotated to various angles
        var rotation = Mat4.FromZRotation(45 * 0.017453292519943295);
        ply1 = ply1.Transform(rotation);
        ply2 = ply2.Transform(rotation);
        ply3 = ply3.Transform(rotation);
        ply4 = ply4.Transform(rotation);
        (ret1a, ret1b) = ply1.BoundingSphere();
        (ret2a, ret2b) = ply2.BoundingSphere();
        (ret3a, ret3b) = ply3.BoundingSphere();
        (ret4a, ret4b) = ply4.BoundingSphere();
        exp1a = new Vec3(0, 0, 0);
        exp1b = 0.0;
        Assert.IsTrue(ret1a == exp1a);
        Helpers.NearlyEqual(ret1b, exp1b, EPSILON);
        exp2a = new Vec3(-3.5355339059327373, 3.5355339059327378, 5);
        exp2b = 7.0710678118654755;
        Assert.IsTrue(ret2a == exp2a);
        Helpers.NearlyEqual(ret2b, exp2b, EPSILON);
        exp3a = new Vec3(-3.5355339059327373, 3.5355339059327378, 5);
        exp3b = 7.0710678118654755;
        Assert.IsTrue(ret3a == exp3a);
        Helpers.NearlyEqual(ret3b, exp3b, EPSILON);
        exp4a = new Vec3(-3.181980515339464, 3.1819805153394642, 3);
        exp4b = 4.6097722286464435;
        Assert.IsTrue(ret4a == exp4a);
        Helpers.NearlyEqual(ret4b, exp4b, EPSILON);
    }

    [Test]
    public void TestSignedVolume()
    {
        var ply1 = new Poly3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(0, 0, 0), new Vec3(0, 0, 0) });
        var ret1 = ply1.SignedVolume();
        Helpers.NearlyEqual(ret1, 0.0, EPSILON);

        // simple triangle
        var ply2 = new Poly3(new Vec3[] { new Vec3(5, 5, 5), new Vec3(5, 15, 5), new Vec3(5, 15, 15) });
        var ret2 = ply2.SignedVolume();
        Helpers.NearlyEqual(ret2, 83.33333333333333, EPSILON);

        // simple square
        var ply3 = new Poly3(new Vec3[] { new Vec3(5, 5, 5), new Vec3(5, 15, 5), new Vec3(5, 15, 15), new Vec3(5, 5, 15) });
        var ret3 = ply3.SignedVolume();
        Helpers.NearlyEqual(ret3, 166.66666666666666, EPSILON);

        // V-shape
        var points = new Vec3[] {
          new Vec3(-50, 3, 0),
          new Vec3(-50, 5, 0),
          new Vec3(-50, 8, 2),
          new Vec3(-50, 6, 5),
          new Vec3(-50, 8, 6),
          new Vec3(-50, 5, 6),
          new Vec3(-50, 5, 2),
          new Vec3(-50, 2, 5),
          new Vec3(-50, 1, 3),
          new Vec3(-50, 3, 3)
        };
        var ply4 = new Poly3(points);
        var ret4 = ply4.SignedVolume();
        Helpers.NearlyEqual(ret4, -325.00000, EPSILON);

        // rotated to various angles
        var rotation = Mat4.FromZRotation(45 * 0.017453292519943295);
        ply1 = ply1.Transform(rotation);
        ply2 = ply2.Transform(rotation);
        ply3 = ply3.Transform(rotation);
        ply4 = ply4.Transform(rotation);
        ret1 = ply1.SignedVolume();
        ret2 = ply2.SignedVolume();
        ret3 = ply3.SignedVolume();
        ret4 = ply4.SignedVolume();
        Helpers.NearlyEqual(ret1, 0.0, EPSILON);
        Helpers.NearlyEqual(ret2, 83.33333333333331, EPSILON);
        Helpers.NearlyEqual(ret3, 166.66666666666663, EPSILON);
        Helpers.NearlyEqual(ret4, -324.9999999999994, EPSILON);

        // inverted (opposite rotation, normal)
        ply2 = ply2.Invert();
        ply3 = ply3.Invert();
        ply4 = ply4.Invert();
        ret2 = ply2.SignedVolume();
        ret3 = ply3.SignedVolume();
        ret4 = ply4.SignedVolume();
        Helpers.NearlyEqual(ret2, -83.33333333333331, EPSILON);
        Helpers.NearlyEqual(ret3, -166.66666666666663, EPSILON);
        Helpers.NearlyEqual(ret4, 324.9999999999994, EPSILON);
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

        var exp1 = new Poly3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(1, 1, 0) });
        var org1 = new Poly3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(1, 1, 0) });
        var ret1 = org1.Transform(identityMatrix);
        Assert.IsTrue(ret1 == exp1);
        Assert.IsFalse(Object.ReferenceEquals(org1, ret1));

        var x = 1;
        var y = 5;
        var z = 7;
        var translationMatrix = new Mat4(
          1, 0, 0, 0,
          0, 1, 0, 0,
          0, 0, 1, 0,
          x, y, z, 1
        );

        var exp2 = new Poly3(new Vec3[] { new Vec3(1, 5, 7), new Vec3(2, 5, 7), new Vec3(2, 6, 7) });
        var org2 = new Poly3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(1, 1, 0) });
        var ret2 = org2.Transform(translationMatrix);
        Assert.IsTrue(ret2 == exp2);
        Assert.IsFalse(Object.ReferenceEquals(org2, ret2));

        var r = (90 * 0.017453292519943295);
        var rotateZMatrix = new Mat4(
          Math.Cos(r), -Math.Sin(r), 0, 0,
          Math.Sin(r), Math.Cos(r), 0, 0,
          0, 0, 1, 0,
          0, 0, 0, 1
        );

        var exp3 = new Poly3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(0, -1, 0), new Vec3(1, -1, 0) });
        var org3 = new Poly3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(1, 1, 0) });
        var ret3 = org3.Transform(rotateZMatrix);
        Assert.IsTrue(ret3.IsNearlyEqual(exp3));
        Assert.IsFalse(Object.ReferenceEquals(org3, ret3));

        var mirrorMatrix = new Mat4(
          -1, 0, 0, 0,
          0, 1, 0, 0,
          0, 0, 1, 0,
          0, 0, 0, 1
        );

        var exp4 = new Poly3(new Vec3[] { new Vec3(-1, 1, 0), new Vec3(-1, 0, 0), new Vec3(0, 0, 0) });
        var org4 = new Poly3(new Vec3[] { new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(1, 1, 0) });
        var ret4 = org4.Transform(mirrorMatrix);
        Assert.IsTrue(ret4.IsNearlyEqual(exp4));
        Assert.IsFalse(Object.ReferenceEquals(org4, ret4));
    }

}