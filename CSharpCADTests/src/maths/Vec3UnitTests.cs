using NUnit.Framework;

using CSharpCAD;
using System;

namespace CSharpCADTests;

[TestFixture]
public class Vec3Tests
{
    private Vec3 ZeroVec;
    [SetUp]
    public void Setup()
    {
        ZeroVec = new Vec3();
    }

    [Test]
    public void TestConstructor()
    {
        Assert.IsTrue(ZeroVec.X == 0, "x should be 0");
        Assert.IsTrue(ZeroVec.Y == 0, "y should be 0");
        Assert.IsTrue(ZeroVec.Z == 0, "z should be 0");

        Vec3 v = new Vec3(1, 2, 3);
        Assert.IsTrue(v.X == 1, "x should be 1");
        Assert.IsTrue(v.Y == 2, "y should be 2");
        Assert.IsTrue(v.Z == 3, "z should be 3");
    }

    [Test]
    public void TestFromScalar()
    {
        Assert.IsTrue(new Vec3(0.0) == (ZeroVec));
        Assert.IsTrue(new Vec3(-5) == (new Vec3(-5, -5, -5)));
    }

    [Test]
    public void TestFromVec2()
    {
        Assert.IsTrue(new Vec3(0, 0, 0) == (new Vec3(new Vec2(0, 0), 0)));
        Assert.IsTrue(new Vec3(0, 1, -5) == (new Vec3(new Vec2(0, 1), -5)));
    }


    [Test]
    public void TestAbs()
    {
        Assert.IsTrue(ZeroVec == (ZeroVec.Abs()));
        Vec3 v = new Vec3(1, 2, 3);
        Assert.IsTrue(v == (v.Abs()));
        v = new Vec3(-1, -2, -3);
        Assert.IsTrue(new Vec3(1, 2, 3) == (v.Abs()));
    }

    [Test]
    public void TestAdd()
    {
        Assert.IsTrue(new Vec3(0, 0, 0) == (ZeroVec.Add(ZeroVec)));
        Assert.IsTrue(new Vec3(4, 4, 4) == (new Vec3(1, 2, 3).Add(new Vec3(3, 2, 1))));
        Assert.IsTrue(new Vec3(0, 0, 0) == (new Vec3(1, 2, 3).Add(new Vec3(-1, -2, -3))));
        Assert.IsTrue(new Vec3(-2, -4, -6) == (new Vec3(-1, -2, -3).Add(new Vec3(-1, -2, -3))));
    }

    [Test]
    public void TestAngle()
    {
        var vec0 = new Vec3(5, 5, 5);
        var vec1 = new Vec3(0, 0, 0);
        var angle1 = vec0.Angle(vec1);
        Helpers.NearlyEqual(angle1, 1.57079, C.EPS); // any vector with all zeros

        var veca2 = new Vec3(1, 0, 0);
        var vec2 = new Vec3(0, 1, 0);
        var angle2 = veca2.Angle(vec2);
        Helpers.NearlyEqual(angle2, 1.57079, C.EPS);

        var veca3 = new Vec3(1, 0, 0);
        var vec3 = new Vec3(1, 0, 0);
        var angle3 = veca3.Angle(vec3);
        Helpers.NearlyEqual(angle3, 0.00000, C.EPS);

        var veca4 = new Vec3(1, 1, 1);
        var vec4 = new Vec3(-1, -1, -1);
        var angle4 = veca4.Angle(vec4);
        Helpers.NearlyEqual(angle4, 3.14159, C.EPS);

        var vec5a = new Vec3(1, 0, 0);
        var vec5b = new Vec3(1, 1, 0);
        var angle5 = vec5a.Angle(vec5b);
        Helpers.NearlyEqual(angle5, 0.785398, C.EPS);

        // tiny values
        var vec6a = new Vec3(1, 0, 0);
        var vec6b = new Vec3(1e-200, 1e-200, 0);
        var angle6 = vec6a.Angle(vec6b);
        Helpers.NearlyEqual(angle6, 0.785398, C.EPS);

        // huge values
        var vec7a = new Vec3(1, 0, 0);
        var vec7b = new Vec3(1e200, 1e200, 0);
        var angle7 = vec7a.Angle(vec7b);
        Helpers.NearlyEqual(angle7, 0.785398, C.EPS);
    }

