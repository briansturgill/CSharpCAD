namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  internal static Geom2.Side[] ExtrudeFSHole1 = new Geom2.Side[] {
      new Geom2.Side(new Vec2((double)(-10), (double)(10)), new Vec2((double)(-10), (double)(-10))),
      new Geom2.Side(new Vec2((double)(-10), (double)(-10)), new Vec2((double)(10), (double)(-10))),
      new Geom2.Side(new Vec2((double)(10), (double)(-10)), new Vec2((double)(10), (double)(10))),
      new Geom2.Side(new Vec2((double)(10), (double)(10)), new Vec2((double)(-10), (double)(10))),
      new Geom2.Side(new Vec2((double)(-5), (double)(-5)), new Vec2((double)(-5), (double)(5))),
      new Geom2.Side(new Vec2((double)(5), (double)(-5)), new Vec2((double)(-5), (double)(-5))),
      new Geom2.Side(new Vec2((double)(5), (double)(5)), new Vec2((double)(5), (double)(-5))),
      new Geom2.Side(new Vec2((double)(-5), (double)(5)), new Vec2((double)(5), (double)(5)))
  };
}
