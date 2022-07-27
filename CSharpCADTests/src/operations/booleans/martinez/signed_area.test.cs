namespace CSharpCADTests;

#nullable disable

using static Geom2Booleans;

[TestFixture]
public class MartinezTestsSignedArea
{
    //static bool WriteTests = false;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestAnalyticalSignedArea()
    {
        Assert.AreEqual(signedArea(new Vec2(0, 0), new Vec2(0, 1), new Vec2(1, 1)), -1, "negative area");
        Assert.AreEqual(signedArea(new Vec2(0, 1), new Vec2(0, 0), new Vec2(1, 0)),  1, "positive area");
        Assert.AreEqual(signedArea(new Vec2(0, 0), new Vec2(1, 1), new Vec2(2, 2)),  0, "collinear, 0 area");

        Assert.AreEqual(signedArea(new Vec2(-1, 0), new Vec2(2, 3), new Vec2(0, 1)), 0, "point on segment");
        Assert.AreEqual(signedArea(new Vec2(2, 3), new Vec2(-1, 0), new Vec2(0, 1)), 0, "point on segment");
    }
}
