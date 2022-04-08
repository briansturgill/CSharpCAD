namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static Vec2[] IntersectGeom2Exp1 = new Vec2[] {
    new Vec2((double)(2), (double)(0)),
    new Vec2((double)(1.4142000000000001), (double)(1.4142000000000001)),
    new Vec2((double)(0), (double)(2)),
    new Vec2((double)(-1.4142000000000001), (double)(1.4142000000000001)),
    new Vec2((double)(-2), (double)(0)),
    new Vec2((double)(-1.4142000000000001), (double)(-1.4142000000000001)),
    new Vec2((double)(0), (double)(-2)),
    new Vec2((double)(1.4142000000000001), (double)(-1.4142000000000001))
  };
}
