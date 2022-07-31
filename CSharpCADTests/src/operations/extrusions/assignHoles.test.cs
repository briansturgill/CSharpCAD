using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class EarAssignHolesTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestAssignHoles()
    {
        var exp1_solid = new List<Vec2> {
          new Vec2(-3, 3),
          new Vec2(-3, -3),
          new Vec2(3, -3),
          new Vec2(3, 3),
        };
        var exp1_holes = new List<List<Vec2>> { new List<Vec2> {
          new Vec2(2, 2),
          new Vec2(2, -2),
          new Vec2(-2, -2),
          new Vec2(-2, 2),
        }};
        var geometry = Subtract(Square(size: 6, center: (0,0)), Square(size: 4, center: (0,0)));
        var obs1 = Earcut.AssignHoles(geometry);
        var (obs1_solid, obs1_holes) = obs1[0];
        Assert.AreEqual(exp1_solid, obs1_solid);
        Assert.AreEqual(exp1_holes, obs1_holes);
    }

    [Test]
    public void TestAssignHolesNested()
    {
        var geometry = Union(
            Subtract(
              Square(size: 6, center: (0,0)),
              Square(size: 4, center: (0,0))
            ),
            Subtract(
              Square(size: 10, center: (0,0)),
              Square(size: 8, center: (0,0))
            )
          );
        var obs1 = Earcut.AssignHoles(geometry);

        var exp1 = new List<(List<Vec2>, List<List<Vec2>>)> {
          (
            new List<Vec2> {
              new Vec2(-5, 5),
              new Vec2(-5, -5),
              new Vec2(5, -5),
              new Vec2(5, 5)
            },
            new List<List<Vec2>> { new List<Vec2> {
              new Vec2(4, 4),
              new Vec2(4, -4),
              new Vec2(-4, -4),
              new Vec2(-4, 4),
            }}
          ),
            ( new List<Vec2> {
              new Vec2(-3, 3),
              new Vec2(-3, -3),
              new Vec2(3, -3),
              new Vec2(3, 3),
            },
            new List<List<Vec2>> { new List<Vec2> {
              new Vec2(2, 2),
              new Vec2(2, -2),
              new Vec2(-2, -2),
              new Vec2(-2, 2),
            }}
          )};
        Assert.AreEqual(obs1, exp1);
    }
}
