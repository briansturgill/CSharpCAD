namespace CSharpCAD;

internal static partial class CSharpCADInternals
{
    /* Original Version in CPP. */

    /* Triangle/triangle intersection test routine,
     * by Tomas Moller, 1997.
     * See article "A Fast Triangle-Triangle Intersection Test",
     * Journal of Graphics Tools, 2(2), 1997
     * updated: 2001-06-20 (added line of intersection)
     *
     * int tri_tri_intersect(double V0[3],double V1[3],double V2[3],
     *                       double U0[3],double U1[3],double U2[3])
     *
     * parameters: vertices of triangle 1: V0,V1,V2
     *             vertices of triangle 2: U0,U1,U2
     * result    : returns 1 if the triangles intersect, otherwise 0
     *
     * Here is a version withouts divisions (a little faster)
     * int NoDivTriTriIsect(double V0[3],double V1[3],double V2[3],
     *                      double U0[3],double U1[3],double U2[3]);
     *
     * This version computes the line of intersection as well (if they are not coplanar):
     * int tri_tri_intersect_with_isectline(double V0[3],double V1[3],double V2[3],
     *				        double U0[3],double U1[3],double U2[3],int *coplanar,
     *				        double isectpt1[3],double isectpt2[3]);
     * coplanar returns whether the tris are coplanar
     * isectpt1, isectpt2 are the endpoints of the line of intersection
     */

    /*
    Copyright 2020 Tomas Akenine-Möller

    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
    documentation files (the "Software"), to deal in the Software without restriction, including without limitation
    the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
    to permit persons to whom the Software is furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all copies or substantial
    portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
    WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
    OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT
    OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    */


    private static bool coplanar_tri_tri(Vec3 N, Vec3 V0, Vec3 V1, Vec3 V2,
                         Vec3 U0, Vec3 U1, Vec3 U2)
    {
        short i0 = 0;
        short i1 = 0;
        /* first project onto an axis-aligned plane, that maximizes the area */
        /* of the triangles, compute indices: i0,i1. */
        var A = new Vec3(Math.Abs(N.X), Math.Abs(N.Y), Math.Abs(N.Z));
        if (A.X > A.Y)
        {
            if (A.X > A.Z)
            {
                i0 = 1;      /* A[0] is greatest */
                i1 = 2;
            }
            else
            {
                i0 = 0;      /* A[2] is greatest */
                i1 = 1;
            }
        }
        else   /* A[0]<=A[1] */
        {
            if (A.Z > A.Y)
            {
                i0 = 0;      /* A[2] is greatest */
                i1 = 1;
            }
            else
            {
                i0 = 0;      /* A[1] is greatest */
                i1 = 2;
            }
        }

        bool EDGE_AGAINST_TRI_EDGES(Vec3 V0, Vec3 V1, Vec3 U0, Vec3 U1, Vec3 U2)
        {
            double Ax, Ay, Bx, By, Cx, Cy, e;
            Ax = V1[i0] - V0[i0];
            Ay = V1[i1] - V0[i1];

            /* this edge to edge test is based on Franlin Antonio's gem:
               "Faster Line Segment Intersection", in Graphics Gems III,
               pp. 199-202 */
            bool EDGE_EDGE_TEST(in Vec3 V0, in Vec3 U0, in Vec3 U1)
            {
                Bx = U0[i0] - U1[i0];
                By = U0[i1] - U1[i1];
                Cx = V0[i0] - U0[i0];
                Cy = V0[i1] - U0[i1];
                var f = Ay * Bx - Ax * By;
                var d = By * Cx - Bx * Cy;
                if ((f > 0 && d >= 0 && d <= f) || (f < 0 && d <= 0 && d >= f))
                {
                    e = Ax * Cy - Ay * Cx;
                    if (f > 0)
                    {
                        if (e >= 0 && e <= f) return true;
                    }
                    else
                    {
                        if (e <= 0 && e >= f) return true;
                    }
                }
                return false;
            }

            /* test edge U0,U1 against V0,V1 */
            if (EDGE_EDGE_TEST(V0, U0, U1)) return true;
            /* test edge U1,U2 against V0,V1 */
            if (EDGE_EDGE_TEST(V0, U1, U2)) return true;
            /* test edge U2,U1 against V0,V1 */
            if (EDGE_EDGE_TEST(V0, U2, U0)) return true;
            return false;
        }

        /* test all edges of triangle 1 against the edges of triangle 2 */
        if (EDGE_AGAINST_TRI_EDGES(V0, V1, U0, U1, U2)) return true;
        if (EDGE_AGAINST_TRI_EDGES(V1, V2, U0, U1, U2)) return true;
        if (EDGE_AGAINST_TRI_EDGES(V2, V0, U0, U1, U2)) return true;

        bool POINT_IN_TRI(in Vec3 V0, in Vec3 U0, in Vec3 U1, in Vec3 U2)
        {
            /* is T1 completly inside T2? */
            /* check if V0 is inside tri(U0,U1,U2) */
            var a = U1[i1] - U0[i1];
            var b = -(U1[i0] - U0[i0]);
            var c = -a * U0[i0] - b * U0[i1];
            var d0 = a * V0[i0] + b * V0[i1] + c;

            a = U2[i1] - U1[i1];
            b = -(U2[i0] - U1[i0]);
            c = -a * U1[i0] - b * U1[i1];
            var d1 = a * V0[i0] + b * V0[i1] + c;

            a = U0[i1] - U2[i1];
            b = -(U0[i0] - U2[i0]);
            c = -a * U2[i0] - b * U2[i1];
            var d2 = a * V0[i0] + b * V0[i1] + c;
            if (d0 * d1 > 0.0)
            {
                if (d0 * d2 > 0.0) return true;
            }
            return false;
        }


        /* finally, test if tri1 is totally contained in tri2 or vice versa */
        if (POINT_IN_TRI(V0, U0, U1, U2)) return true;
        if (POINT_IN_TRI(U0, V0, V1, V2)) return true;

        return false;
    }

