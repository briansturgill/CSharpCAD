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
}