    [Test]
    public void TestCopy()
    {
        var a = new Vec3(1, 2, 3);
        var b = new Vec3();
        Assert.IsFalse(a == (b));
        b = a;
        Assert.IsTrue(a == (b));
    }

    [Test]
    public void TestCross()
    {
        var ret1 = new Vec3(0, 0, 0).Cross(new Vec3(0, 0, 0));
        Assert.IsTrue(ret1 == (new Vec3(0, 0, 0)));

        var ret2 = new Vec3(5, 5, 5).Cross(new Vec3(10, 20, 30));
        Assert.IsTrue(ret2 == (new Vec3(50, -100, 50)));

        var ret3 = new Vec3(5, 5, 5).Cross(new Vec3(10, -20, 30));
        Assert.IsTrue(ret3 == (new Vec3(250, -100, -150)));

        var ret4 = new Vec3(5, 5, 5).Cross(new Vec3(-10, -20, 30));
        Assert.IsTrue(ret4 == (new Vec3(250, -200, -50)));

        var ret5 = new Vec3(5, 5, 5).Cross(new Vec3(-10, 20, 30));
        Assert.IsTrue(ret5 == (new Vec3(50, -200, 150)));

        var ret6 = new Vec3(5, 5, 5).Cross(new Vec3(10, 20, -30));
        Assert.IsTrue(ret6 == (new Vec3(-250, 200, 50)));

        var ret7 = new Vec3(5, 5, 5).Cross(new Vec3(10, -20, -30));
        Assert.IsTrue(ret7 == (new Vec3(-50, 200, -150)));

        var ret8 = new Vec3(5, 5, 5).Cross(new Vec3(-10, -20, -30));
        Assert.IsTrue(ret8 == (new Vec3(-50, 100, -50)));

        var ret9 = new Vec3(5, 5, 5).Cross(new Vec3(-10, 20, -30));
        Assert.IsTrue(ret9 == (new Vec3(-250, 100, 150)));
    }

    [Test]
    public void TestDistance()
    {
        var vec0 = new Vec3(0, 0, 0);
        var vec1 = new Vec3(0, 0, 0);
        var distance1 = vec0.Distance(vec1);
        Helpers.NearlyEqual(distance1, 0.0, C.EPS);

        var vec2 = new Vec3(1, 2, 3);
        var distance2 = vec0.Distance(vec2);
        Helpers.NearlyEqual(distance2, 3.74165, C.EPS);

        var vec3 = new Vec3(1, -2, 3);
        var distance3 = vec0.Distance(vec3);
        Helpers.NearlyEqual(distance3, 3.74165, C.EPS);

        var vec4 = new Vec3(-1, -2, 3);
        var distance4 = vec0.Distance(vec4);
        Helpers.NearlyEqual(distance4, 3.74165, C.EPS);

        var vec5 = new Vec3(-1, 2, 3);
        var distance5 = vec0.Distance(vec5);
        Helpers.NearlyEqual(distance5, 3.74165, C.EPS);

        var vec6 = new Vec3(1, 2, -3);
        var distance6 = vec0.Distance(vec6);
        Helpers.NearlyEqual(distance6, 3.74165, C.EPS);

        var vec7 = new Vec3(1, -2, -3);
        var distance7 = vec0.Distance(vec7);
        Helpers.NearlyEqual(distance7, 3.74165, C.EPS);

        var vec8 = new Vec3(-1, -2, -3);
        var distance8 = vec0.Distance(vec8);
        Helpers.NearlyEqual(distance8, 3.74165, C.EPS);

        var vec9 = new Vec3(-1, 2, -3);
        var distance9 = vec0.Distance(vec9);
        Helpers.NearlyEqual(distance9, 3.74165, C.EPS);
    }