    /* sort so that a<=b */
    private static void SORT2(ref double a, ref double b, ref bool smallest)
    {
        if (a > b)
        {
            var c = a;
            a = b;
            b = c;
            smallest = true;
        }
        else smallest = false;
    }


    private static void isect2(Vec3 VTX0, Vec3 VTX1, Vec3 VTX2, double VV0, double VV1, double VV2,
            double D0, double D1, double D2, ref double isect0, ref double isect1, ref Vec3 isectpoint0, ref Vec3 isectpoint1)
    {
        var tmp = D0 / (D0 - D1);
        isect0 = VV0 + (VV1 - VV0) * tmp;
        var diff = VTX1.Subtract(VTX0);
        diff = diff.Multiply(new Vec3(tmp));
        isectpoint0 = diff.Add(VTX0);
        tmp = D0 / (D0 - D2);
        isect1 = VV0 + (VV2 - VV0) * tmp;
        diff = VTX2.Subtract(VTX0);
        diff = diff.Multiply(new Vec3(tmp));
        isectpoint1 = VTX0.Add(diff);
    }

    private static bool compute_intervals_isectline(Vec3 VERT0, Vec3 VERT1, Vec3 VERT2,
                           double VV0, double VV1, double VV2, double D0, double D1, double D2,
                           double D0D1, double D0D2, ref double isect0, ref double isect1,
                           ref Vec3 isectpoint0, ref Vec3 isectpoint1)
    {
        if (D0D1 > 0.0)
        {
            /* here we know that D0D2<=0.0 */
            /* that is D0, D1 are on the same side, D2 on the other or on the plane */
            isect2(VERT2, VERT0, VERT1, VV2, VV0, VV1, D2, D0, D1, ref isect0, ref isect1, ref isectpoint0, ref isectpoint1);
        }
        else if (D0D2 > 0.0)
        {
            /* here we know that d0d1<=0.0 */
            isect2(VERT1, VERT0, VERT2, VV1, VV0, VV2, D1, D0, D2, ref isect0, ref isect1, ref isectpoint0, ref isectpoint1);
        }
        else if (D1 * D2 > 0.0 || D0 != 0.0)
        {
            /* here we know that d0d1<=0.0 or that D0!=0.0 */
            isect2(VERT0, VERT1, VERT2, VV0, VV1, VV2, D0, D1, D2, ref isect0, ref isect1, ref isectpoint0, ref isectpoint1);
        }
        else if (D1 != 0.0)
        {
            isect2(VERT1, VERT0, VERT2, VV1, VV0, VV2, D1, D0, D2, ref isect0, ref isect1, ref isectpoint0, ref isectpoint1);
        }
        else if (D2 != 0.0)
        {
            isect2(VERT2, VERT0, VERT1, VV2, VV0, VV1, D2, D0, D1, ref isect0, ref isect1, ref isectpoint0, ref isectpoint1);
        }
        else
        {
            /* triangles are coplanar */
            return true;
        }
        return false;
    }

