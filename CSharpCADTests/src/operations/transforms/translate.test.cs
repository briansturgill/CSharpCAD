namespace CSharpCADTests;

[TestFixture]
public class TranslateTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestTranslateGeom2()
    {
        var geometry = new Geom2(new List<Vec2> { new Vec2(0, 0), new Vec2(1, 0), new Vec2(0, 1) });

        // translate X
        var translated = Translate(new Vec2(1, 0), geometry);
        Assert.DoesNotThrow(() => translated.Validate());
        var obs = translated.ToPoints();
        var exp = new Vec2[] { new Vec2(1, 0), new Vec2(2, 0), new Vec2(1, 1) };
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        translated = TranslateX(1, geometry);
        Assert.DoesNotThrow(() => translated.Validate());
        obs = translated.ToPoints();
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // translate Y
        translated = Translate(new Vec2(0, 1), geometry);
        Assert.DoesNotThrow(() => translated.Validate());
        obs = translated.ToPoints();
        exp = new Vec2[] { new Vec2(0, 1), new Vec2(1, 1), new Vec2(0, 2) };
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        translated = TranslateY(1, geometry);
        Assert.DoesNotThrow(() => translated.Validate());
        obs = translated.ToPoints();
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));
    }

    [Test]
    public void TestTranslateGeom3()
    {
        var points = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-2, -7, -12), new Vec3(-2, -7, 18), new Vec3(-2, 13, 18), new Vec3(-2, 13, -12)},
          new List<Vec3>{new Vec3(8, -7, -12), new Vec3(8, 13, -12), new Vec3(8, 13, 18), new Vec3(8, -7, 18)},
          new List<Vec3>{new Vec3(-2, -7, -12), new Vec3(8, -7, -12), new Vec3(8, -7, 18), new Vec3(-2, -7, 18)},
          new List<Vec3>{new Vec3(-2, 13, -12), new Vec3(-2, 13, 18), new Vec3(8, 13, 18), new Vec3(8, 13, -12)},
          new List<Vec3>{new Vec3(-2, -7, -12), new Vec3(-2, 13, -12), new Vec3(8, 13, -12), new Vec3(8, -7, -12)},
          new List<Vec3>{new Vec3(-2, -7, 18), new Vec3(8, -7, 18), new Vec3(8, 13, 18), new Vec3(-2, 13, 18)}
        };
        var geometry = new Geom3(points);

        // translate X
        var translated = Translate(new Vec3(3, 0, 0), geometry);
        Assert.DoesNotThrow(() => translated.Validate());
        var obs = translated.ToPoints();
        var exp = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(1, -7, -12), new Vec3(1, -7, 18), new Vec3(1, 13, 18), new Vec3(1, 13, -12)},
          new List<Vec3>{new Vec3(11, -7, -12), new Vec3(11, 13, -12), new Vec3(11, 13, 18), new Vec3(11, -7, 18)},
          new List<Vec3>{new Vec3(1, -7, -12), new Vec3(11, -7, -12), new Vec3(11, -7, 18), new Vec3(1, -7, 18)},
          new List<Vec3>{new Vec3(1, 13, -12), new Vec3(1, 13, 18), new Vec3(11, 13, 18), new Vec3(11, 13, -12)},
          new List<Vec3>{new Vec3(1, -7, -12), new Vec3(1, 13, -12), new Vec3(11, 13, -12), new Vec3(11, -7, -12)},
          new List<Vec3>{new Vec3(1, -7, 18), new Vec3(11, -7, 18), new Vec3(11, 13, 18), new Vec3(1, 13, 18)}
        };
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        translated = TranslateX(3, geometry);
        Assert.DoesNotThrow(() => translated.Validate());
        obs = translated.ToPoints();
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        // translated Y
        translated = Translate(new Vec3(0, 3, 0), geometry);
        Assert.DoesNotThrow(() => translated.Validate());
        obs = translated.ToPoints();
        exp = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-2, -4, -12), new Vec3(-2, -4, 18), new Vec3(-2, 16, 18), new Vec3(-2, 16, -12)},
          new List<Vec3>{new Vec3(8, -4, -12), new Vec3(8, 16, -12), new Vec3(8, 16, 18), new Vec3(8, -4, 18)},
          new List<Vec3>{new Vec3(-2, -4, -12), new Vec3(8, -4, -12), new Vec3(8, -4, 18), new Vec3(-2, -4, 18)},
          new List<Vec3>{new Vec3(-2, 16, -12), new Vec3(-2, 16, 18), new Vec3(8, 16, 18), new Vec3(8, 16, -12)},
          new List<Vec3>{new Vec3(-2, -4, -12), new Vec3(-2, 16, -12), new Vec3(8, 16, -12), new Vec3(8, -4, -12)},
          new List<Vec3>{new Vec3(-2, -4, 18), new Vec3(8, -4, 18), new Vec3(8, 16, 18), new Vec3(-2, 16, 18)}
        };
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        translated = TranslateY(3, geometry);
        Assert.DoesNotThrow(() => translated.Validate());
        obs = translated.ToPoints();
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        // translate Z
        translated = Translate(new Vec3(0, 0, 3), geometry);
        Assert.DoesNotThrow(() => translated.Validate());
        obs = translated.ToPoints();
        exp = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-2, -7, -9), new Vec3(-2, -7, 21), new Vec3(-2, 13, 21), new Vec3(-2, 13, -9)},
          new List<Vec3>{new Vec3(8, -7, -9), new Vec3(8, 13, -9), new Vec3(8, 13, 21), new Vec3(8, -7, 21)},
          new List<Vec3>{new Vec3(-2, -7, -9), new Vec3(8, -7, -9), new Vec3(8, -7, 21), new Vec3(-2, -7, 21)},
          new List<Vec3>{new Vec3(-2, 13, -9), new Vec3(-2, 13, 21), new Vec3(8, 13, 21), new Vec3(8, 13, -9)},
          new List<Vec3>{new Vec3(-2, -7, -9), new Vec3(-2, 13, -9), new Vec3(8, 13, -9), new Vec3(8, -7, -9)},
          new List<Vec3>{new Vec3(-2, -7, 21), new Vec3(8, -7, 21), new Vec3(8, 13, 21), new Vec3(-2, 13, 21)}
        };
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        translated = TranslateZ(3, geometry);
        Assert.DoesNotThrow(() => translated.Validate());
        obs = translated.ToPoints();
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));
    }

    [Test]
    public void TestTranslateMisc()
    {
        var geometry2 = new Geom2(new List<Vec2> { new Vec2(10, -5), new Vec2(0, 5), new Vec2(-5, -5) });

        var translated = Translate(new Vec2(3, 3), geometry2);
        Assert.DoesNotThrow(() => translated.Validate());

        var obs = translated.ToPoints();
        var exp = new Vec2[] { new Vec2(13, -2), new Vec2(3, 8), new Vec2(-2, -2) };
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));
    }
}
