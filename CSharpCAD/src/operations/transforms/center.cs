namespace CSharpCAD;

public static partial class CSCAD {
private static Geometry centerGeometry(Geom2 obj, bool axisX, bool axisY, Vec3 relativeTo) {
  var (min, max) = obj.BoundingBox();
  var offset_x = 0.0;
  var offset_y = 0.0;
  if (axisX) offset_x = relativeTo.x - (min.x + ((max.x - min.x) / 2));
  if (axisY) offset_y = relativeTo.y - (min.y + ((max.y - min.y) / 2));
  return Translate(new Vec3(offset_x, offset_y, 0), obj);
}

private static Geometry centerGeometry(Geom3 obj, bool axisX, bool axisY, bool axisZ, Vec3 relativeTo) {
  var (min, max) = obj.BoundingBox();
  var offset_x = 0.0;
  var offset_y = 0.0;
  var offset_z = 0.0;
  if (axisX) offset_x = relativeTo.x - (min.x + ((max.x - min.x) / 2));
  if (axisY) offset_y = relativeTo.y - (min.y + ((max.y - min.y) / 2));
  if (axisZ) offset_z = relativeTo.z - (min.z + ((max.z - min.z) / 2));
  return Translate(new Vec3(offset_x, offset_y, offset_z), obj);
}

/**
 * <summary>Center the given objects.</summary>
 * <param name="axisX">=Center along X axis.</param>
 * <param name="axisY">=Center along Y axis.</param>
 * <param name="axisZ">=Center along Z axis.</param>
 * <param name="relativeTo" default="(0,0,0)">Centering occurs relative to this point.</param>
 * <param name="obj">The geometry object to center</param>
 * <remarks>The Z coordiantes are ignored for 2D geometry objects.</remarks>
 * <returns>The centered object.</returns>
 * <group>Transformations</group>
 */
public static Geometry Center(Geometry obj, bool axisX = true, bool axisY = true, bool axisZ = true, Vec3? relativeTo = null) {
  var _relativeTo = relativeTo ?? new Vec3(0, 0, 0);

  if (obj.Is2D) {
    return centerGeometry((Geom2)obj, axisX, axisY, _relativeTo);
  } else {
    return centerGeometry((Geom3)obj, axisX, axisY, axisZ, _relativeTo);
  }
}

/**
 * <summary>Center the given objects about the X axis.</summary>
 * <param name="obj">The object to center.</param>
 * <returns>The centered object.</returns>
 * <group>Transformations</group>
 */
public static Geometry CenterX(Geometry obj) => Center(obj, axisX: true, axisY: false, axisZ: false);

/**
 * <summary>Center the given objects about the Y axis.</summary>
 * <param name="obj">The object to center.</param>
 * <returns>The centered object.</returns>
 * <group>Transformations</group>
 */
public static Geometry CenterY(Geometry obj) => Center(obj, axisX: false, axisY: true, axisZ: false);

/**
 * <summary>Center the given object about the Z axis.</summary>
 * <param name="obj">The object to center.</param>
 * <returns>The centered object.</returns>
 * <group>Transformations</group>
 */
public static Geometry CenterZ(Geometry obj) => Center(obj, axisX: false, axisY: false, axisZ: true);

}