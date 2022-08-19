using System;
#nullable disable
#pragma warning disable CS1591

namespace CSharpCAD.PolyBool
{
    public class IntersectionPoint
    {
        public Vec2 Pt { get; set; }
        public int AlongA { get; set; }
        public int AlongB { get; set; }

        public IntersectionPoint(int alongA, int alongB, Vec2 pt)
        {
            AlongA = alongA;
            AlongB = alongB;
            Pt = pt;
        }
    }

    public class Epsilon
    {
        private readonly double eps;

        public Epsilon(double eps)
        {
            this.eps = eps;
        }

        public static Epsilon Default = new Epsilon(C.EPS);

        public bool PointAboveOrOnLine(Vec2 pt, Vec2 left, Vec2 right)
        {
            var Ax = left.X;
            var Ay = left.Y;
            var Bx = right.X;
            var By = right.Y;
            var Cx = pt.X;
            var Cy = pt.Y;
            var ABx = Bx - Ax;
            var ABy = By - Ay;
            var AB = Math.Sqrt(ABx * ABx + ABy * ABy);
            // algebraic distance of 'pt' to ('left', 'right') line is:
            // [ABx * (Cy - Ay) - ABy * (Cx - Ax)] / AB
            return ABx * (Cy - Ay) - ABy * (Cx - Ax) >= -eps * AB;
        }

        public bool PointBetween(Vec2 p, Vec2 left, Vec2 right)
        {
            // p must be collinear with left->right
            // returns false if p == left, p == right, or left == right
            if (PointsSame(p, left) || PointsSame(p, right)) return false;
            double dPyLy = p.Y - left.Y;
            double dRxLx = right.X - left.X;
            double dPxLx = p.X - left.X;
            double dRyLy = right.Y - left.Y;

            var dot = dPxLx * dRxLx + dPyLy * dRyLy;
            // dot < 0 is p is to the left of 'left'
            if (dot < 0) return false;

            var sqlen = dRxLx * dRxLx + dRyLy * dRyLy;

            // dot <= sqlen is p is to the left of 'right'
            return dot <= sqlen;
        }

        private bool PointsSameX(Vec2 p1, Vec2 p2)
        {
            return Math.Abs(p1.X - p2.X) < eps;
        }

        private bool PointsSameY(Vec2 p1, Vec2 p2)
        {
            return Math.Abs(p1.Y - p2.Y) < eps;
        }

        public bool PointsSame(Vec2 p1, Vec2 p2)
        {
            return PointsSameX(p1, p2) && PointsSameY(p1, p2);
        }

        public int PointsCompare(Vec2 p1, Vec2 p2)
        {
            if (PointsSameX(p1, p2))
                return PointsSameY(p1, p2) ? 0 : (p1.Y < p2.Y ? -1 : 1);
            return p1.X < p2.X ? -1 : 1;
        }

        public bool PointsCollinear(Vec2 pt1, Vec2 pt2, Vec2 pt3)
        {
            var dx1 = pt1.X - pt2.X;
            var dy1 = pt1.Y - pt2.Y;
            var dx2 = pt2.X - pt3.X;
            var dy2 = pt2.Y - pt3.Y;
            var n1 = Math.Sqrt(dx1 * dx1 + dy1 * dy1);
            var n2 = Math.Sqrt(dx2 * dx2 + dy2 * dy2);
            // Assuming det(u, v) = 0, we have:
            // |det(u + u_err, v + v_err)| = |det(u + u_err, v + v_err) - det(u,v)|
            // =|det(u, v_err) + det(u_err. v) + det(u_err, v_err)|
            // <= |det(u, v_err)| + |det(u_err, v)| + |det(u_err, v_err)|
            // <= N(u)N(v_err) + N(u_err)N(v) + N(u_err)N(v_err)
            // <= eps * (N(u) + N(v) + eps)
            // We have N(u) ~ N(u + u_err) and N(v) ~ N(v + v_err).
            // Assuming eps << N(u) and eps << N(v), we end with:
            // |det(u + u_err, v + v_err)| <= eps * (N(u + u_err) + N(v + v_err))
            return Math.Abs(dx1 * dy2 - dx2 * dy1) <= eps * (n1 + n2);
        }

        public IntersectionPoint LinesIntersect(Vec2 a0, Vec2 a1, Vec2 b0, Vec2 b1)
        {
            var adx = a1.X - a0.X;
            var ady = a1.Y - a0.Y;
            var bdx = b1.X - b0.X;
            var bdy = b1.Y - b0.Y;

            var axb = adx * bdy - ady * bdx;
            var n1 = Math.Sqrt(adx * adx + ady * ady);
            var n2 = Math.Sqrt(bdx * bdx + bdy * bdy);
            if (Math.Abs(axb) <= eps * (n1 + n2))
                return null; // lines are coincident

            var dx = a0.X - b0.X;
            var dy = a0.Y - b0.Y;

            var A = (bdx * dy - bdy * dx) / axb;
            var B = (adx * dy - ady * dx) / axb;

            var pt = new Vec2(a0.X + A * adx, a0.Y + A * ady);

            int alongA = 0;
            if (PointsSame(pt, a0))
                alongA = -1;
            else if (PointsSame(pt, a1))
                alongA = 1;
            else if (A < 0)
                alongA = -2;
            else if (A > 1)
                alongA = 2;

            int alongB = 0;
            if (PointsSame(pt, b0))
                alongB = -1;
            else if (PointsSame(pt, b1))
                alongB = 1;
            else if (B < 0)
                alongB = -2;
            else if (B > 1)
                alongB = 2;

            return new IntersectionPoint(alongA, alongB, pt);
        }
    }
}
