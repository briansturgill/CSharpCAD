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
    public void TestOpts()
    {
        var opts = new Opts {
            {"vec2ii", (1,1)},
            {"vec2id", (1,1.0)},
            {"vec2di", (1.0,1)},
            {"vec2dd", (1.0,1.0)},
            {"vec3iii", (1,1,1)},
            {"vec3iid", (1,1,1.0)},
            {"vec3idi", (1,1.0,1)},
            {"vec3idd", (1,1.0,1.0)},
            {"vec3dii", (1.0,1,1)},
            {"vec3did", (1.0,1,1.0)},
            {"vec3ddi", (1.0,1,1.0)},
            {"vec3ddd", (1.0,1.0,1.0)},
            {"string", "42.0"},
            {"double", 42.0},
            {"doubleasi", 42},
            {"int", 42},
            {"bool", true},
            {"colorList", new List<Color>()},
            {"lv3", new List<Vec3>()},
            {"lv2", new List<Vec2>()},
            {"lli", new List<List<int>>()},
        };
        Assert.IsTrue(opts.GetVec2("vec2ii", (57.3, 58.3)) == new Vec2(1.0,1.0));
        Assert.IsTrue(opts.GetVec2("vec2id", (57.3, 58.3)) == new Vec2(1.0,1.0));
        Assert.IsTrue(opts.GetVec2("vec2di", (57.3, 58.3)) == new Vec2(1.0,1.0));
        Assert.IsTrue(opts.GetVec2("vec2dd", (57.3, 58.3)) == new Vec2(1.0,1.0));
        Assert.IsTrue(opts.GetVec2("vec2CheckDefault", (57.3, 58.3)) == new Vec2(57.3,58.3));
        Assert.IsTrue(opts.GetString("string", "default") == "42.0");
        Assert.IsTrue(opts.GetString("stringCheckDefault", "default") == "default");
        Assert.IsTrue(opts.GetDouble("double", 58.9) == 42.0);
        Assert.IsTrue(opts.GetDouble("doubleasi", 58.9) == 42.0);
        Assert.IsTrue(opts.GetDouble("doubleCheckDefault", 58.9) == 58.9);
        Assert.IsTrue(opts.GetInt("int", 57) == 42);
        Assert.IsTrue(opts.GetInt("intCheckDefault", 57) == 57);
        Assert.IsTrue(opts.GetBool("bool", false) == true);
        Assert.IsTrue(opts.GetBool("boolCheckDefault", false) == false);
        Assert.IsTrue(opts.GetListOfColor("colorList", new List<Color>{new Color("brown")}).Count == 0);
        Assert.IsTrue(opts.GetListOfColor("colorListCheckDefault", new List<Color>{new Color("brown")}).Count == 1);
        Assert.IsTrue(opts.GetListOfVec2("lv2", new List<Vec2>{new Vec2(57.3, 58.3)}).Count == 0);
        Assert.IsTrue(opts.GetListOfVec2("lv2CheckDefault", new List<Vec2>{new Vec2(57.3, 58.3)}).Count == 1);
        Assert.IsTrue(opts.GetListOfVec3("lv3", new List<Vec3>{new Vec3(57.3, 58.3, 20.3)}).Count == 0);
        Assert.IsTrue(opts.GetListOfVec3("lv3CheckDefault", new List<Vec3>{new Vec3(57.3, 58.3, 20.3)}).Count == 1);
        Assert.IsTrue(opts.GetListOfListOfInt("lli", new List<List<int>>{new List<int>{1,2,3}, new List<int>{8,5,9,4}}).Count == 0);
        Assert.IsTrue(opts.GetListOfListOfInt("lliCheckDefault", new List<List<int>>{new List<int>{1,2,3}, new List<int>{8,5,9,4}}).Count == 2);
    }
}