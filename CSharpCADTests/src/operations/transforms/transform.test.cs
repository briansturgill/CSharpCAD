namespace CSharpCADTests;

[TestFixture]
public class TransformTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestTransformGeom2()
    {
        var matrix = Mat4.FromScaling(new Vec3(5, 5, 5));
        var geometry = new Geom2(new List<Vec2> { new Vec2(0, 0), new Vec2(1, 0), new Vec2(0, 1) });

        geometry = (Geom2)Transform(matrix, geometry);
        Assert.DoesNotThrow(() => geometry.Validate());
        var obs = geometry.ToPoints();
        var exp = new Vec2[] { new Vec2(0, 0), new Vec2(5, 0), new Vec2(0, 5) };
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));
    }

    [Test]
    public void TestTransformGeom3()
    {
        var matrix = Mat4.FromTranslation(new Vec3(-3, -3, -3));
        var points = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-2, -7, -12), new Vec3(-2, -7, 18), new Vec3(-2, 13, 18), new Vec3(-2, 13, -12)},
          new List<Vec3>{new Vec3(8, -7, -12), new Vec3(8, 13, -12), new Vec3(8, 13, 18), new Vec3(8, -7, 18)},
          new List<Vec3>{new Vec3(-2, -7, -12), new Vec3(8, -7, -12), new Vec3(8, -7, 18), new Vec3(-2, -7, 18)},
          new List<Vec3>{new Vec3(-2, 13, -12), new Vec3(-2, 13, 18), new Vec3(8, 13, 18), new Vec3(8, 13, -12)},
          new List<Vec3>{new Vec3(-2, -7, -12), new Vec3(-2, 13, -12), new Vec3(8, 13, -12), new Vec3(8, -7, -12)},
          new List<Vec3>{new Vec3(-2, -7, 18), new Vec3(8, -7, 18), new Vec3(8, 13, 18), new Vec3(-2, 13, 18)}
        };
        var geometry = new Geom3(points);
        geometry = (Geom3)Transform(matrix, geometry);
        Assert.DoesNotThrow(() => geometry.Validate());
        var obs = geometry.ToPoints();
        var exp = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-5, -10, -15), new Vec3(-5, -10, 15), new Vec3(-5, 10, 15), new Vec3(-5, 10, -15)},
          new List<Vec3>{new Vec3(5, -10, -15), new Vec3(5, 10, -15), new Vec3(5, 10, 15), new Vec3(5, -10, 15)},
          new List<Vec3>{new Vec3(-5, -10, -15), new Vec3(5, -10, -15), new Vec3(5, -10, 15), new Vec3(-5, -10, 15)},
          new List<Vec3>{new Vec3(-5, 10, -15), new Vec3(-5, 10, 15), new Vec3(5, 10, 15), new Vec3(5, 10, -15)},
          new List<Vec3>{new Vec3(-5, -10, -15), new Vec3(-5, 10, -15), new Vec3(5, 10, -15), new Vec3(5, -10, -15)},
          new List<Vec3>{new Vec3(-5, -10, 15), new Vec3(5, -10, 15), new Vec3(5, 10, 15), new Vec3(-5, 10, 15)}
        };
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));
    }

    [Test]
    public void TestTransformMisc()
    {
        var geometry2 = new Geom2(new List<Vec2> { new Vec2(-5, -5), new Vec2(0, 5), new Vec2(10, -5) });

        var matrix = Mat4.FromTranslation(new Vec3(2, 2, 0));
        var transformed = (Geom2)Transform(matrix, geometry2);
        Assert.DoesNotThrow(() => transformed.Validate());

        var obs = transformed.ToPoints();
        var exp = new Vec2[] { new Vec2(-3, -3), new Vec2(2, 7), new Vec2(12, -3) };
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));
    }
}