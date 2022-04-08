namespace CSharpCADTests;

[TestFixture]
public class Mat4Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestConstructors()
    {
        // Tests fromValues constructor, clone constructor, and identity matrix constructor.
        var org1 = new Mat4();
        var obs1 = new Mat4(org1);
        Assert.IsTrue(obs1 == new Mat4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1));

        var org2 = new Mat4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        var obs2 = new Mat4(org2);
        Assert.IsTrue(obs2 == new Mat4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));

        var org3 = new Mat4(-1, -2, -3, -4, -5, -6, -7, -8, -9, -10, -11, -12, -13, -14, -15, -16);
        var obs3 = new Mat4(org3);
        Assert.IsTrue(obs3 == new Mat4(-1, -2, -3, -4, -5, -6, -7, -8, -9, -10, -11, -12, -13, -14, -15, -16));

        Assert.IsTrue(org2 != org3); // Quick check of != operator.
    }

    [Test]
    public void TestAdd()
    {
        var ret1 = new Mat4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).Add(
            new Mat4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));
        Assert.IsTrue(ret1 == new Mat4(2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32));

        var ret2 = new Mat4(-1, -2, -3, -4, -5, -6, -7, -8, -9, -10, -11, -12, -13, -14, -15, -16).Add(
            new Mat4(-1, -2, -3, -4, -5, -6, -7, -8, -9, -10, -11, -12, -13, -14, -15, -16));
        Assert.IsTrue(ret2 == new Mat4(-2, -4, -6, -8, -10, -12, -14, -16, -18, -20, -22, -24, -26, -28, -30, -32));

        var ret3 = new Mat4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).Add(
            new Mat4(-1, -2, -3, -4, -5, -6, -7, -8, -9, -10, -11, -12, -13, -14, -15, -16));
        Assert.IsTrue(ret3 == new Mat4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
    }

    [Test]
    public void TestFromRotation()
    {
        var rotation = 90 * 0.017453292519943295;

        // invalid condition when axis is 0,0,0
        var obs1 = Mat4.FromRotation(rotation, new Vec3(0, 0, 0));
        Assert.IsTrue(obs1.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)));

        var obs2 = Mat4.FromRotation(rotation, new Vec3(0, 0, 1));
        Assert.IsTrue(obs2.IsNearlyEqual(new Mat4(0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)));

        var obs3 = Mat4.FromRotation(-rotation, new Vec3(0, 0, 1));
        Assert.IsTrue(obs3.IsNearlyEqual(new Mat4(0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)));
    }

    [Test]
    public void TestFromScaling()
    {
        var obs1 = Mat4.FromScaling(new Vec3(2, 4, 6));
        Assert.IsTrue(obs1.IsNearlyEqual(new Mat4(2, 0, 0, 0, 0, 4, 0, 0, 0, 0, 6, 0, 0, 0, 0, 1)));

        var obs2 = Mat4.FromScaling(new Vec3(-2, -4, -6));
        Assert.IsTrue(obs2.IsNearlyEqual(new Mat4(-2, 0, 0, 0, 0, -4, 0, 0, 0, 0, -6, 0, 0, 0, 0, 1)));
    }

    [Test]
    public void TestFromTaitBryanRotation()
    {
        var rotation = 90 * 0.017453292519943295;

        // rotation using YAW / Z
        var obs1 = Mat4.FromTaitBryanRotation(rotation, 0, 0);
        Assert.IsTrue(obs1.IsNearlyEqual(new Mat4(0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)));

        // rotation using PITCH / Y
        var obs2 = Mat4.FromTaitBryanRotation(0, rotation, 0);
        Assert.IsTrue(obs2.IsNearlyEqual(new Mat4(0, 0, -1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1)));

        // rotation using ROLL / X
        var obs3 = Mat4.FromTaitBryanRotation(0, 0, rotation);
        Assert.IsTrue(obs3.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, 0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 1)));

        var obs4 = Mat4.FromTaitBryanRotation(-rotation, -rotation, -rotation);
        Assert.IsTrue(obs4.IsNearlyEqual(new Mat4(0, 0, 1, 0, 0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1)));
    }

    [Test]
    public void TestFromTranslation()
    {
        var obs1 = Mat4.FromTranslation(new Vec3(2, 4, 6));
        Assert.IsTrue(obs1.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 2, 4, 6, 1)));

        var obs2 = Mat4.FromTranslation(new Vec3(-2, -4, -6));
        Assert.IsTrue(obs2.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, -2, -4, -6, 1)));
    }

    [Test]
    public void TestFromVectorRotation()
    {
        // unit vectors, same directions
        var ret = Mat4.FromVectorRotation(new Vec3(1, 0, 0), new Vec3(1, 0, 0));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)));

        ret = Mat4.FromVectorRotation(new Vec3(0, 1, 0), new Vec3(0, 1, 0));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)));

        ret = Mat4.FromVectorRotation(new Vec3(0, 0, 1), new Vec3(0, 0, 1));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)));

        // unit vectors, axis rotations
        ret = Mat4.FromVectorRotation(new Vec3(1, 0, 0), new Vec3(0, 1, 0));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)));

        ret = Mat4.FromVectorRotation(new Vec3(1, 0, 0), new Vec3(0, 0, 1));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(0, 0, 1, 0, 0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 0, 1)));

        ret = Mat4.FromVectorRotation(new Vec3(0, 1, 0), new Vec3(1, 0, 0));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)));

        ret = Mat4.FromVectorRotation(new Vec3(0, 1, 0), new Vec3(0, 0, 1));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, 0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 1)));

        ret = Mat4.FromVectorRotation(new Vec3(0, 0, 1), new Vec3(1, 0, 0));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(0, 0, -1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1)));

        ret = Mat4.FromVectorRotation(new Vec3(0, 0, 1), new Vec3(0, 1, 0));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, 0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1)));

        ret = Mat4.FromVectorRotation(new Vec3(-1, 0, 0), new Vec3(0, -1, 0));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)));

        ret = Mat4.FromVectorRotation(new Vec3(-1, 0, 0), new Vec3(0, 0, -1));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(0, 0, 1, 0, 0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 0, 1)));

        ret = Mat4.FromVectorRotation(new Vec3(0, -1, 0), new Vec3(-1, 0, 0));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)));

        ret = Mat4.FromVectorRotation(new Vec3(0, -1, 0), new Vec3(0, 0, -1));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, 0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 1)));

        ret = Mat4.FromVectorRotation(new Vec3(0, 0, -1), new Vec3(-1, 0, 0));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(0, 0, -1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1)));

        ret = Mat4.FromVectorRotation(new Vec3(0, 0, -1), new Vec3(0, -1, 0));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, 0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1)));

        // unit vector, opposite directions
        ret = Mat4.FromVectorRotation(new Vec3(1, 0, 0), new Vec3(-1, 0, 0));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(-1, 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0, 0, 0, 0, 1)));

        ret = Mat4.FromVectorRotation(new Vec3(-1, 0, 0), new Vec3(1, 0, 0));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(-1, 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0, 0, 0, 0, 1)));

        ret = Mat4.FromVectorRotation(new Vec3(0, 1, 0), new Vec3(0, -1, 0));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, -1, 0, 0, 0, 0, -1, 0, 0, 0, 0, 1)));

        ret = Mat4.FromVectorRotation(new Vec3(0, -1, 0), new Vec3(0, 1, 0));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, -1, 0, 0, 0, 0, -1, 0, 0, 0, 0, 1)));

        ret = Mat4.FromVectorRotation(new Vec3(0, 0, 1), new Vec3(0, 0, -1));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, -1, 0, 0, 0, 0, -1, 0, 0, 0, 0, 1)));

        ret = Mat4.FromVectorRotation(new Vec3(0, 0, -1), new Vec3(0, 0, 1));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, -1, 0, 0, 0, 0, -1, 0, 0, 0, 0, 1)));

        // differnt units
        ret = Mat4.FromVectorRotation(new Vec3(11, 0, 0), new Vec3(0, 33, 0));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)));

        ret = Mat4.FromVectorRotation(new Vec3(0, 0.11, 0), new Vec3(0, 0, 0.33));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, 0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 1)));

        ret = Mat4.FromVectorRotation(new Vec3(0, 0, 111111.0), new Vec3(0, 0.33, 0));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, 0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1)));

        // different quadrants
        ret = Mat4.FromVectorRotation(new Vec3(0.5, 0.5, 0.5), new Vec3(-0.5, 0.5, 0.5));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(0.3333333333333334, 0.6666666666666669, 0.6666666666666669, 0, -0.6666666666666669, 0.666666666666667, -0.3333333333333335, 0, -0.6666666666666669, -0.3333333333333335, 0.666666666666667, 0, 0, 0, 0, 1)));
        Assert.IsTrue(new Vec3(-0.5, 0.5, 0.5).IsNearlyEqual(new Vec3(0.5, 0.5, 0.5).Transform(ret)));

        ret = Mat4.FromVectorRotation(new Vec3(5, 5, 5), new Vec3(5, 5, -5));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(0.6666666666666666, -0.3333333333333333, -0.6666666666666666, 0, -0.3333333333333333, 0.6666666666666666, -0.6666666666666666, 0, 0.6666666666666666, 0.6666666666666666, 0.3333333333333333, 0, 0, 0, 0, 1)));
        Assert.IsTrue(new Vec3(5, 5, -5).IsNearlyEqual(new Vec3(5, 5, 5).Transform(ret)));

        ret = Mat4.FromVectorRotation(new Vec3(5, 5, 5), new Vec3(-5, -5, -5));
        Assert.IsTrue(ret.IsNearlyEqual(new Mat4(0, -1, 0, 0, -1, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 1)));
        Assert.IsTrue(new Vec3(-5, -5, -5).IsNearlyEqual(new Vec3(5, 5, 5).Transform(ret)));
    }

    [Test]
    public void TestFromXRotation()
    {
        var rotation = 90 * 0.017453292519943295;

        var obs2 = Mat4.FromXRotation(rotation);
        Assert.IsTrue(obs2.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, 0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 1)));

        var obs3 = Mat4.FromXRotation(-rotation);
        Assert.IsTrue(obs3.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, 0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1)));
    }

    [Test]
    public void TestFromYRotation()
    {
        var rotation = 90 * 0.017453292519943295;

        var obs2 = Mat4.FromYRotation(rotation);
        Assert.IsTrue(obs2.IsNearlyEqual(new Mat4(0, 0, -1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1)));

        var obs3 = Mat4.FromYRotation(-rotation);
        Assert.IsTrue(obs3.IsNearlyEqual(new Mat4(0, 0, 1, 0, 0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 0, 1)));
    }

    [Test]
    public void TestFromZRotation()
    {
        var rotation = 90 * 0.017453292519943295;

        var obs2 = Mat4.FromZRotation(rotation);
        Assert.IsTrue(obs2.IsNearlyEqual(new Mat4(0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)));
        var obs3 = Mat4.FromZRotation(-rotation);
        Assert.IsTrue(obs3.IsNearlyEqual(new Mat4(0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)));
    }

    [Test]
    public void TestInvert()
    {
        // mat4: invert() translate
        var matrix = Mat4.FromTranslation(new Vec3(10, 10, 0));
        var matrixInv = matrix.Invert();

        var vec1 = new Vec3(0, 0, 0);
        var vec2 = vec1.Transform(matrix);
        Assert.IsTrue(vec2.IsNearlyEqual(new Vec3(10, 10, 0)));

        var vec2back = vec2.Transform(matrixInv);
        Assert.IsTrue(vec2back.IsNearlyEqual(vec1));

        // mat4: invert() rotate
        matrix = Mat4.FromXRotation(Math.PI / 2);
        matrixInv = matrix.Invert();

        vec1 = new Vec3(10, 10, 10);
        vec2 = vec1.Transform(matrix);
        Assert.IsTrue(vec2.IsNearlyEqual(new Vec3(10, -10, 10)));

        vec2back = vec2.Transform(matrixInv);
        Assert.IsTrue(vec2back.IsNearlyEqual(vec1));
    }

    [Test]
    public void TestIsIdentity()
    {
        Assert.IsTrue(new Mat4().IsIdentity());

        var notidentity = Mat4.FromTranslation(new Vec3(5, 5, 5));
        Assert.IsFalse(notidentity.IsIdentity());
    }

    [Test]
    public void TestIsMirroring()
    {
        var matrix = new Mat4();
        Assert.IsFalse(matrix.IsMirroring());

        matrix = Mat4.FromScaling(new Vec3(2, 4, 6));
        Assert.IsFalse(matrix.IsMirroring());

        // additional transforms
        // Original tests did some stuff with planes, which confuse the testing... removed.
        // So for the rest of the tests, we need to start with this matrix.
        matrix = new Mat4(1, -0, -0, 0, -0, 1, -0, 0, -0, -0, -1, 0, 0, 0, 0, 1);
        var rotation = 90 * 0.017453292519943295;
        matrix = matrix.Rotate(rotation, new Vec3(0, 0, 1));
        Assert.IsTrue(matrix.IsMirroring());

        matrix = matrix.Scale(new Vec3(0.5, 2, 5));
        Assert.IsTrue(matrix.IsMirroring());

        matrix = matrix.Translate(new Vec3(2, -3, 600));
        Assert.IsTrue(matrix.IsMirroring());
    }

    [Test]
    public void TestIsOnlyTransformScaleRightAngles()
    {
        // IsOnlyTransformScale() should return true for right angles
        var someRotation = Mat4.FromTaitBryanRotation(Math.PI, 0, 0);
        Assert.IsTrue(someRotation.IsOnlyTransformScale());
        Assert.IsTrue(someRotation.Invert().IsOnlyTransformScale());

        someRotation = Mat4.FromTaitBryanRotation(0, 0, 0);
        Assert.IsTrue(someRotation.IsOnlyTransformScale());
    }

    [Test]
    public void TestIsOnlyTransformScale()
    {
        var identity = new Mat4();
        Assert.IsTrue(identity.IsOnlyTransformScale());

        var someTranslation = Mat4.FromTranslation(new Vec3(5, 5, 5));
        Assert.IsTrue(someTranslation.IsOnlyTransformScale());

        var someScaling = Mat4.FromScaling(new Vec3(5, 5, 5));
        Assert.IsTrue(someScaling.IsOnlyTransformScale());
        Assert.IsTrue(someScaling.Invert().IsOnlyTransformScale());

        var combined = someTranslation.Multiply(someScaling);
        Assert.IsTrue(combined.IsOnlyTransformScale());
    }

    [Test]
    public void TestMirrorByPlane()
    {
        var planeX = Plane.From3Points(new Vec3(0, 0, 0), new Vec3(0, 1, 1), new Vec3(0, 1, 0));
        var planeY = Plane.From3Points(new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(1, 0, 1));
        var planeZ = Plane.From3Points(new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(1, 1, 0));

        var ret1 = Mat4.MirrorByPlane(planeX);
        Assert.IsTrue(ret1 == new Mat4(-1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1));;

        var ret2 = Mat4.MirrorByPlane(planeY);
        Assert.IsTrue(ret2 == new Mat4(1, 0, 0, 0, 0, -1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1));;

        var ret3 = Mat4.MirrorByPlane(planeZ);
        Assert.IsTrue(ret3 == new Mat4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0, 0, 0, 0, 1));;
    }

    [Test]
    public void TestMultiply()
    {
        var ret1 = new Mat4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).Multiply(
              new Mat4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));
        Assert.IsTrue(ret1 == new Mat4(90, 100, 110, 120, 202, 228, 254, 280, 314, 356, 398, 440, 426, 484, 542, 600));

        var ret2 = new Mat4(-1, -2, -3, -4, -5, -6, -7, -8, -9, -10, -11, -12, -13, -14, -15, -16).Multiply(
          new Mat4(-1, -2, -3, -4, -5, -6, -7, -8, -9, -10, -11, -12, -13, -14, -15, -16));
        Assert.IsTrue(ret2 == new Mat4(90, 100, 110, 120, 202, 228, 254, 280, 314, 356, 398, 440, 426, 484, 542, 600));

        var ret3 = new Mat4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).Multiply(
          new Mat4(-1, -2, -3, -4, -5, -6, -7, -8, -9, -10, -11, -12, -13, -14, -15, -16));
        Assert.IsTrue(ret3 == new Mat4(-90, -100, -110, -120, -202, -228, -254, -280, -314, -356, -398, -440, -426, -484, -542, -600));
    }

    [Test]
    public void TestRotate()
    {
        var rotation = 90 * 0.017453292519943295;
        var idn = new Mat4();

        // invalid condition when axis is 0,0,0
        var ret1 = idn.Rotate(rotation, new Vec3(0, 0, 0));
        Assert.IsTrue(ret1.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)));

        var ret2 = idn.Rotate(rotation, new Vec3(0, 0, 1));
        Assert.IsTrue(ret2.IsNearlyEqual(new Mat4(0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)));

        var ret3 = idn.Rotate(-rotation, new Vec3(0, 0, 1));
        Assert.IsTrue(ret3.IsNearlyEqual(new Mat4(0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)));
    }

    [Test]
    public void TestRotateX()
    {
        var rotation = 90 * 0.017453292519943295;
        var idn = new Mat4();

        var ret2 = idn.RotateX(rotation);
        Assert.IsTrue(ret2.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, 0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 1)));

        var ret3 = idn.RotateX(-rotation);
        Assert.IsTrue(ret3.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, 0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1)));
    }

    [Test]
    public void TestRotateY()
    {
        var rotation = 90 * 0.017453292519943295;

        var idn = new Mat4();

        var ret2 = idn.RotateY(rotation);
        Assert.IsTrue(ret2.IsNearlyEqual(new Mat4(0, 0, -1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1)));

        var ret3 = idn.RotateY(-rotation);
        Assert.IsTrue(ret3.IsNearlyEqual(new Mat4(0, 0, 1, 0, 0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 0, 1)));
    }

    [Test]
    public void TestRotateZ()
    {
        var rotation = 90 * 0.017453292519943295;

        var idn = new Mat4();

        var ret2 = idn.RotateZ(rotation);
        Assert.IsTrue(ret2.IsNearlyEqual(new Mat4(0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)));

        var ret3 = idn.RotateZ(-rotation);
        Assert.IsTrue(ret3.IsNearlyEqual(new Mat4(0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)));
    }

    [Test]
    public void TestScale()
    {
        var ret1 = new Mat4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).Scale(new Vec3(1, 1, 1));
        Assert.IsTrue(ret1.IsNearlyEqual(new Mat4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)));


        var ret2 = new Mat4(-1, -2, -3, -4, -5, -6, -7, -8, -9, -10, -11, -12, -13, -14, -15, -16).Scale(new Vec3(2, 4, 6));
        Assert.IsTrue(ret2.IsNearlyEqual(new Mat4(-2, -4, -6, -8, -20, -24, -28, -32, -54, -60, -66, -72, -13, -14, -15, -16)));


        var ret3 = new Mat4(-1, -2, -3, -4, -5, -6, -7, -8, -9, -10, -11, -12, -13, -14, -15, -16).Scale(new Vec3(6, 4, 2));
        Assert.IsTrue(ret3.IsNearlyEqual(new Mat4(-6, -12, -18, -24, -20, -24, -28, -32, -18, -20, -22, -24, -13, -14, -15, -16)));
    }

    [Test]
    public void TestSubtract()
    {
        var ret1 = new Mat4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).Subtract(
          new Mat4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));
        Assert.IsTrue(ret1.IsNearlyEqual(new Mat4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)));


        var ret2 = new Mat4(-1, -2, -3, -4, -5, -6, -7, -8, -9, -10, -11, -12, -13, -14, -15, -16).Subtract(
          new Mat4(-1, -2, -3, -4, -5, -6, -7, -8, -9, -10, -11, -12, -13, -14, -15, -16));
        Assert.IsTrue(ret2.IsNearlyEqual(new Mat4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)));


        var ret3 = new Mat4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).Subtract(
            new Mat4(-1, -2, -3, -4, -5, -6, -7, -8, -9, -10, -11, -12, -13, -14, -15, -16));
        Assert.IsTrue(ret3.IsNearlyEqual(new Mat4(2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32)));
    }

    [Test]
    public void TestTranslate()
    {
        var identityMatrix = new Mat4(
          1, 0, 0, 0,
          0, 1, 0, 0,
          0, 0, 1, 0,
          0, 0, 0, 1
        );

        var ret1 = identityMatrix.Translate(new Vec3(0, 0, 0));
        Assert.IsTrue(ret1.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)));

        var ret2 = identityMatrix.Translate(new Vec3(2, 3, 6));
        Assert.IsTrue(ret2.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 2, 3, 6, 1)));

        var x = 1;
        var y = 5;
        var z = 7;
        var translationMatrix = new Mat4(
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            x, y, z, 1
        );

        var ret3 = translationMatrix.Translate(new Vec3(-2, -3, -6));
        Assert.IsTrue(ret3.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, -1, 2, 1, 1)));

        var w = 1;
        var h = 3;
        var d = 5;
        var scaleMatrix = new Mat4(
          w, 0, 0, 0,
          0, h, 0, 0,
          0, 0, d, 0,
          0, 0, 0, 1
        );


        var ret4 = scaleMatrix.Translate(new Vec3(2, 3, 6));
        Assert.IsTrue(ret4.IsNearlyEqual(new Mat4(1, 0, 0, 0, 0, 3, 0, 0, 0, 0, 5, 0, 2, 9, 30, 1)));

        var r = (90 * 0.017453292519943295);
        var rotateZMatrix = new Mat4(
            Math.Cos(r), -Math.Sin(r), 0, 0,
            Math.Sin(r), Math.Cos(r), 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1
        );


        var ret5 = rotateZMatrix.Translate(new Vec3(6, 4, 2));
        Assert.IsTrue(ret5.IsNearlyEqual(new Mat4(0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 4, -6, 2, 1))); // close to zero;
    }
}
