namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static Vec2[][] ToOutlinesForHolesExp2 = new Vec2[][] {
    new Vec2[] {
      new Vec2(-10, -10),
      new Vec2(10, -10),
      new Vec2(10, 10)
    },
    new Vec2[] {
      new Vec2(8, 6),
      new Vec2(8, -8),
      new Vec2(-6, -8)
    },
    new Vec2[] {
      new Vec2(5, -5),
      new Vec2(6, -5),
      new Vec2(6, -4)
    }
  };
}
