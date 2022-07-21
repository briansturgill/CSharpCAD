namespace CSharpCAD;

public static partial class CSCAD {
private static Geom2 centerGeometry(Geom2 obj, bool axisX, bool axisY, Vec2 relativeTo) {
  var (min, max) = obj.BoundingBox();
  var offset_x = 0.0;
  var offset_y = 0.0;
  if (axisX) offset_x = relativeTo.X - (min.X + ((max.X - min.X) / 2));
  if (axisY) offset_y = relativeTo.Y - (min.Y + ((max.Y - min.Y) / 2));
  return Translate(new Vec2(offset_x, offset_y), obj);
}

private static Geom3 centerGeometry(Geom3 obj, bool axisX, bool axisY, bool axisZ, Vec3 relativeTo) {
  var (min, max) = obj.BoundingBox();
  var offset_x = 0.0;
  var offset_y = 0.0;
  var offset_z = 0.0;
  if (axisX) offset_x = relativeTo.X - (min.X + ((max.X - min.X) / 2));
  if (axisY) offset_y = relativeTo.Y - (min.Y + ((max.Y - min.Y) / 2));
  if (axisZ) offset_z = relativeTo.Z - (min.Z + ((max.Z - min.Z) / 2));
  return Translate(new Vec3(offset_x, offset_y, offset_z), obj);
}

/**
 * <summary>Center the given object along the specified axes.</summary>
 * <param name="axisX">=Center along X axis.</param>
 * <param name="axisY">=Center along Y axis.</param>
 * <param name="relativeTo" default="(0,0)">Centering occurs relative to this point.</param>
 * <param name="obj">The geometry object to center</param>
 * <returns>The centered object.</returns>
 * <group>Transformations</group>
 */
public static Geom2 Center(Geom2 obj, bool axisX = true, bool axisY = true, Vec2? relativeTo = null) {
  var _relativeTo = relativeTo ?? new Vec2(0, 0);

  return centerGeometry(obj, axisX, axisY, _relativeTo);
}

/**
 * <summary>Center the given object along the specified axes.</summary>
 * <param name="axisX">=Center along X axis.</param>
 * <param name="axisY">=Center along Y axis.</param>
 * <param name="axisZ">=Center along Z axis.</param>
 * <param name="relativeTo" default="(0,0,0)">Centering occurs relative to this point.</param>
 * <param name="obj">The geometry object to center</param>
 * <returns>The centered object.</returns>
 * <group>Transformations</group>
 */
public static Geom3 Center(Geom3 obj, bool axisX = true, bool axisY = true, bool axisZ = true, Vec3? relativeTo = null) {
  var _relativeTo = relativeTo ?? new Vec3(0, 0, 0);

    return centerGeometry(obj, axisX, axisY, axisZ, _relativeTo);
}

/**
 * <summary>Center the given object along the X axis.</summary>
 * <param name="obj">The object to center.</param>
 * <returns>The centered object.</returns>
 * <group>Transformations</group>
 */
public static Geom2 CenterX(Geom2 obj) => Center(obj, axisX: true, axisY: false);

/**
 * <summary>Center the given object along the Y axis.</summary>
 * <param name="obj">The object to center.</param>
 * <returns>The centered object.</returns>
 * <group>Transformations</group>
 */
public static Geom2 CenterY(Geom2 obj) => Center(obj, axisX: false, axisY: true);

/**
 * <summary>Center the given object along the X axis.</summary>
 * <param name="obj">The object to center.</param>
 * <returns>The centered object.</returns>
 * <group>Transformations</group>
 */
public static Geom3 CenterX(Geom3 obj) => Center(obj, axisX: true, axisY: false, axisZ: false);

/**
 * <summary>Center the given object along the Y axis.</summary>
 * <param name="obj">The object to center.</param>
 * <returns>The centered object.</returns>
 * <group>Transformations</group>
 */
public static Geom3 CenterY(Geom3 obj) => Center(obj, axisX: false, axisY: true, axisZ: false);

/**
 * <summary>Center the given object along the Z axis.</summary>
 * <param name="obj">The object to center.</param>
 * <returns>The centered object.</returns>
 * <group>Transformations</group>
 */
public static Geom3 CenterZ(Geom3 obj) => Center(obj, axisX: false, axisY: false, axisZ: true);
}