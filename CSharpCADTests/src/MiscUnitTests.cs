using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class MiscTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestDegToRad()
    {
        Assert.IsTrue(DegToRad(360) == Math.PI * 2);
        Assert.IsTrue(DegToRad(180) == Math.PI);
        Assert.IsTrue(DegToRad(90) == Math.PI / 2);
        Assert.IsTrue(DegToRad(45) == Math.PI / 4);
    }

    [Test]
    public void TestRadToDeg()
    {
        Assert.IsTrue(RadToDeg(Math.PI * 2) == 360);
        Assert.IsTrue(RadToDeg(Math.PI) == 180);
        Assert.IsTrue(RadToDeg(Math.PI / 2) == 90);
        Assert.IsTrue(RadToDeg(Math.PI / 4) == 45);
    }

    [Test]
    public void TestDegToRadAndBack()
    {
        bool fail = false;
        for (int i = 0; i <= 360*10; i++)
        {
            if (Math.Round(RadToDeg(DegToRad(i)), 12) != i)
            {
                fail = true;
                Console.WriteLine($"{i}, {DegToRad(i)}, {RadToDeg(DegToRad(i))}");
            }
        }
        if (fail) Assert.Fail("Failure in TestDegToRadAndBack");
    }

    [Test]
    public void TestAcosCosDegree()
    {
        bool fail = false;
        for (int i = 0; i <= 180; i++)
        {
            if (Math.Round(Acos(Cos(i)), 12) != i)
            {
                fail = true;
                Console.WriteLine($"{i}, {Cos(i)}, {Acos(Cos(i))}");
            }
        }
        if (fail) Assert.Fail("Failure in TestAcosCosDegree");
    }

    [Test]
    public void TestMathAcosCosDegree()
    {
        bool fail = false;
        for (int i = 0; i <= 180; i++)
        {
            if (Math.Round(RadToDeg(Math.Acos(Math.Cos(DegToRad(i)))), 12) != i)
            {
                fail = true;
                Console.WriteLine($"{i}, {Math.Cos(i)}, {Math.Acos(Math.Cos(i))}");
            }
        }
        if (fail) Assert.Fail("Failure in TestMathAcosCosDegree");
    }

    [Test]
    public void TestAsinSinDegree()
    {
        bool fail = false;
        for (int i = 0; i <= 90; i++)
        {
            if (Math.Round(Asin(Sin(i)), 12) != i)
            {
                fail = true;
                Console.WriteLine($"{i}, {Sin(i)}, {Asin(Sin(i))}");
            }
        }
        if (fail) Assert.Fail("Failure in TestAsinSinDegree");
    }

    [Test]
    public void TestMathAsinSinDegree()
    {
        bool fail = false;
        for (int i = 0; i <= 90; i++)
        {
            if (Math.Round(RadToDeg(Math.Asin(Math.Sin(DegToRad(i)))), 12) != i)
            {
                fail = true;
                Console.WriteLine($"{i}, {Math.Sin(i)}, {Math.Asin(Math.Sin(i))}");
            }
        }
        if (fail) Assert.Fail("Failure in TestMathAsinSinDegree");
    }

    [Test]
    public void TestAtanTanDegree()
    {
        bool fail = false;
        for (int i = 0; i <= 90; i++)
        {
            if (Math.Round(Atan(Tan(i)), 12) != i)
            {
                fail = true;
                Console.WriteLine($"{i}, {Tan(i)}, {Atan(Tan(i))}");
            }
        }
        if (fail) Assert.Fail("Failure in TestAtanTanDegree");
    }

    [Test]
    public void TestMathAtanTanDegree()
    {
        bool fail = false;
        for (int i = 0; i <= 90; i++)
        {
            if (Math.Round(RadToDeg(Math.Atan(Math.Tan(DegToRad(i)))), 12) != i)
            {
                fail = true;
                Console.WriteLine($"{i}, {Math.Tan(i)}, {Math.Atan(Math.Tan(i))}");
            }
        }
        if (fail) Assert.Fail("Failure in TestMathAtanTanDegree");
    }

    [Test]
    public void TestAcoshCoshDegree()
    {
        bool fail = false;
        for (int i = 0; i <= 180; i++)
        {
            if (Math.Round(Acosh(Cosh(i)), 12) != i)
            {
                fail = true;
                Console.WriteLine($"{i}, {Cosh(i)}, {Acosh(Cosh(i))}");
            }
        }
        if (fail) Assert.Fail("Failure in TestAcoshCoshDegree");
    }

    [Test]
    public void TestMathAcoshCoshDegree()
    {
        bool fail = false;
        for (int i = 0; i <= 180; i++)
        {
            if (Math.Round(RadToDeg(Math.Acosh(Math.Cosh(DegToRad(i)))), 12) != i)
            {
                fail = true;
                Console.WriteLine($"{i}, {Math.Cosh(i)}, {Math.Acosh(Math.Cosh(i))}");
            }
        }
        if (fail) Assert.Fail("Failure in TestMathAcoshCoshDegree");
    }

    [Test]
    public void TestAsinhSinhDegree()
    {
        bool fail = false;
        for (int i = 0; i <= 90; i++)
        {
            if (Math.Round(Asinh(Sinh(i)), 12) != i)
            {
                fail = true;
                Console.WriteLine($"{i}, {Sinh(i)}, {Asinh(Sinh(i))}");
            }
        }
        if (fail) Assert.Fail("Failure in TestAsinhSinhDegree");
    }

    [Test]
    public void TestMathAsinhSinhDegree()
    {
        bool fail = false;
        for (int i = 0; i <= 90; i++)
        {
            if (Math.Round(RadToDeg(Math.Asinh(Math.Sinh(DegToRad(i)))), 12) != i)
            {
                fail = true;
                Console.WriteLine($"{i}, {Math.Sinh(i)}, {Math.Asinh(Math.Sinh(i))}");
            }
        }
        if (fail) Assert.Fail("Failure in TestMathAsinhSinhDegree");
    }

    [Test]
    public void TestAtanhTanhDegree()
    {
        bool fail = false;
        for (int i = 0; i <= 90; i++)
        {
            if (Math.Round(Atanh(Tanh(i)), 12) != i)
            {
                fail = true;
                Console.WriteLine($"{i}, {Tanh(i)}, {Atanh(Tanh(i))}");
            }
        }
        if (fail) Assert.Fail("Failure in TestAtanhTanhDegree");
    }

    [Test]
    public void TestMathAtanhTanhDegree()
    {
        bool fail = false;
        for (int i = 0; i <= 90; i++)
        {
            if (Math.Round(RadToDeg(Math.Atanh(Math.Tanh(DegToRad(i)))), 12) != i)
            {
                fail = true;
                Console.WriteLine($"{i}, {Math.Tanh(i)}, {Math.Atanh(Math.Tanh(i))}");
            }
        }
        if (fail) Assert.Fail("Failure in TestMathAtanhTanhDegree");
    }
}