    [Test]
    public void TestDivide()
    {
        var z = ZeroVec.Divide(ZeroVec);
        Assert.IsTrue(double.IsNaN(z.X));
        Assert.IsTrue(double.IsNaN(z.Y));
        Assert.IsTrue(double.IsNaN(z.Z));
        Assert.IsTrue(new Vec3(0, 0, 0) == (new Vec3(0, 0, 0).Divide(new Vec3(1, 2, 3))));
        Assert.IsTrue(new Vec3(6, 3, 2) == (new Vec3(6, 6, 6).Divide(new Vec3(1, 2, 3))));
        Assert.IsTrue(new Vec3(-6, -3, -2) == (new Vec3(-6, -6, -6).Divide(new Vec3(1, 2, 3))));
        Assert.IsTrue(new Vec3(-6, -3, -2) == (new Vec3(6, 6, 6).Divide(new Vec3(-1, -2, -3))));
        Assert.IsTrue(new Vec3(6, 3, 2) == (new Vec3(-6, -6, -6).Divide(new Vec3(-1, -2, -3))));
    }

    [Test]
    public void TestDot()
    {
        var veca1 = new Vec3(0, 0, 0);
        var vecb1 = new Vec3(0, 0, 0);
        var dot1 = veca1.Dot(vecb1);
        Assert.IsTrue(dot1 == 0.0);

        var veca2 = new Vec3(1, 1, 1);
        var vecb2 = new Vec3(-1, -1, -1);
        var dot2 = veca2.Dot(vecb2);
        Assert.IsTrue(dot2 == -3.0);

        var veca3 = new Vec3(5, 5, 5);
        var vecb3 = new Vec3(5, 5, 5);
        var dot3 = veca3.Dot(vecb3);
        Assert.IsTrue(dot3 == 75.0);

        var veca4 = new Vec3(5, 5, 5);
        var vecb4 = new Vec3(-2, 3, -4);
        var dot4 = veca4.Dot(vecb4);
        Assert.IsTrue(dot4 == -15.0);
    }

    [Test]
    public void TestLength()
    {
        var vec1 = new Vec3(0, 0, 0);
        var length1 = vec1.Length();
        Helpers.NearlyEqual(length1, 0.0, C.EPS);

        var vec2 = new Vec3(1, 2, 3);
        var length2 = vec2.Length();
        Helpers.NearlyEqual(length2, 3.74165, C.EPS);

        var vec3 = new Vec3(1, -2, 3);
        var length3 = vec3.Length();
        Helpers.NearlyEqual(length3, 3.74165, C.EPS);

        var vec4 = new Vec3(-1, -2, 3);
        var length4 = vec4.Length();
        Helpers.NearlyEqual(length4, 3.74165, C.EPS);

        var vec5 = new Vec3(-1, 2, 3);
        var length5 = vec5.Length();
        Helpers.NearlyEqual(length5, 3.74165, C.EPS);

        var vec6 = new Vec3(1, 2, -3);
        var length6 = vec6.Length();
        Helpers.NearlyEqual(length6, 3.74165, C.EPS);

        var vec7 = new Vec3(1, -2, -3);
        var length7 = vec7.Length();
        Helpers.NearlyEqual(length7, 3.74165, C.EPS);

        var vec8 = new Vec3(-1, -2, -3);
        var length8 = vec8.Length();
        Helpers.NearlyEqual(length8, 3.74165, C.EPS);

        var vec9 = new Vec3(-1, 2, -3);
        var length9 = vec9.Length();
        Helpers.NearlyEqual(length9, 3.74165, C.EPS);

        // huge vector
        var vec10 = new Vec3(1e200, 0, 1e200);
        var length10 = vec10.Length();
        Helpers.NearlyEqual(length10, Math.Sqrt(2.0) * 1e200, C.EPS);

        // tiny vector
        var vec11 = new Vec3(1e-200, 0, 1e-200);
        var length11 = vec11.Length();
        Helpers.NearlyEqual(length11, Math.Sqrt(2.0) * 1e-200, C.EPS);
    }

