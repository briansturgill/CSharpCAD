namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static Vec2[] SubtractOverLappingObjectsExp1 = new Vec2[] {
    new Vec2(8, 12),
    new Vec2(8, 9),
    new Vec2(9, 9),
    new Vec2(9, 8),
    new Vec2(12, 8),
    new Vec2(12, 12)
  };
}
