using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class ExtrudeWalls
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestExtrudeWallsSameShape()
    {
        var matrix = Mat4.FromTranslation(new Vec3(0, 0, 10));

        var shape0 = new Geom2.Side[] {new Geom2.Side()};
        var shape1 = new Geom2.Side[] {
            new Geom2.Side(new Vec2(-10.0, 10.0), new Vec2(-10.0, -10.0)),
            new Geom2.Side(new Vec2(-10.0, -10.0), new Vec2(10.0, -10.0)),
            new Geom2.Side(new Vec2(10.0, -10.0), new Vec2(10.0, 10.0)),
            new Geom2.Side(new Vec2(10.0, 10.0), new Vec2(-10.0, 10.0))
          };
        var shape2 = new Geom2.Side[] { // hole
          new Geom2.Side(new Vec2(-10.0, 10.0), new Vec2(-10.0, -10.0)),
          new Geom2.Side(new Vec2(-10.0, -10.0), new Vec2(10.0, -10.0)),
          new Geom2.Side(new Vec2(10.0, -10.0), new Vec2(10.0, 10.0)),
          new Geom2.Side(new Vec2(10.0, 10.0), new Vec2(-10.0, 10.0)),
          new Geom2.Side(new Vec2(-5.0, -5.0), new Vec2(-5.0, 5.0)),
          new Geom2.Side(new Vec2(5.0, -5.0), new Vec2(-5.0, -5.0)),
          new Geom2.Side(new Vec2(5.0, 5.0), new Vec2(5.0, -5.0)),
          new Geom2.Side(new Vec2(-5.0, 5.0), new Vec2(5.0, 5.0))
        };

        var slice0 = new Slice(shape0);
        var slice1 = new Slice(shape1);
        var slice2 = new Slice(shape2);

        // empty slices
        var walls = ExtrudeWalls(slice0, slice0).ToArray();
        Assert.AreEqual(walls.Length, 0);

        // outline slices
        walls = ExtrudeWalls(slice1, slice1.Transform(matrix)).ToArray();
        Assert.AreEqual(walls.Length, 8);

        // slices with holes
        walls = ExtrudeWalls(slice2, slice2.Transform(matrix)).ToArray();
        Assert.AreEqual(walls.Length, 16);
    }


    [Test]
    public void TestExtrudeWallsDifferentShapes()
    {
        var matrix = Mat4.FromTranslation(new Vec3(0, 0, 10));

        var shape1 = new Geom2.Side[] {
          new Geom2.Side(new Vec2(-10.0, 10.0), new Vec2(-10.0, -10.0)),
          new Geom2.Side(new Vec2(-10.0, -10.0), new Vec2(10.0, -10.0)),
          new Geom2.Side(new Vec2(10.0, -10.0), new Vec2(10.0, 10.0))
        };
        var shape2 = new Geom2.Side[] {
          new Geom2.Side(new Vec2(-10.0, 10.0), new Vec2(-10.0, -10.0)),
          new Geom2.Side(new Vec2(-10.0, -10.0), new Vec2(10.0, -10.0)),
          new Geom2.Side(new Vec2(10.0, -10.0), new Vec2(10.0, 10.0)),
          new Geom2.Side(new Vec2(10.0, 10.0), new Vec2(-10.0, 10.0))
        };
        var shape3 = new Geom2.Side[] {
          new Geom2.Side(new Vec2(2.50000, -4.33013), new Vec2(5.00000, 0.00000)),
          new Geom2.Side(new Vec2(5.00000, 0.00000), new Vec2(2.50000, 4.33013)),
          new Geom2.Side(new Vec2(2.50000, 4.33013), new Vec2(-2.50000, 4.33013)),
          new Geom2.Side(new Vec2(-2.50000, 4.33013), new Vec2(-5.00000, 0.00000)),
          new Geom2.Side(new Vec2(-5.00000, 0.00000), new Vec2(-2.50000, -4.33013)),
          new Geom2.Side(new Vec2(-2.50000, -4.33013), new Vec2(2.50000, -4.33013))
        };

        var slice1 = new Slice(shape1);
        var slice2 = new Slice(shape2);
        var slice3 = new Slice(shape3);

        var walls = ExtrudeWalls(slice1, slice2.Transform(matrix)).ToArray();
        Assert.AreEqual(walls.Length, 24);

        walls = ExtrudeWalls(slice1, slice3.Transform(matrix)).ToArray();
        Assert.AreEqual(walls.Length, 12);

        walls = ExtrudeWalls(slice3, slice2.Transform(matrix)).ToArray();
        Assert.AreEqual(walls.Length, 24);
    }
}