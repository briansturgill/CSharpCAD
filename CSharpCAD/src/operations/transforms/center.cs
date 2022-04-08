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
 * <summary>Center the given objects using the given options.</summary>
 * <param name="axisX">=Center along X axis. Default: true.</param>
 * <param name="axisY">=Center along Y axis. Default: true.</param>
 * <param name="axisZ">=Center along Z axis. Default: true.</param>
 * <param name="relativeTo">Centering occurs relative to this point. Default: Vec3(0,0,0)</param>
 * <param name="obj">The geometry object to center</param>
 * <remarks>The Z components are ignored for 2D geometry objects.</remarks>
 * <returns>The centered object.</returns>
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
 * Center the given objects about the X axis.
 * @param {...Object} objects - the objects to center
 * @return {Object|Array} the centered object, or a list of centered objects
 * @alias module:modeling/transforms.centerX
 */
public static Geometry CenterX(Geometry obj) => Center(obj, axisX: true, axisY: false, axisZ: false);

/**
 * Center the given objects about the Y axis.
 * @param {...Object} objects - the objects to center
 * @return {Object|Array} the centered object, or a list of centered objects
 * @alias module:modeling/transforms.centerY
 */
public static Geometry CenterY(Geometry obj) => Center(obj, axisX: false, axisY: true, axisZ: false);

/**
 * Center the given objects about the Z axis.
 * @param {...Object} objects - the objects to center
 * @return {Object|Array} the centered object, or a list of centered objects
 * @alias module:modeling/transforms.centerZ
 */
public static Geometry CenterZ(Geometry obj) => Center(obj, axisX: false, axisY: false, axisZ: true);

}