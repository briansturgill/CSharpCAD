namespace CSharpCADTests;

[TestFixture]
public class ProjectTests
{
    static bool WriteTests = false;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestProjectDefaults()
    {
        var result = Project(new Geom3());

        result = Project(Torus(outerSegments: 4));
        Assert.DoesNotThrow(() => result.Validate());
        var pts = result.ToPoints();
        if(WriteTests) TestData.Make("ProjectDefExp1", pts);
        var exp = UnitTestData.ProjectDefExp1;
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));
    }

    [Test]
    public void TestProjectXandYAxis()
    {
        var result = Project(Torus(outerSegments: 4), axis: new Vec3(1, 0, 0), origin: new Vec3(1, 0, 0));
        Assert.DoesNotThrow(() => result.Validate());
        var pts = result.ToPoints();
        if(WriteTests) TestData.Make("ProjectXandYAxisExp1", pts);
        var exp = UnitTestData.ProjectXandYAxisExp1;
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));

        result = Project(Torus(outerSegments: 4), axis: new Vec3(0, 1, 0), origin: new Vec3(0, -1, 0));
        Assert.DoesNotThrow(() => result.Validate());
        pts = result.ToPoints();
        if(WriteTests) TestData.Make("ProjectXandYAxisExp2", pts);
        exp = UnitTestData.ProjectXandYAxisExp2;
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));
    }
}