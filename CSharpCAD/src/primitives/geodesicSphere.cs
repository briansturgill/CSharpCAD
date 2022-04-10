namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Construct a geodesic sphere based on icosahedron symmetry.</summary>
     * <param name="radius">Target radius of sphere</param>
     * <param name="frequency">Subdivision frequency per face, must be multiple of 6.</param>
     */
    public static Geom3 GeodesicSphere(double radius = 1, int frequency = 6)
    {
        if (radius <= 0) throw new ArgumentException("Option radius must be greater than zero.");
        if (frequency < 6) throw new ArgumentException("Option frequency must be six or more.");
        if (frequency % 6 != 0) throw new ArgumentException("Option frequency must be a multiple of six.");

        // adjust the frequency to base 6
        frequency = frequency / 6;

        var ci = new Vec3[] { // hard-coded data of icosahedron (20 faces, all triangles)
          new Vec3(0.850651, 0.000000, -0.525731),
          new Vec3(0.850651, -0.000000, 0.525731),
          new Vec3(-0.850651, -0.000000, 0.525731),
          new Vec3(-0.850651, 0.000000, -0.525731),
          new Vec3(0.000000, -0.525731, 0.850651),
          new Vec3(0.000000, 0.525731, 0.850651),
          new Vec3(0.000000, 0.525731, -0.850651),
          new Vec3(0.000000, -0.525731, -0.850651),
          new Vec3(-0.525731, -0.850651, -0.000000),
          new Vec3(0.525731, -0.850651, -0.000000),
          new Vec3(0.525731, 0.850651, 0.000000),
          new Vec3(-0.525731, 0.850651, 0.000000)
        };

        var ti = new Vec3[] {
          new Vec3(0, 9, 1), new Vec3(1, 10, 0), new Vec3(6, 7, 0), new Vec3(10, 6, 0), new Vec3(7, 9, 0),
          new Vec3(5, 1, 4), new Vec3(4, 1, 9), new Vec3(5, 10, 1), new Vec3(2, 8, 3), new Vec3(3, 11, 2),
          new Vec3(2, 5, 4), new Vec3(4, 8, 2), new Vec3(2, 11, 5), new Vec3(3, 7, 6), new Vec3(6, 11, 3),
          new Vec3(8, 7, 3), new Vec3(9, 8, 4), new Vec3(11, 10, 5), new Vec3(10, 11, 6), new Vec3(8, 9, 7)
        };

        // Note the switch over of frequency from int to double here is intentional.
        // Frequency is truly used as a double in this subroutine.
        (List<Vec3>, List<List<int>>, int) geodesicSubDivide((Vec3, Vec3, Vec3) p, double frequency, int offset)
        {
            var (p1, p2, p3) = p;
            var n = offset;
            var c = new List<Vec3>();
            var f = new List<List<int>>();

            //           p3
            //           /\
            //          /__\     frequency = 3
            //      i  /\  /\
            //        /__\/__\       total triangles = 9 (frequency*frequency)
            //       /\  /\  /\
            //     0/__\/__\/__\
            //    p1 0   j      p2

            Vec3 mix3(Vec3 a, Vec3 b, double f)
            {
                var _f = 1 - f;
                return new Vec3(
                  a.x * _f + b.x * f,
                  a.y * _f + b.y * f,
                  a.z * _f + b.z * f
                );
            }

            for (var i = 0; i < frequency; i++)
            {
                for (var j = 0; j < frequency - i; j++)
                {
                    var t0 = i / frequency;
                    var t1 = (i + 1) / frequency;
                    var s0 = j / (frequency - i);
                    var s1 = (j + 1) / (frequency - i);
                    var s2 = (frequency - i - 1 != 0) ? j / (frequency - i - 1) : 1;
                    var q = new Vec3[3];

                    q[0] = mix3(mix3(p1, p2, s0), p3, t0);
                    q[1] = mix3(mix3(p1, p2, s1), p3, t0);
                    q[2] = mix3(mix3(p1, p2, s2), p3, t1);

                    // -- normalize
                    for (var k = 0; k < 3; k++)
                    {
                        var r = Vec3.Hypot(q[k].x, q[k].y, q[k].z);
                        q[k] = new Vec3(q[k].x / r, q[k].y / r, q[k].z / r);
                    }
                    for (var l = 0; l < 3; l++)
                    {
                        c.Add(q[l]);
                    }
                    f.Add(new List<int> { n, n + 1, n + 2 }); n += 3;

                    if (j < frequency - i - 1)
                    {
                        var s3 = (frequency - i - 1 != 0) ? (j + 1) / (frequency - i - 1) : 1;
                        q[0] = mix3(mix3(p1, p2, s1), p3, t0);
                        q[1] = mix3(mix3(p1, p2, s3), p3, t1);
                        q[2] = mix3(mix3(p1, p2, s2), p3, t1);

                        // -- normalize
                        for (var k = 0; k < 3; k++)
                        {
                            var r = Vec3.Hypot(q[k].x, q[k].y, q[k].z);
                            q[k] = new Vec3(q[k].x / r, q[k].y / r, q[k].z / r);
                        }
                        for (var l = 0; l < 3; l++)
                        {
                            c.Add(q[l]);
                        }
                        f.Add(new List<int> { n, n + 1, n + 2 }); n += 3;
                    }
                }
            }
            // points, triangles, offset
            return (c, f, n);
        }

        var points = new List<Vec3>();
        var faces = new List<List<int>>();
        var offset = 0;

        for (var i = 0; i < ti.Length; i++)
        {
            var g = geodesicSubDivide((ci[(int)ti[i].x], ci[(int)ti[i].y], ci[(int)ti[i].z]), frequency, offset);
            var (p, f, o) = g;
            points.AddRange(p);
            faces.AddRange(f);
            offset = o;
        }

        var geometry = Polyhedron(new Opts { { "points", points }, { "faces", faces }, { "orientation", "inward" } });
        if (radius != 1) geometry = geometry.Transform(Mat4.FromScaling(new Vec3(radius, radius, radius)));
        return geometry;
    }
}