    [Test]
    public void TestLerp()
    {
        var ret1 = new Vec3(0, 0, 0).Lerp(new Vec3(0, 0, 0), 0);
        Assert.IsTrue(ret1 == (new Vec3(0, 0, 0)));

        var ret2 = new Vec3(1, 2, 3).Lerp(new Vec3(5, 6, 7), 0.00);
        Assert.IsTrue(ret2 == (new Vec3(1, 2, 3)));

        var ret3 = new Vec3(1, 2, 3).Lerp(new Vec3(5, 6, 7), 0.75);
        Assert.IsTrue(ret3 == (new Vec3(4, 5, 6)));

        var ret4 = new Vec3(1, 2, 3).Lerp(new Vec3(5, 6, 7), 1.00);
        Assert.IsTrue(ret4 == (new Vec3(5, 6, 7)));
    }

    [Test]
    public void TestMax()
    {
        var vec0 = new Vec3(0, 0, 0);
        var vec1 = new Vec3(0, 0, 0);
        var ret1 = vec0.Max(vec1);
        Assert.IsTrue(ret1 == (new Vec3(0, 0, 0)));

        var vec2 = new Vec3(1, 1, 1);
        var ret2 = vec0.Max(vec2);
        Assert.IsTrue(ret2 == (new Vec3(1, 1, 1)));

        var vec3 = new Vec3(0, 1, 1);
        var ret3 = vec0.Max(vec3);
        Assert.IsTrue(ret3 == (new Vec3(0, 1, 1)));

        var vec4 = new Vec3(0, 0, 1);
        var ret4 = vec0.Max(vec4);
        Assert.IsTrue(ret4 == (new Vec3(0, 0, 1)));
    }

    [Test]
    public void TestMin()
    {
        var vec0 = new Vec3(0, 0, 0);
        var vec1 = new Vec3(0, 0, 0);
        var ret1 = vec0.Min(vec1);
        Assert.IsTrue(ret1 == (new Vec3(0, 0, 0)));

        var vec2 = new Vec3(1, 1, 1);
        var ret2 = vec0.Min(vec2);
        Assert.IsTrue(ret2 == (new Vec3(0, 0, 0)));

        var vec3 = new Vec3(0, 1, 1);
        var ret3 = vec0.Min(vec3);
        Assert.IsTrue(ret3 == (new Vec3(0, 0, 0)));

        var vec4 = new Vec3(0, 0, 1);
        var ret4 = vec0.Min(vec4);
        Assert.IsTrue(ret4 == (new Vec3(0, 0, 0)));
    }

    [Test]
    public void TestMultiply()
    {
        Assert.IsTrue(new Vec3(0, 0, 0) == (new Vec3(0, 0, 0).Multiply(new Vec3(0, 0, 0))));
        Assert.IsTrue(new Vec3(0, 0, 0) == (new Vec3(0, 0, 0).Multiply(new Vec3(1, 2, 3))));
        Assert.IsTrue(new Vec3(6, 12, 18) == (new Vec3(6, 6, 6).Multiply(new Vec3(1, 2, 3))));
        Assert.IsTrue(new Vec3(-6, -12, -18) == (new Vec3(-6, -6, -6).Multiply(new Vec3(1, 2, 3))));
        Assert.IsTrue(new Vec3(-6, -12, -18) == (new Vec3(6, 6, 6).Multiply(new Vec3(-1, -2, -3))));
        Assert.IsTrue(new Vec3(6, 12, 18) == (new Vec3(-6, -6, -6).Multiply(new Vec3(-1, -2, -3))));
    }

    [Test]
    public void TestNegate()
    {
        Assert.IsTrue(new Vec3(-0, -0, -0) == (new Vec3(0, 0, 0).Negate()));
        Assert.IsTrue(new Vec3(-1, -2, -3) == (new Vec3(1, 2, 3).Negate()));
        Assert.IsTrue(new Vec3(1, 2, 3) == (new Vec3(-1, -2, -3).Negate()));
        Assert.IsTrue(new Vec3(1, -2, 3) == (new Vec3(-1, 2, -3).Negate()));

    }