    internal static bool tri_tri_intersect_with_isectline(Vec3 V0, Vec3 V1, Vec3 V2,
                         Vec3 U0, Vec3 U1, Vec3 U2, ref bool coplanar,
                         Vec3 isectpt1, Vec3 isectpt2)
    {
        var isect1 = new double[] { 0.0, 0.0 };
        var isect2 = new double[] { 0.0, 0.0 };
        Vec3 isectpointA1 = new Vec3();
        Vec3 isectpointA2 = new Vec3();
        Vec3 isectpointB1 = new Vec3();
        Vec3 isectpointB2 = new Vec3();
        bool smallest1 = false;
        bool smallest2 = false;

        /* compute plane equation of triangle(V0,V1,V2) */
        var E1 = V1.Subtract(V0);
        var E2 = V2.Subtract(V0);
        var N1 = E1.Cross(E2);
        var d1 = -N1.Dot(V0);
        /* plane equation 1: N1.X+d1=0 */

        /* put U0,U1,U2 into plane equation 1 to compute signed distances to the plane*/
        var du0 = N1.Dot(U0) + d1;
        var du1 = N1.Dot(U1) + d1;
        var du2 = N1.Dot(U2) + d1;

        /* coplanarity robustness check */
        if (Math.Abs(du0) < C.EPS) du0 = 0.0;
        if (Math.Abs(du1) < C.EPS) du1 = 0.0;
        if (Math.Abs(du2) < C.EPS) du2 = 0.0;

        var du0du1 = du0 * du1;
        var du0du2 = du0 * du2;

        if (du0du1 > 0.0 && du0du2 > 0.0) /* same sign on all of them + not equal 0 ? */
            return false;                    /* no intersection occurs */

        /* compute plane of triangle (U0,U1,U2) */
        E1 = U1.Subtract(U0);
        E2 = U2.Subtract(U0);
        var N2 = E1.Cross(E2);
        var d2 = -N2.Dot(U0);
        /* plane equation 2: N2.X+d2=0 */

        /* put V0,V1,V2 into plane equation 2 */
        var dv0 = N2.Dot(V0) + d2;
        var dv1 = N2.Dot(V1) + d2;
        var dv2 = N2.Dot(V2) + d2;

        if (Math.Abs(dv0) < C.EPS) dv0 = 0.0;
        if (Math.Abs(dv1) < C.EPS) dv1 = 0.0;
        if (Math.Abs(dv2) < C.EPS) dv2 = 0.0;

        var dv0dv1 = dv0 * dv1;
        var dv0dv2 = dv0 * dv2;

        if (dv0dv1 > 0.0 && dv0dv2 > 0.0) /* same sign on all of them + not equal 0 ? */
            return false;                    /* no intersection occurs */

        /* compute direction of intersection line */
        var D = N1.Cross(N2);

        /* compute and index to the largest component of D */
        var max = Math.Abs(D[0]);
        var index = 0;
        var b = Math.Abs(D[1]);
        var c = Math.Abs(D[2]);
        if (b > max)
        {
            max = b;
            index = 1;
        }
        if (c > max)
        {
            max = c;
            index = 2;
        }

        /* this is the simplified projection onto L*/
        var vp0 = V0[index];
        var vp1 = V1[index];
        var vp2 = V2[index];

        var up0 = U0[index];
        var up1 = U1[index];
        var up2 = U2[index];

        /* compute interval for triangle 1 */
        coplanar = compute_intervals_isectline(V0, V1, V2, vp0, vp1, vp2, dv0, dv1, dv2,
                             dv0dv1, dv0dv2, ref isect1[0], ref isect1[1], ref isectpointA1, ref isectpointA2);
        if (coplanar) return coplanar_tri_tri(N1, V0, V1, V2, U0, U1, U2);


        /* compute interval for triangle 2 */
        compute_intervals_isectline(U0, U1, U2, up0, up1, up2, du0, du1, du2,
                        du0du1, du0du2, ref isect2[0], ref isect2[1], ref isectpointB1, ref isectpointB2);

        SORT2(ref isect1[0], ref isect1[1], ref smallest1);
        SORT2(ref isect2[0], ref isect2[1], ref smallest2);

        if (isect1[1] < isect2[0] || isect2[1] < isect1[0]) return false;

        /* at this point, we know that the triangles intersect */

        if (isect2[0] < isect1[0])
        {
            if (!smallest1) { isectpt1 = isectpointA1; }
            else { isectpt1 = isectpointA2; }

            if (isect2[1] < isect1[1])
            {
                if (!smallest2) { isectpt2 = isectpointB2; }
                else { isectpt2 = isectpointB1; }
            }
            else
            {
                if (!smallest1) { isectpt2 = isectpointA2; }
                else { isectpt2 = isectpointA1; }
            }
        }
        else
        {
            if (!smallest2) { isectpt1 = isectpointB1; }
            else { isectpt1 = isectpointB2; }

            if (isect2[1] > isect1[1])
            {
                if (!smallest1) { isectpt2 = isectpointA2; }
                else { isectpt2 = isectpointA1; }
            }
            else
            {
                if (!smallest2) { isectpt2 = isectpointB2; }
                else { isectpt2 = isectpointB1; }
            }
        }
        return true;
    }
}