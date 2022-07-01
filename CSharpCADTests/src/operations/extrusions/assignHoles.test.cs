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
          new Vec2(-3.000013333333334, -3.000013333333334),
          new Vec2(3.000013333333334, -3.000013333333334),
          new Vec2(3.000013333333334, 3.000013333333334),
          new Vec2(-3.000013333333334, 3.000013333333334)
        };
        var exp1_holes = new List<List<Vec2>> { new List<Vec2> {
          new Vec2(-1.9999933333333335, 1.9999933333333335),
          new Vec2(1.9999933333333335, 1.9999933333333335),
          new Vec2(1.9999933333333335, -1.9999933333333335),
          new Vec2(-1.9999933333333335, -1.9999933333333335)
        }};
        var geometry = Subtract(Square(size: 6, center: (0,0)), Square(size: 4, center: (0,0)));
        var obs1 = Earcut.AssignHoles(geometry);
        var (obs1_solid, obs1_holes) = obs1[0];
        Assert.IsTrue(Helpers.CompareListsNEVec2(exp1_solid, obs1_solid));
        Assert.IsTrue(Helpers.CompareListOfListsNEVec2(exp1_holes, obs1_holes));
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
            ( new List<Vec2> {
              new Vec2(-3.0000006060444444, -3.0000006060444444),
              new Vec2(3.0000006060444444, -3.0000006060444444),
              new Vec2(3.0000006060444444, 3.0000006060444444),
              new Vec2(-3.0000006060444444, 3.0000006060444444)
            },
            new List<List<Vec2>> { new List<Vec2> {
              new Vec2(-2.0000248485333336, 2.0000248485333336),
              new Vec2(2.0000248485333336, 2.0000248485333336),
              new Vec2(2.0000248485333336, -2.0000248485333336),
              new Vec2(-2.0000248485333336, -2.0000248485333336)
            }}
          ),
          (
            new List<Vec2> {
              new Vec2(-5.000025454577778, -5.000025454577778),
              new Vec2(5.000025454577778, -5.000025454577778),
              new Vec2(5.000025454577778, 5.000025454577778),
              new Vec2(-5.000025454577778, 5.000025454577778)
            },
            new List<List<Vec2>> { new List<Vec2> {
              new Vec2(-3.9999763635555556, 3.9999763635555556),
              new Vec2(3.9999763635555556, 3.9999763635555556),
              new Vec2(3.9999763635555556, -3.9999763635555556),
              new Vec2(-3.9999763635555556, -3.9999763635555556)
            }}
          )};
        Assert.AreEqual(obs1, exp1);
    }
}