    [Test]
    public void TestNormalize()
    {
        Assert.IsTrue(new Vec3(0, 0, 0) == (new Vec3(0, 0, 0).Normalize()));
        Assert.IsTrue(new Vec3(0.2672612419124244, 0.5345224838248488, 0.8017837257372732) == (new Vec3(1, 2, 3).Normalize()));
        Assert.IsTrue(new Vec3(-0.2672612419124244, -0.5345224838248488, -0.8017837257372732) == (new Vec3(-1, -2, -3).Normalize()));
        Assert.IsTrue(new Vec3(-0.2672612419124244, 0.5345224838248488, -0.8017837257372732) == (new Vec3(-1, 2, -3).Normalize()));
        Assert.IsTrue(new Vec3(0.30151134457776363, 0.9045340337332909, 0.30151134457776363) == (new Vec3(0.5, 1.5, 0.5).Normalize()));
        Assert.IsTrue(new Vec3(0.5773502691896258, 0.5773502691896258, 0.5773502691896258) == (new Vec3(0.5, 0.5, 0.5).Normalize()));
    }

    [Test]
    public void TestOrthogonal()
    {
        Assert.IsTrue(new Vec3(0, 0, 0) == (new Vec3(0, 0, 0).Orthogonal()));
        Assert.IsTrue(new Vec3(-3, 0, 3) == (new Vec3(3, 1, 3).Orthogonal()));
        Assert.IsTrue(new Vec3(2, -3, 0) == (new Vec3(3, 2, 1).Orthogonal()));
    }

    [Test]
    public void TestRotateX()
    {
        var radians = 90 * Math.PI / 180;

        var ret1 = new Vec3(0, 0, 0).RotateX(new Vec3(0, 0, 0), 0);
        Assert.IsTrue(ret1 == new Vec3(0, 0, 0));

        var ret2 = new Vec3(3, 2, 1).RotateX(new Vec3(1, 2, 3), 0);
        Assert.IsTrue(ret2 == new Vec3(3, 2, 1));

        var ret3 = new Vec3(-1, -2, -3).RotateX(new Vec3(1, 2, 3), radians);
        Assert.IsTrue(ret3 == new Vec3(-1, 8, -1));

        var ret4 = new Vec3(1, 2, 3).RotateX(new Vec3(-1, -2, -3), -radians);
        Assert.IsTrue(ret4 == new Vec3(1, 4, -7));
    }

    [Test]
    public void TestRotateY()
    {
        var radians = 90 * Math.PI / 180;

        var ret1 = new Vec3(0, 0, 0).RotateY(new Vec3(0, 0, 0), 0);
        Assert.IsTrue(ret1 == new Vec3(0, 0, 0));

        var ret2 = new Vec3(3, 2, 1).RotateY(new Vec3(1, 2, 3), 0);
        Assert.IsTrue(ret2 == new Vec3(3, 2, 1));

        var ret3 = new Vec3(-1, -2, -3).RotateY(new Vec3(1, 2, 3), radians);
        Assert.IsTrue(ret3 == new Vec3(-5, -2, 5));

        var ret4 = new Vec3(1, 2, 3).RotateY(new Vec3(-1, -2, -3), -radians);
        Assert.IsTrue(ret4.IsNearlyEqual(new Vec3(-7, 2, -1)));
    }

    [Test]
    public void TestRotateZ()
    {
        var radians = 90 * Math.PI / 180;

        var ret1 = new Vec3(0, 0, 0).RotateZ(new Vec3(0, 0, 0), 0);
        Assert.IsTrue(ret1 == new Vec3(0, 0, 0));

        var ret2 = new Vec3(3, 2, 1).RotateZ(new Vec3(1, 2, 3), 0);
        Assert.IsTrue(ret2 == new Vec3(3, 2, 1));

        var ret3 = new Vec3(-1, -2, -3).RotateZ(new Vec3(1, 2, 3), radians);
        Assert.IsTrue(ret3.IsNearlyEqual(new Vec3(5, -0, -3)));

        var ret4 = new Vec3(1, 2, 3).RotateZ(new Vec3(-1, -2, -3), -radians);
        Assert.IsTrue(ret4 == new Vec3(3, -4, 3));
    }

