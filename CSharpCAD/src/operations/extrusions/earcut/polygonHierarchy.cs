namespace CSharpCAD;

internal static partial class CSharpCADInternals
{

    internal static partial class Earcut
    {
        /*
         * Constructs a polygon hierarchy which associates holes with their outer solids.
         * This class maps a 3D polygon onto a 2D space using an orthonormal basis.
         * It tracks the mapping so that points can be reversed back to 3D losslessly.
         */
        internal class PolygonHierarchy
        {
            private Plane plane;
            private Vec3 v;
            private Vec3 u;
            private Dictionary<Vec2, Vec3> basisMap;
            internal List<(List<Vec2>, List<List<Vec2>>)> roots;
            internal PolygonHierarchy(Slice slice)
            {
                this.plane = slice.CalculatePlane();

                // create an orthonormal basis
                // choose an arbitrary right hand vector, making sure it is somewhat orthogonal to the plane normal
                var rightvector = this.plane.Normal.Orthogonal();
                var perp = this.plane.Normal.Cross(rightvector);
                perp = perp.Normalize();
                this.v = perp;
                this.u = this.v.Cross(this.plane.Normal);

                // map from 2D to original 3D points
                this.basisMap = new Dictionary<Vec2, Vec3>();

                // project slice onto 2D plane
                var len = slice.edges.Length;
                var projected = new Geom2.Side[len];
                for (var i = 0; i < slice.edges.Length; i++)
                {
                    var edge = slice.edges[i];
                    var v0 = this.To2D(edge.v0);
                    var v1 = this.To2D(edge.v1);
                    projected[i] = new Geom2.Side(v0, v1);
                }

                // compute polygon hierarchies, assign holes to solids
                var geometry = new Geom2(projected);
                this.roots = AssignHoles(geometry);
            }

            /*
             * project a 3D point onto the 2D plane
             */
            public Vec2 To2D(Vec3 vector3)
            {
                var vector2 = new Vec2(vector3.Dot(this.u), vector3.Dot(this.v));
                this.basisMap[vector2] = vector3;
                return vector2;
            }

            /*
             * un-project a 2D point back into 3D
             */
            public Vec3 To3D(Vec2 vector2)
            {
                // use a map to get the original 3D, no floating point error
                if (this.basisMap.ContainsKey(vector2))
                {
                    return this.basisMap[vector2];
                }
                else
                {
                    Console.WriteLine("Warning: point not in original slice");
                    var v1 = this.u.Scale(vector2.X);
                    var v2 = this.v.Scale(vector2.Y);

                    var planeOrigin = plane.Normal.Scale(plane.W);
                    var v3 = v1.Add(planeOrigin);
                    return v2.Add(v3);
                }
            }
        }

    }
}