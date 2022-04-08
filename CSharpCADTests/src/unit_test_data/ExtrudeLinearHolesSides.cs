namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static Geom2.Side[] ExtrudeLinearHolesSides = new Geom2.Side[] {
      new Geom2.Side(new Vec2((double)(-5), (double)(5)), new Vec2((double)(-5), (double)(-5))),
      new Geom2.Side(new Vec2((double)(-5), (double)(-5)), new Vec2((double)(5), (double)(-5))),
      new Geom2.Side(new Vec2((double)(5), (double)(-5)), new Vec2((double)(5), (double)(5))),
      new Geom2.Side(new Vec2((double)(5), (double)(5)), new Vec2((double)(-5), (double)(5))),
      new Geom2.Side(new Vec2((double)(-2), (double)(-2)), new Vec2((double)(-2), (double)(2))),
      new Geom2.Side(new Vec2((double)(2), (double)(-2)), new Vec2((double)(-2), (double)(-2))),
      new Geom2.Side(new Vec2((double)(2), (double)(2)), new Vec2((double)(2), (double)(-2))),
      new Geom2.Side(new Vec2((double)(-2), (double)(2)), new Vec2((double)(2), (double)(2)))
  };
}
