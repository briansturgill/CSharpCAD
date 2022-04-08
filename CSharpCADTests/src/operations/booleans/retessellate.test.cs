using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class RetessallateTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestRetesselation()
    {
        var box1 = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-5.0, -5.0, -5.0), new Vec3(-5.0, -5.0, 5.0), new Vec3(-5.0, 5.0, 5.0), new Vec3(-5.0, 5.0, -5.0)},
          new List<Vec3>{new Vec3(5.0, -5.0, -5.0), new Vec3(5.0, 5.0, -5.0), new Vec3(5.0, 5.0, 5.0), new Vec3(5.0, -5.0, 5.0)},
          new List<Vec3>{new Vec3(-5.0, -5.0, -5.0), new Vec3(5.0, -5.0, -5.0), new Vec3(5.0, -5.0, 5.0), new Vec3(-5.0, -5.0, 5.0)},
          new List<Vec3>{new Vec3(-5.0, 5.0, -5.0), new Vec3(-5.0, 5.0, 5.0), new Vec3(5.0, 5.0, 5.0), new Vec3(5.0, 5.0, -5.0)},
          new List<Vec3>{new Vec3(-5.0, -5.0, -5.0), new Vec3(-5.0, 5.0, -5.0), new Vec3(5.0, 5.0, -5.0), new Vec3(5.0, -5.0, -5.0)},
          new List<Vec3>{new Vec3(-5.0, -5.0, 5.0), new Vec3(5.0, -5.0, 5.0), new Vec3(5.0, 5.0, 5.0), new Vec3(-5.0, 5.0, 5.0)}
        };

        var box2 = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(15.0, 15.0, 15.0), new Vec3(15.0, 15.0, 25.0), new Vec3(15.0, 25.0, 25.0), new Vec3(15.0, 25.0, 15.0)},
          new List<Vec3>{new Vec3(25.0, 15.0, 15.0), new Vec3(25.0, 25.0, 15.0), new Vec3(25.0, 25.0, 25.0), new Vec3(25.0, 15.0, 25.0)},
          new List<Vec3>{new Vec3(15.0, 15.0, 15.0), new Vec3(25.0, 15.0, 15.0), new Vec3(25.0, 15.0, 25.0), new Vec3(15.0, 15.0, 25.0)},
          new List<Vec3>{new Vec3(15.0, 25.0, 15.0), new Vec3(15.0, 25.0, 25.0), new Vec3(25.0, 25.0, 25.0), new Vec3(25.0, 25.0, 15.0)},
          new List<Vec3>{new Vec3(15.0, 15.0, 15.0), new Vec3(15.0, 25.0, 15.0), new Vec3(25.0, 25.0, 15.0), new Vec3(25.0, 15.0, 15.0)},
          new List<Vec3>{new Vec3(15.0, 15.0, 25.0), new Vec3(25.0, 15.0, 25.0), new Vec3(25.0, 25.0, 25.0), new Vec3(15.0, 25.0, 25.0)}
        };

        var box3 = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-5.0, -5.0, -5.0), new Vec3(-5.0, -5.0, 5.0), new Vec3(-5.0, 5.0, 5.0), new Vec3(-5.0, 5.0, -5.0)},
          new List<Vec3>{new Vec3(5.0, -5.0, -5.0), new Vec3(5.0, 5.0, -5.0), new Vec3(5.0, 5.0, 5.0), new Vec3(5.0, -5.0, 5.0)},
          new List<Vec3>{new Vec3(-5.0, -5.0, -5.0), new Vec3(5.0, -5.0, -5.0), new Vec3(5.0, -5.0, 5.0), new Vec3(-5.0, -5.0, 5.0)},
          new List<Vec3>{new Vec3(-5.0, 5.0, -5.0), new Vec3(-5.0, 5.0, 5.0), new Vec3(5.0, 5.0, 5.0), new Vec3(5.0, 5.0, -5.0)},
          new List<Vec3>{new Vec3(-5.0, -5.0, -5.0), new Vec3(-5.0, 5.0, -5.0), new Vec3(5.0, 5.0, -5.0), new Vec3(5.0, -5.0, -5.0)},
          new List<Vec3>{new Vec3(-5.0, -5.0, 5.0), new Vec3(-5.0, -5.0, 15.0), new Vec3(-5.0, 5.0, 15.0), new Vec3(-5.0, 5.0, 5.0)},
          new List<Vec3>{new Vec3(5.0, -5.0, 5.0), new Vec3(5.0, 5.0, 5.0), new Vec3(5.0, 5.0, 15.0), new Vec3(5.0, -5.0, 15.0)},
          new List<Vec3>{new Vec3(-5.0, -5.0, 5.0), new Vec3(5.0, -5.0, 5.0), new Vec3(5.0, -5.0, 15.0), new Vec3(-5.0, -5.0, 15.0)},
          new List<Vec3>{new Vec3(-5.0, 5.0, 5.0), new Vec3(-5.0, 5.0, 15.0), new Vec3(5.0, 5.0, 15.0), new Vec3(5.0, 5.0, 5.0)},
          new List<Vec3>{new Vec3(-5.0, -5.0, 15.0), new Vec3(5.0, -5.0, 15.0), new Vec3(5.0, 5.0, 15.0), new Vec3(-5.0, 5.0, 15.0)}
        };

        var box4 = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-5.0, -5.0, -5.0), new Vec3(-5.0, -5.0, 5.0), new Vec3(-5.0, 5.0, 5.0), new Vec3(-5.0, 5.0, -5.0)},
          new List<Vec3>{new Vec3(-5.0, -5.0, -5.0), new Vec3(5.0, -5.0, -5.0), new Vec3(5.0, -5.0, 5.0), new Vec3(-5.0, -5.0, 5.0)},
          new List<Vec3>{new Vec3(-5.0, -5.0, -5.0), new Vec3(-5.0, 5.0, -5.0), new Vec3(5.0, 5.0, -5.0), new Vec3(5.0, -5.0, -5.0)},
          new List<Vec3>{new Vec3(5.0, -5.0, -5.0), new Vec3(5.0, 0.0, -5.0), new Vec3(5.0, 0.0, 5.0), new Vec3(5.0, -5.0, 5.0)},
          new List<Vec3>{new Vec3(-5.0, 5.0, -5.0), new Vec3(-5.0, 5.0, 5.0), new Vec3(0.0, 5.0, 5.0), new Vec3(0.0, 5.0, -5.0)},
          new List<Vec3>{new Vec3(-5.0, -5.0, 5.0), new Vec3(0.0, -5.0, 5.0), new Vec3(0.0, 5.0, 5.0), new Vec3(-5.0, 5.0, 5.0)},
          new List<Vec3>{new Vec3(5.0, 0.0, -5.0), new Vec3(5.0, 5.0, -5.0), new Vec3(5.0, 5.0, 0.0), new Vec3(5.0, 0.0, 0.0)},
          new List<Vec3>{new Vec3(5.0, 5.0, 0.0), new Vec3(5.0, 5.0, -5.0), new Vec3(0.0, 5.0, -5.0), new Vec3(0.0, 5.0, 0.0)},
          new List<Vec3>{new Vec3(0.0, -5.0, 5.0), new Vec3(5.0, -5.0, 5.0), new Vec3(5.0, 0.0, 5.0), new Vec3(0.0, 0.0, 5.0)},
          new List<Vec3>{new Vec3(10.0, 0.0, 0.0), new Vec3(10.0, 10.0, 0.0), new Vec3(10.0, 10.0, 10.0), new Vec3(10.0, 0.0, 10.0)},
          new List<Vec3>{new Vec3(0.0, 10.0, 0.0), new Vec3(0.0, 10.0, 10.0), new Vec3(10.0, 10.0, 10.0), new Vec3(10.0, 10.0, 0.0)},
          new List<Vec3>{new Vec3(0.0, 0.0, 10.0), new Vec3(10.0, 0.0, 10.0), new Vec3(10.0, 10.0, 10.0), new Vec3(0.0, 10.0, 10.0)},
          new List<Vec3>{new Vec3(0.0, 5.0, 10.0), new Vec3(0.0, 10.0, 10.0), new Vec3(0.0, 10.0, 0.0), new Vec3(0.0, 5.0, 0.0)},
          new List<Vec3>{new Vec3(5.0, 0.0, 0.0), new Vec3(10.0, 0.0, 0.0), new Vec3(10.0, 0.0, 10.0), new Vec3(5.0, 0.0, 10.0)},
          new List<Vec3>{new Vec3(5.0, 10.0, 0.0), new Vec3(10.0, 10.0, 0.0), new Vec3(10.0, 0.0, 0.0), new Vec3(5.0, 0.0, 0.0)},
          new List<Vec3>{new Vec3(0.0, 0.0, 5.0), new Vec3(0.0, 0.0, 10.0), new Vec3(0.0, 5.0, 10.0), new Vec3(0.0, 5.0, 5.0)},
          new List<Vec3>{new Vec3(5.0, 0.0, 5.0), new Vec3(5.0, 0.0, 10.0), new Vec3(0.0, 0.0, 10.0), new Vec3(0.0, 0.0, 5.0)},
          new List<Vec3>{new Vec3(0.0, 5.0, 0.0), new Vec3(0.0, 10.0, 0.0), new Vec3(5.0, 10.0, 0.0), new Vec3(5.0, 5.0, 0.0)}
        };

        var box5 = new List<List<Vec3>> { // with coplanar polygons
          new List<Vec3>{new Vec3(-5.0, -5.0, -5.0), new Vec3(-5.0, -5.0, 5.0), new Vec3(-5.0, 5.0, 5.0), new Vec3(-5.0, 5.0, -5.0)}, // end
          new List<Vec3>{new Vec3(10.0, -5.0, -5.0), new Vec3(10.0, -5.0, 5.0), new Vec3(-5.0, -5.0, 5.0), new Vec3(-5.0, -5.0, -5.0)}, // side
          new List<Vec3>{new Vec3(10.0, 5.0, 5.0), new Vec3(10.0, 5.0, -5.0), new Vec3(-5.0, 5.0, -5.0), new Vec3(-5.0, 5.0, 5.0)}, // side
          new List<Vec3>{new Vec3(10.0, 5.0, -5.0), new Vec3(10.0, -5.0, -5.0), new Vec3(-5.0, -5.0, -5.0), new Vec3(-5.0, 5.0, -5.0)}, // bottom
          new List<Vec3>{new Vec3(10.0, -5.0, 5.0), new Vec3(10.0, 0.0, 5.0), new Vec3(-5.0, 0.0, 5.0), new Vec3(-5.0, -5.0, 5.0)}, // top
          new List<Vec3>{new Vec3(10.0, 0.0, 5.0), new Vec3(10.0, 5.0, 5.0), new Vec3(-5.0, 5.0, 5.0), new Vec3(-5.0, 0.0, 5.0)}, // top
          new List<Vec3>{new Vec3(10.0, -5.0, -5.0), new Vec3(10.0, 5.0, -5.0), new Vec3(10.0, 5.0, 5.0), new Vec3(10.0, -5.0, 5.0)} // end
        };

        var obj1 = new Geom3(box1);
        var tmp = new List<List<Vec3>>();
        tmp.AddRange(box1);
        tmp.AddRange(box2);
        var obj2 = new Geom3(tmp); // combined geometry
        var obj3 = new Geom3(box3);
        var obj4 = new Geom3(box4);
        var obj5 = new Geom3(box5);

        // one solid geometry
        var ret1 = Retessellate(obj1);
        var pts1 = ret1.ToPoints();
        var exp1 = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-5, -5, -5), new Vec3(-5, -5, 5), new Vec3(-5, 5, 5), new Vec3(-5, 5, -5)},
          new List<Vec3>{new Vec3(5, -5, -5), new Vec3(5, 5, -5), new Vec3(5, 5, 5), new Vec3(5, -5, 5)},
          new List<Vec3>{new Vec3(-5, -5, -5), new Vec3(5, -5, -5), new Vec3(5, -5, 5), new Vec3(-5, -5, 5)},
          new List<Vec3>{new Vec3(-5, 5, -5), new Vec3(-5, 5, 5), new Vec3(5, 5, 5), new Vec3(5, 5, -5)},
          new List<Vec3>{new Vec3(-5, -5, -5), new Vec3(-5, 5, -5), new Vec3(5, 5, -5), new Vec3(5, -5, -5)},
          new List<Vec3>{new Vec3(-5, -5, 5), new Vec3(5, -5, 5), new Vec3(5, 5, 5), new Vec3(-5, 5, 5)}
        };
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts1, exp1));

        // two non-overlapping geometries
        var ret2 = Retessellate(obj2);
        var pts2 = ret2.ToPoints();
        var exp2 = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-5, -5, -5), new Vec3(-5, -5, 5), new Vec3(-5, 5, 5), new Vec3(-5, 5, -5)},
          new List<Vec3>{new Vec3(5, -5, -5), new Vec3(5, 5, -5), new Vec3(5, 5, 5), new Vec3(5, -5, 5)},
          new List<Vec3>{new Vec3(-5, -5, -5), new Vec3(5, -5, -5), new Vec3(5, -5, 5), new Vec3(-5, -5, 5)},
          new List<Vec3>{new Vec3(-5, 5, -5), new Vec3(-5, 5, 5), new Vec3(5, 5, 5), new Vec3(5, 5, -5)},
          new List<Vec3>{new Vec3(-5, -5, -5), new Vec3(-5, 5, -5), new Vec3(5, 5, -5), new Vec3(5, -5, -5)},
          new List<Vec3>{new Vec3(-5, -5, 5), new Vec3(5, -5, 5), new Vec3(5, 5, 5), new Vec3(-5, 5, 5)},
          new List<Vec3>{new Vec3(15, 15, 15), new Vec3(15, 15, 25), new Vec3(15, 25, 25), new Vec3(15, 25, 15)},
          new List<Vec3>{new Vec3(25, 15, 15), new Vec3(25, 25, 15), new Vec3(25, 25, 25), new Vec3(25, 15, 25)},
          new List<Vec3>{new Vec3(15, 15, 15), new Vec3(25, 15, 15), new Vec3(25, 15, 25), new Vec3(15, 15, 25)},
          new List<Vec3>{new Vec3(15, 25, 15), new Vec3(15, 25, 25), new Vec3(25, 25, 25), new Vec3(25, 25, 15)},
          new List<Vec3>{new Vec3(15, 15, 15), new Vec3(15, 25, 15), new Vec3(25, 25, 15), new Vec3(25, 15, 15)},
          new List<Vec3>{new Vec3(15, 15, 25), new Vec3(25, 15, 25), new Vec3(25, 25, 25), new Vec3(15, 25, 25)}
        };
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts2, exp2));

        // two touching geometries (faces)
        var ret3 = Retessellate(obj3);
        var pts3 = ret3.ToPoints();
        var exp3 = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-5, 5, 15), new Vec3(-5, 5, -5), new Vec3(-5, -5, -5), new Vec3(-5, -5, 15)},
          new List<Vec3>{new Vec3(5, -5, 15), new Vec3(5, -5, -5), new Vec3(5, 5, -5), new Vec3(5, 5, 15)},
          new List<Vec3>{new Vec3(-5, -5, 15), new Vec3(-5, -5, -5), new Vec3(5, -5, -5), new Vec3(5, -5, 15)},
          new List<Vec3>{new Vec3(5, 5, 15), new Vec3(5, 5, -5), new Vec3(-5, 5, -5), new Vec3(-5, 5, 15)},
          new List<Vec3>{new Vec3(-5, -5, -5), new Vec3(-5, 5, -5), new Vec3(5, 5, -5), new Vec3(5, -5, -5)},
          new List<Vec3>{new Vec3(-5, -5, 15), new Vec3(5, -5, 15), new Vec3(5, 5, 15), new Vec3(-5, 5, 15)}
        };
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts3, exp3));

        // two overlapping geometries
        var ret4 = Retessellate(obj4);
        var pts4 = ret4.ToPoints();
        var exp4 = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-5, -5, -5), new Vec3(-5, -5, 5), new Vec3(-5, 5, 5), new Vec3(-5, 5, -5)},
          new List<Vec3>{new Vec3(-5, -5, -5), new Vec3(5, -5, -5), new Vec3(5, -5, 5), new Vec3(-5, -5, 5)},
          new List<Vec3>{new Vec3(-5, -5, -5), new Vec3(-5, 5, -5), new Vec3(5, 5, -5), new Vec3(5, -5, -5)},
          new List<Vec3>{new Vec3(5, -5, 5), new Vec3(5, -5, 0), new Vec3(5, 0, 0), new Vec3(5, 0, 5)},
          new List<Vec3>{new Vec3(5, -5, 0), new Vec3(5, -5, -5), new Vec3(5, 5, -5), new Vec3(5, 5, 0)},
          new List<Vec3>{new Vec3(0, 5, 5), new Vec3(0, 5, 0), new Vec3(-5, 5, 0), new Vec3(-5, 5, 5)},
          new List<Vec3>{new Vec3(5, 5, 0), new Vec3(5, 5, -5), new Vec3(-5, 5, -5), new Vec3(-5, 5, 0)},
          new List<Vec3>{new Vec3(-5, 5, 5), new Vec3(-5, 0, 5), new Vec3(0, 0, 5), new Vec3(0, 5, 5)},
          new List<Vec3>{new Vec3(-5, 0, 5), new Vec3(-5, -5, 5), new Vec3(5, -5, 5), new Vec3(5, 0, 5)},
          new List<Vec3>{new Vec3(10, 0, 0), new Vec3(10, 10, 0), new Vec3(10, 10, 10), new Vec3(10, 0, 10)},
          new List<Vec3>{new Vec3(0, 10, 0), new Vec3(0, 10, 10), new Vec3(10, 10, 10), new Vec3(10, 10, 0)},
          new List<Vec3>{new Vec3(0, 0, 10), new Vec3(10, 0, 10), new Vec3(10, 10, 10), new Vec3(0, 10, 10)},
          new List<Vec3>{new Vec3(0, 10, 10), new Vec3(0, 10, 5), new Vec3(0, 0, 5), new Vec3(0, 0, 10)},
          new List<Vec3>{new Vec3(0, 10, 5), new Vec3(0, 10, 0), new Vec3(0, 5, 0), new Vec3(0, 5, 5)},
          new List<Vec3>{new Vec3(0, 0, 10), new Vec3(0, 0, 5), new Vec3(10, 0, 5), new Vec3(10, 0, 10)},
          new List<Vec3>{new Vec3(5, 0, 5), new Vec3(5, -0, 0), new Vec3(10, -0, 0), new Vec3(10, 0, 5)},
          new List<Vec3>{new Vec3(10, 10, 0), new Vec3(10, 5, 0), new Vec3(0, 5, 0), new Vec3(0, 10, 0)},
          new List<Vec3>{new Vec3(10, 5, 0), new Vec3(10, 0, 0), new Vec3(5, 0, 0), new Vec3(5, 5, 0)}
        };
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts4, exp4));

        // coplanar polygons
        var ret5 = Retessellate(obj5);
        var pts5 = ret5.ToPoints();
        var exp5 = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-5, -5, -5), new Vec3(-5, -5, 5), new Vec3(-5, 5, 5), new Vec3(-5, 5, -5)},
          new List<Vec3>{new Vec3(10, -5, -5), new Vec3(10, -5, 5), new Vec3(-5, -5, 5), new Vec3(-5, -5, -5)},
          new List<Vec3>{new Vec3(10, 5, 5), new Vec3(10, 5, -5), new Vec3(-5, 5, -5), new Vec3(-5, 5, 5)},
          new List<Vec3>{new Vec3(10, 5, -5), new Vec3(10, -5, -5), new Vec3(-5, -5, -5), new Vec3(-5, 5, -5)},
          new List<Vec3>{new Vec3(-5, 5, 5), new Vec3(-5, -5, 5), new Vec3(10, -5, 5), new Vec3(10, 5, 5)},
          new List<Vec3>{new Vec3(10, -5, -5), new Vec3(10, 5, -5), new Vec3(10, 5, 5), new Vec3(10, -5, 5)}
        };
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts5, exp5));
    }
}