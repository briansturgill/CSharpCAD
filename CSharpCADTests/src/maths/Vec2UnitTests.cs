using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class Vec2Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestAbs()
    {
        var ret1 = new Vec2(0, 0).Abs();
        Assert.IsTrue(ret1 == new Vec2(0, 0));

        var ret2 = new Vec2(1, 2).Abs();
        Assert.IsTrue(ret2 == new Vec2(1, 2));

        var ret3 = new Vec2(-1, -2).Abs();
        Assert.IsTrue(ret3 == new Vec2(1, 2));
    }

    [Test]
    public void TestAdd()
    {
        var ret1 = new Vec2().Add(new Vec2());
        Assert.IsTrue(ret1 == new Vec2());

        var ret2 = new Vec2(1, 2).Add(new Vec2(3, 2));
        Assert.IsTrue(ret2 == new Vec2(4, 4));

        var ret3 = new Vec2(1, 2).Add(new Vec2(-1, -2));
        Assert.IsTrue(ret3 == new Vec2());

        var ret4 = new Vec2(-1, -2).Add(new Vec2(-1, -2));
        Assert.IsTrue(ret4 == new Vec2(-2, -4));
    }

    [Test]
    public void TestAngleDegrees()
    {
        var distance1 = new Vec2().AngleDegrees();
        Assert.IsTrue(Helpers.NearlyEqual(distance1, 0.0, C.EPS));

        var distance2 = new Vec2(1, 2).AngleDegrees();
        Assert.IsTrue(Helpers.NearlyEqual(distance2, 63.4349488, C.EPS));

        var distance3 = new Vec2(1, -2).AngleDegrees();
        Assert.IsTrue(Helpers.NearlyEqual(distance3, -63.4349488, C.EPS));

        var distance4 = new Vec2(-1, -2).AngleDegrees();
        Assert.IsTrue(Helpers.NearlyEqual(distance4, -116.5650511, C.EPS));

        var distance5 = new Vec2(-1, 2).AngleDegrees();
        Assert.IsTrue(Helpers.NearlyEqual(distance5, 116.5650511, C.EPS));
    }

    [Test]
    public void TestAngleRadians()
    {
        var distance1 = new Vec2().AngleRadians();
        Assert.IsTrue(Helpers.NearlyEqual(distance1, 0.0, C.EPS));

        var distance2 = new Vec2(1, 2).AngleRadians();
        Assert.IsTrue(Helpers.NearlyEqual(distance2, 1.1071487177940904, C.EPS));

        var distance3 = new Vec2(1, -2).AngleRadians();
        Assert.IsTrue(Helpers.NearlyEqual(distance3, -1.1071487177940904, C.EPS));

        var distance4 = new Vec2(-1, -2).AngleRadians();
        Assert.IsTrue(Helpers.NearlyEqual(distance4, -2.0344439357957027, C.EPS));

        var distance5 = new Vec2(-1, 2).AngleRadians();
        Assert.IsTrue(Helpers.NearlyEqual(distance5, 2.0344439357957027, C.EPS));
    }

    [Test]
    public void TestCopy()
    {
        var org1 = new Vec2(0, 0);
        var ret1 = org1;
        Assert.IsTrue(ret1 == new Vec2());

        var org2 = new Vec2(1, 2);
        var ret2 = org2;
        Assert.IsTrue(ret2 == new Vec2(1, 2));

        var org3 = new Vec2(-1, -2);
        var ret3 = org3;
        Assert.IsTrue(ret3 == new Vec2(-1, -2));
    }

    [Test]
    public void TestZeroConstructor()
    {
        var obs = new Vec2();
        Assert.IsTrue(obs == new Vec2(0, 0));
    }

    [Test]
    public void TestCross()
    {
        var ret1 = new Vec2().Cross(new Vec2());
        Assert.IsTrue(ret1 == new Vec3());

        var ret2 = new Vec2(5, 5).Cross(new Vec2(10, 20));
        Assert.IsTrue(ret2 == new Vec3(0, 0, 50));

        var ret3 = new Vec2(5, 5).Cross(new Vec2(10, -20));
        Assert.IsTrue(ret3 == new Vec3(0, 0, -150));

        var ret4 = new Vec2(5, 5).Cross(new Vec2(-10, -20));
        Assert.IsTrue(ret4 == new Vec3(0, 0, -50));

        var ret5 = new Vec2(5, 5).Cross(new Vec2(-10, 20));
        Assert.IsTrue(ret5 == new Vec3(0, 0, 150));

        var ret6 = new Vec2(5, 5).Cross(new Vec2(10, 20));
        Assert.IsTrue(ret6 == new Vec3(0, 0, 50));

        var ret7 = new Vec2(5, 5).Cross(new Vec2(10, -20));
        Assert.IsTrue(ret7 == new Vec3(0, 0, -150));

        var ret8 = new Vec2(5, 5).Cross(new Vec2(-10, -20));
        Assert.IsTrue(ret8 == new Vec3(0, 0, -50));

        var ret9 = new Vec2(5, 5).Cross(new Vec2(-10, 20));
        Assert.IsTrue(ret9 == new Vec3(0, 0, 150));
    }

    [Test]
    public void TestDistance()
    {
        var vec0 = new Vec2(0, 0);
        var vec1 = new Vec2(0, 0);
        var distance1 = vec1.Distance(vec0);
        Assert.IsTrue(Helpers.NearlyEqual(distance1, 0.0, C.EPS));

        var vec2 = new Vec2(1, 2);
        var distance2 = vec2.Distance(vec0);
        Assert.IsTrue(Helpers.NearlyEqual(distance2, 2.23606, C.EPS));

        var vec3 = new Vec2(1, -2);
        var distance3 = vec3.Distance(vec0);
        Assert.IsTrue(Helpers.NearlyEqual(distance3, 2.23606, C.EPS));

        var vec4 = new Vec2(-1, -2);
        var distance4 = vec4.Distance(vec0);
        Assert.IsTrue(Helpers.NearlyEqual(distance4, 2.23606, C.EPS));

        var vec5 = new Vec2(-1, 2);
        var distance5 = vec5.Distance(vec0);
        Assert.IsTrue(Helpers.NearlyEqual(distance5, 2.23606, C.EPS));
    }

    [Test]
    public void TestDivide()
    {
        var ret1 = new Vec2().Divide(new Vec2());
        Assert.IsTrue(double.IsNaN(ret1.x) && double.IsNaN(ret1.y));

        var ret2 = new Vec2().Divide(new Vec2(1, 2));
        Assert.IsTrue(ret2 == new Vec2());

        var ret3 = new Vec2(6, 6).Divide(new Vec2(1, 2));
        Assert.IsTrue(ret3 == new Vec2(6, 3));

        var ret4 = new Vec2(-6, -6).Divide(new Vec2(1, 2));
        Assert.IsTrue(ret4 == new Vec2(-6, -3));

        var ret5 = new Vec2(6, 6).Divide(new Vec2(-1, -2));
        Assert.IsTrue(ret5 == new Vec2(-6, -3));

        var ret6 = new Vec2(-6, -6).Divide(new Vec2(-1, -2));
        Assert.IsTrue(ret6 == new Vec2(6, 3));
    }

    [Test]
    public void TestDot()
    {
        var veca1 = new Vec2(0, 0);
        var vecb1 = new Vec2(0, 0);
        var dot1 = veca1.Dot(vecb1);
        Assert.IsTrue(dot1 == 0.0);

        var veca2 = new Vec2(1, 1);
        var vecb2 = new Vec2(-1, -1);
        var dot2 = veca2.Dot(vecb2);
        Assert.IsTrue(dot2 == -2.0);

        var veca3 = new Vec2(5, 5);
        var vecb3 = new Vec2(5, 5);
        var dot3 = veca3.Dot(vecb3);
        Assert.IsTrue(dot3 == 50.0);

        var veca4 = new Vec2(5, 5);
        var vecb4 = new Vec2(-2, 3);
        var dot4 = veca4.Dot(vecb4);
        Assert.IsTrue(dot4 == 5.0);
    }

    [Test]
    public void TestFromAngleDegrees()
    {
        var obs1 = Vec2.FromAngleDegrees(0);
        Assert.IsTrue(obs1 == new Vec2(1.0, 0.0));

        var obs2 = Vec2.FromAngleDegrees(180);
        Assert.IsTrue(obs2.IsNearlyEqual(new Vec2(-1, 1.2246468525851679e-16)));
    }

    [Test]
    public void TestFromAngleRadians()
    {
        var obs1 = Vec2.FromAngleRadians(0);
        Assert.IsTrue(obs1 == new Vec2(1.0, 0.0));

        var obs2 = Vec2.FromAngleRadians(Math.PI);
        Assert.IsTrue(obs2.IsNearlyEqual(new Vec2(-1, 1.2246468525851679e-16)));
    }

    [Test]
    public void TestFromScalar()
    {
        var obs1 = Vec2.FromScalar(0);
        Assert.IsTrue(obs1 == new Vec2());

        var obs2 = Vec2.FromScalar(-5);
        Assert.IsTrue(obs2 == new Vec2(-5, -5));
    }

    [Test]
    public void TestLength()
    {
        var vec1 = new Vec2(0, 0);
        var length1 = vec1.Length();
        Assert.IsTrue(Helpers.NearlyEqual(length1, 0.0, C.EPS));

        var vec2 = new Vec2(1, 2);
        var length2 = vec2.Length();
        Assert.IsTrue(Helpers.NearlyEqual(length2, 2.23606, C.EPS));

        var vec3 = new Vec2(1, -2);
        var length3 = vec3.Length();
        Assert.IsTrue(Helpers.NearlyEqual(length3, 2.23606, C.EPS));

        var vec4 = new Vec2(-1, -2);
        var length4 = vec4.Length();
        Assert.IsTrue(Helpers.NearlyEqual(length4, 2.23606, C.EPS));

        var vec5 = new Vec2(-1, 2);
        var length5 = vec5.Length();
        Assert.IsTrue(Helpers.NearlyEqual(length5, 2.23606, C.EPS));

        // huge vector
        var vec6 = new Vec2(1e200, 1e200);
        var length6 = vec6.Length();
        Assert.IsTrue(Helpers.NearlyEqual(length6, Math.Sqrt(2) * 1e200, C.EPS));

        // tiny vector
        var vec7 = new Vec2(1e-200, 1e-200);
        var length7 = vec7.Length();
        Assert.IsTrue(Helpers.NearlyEqual(length7, Math.Sqrt(2) * 1e-200, C.EPS));
    }

    [Test]
    public void TestLerp()
    {
        var ret1 = new Vec2().Lerp(new Vec2(), 0);
        Assert.IsTrue(ret1 == new Vec2());

        var ret2 = new Vec2(1, 2).Lerp(new Vec2(5, 6), 0.00);
        Assert.IsTrue(ret2 == new Vec2(1, 2));

        var ret3 = new Vec2(1, 2).Lerp(new Vec2(5, 6), 0.75);
        Assert.IsTrue(ret3 == new Vec2(4, 5));

        var ret4 = new Vec2(1, 2).Lerp(new Vec2(5, 6), 1.00);
        Assert.IsTrue(ret4 == new Vec2(5, 6));
    }

    [Test]
    public void TestMax()
    {
        var vec0 = new Vec2(0, 0);
        var vec1 = new Vec2(0, 0);
        var ret1 = vec1.Max(vec0);
        Assert.IsTrue(ret1 == new Vec2());

        var vec2 = new Vec2(1, 1);
        var ret2 = vec2.Max(vec0);
        Assert.IsTrue(ret2 == new Vec2(1, 1));

        var vec3 = new Vec2(0, 1);
        var ret3 = vec3.Max(vec0);
        Assert.IsTrue(ret3 == new Vec2(0, 1));

        var vec4 = new Vec2(0, 0);
        var ret4 = vec4.Max(vec0);
        Assert.IsTrue(ret4 == new Vec2());
    }

    [Test]
    public void TestMin()
    {
        var vec0 = new Vec2(0, 0);
        var vec1 = new Vec2(0, 0);
        var ret1 = vec0.Min(vec1);
        Assert.IsTrue(ret1 == new Vec2());

        var vec2 = new Vec2(1, 1);
        var ret2 = vec0.Min(vec2);
        Assert.IsTrue(ret2 == new Vec2());

        var vec3 = new Vec2(0, 1);
        var ret3 = vec0.Min(vec3);
        Assert.IsTrue(ret3 == new Vec2());

        var vec4 = new Vec2(0, 0);
        var ret4 = vec0.Min(vec4);
        Assert.IsTrue(ret4 == new Vec2());
    }

    [Test]
    public void TestMultiply()
    {
        var ret1 = new Vec2().Multiply(new Vec2());
        Assert.IsTrue(ret1 == new Vec2());

        var ret2 = new Vec2().Multiply(new Vec2(1, 2));
        Assert.IsTrue(ret2 == new Vec2());

        var ret3 = new Vec2(6, 6).Multiply(new Vec2(1, 2));
        Assert.IsTrue(ret3 == new Vec2(6, 12));

        var ret4 = new Vec2(-6, -6).Multiply(new Vec2(1, 2));
        Assert.IsTrue(ret4 == new Vec2(-6, -12));

        var ret5 = new Vec2(6, 6).Multiply(new Vec2(-1, -2));
        Assert.IsTrue(ret5 == new Vec2(-6, -12));

        var ret6 = new Vec2(-6, -6).Multiply(new Vec2(-1, -2));
        Assert.IsTrue(ret6 == new Vec2(6, 12));
    }

    [Test]
    public void TestNegate()
    {
        var ret1 = new Vec2().Negate();
        Assert.IsTrue(ret1 == new Vec2());

        var ret2 = new Vec2(1, 2).Negate();
        Assert.IsTrue(ret2 == new Vec2(-1, -2));

        var ret3 = new Vec2(-1, -2).Negate();
        Assert.IsTrue(ret3 == new Vec2(1, 2));

        var ret4 = new Vec2(-1, 2).Negate();
        Assert.IsTrue(ret4 == new Vec2(1, -2));
    }

    [Test]
    public void TestNormalize()
    {
        var ret1 = new Vec2().Normalize();
        Assert.IsTrue(ret1.IsNearlyEqual(new Vec2()));

        var ret2 = new Vec2(1, 2).Normalize();
        Assert.IsTrue(ret2.IsNearlyEqual(new Vec2(0.4472135954999579, 0.8944271909999159)));

        var ret3 = new Vec2(-1, -2).Normalize();
        Assert.IsTrue(ret3.IsNearlyEqual(new Vec2(-0.4472135954999579, -0.8944271909999159)));

        var ret4 = new Vec2(-1, 2).Normalize();
        Assert.IsTrue(ret4.IsNearlyEqual(new Vec2(-0.4472135954999579, 0.8944271909999159)));

        var ret5 = new Vec2(0.5, 1.5).Normalize();
        Assert.IsTrue(ret5.IsNearlyEqual(new Vec2(0.31622776601683794, 0.9486832980505138)));

        var ret6 = new Vec2(0.5, 0.5).Normalize();
        Assert.IsTrue(ret6.IsNearlyEqual(new Vec2(0.7071067811865475, 0.7071067811865475)));
    }

    [Test]
    public void TestNormal()
    {
        var ret1 = new Vec2().Normal();
        Assert.IsTrue(ret1.IsNearlyEqual(new Vec2()));

        var ret2 = new Vec2(1, 2).Normal();
        Assert.IsTrue(ret2.IsNearlyEqual(new Vec2(-2, 1)));

        var ret3 = new Vec2(-1, -2).Normal();
        Assert.IsTrue(ret3.IsNearlyEqual(new Vec2(2, -1)));

        var ret4 = new Vec2(-1, 2).Normal();
        Assert.IsTrue(ret4.IsNearlyEqual(new Vec2(-2, -1)));
    }

    [Test]
    public void TestRotate()
    {
        var radians = 90 * Math.PI / 180;

        var obs1 = new Vec2(0, 0);
        var ret1 = new Vec2().Rotate(new Vec2(), 0);
        Assert.IsTrue(ret1.IsNearlyEqual(new Vec2()));

        var ret2 = new Vec2(1, 2).Rotate(new Vec2(), 0);
        Assert.IsTrue(ret2.IsNearlyEqual(new Vec2(1, 2)));

        var ret3 = new Vec2(-1, -2).Rotate(new Vec2(), radians);
        Assert.IsTrue(ret3.IsNearlyEqual(new Vec2(2, -1)));

        var ret4 = new Vec2(-1, 2).Rotate(new Vec2(-3, -3), -radians);
        Assert.IsTrue(ret4.IsNearlyEqual(new Vec2(2, -5)));
    }

    [Test]
    public void TestScale()
    {
        var ret1 = new Vec2().Scale(0);
        Assert.IsTrue(ret1 == new Vec2());

        var ret2 = new Vec2(1, 2).Scale(3);
        Assert.IsTrue(ret2 == new Vec2(3, 6));

        var ret3 = new Vec2(-1, -2).Scale(3);
        Assert.IsTrue(ret3 == new Vec2(-3, -6));
    }

    [Test]
    public void TestSnap()
    {
        var obs1 = new Vec2().Snap(0.1);
        Assert.IsTrue(obs1.IsNearlyEqual(new Vec2()));

        var obs2 = new Vec2(1, 2).Snap(0.1);
        Assert.IsTrue(obs2.IsNearlyEqual(new Vec2(1, 2)));

        var obs3 = new Vec2(-1, -2).Snap(0.01);
        Assert.IsTrue(obs3.IsNearlyEqual(new Vec2(-1, -2)));

        var obs4 = new Vec2(-1.123456789, -2.123456789).Snap(0.01);
        Assert.IsTrue(obs4.IsNearlyEqual(new Vec2(-1.12, -2.12)));

        var obs5 = new Vec2(-1.123456789, -2.123456789).Snap(0.0001);
        Assert.IsTrue(obs5.IsNearlyEqual(new Vec2(-1.1235, -2.1235)));

        var obs6 = new Vec2(-1.123456789, -2.123456789).Snap(0.000001);
        Assert.IsTrue(obs6.IsNearlyEqual(new Vec2(-1.123457, -2.1234569999999997)));
    }

    [Test]
    public void TestSquaredDistance()
    {
        var vec0 = new Vec2(0, 0);
        var vec1 = new Vec2(0, 0);
        var distance1 = vec0.SquaredDistance(vec1);
        Assert.IsTrue(Helpers.NearlyEqual(distance1, 0.0, C.EPS));

        var vec2 = new Vec2(1, 2);
        var distance2 = vec0.SquaredDistance(vec2);
        Assert.IsTrue(Helpers.NearlyEqual(distance2, 5.00000, C.EPS));

        var vec3 = new Vec2(1, -2);
        var distance3 = vec0.SquaredDistance(vec3);
        Assert.IsTrue(Helpers.NearlyEqual(distance3, 5.00000, C.EPS));

        var vec4 = new Vec2(-1, -2);
        var distance4 = vec0.SquaredDistance(vec4);
        Assert.IsTrue(Helpers.NearlyEqual(distance4, 5.00000, C.EPS));

        var vec5 = new Vec2(-1, 2);
        var distance5 = vec0.SquaredDistance(vec5);
        Assert.IsTrue(Helpers.NearlyEqual(distance5, 5.00000, C.EPS));
    }


    [Test]
    public void TestSquaredLength()
    {
        var vec1 = new Vec2(0, 0);
        var length1 = vec1.SquaredLength();
        Assert.IsTrue(Helpers.NearlyEqual(length1, 0.0, C.EPS));

        var vec2 = new Vec2(1, 2);
        var length2 = vec2.SquaredLength();
        Assert.IsTrue(Helpers.NearlyEqual(length2, 5.00000, C.EPS));

        var vec3 = new Vec2(1, -2);
        var length3 = vec3.SquaredLength();
        Assert.IsTrue(Helpers.NearlyEqual(length3, 5.00000, C.EPS));

        var vec4 = new Vec2(-1, -2);
        var length4 = vec4.SquaredLength();
        Assert.IsTrue(Helpers.NearlyEqual(length4, 5.00000, C.EPS));

        var vec5 = new Vec2(-1, 2);
        var length5 = vec5.SquaredLength();
        Assert.IsTrue(Helpers.NearlyEqual(length5, 5.00000, C.EPS));
    }

    [Test]
    public void TestSubtract()
    {
        var ret1 = new Vec2().Subtract(new Vec2());
        Assert.IsTrue(ret1 == new Vec2());

        var ret2 = new Vec2(1, 2).Subtract(new Vec2(3, 2));
        Assert.IsTrue(ret2 == new Vec2(-2, 0));

        var ret3 = new Vec2(1, 2).Subtract(new Vec2(-1, -2));
        Assert.IsTrue(ret3 == new Vec2(2, 4));

        var ret4 = new Vec2(-1, -2).Subtract(new Vec2(-1, -2));
        Assert.IsTrue(ret4 == new Vec2());
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

        var ret1 = new Vec2(0, 0).Transform(identityMatrix);
        Assert.IsTrue(ret1 == new Vec2(0, 0));

        var ret2 = new Vec2(3, 2).Transform(identityMatrix);
        Assert.IsTrue(ret2 == new Vec2(3, 2));

        var x = 1;
        var y = 5;
        var z = 7;
        var translationMatrix = new Mat4(
          1, 0, 0, 0,
          0, 1, 0, 0,
          0, 0, 1, 0,
          x, y, z, 1
        );

        var ret3 = new Vec2(-1, -2).Transform(translationMatrix);
        Assert.IsTrue(ret3 == new Vec2(0, 3));

        var w = 1;
        var h = 3;
        var d = 5;

        var scaleMatrix = new Mat4(
          w, 0, 0, 0,
          0, h, 0, 0,
          0, 0, d, 0,
          0, 0, 0, 1
        );

        var ret4 = new Vec2(1, 2).Transform(scaleMatrix);
        Assert.IsTrue(ret4 == new Vec2(1, 6));

        var r = (90 * 0.017453292519943295);
        var rotateZMatrix = new Mat4(
          Math.Cos(r), -Math.Sin(r), 0, 0,
          Math.Sin(r), Math.Cos(r), 0, 0,
          0, 0, 1, 0,
          0, 0, 0, 1
        );

        var ret5 = new Vec2(1, 2).Transform(rotateZMatrix);
        Assert.IsTrue(ret5.IsNearlyEqual(new Vec2(2, -1)));
    }
}