    [Test]
    public void TestScale()
    {
        var ret1 = new Vec3(0, 0, 0).Scale(0);
        Assert.IsTrue(ret1 == new Vec3(0, 0, 0));

        var ret2 = new Vec3(1, 2, 3).Scale(0);
        Assert.IsTrue(ret2 == new Vec3(0, 0, 0));

        var ret3 = new Vec3(1, 2, 3).Scale(6);
        Assert.IsTrue(ret3 == new Vec3(6, 12, 18));

        var ret4 = new Vec3(1, 2, 3).Scale(-6);
        Assert.IsTrue(ret4 == new Vec3(-6, -12, -18));

        var ret5 = new Vec3(-1, -2, -3).Scale(6);
        Assert.IsTrue(ret5 == new Vec3(-6, -12, -18));

        var ret6 = new Vec3(-1, -2, -3).Scale(-6);
        Assert.IsTrue(ret6 == new Vec3(6, 12, 18));
    }

    [Test]
    public void TestSnap()
    {
        var obs1 = new Vec3(0, 0, 0).Snap(0.1);
        Assert.IsTrue(obs1 == new Vec3(0, 0, 0));

        var obs2 = new Vec3(1, 2, 3).Snap(0.1);
        Assert.IsTrue(obs2 == new Vec3(1, 2, 3));

        var obs3 = new Vec3(-1, -2, -3).Snap(0.01);
        Assert.IsTrue(obs3 == new Vec3(-1, -2, -3));

        var obs4 = new Vec3(-1.123456789, -2.123456789, -3.123456789).Snap(0.01);
        Assert.IsTrue(obs4 == new Vec3(-1.12, -2.12, -3.12));

        var obs5 = new Vec3(-1.123456789, -2.123456789, -3.123456789).Snap(0.0001);
        Assert.IsTrue(obs5 == new Vec3(-1.1235, -2.1235, -3.1235));

        var obs6 = new Vec3(-1.123456789, -2.123456789, -3.123456789).Snap(0.000001);
        Assert.IsTrue(obs6 == new Vec3(-1.123457, -2.1234569999999997, -3.1234569999999997));
    }

    [Test]
    public void TestSquaredDistance()
    {
        var vec0 = new Vec3(0, 0, 0);
        var vec1 = new Vec3(0, 0, 0);
        var distance1 = vec0.SquaredDistance(vec1);
        Helpers.NearlyEqual(distance1, 0.0, C.EPS);

        var vec2 = new Vec3(1, 2, 3);
        var distance2 = vec1.SquaredDistance(vec2);
        Helpers.NearlyEqual(distance2, 14.00000, C.EPS);

        var vec3 = new Vec3(1, -2, 3);
        var distance3 = vec1.SquaredDistance(vec3);
        Helpers.NearlyEqual(distance3, 14.00000, C.EPS);

        var vec4 = new Vec3(-1, -2, 3);
        var distance4 = vec1.SquaredDistance(vec4);
        Helpers.NearlyEqual(distance4, 14.00000, C.EPS);

        var vec5 = new Vec3(-1, 2, 3);
        var distance5 = vec1.SquaredDistance(vec5);
        Helpers.NearlyEqual(distance5, 14.00000, C.EPS);

        var vec6 = new Vec3(1, 2, -3);
        var distance6 = vec1.SquaredDistance(vec6);
        Helpers.NearlyEqual(distance6, 14.00000, C.EPS);

        var vec7 = new Vec3(1, -2, -3);
        var distance7 = vec1.SquaredDistance(vec7);
        Helpers.NearlyEqual(distance7, 14.00000, C.EPS);

        var vec8 = new Vec3(-1, -2, -3);
        var distance8 = vec1.SquaredDistance(vec8);
        Helpers.NearlyEqual(distance8, 14.00000, C.EPS);

        var vec9 = new Vec3(-1, 2, -3);
        var distance9 = vec1.SquaredDistance(vec9);
        Helpers.NearlyEqual(distance9, 14.00000, C.EPS);
    }

