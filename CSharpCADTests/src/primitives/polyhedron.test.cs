using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class PolyhedronTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestPolyhedron()
    {
        // points and faces form a cube
        var points = new Points3 { new Vec3(-1, -1, -1), new Vec3(-1, -1, 1), new Vec3(-1, 1, 1),
              new Vec3(-1, 1, -1), new Vec3(1, -1, 1), new Vec3(1, -1, -1), new Vec3(1, 1, -1), new Vec3(1, 1, 1) };
        var faces = new Faces{new Face{0, 1, 2, 3}, new Face{5, 6, 7, 4},
            new Face{0, 5, 4, 1}, new Face{3, 2, 7, 6}, new Face{0, 3, 6, 5}, new Face{1, 4, 7, 2}};
        var colors = new List<Color>{new Color(0, 0, 0, 255), new Color(1, 0, 0, 255), new Color(0, 255, 0, 255), new Color(0, 0, 255, 255),
            new Color((byte)(255*0.5),(byte)(255*0.5),(byte)(255*0.5), 255), new Color(255, 255, 255, 255)};
        var obs = Polyhedron(points: points, faces: faces, colors: colors);
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();
        var exp = new List<List<Vec3>> {
          new List<Vec3> {new Vec3(-1.0000000, -1.0000000, -1.0000000),
            new Vec3(-1.0000000, -1.0000000, 1.0000000),
            new Vec3(-1.0000000, 1.0000000, 1.0000000),
            new Vec3(-1.0000000, 1.0000000, -1.0000000)},
          new List<Vec3> {new Vec3(1.0000000, -1.0000000, -1.0000000),
            new Vec3(1.0000000, 1.0000000, -1.0000000),
            new Vec3(1.0000000, 1.0000000, 1.0000000),
            new Vec3(1.0000000, -1.0000000, 1.0000000)},
          new List<Vec3> {new Vec3(-1.0000000, -1.0000000, -1.0000000),
            new Vec3(1.0000000, -1.0000000, -1.0000000),
            new Vec3(1.0000000, -1.0000000, 1.0000000),
            new Vec3(-1.0000000, -1.0000000, 1.0000000)},
          new List<Vec3> {new Vec3(-1.0000000, 1.0000000, -1.0000000),
            new Vec3(-1.0000000, 1.0000000, 1.0000000),
            new Vec3(1.0000000, 1.0000000, 1.0000000),
            new Vec3(1.0000000, 1.0000000, -1.0000000)},
          new List<Vec3> {new Vec3(-1.0000000, -1.0000000, -1.0000000),
            new Vec3(-1.0000000, 1.0000000, -1.0000000),
            new Vec3(1.0000000, 1.0000000, -1.0000000),
            new Vec3(1.0000000, -1.0000000, -1.0000000)},
          new List<Vec3> {new Vec3(-1.0000000, -1.0000000, 1.0000000),
            new Vec3(1.0000000, -1.0000000, 1.0000000),
            new Vec3(1.0000000, 1.0000000, 1.0000000),
            new Vec3(-1.0000000, 1.0000000, 1.0000000)}
        };
        Assert.AreEqual(pts.Count, 6);
        Assert.IsTrue(Helpers.CompareListOfLists(pts, exp));

        // test orientation
        points = new Points3 { new Vec3(10, 10, 0), new Vec3(10, -10, 0), new Vec3(-10, -10, 0), new Vec3(-10, 10, 0), new Vec3(0, 0, 10) };
        faces = new Faces{new Face{0, 1, 4}, new Face{1, 2, 4}, new Face{2, 3, 4},
              new Face{3, 0, 4}, new Face{1, 0, 3}, new Face{2, 1, 3}};
        obs = Polyhedron(points: points, faces: faces, orientationOutward: false);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        exp = new List<List<Vec3>> {
          new List<Vec3> {new Vec3(0, 0, 10),
            new Vec3(10, -10, 0),
            new Vec3(10, 10, 0)},
          new List<Vec3> {new Vec3(0, 0, 10),
            new Vec3(-10, -10, 0),
            new Vec3(10, -10, 0)},
          new List<Vec3> {new Vec3(0, 0, 10),
            new Vec3(-10, 10, 0),
            new Vec3(-10, -10, 0)},
          new List<Vec3> {new Vec3(0, 0, 10),
            new Vec3(10, 10, 0),
            new Vec3(-10, 10, 0)},
          new List<Vec3> {new Vec3(-10, 10, 0),
            new Vec3(10, 10, 0),
            new Vec3(10, -10, 0)},
          new List<Vec3> {new Vec3(-10, 10, 0),
            new Vec3(10, -10, 0),
            new Vec3(-10, -10, 0)}
        };
        Assert.AreEqual(pts.Count, 6);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }

}