    [Test]
    public void TestSquaredLength()
    {

        var vec1 = new Vec3(0, 0, 0);
        var length1 = vec1.SquaredLength();
        Helpers.NearlyEqual(length1, 0.0, C.EPS);

        var vec2 = new Vec3(1, 2, 3);
        var length2 = vec2.SquaredLength();
        Helpers.NearlyEqual(length2, 14.00000, C.EPS);

        var vec3 = new Vec3(1, -2, 3);
        var length3 = vec3.SquaredLength();
        Helpers.NearlyEqual(length3, 14.00000, C.EPS);

        var vec4 = new Vec3(-1, -2, 3);
        var length4 = vec4.SquaredLength();
        Helpers.NearlyEqual(length4, 14.00000, C.EPS);

        var vec5 = new Vec3(-1, 2, 3);
        var length5 = vec5.SquaredLength();
        Helpers.NearlyEqual(length5, 14.00000, C.EPS);

        var vec6 = new Vec3(1, 2, -3);
        var length6 = vec6.SquaredLength();
        Helpers.NearlyEqual(length6, 14.00000, C.EPS);

        var vec7 = new Vec3(1, -2, -3);
        var length7 = vec7.SquaredLength();
        Helpers.NearlyEqual(length7, 14.00000, C.EPS);

        var vec8 = new Vec3(-1, -2, -3);
        var length8 = vec8.SquaredLength();
        Helpers.NearlyEqual(length8, 14.00000, C.EPS);

        var vec9 = new Vec3(-1, 2, -3);
        var length9 = vec9.SquaredLength();
        Helpers.NearlyEqual(length9, 14.00000, C.EPS);
    }

    [Test]
    public void TestSubtract()
    {
        var ret1 = new Vec3(0, 0, 0).Subtract(new Vec3(0, 0, 0));
        Assert.IsTrue(ret1 == new Vec3(0, 0, 0));

        var ret2 = new Vec3(1, 2, 3).Subtract(new Vec3(3, 2, 1));
        Assert.IsTrue(ret2 == new Vec3(-2, 0, 2));

        var ret3 = new Vec3(1, 2, 3).Subtract(new Vec3(-1, -2, -3));
        Assert.IsTrue(ret3 == new Vec3(2, 4, 6));

        var ret4 = new Vec3(-1, -2, -3).Subtract(new Vec3(-1, -2, -3));
        Assert.IsTrue(ret4 == new Vec3(0, 0, 0));
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

        var ret1 = new Vec3(0, 0, 0).Transform(identityMatrix);
        Assert.IsTrue(ret1 == new Vec3(0, 0, 0));

        var ret2 = new Vec3(3, 2, 1).Transform(identityMatrix);
        Assert.IsTrue(ret2 == new Vec3(3, 2, 1));

        var x = 1;
        var y = 5;
        var z = 7;
        var translationMatrix = new Mat4(
          1, 0, 0, 0,
          0, 1, 0, 0,
          0, 0, 1, 0,
          x, y, z, 1
        );

        var ret3 = new Vec3(-1, -2, -3).Transform(translationMatrix);
        Assert.IsTrue(ret3 == new Vec3(0, 3, 4));

        var w = 1;
        var h = 3;
        var d = 5;
        var scaleMatrix = new Mat4(
          w, 0, 0, 0,
          0, h, 0, 0,
          0, 0, d, 0,
          0, 0, 0, 1
        );

        var ret4 = new Vec3(1, 2, 3).Transform(scaleMatrix);
        Assert.IsTrue(ret4 == new Vec3(1, 6, 15));

        var r = (90 * 0.017453292519943295);
        var RotateZMatrix = new Mat4(
          Math.Cos(r), -Math.Sin(r), 0, 0,
          Math.Sin(r), Math.Cos(r), 0, 0,
          0, 0, 1, 0,
          0, 0, 0, 1
        );

        var ret5 = new Vec3(1, 2, 3).Transform(RotateZMatrix);
        Assert.IsTrue(ret5.IsNearlyEqual(new Vec3(2, -1, 3)));
    }

}
