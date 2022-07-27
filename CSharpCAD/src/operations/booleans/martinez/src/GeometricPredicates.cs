namespace CSharpCAD;

using EA = Geom2Booleans.ExactArithmetic;

/*
    The full package came from:
        https://github.com/govert/RobustGeometry.NET

    Below, is only what we are using.
*/

internal static partial class Geom2Booleans
{
    // License for this implementation in C#:
    // --------------------------------------
    //
    // Copyright (c) 2012 Govert van Drimmelen
    //
    // This software is provided 'as-is', without any express or implied
    // warranty. In no event will the authors be held liable for any damages
    // arising from the use of this software.
    //
    // Permission is granted to anyone to use this software for any purpose,
    // including commercial applications, and to alter it and redistribute it
    // freely, subject to the following restrictions:
    //
    // 1. The origin of this software must not be misrepresented; you must not
    //    claim that you wrote the original software. If you use this software
    //    in a product, an acknowledgment in the product documentation would
    //    be appreciated but is not required.
    //
    // 2. Altered source versions must be plainly marked as such, and must not
    //    be misrepresented as being the original software.
    //
    // 3. This notice may not be removed or altered from any source distribution.
    //
    //
    // License from original C source version:
    // ---------------------------------------
    //                                                                           
    //  Routines for Arbitrary Precision Floating-point Arithmetic               
    //  and Fast Robust Geometric Predicates                                     
    //  (predicates.c)                                                           
    //                                                                           
    //  May 18, 1996                                                             
    //                                                                           
    //  Placed in the public domain by                                           
    //  Jonathan Richard Shewchuk                                                
    //  School of Computer Science                                               
    //  Carnegie Mellon University                                               
    //  5000 Forbes Avenue                                                       
    //  Pittsburgh, Pennsylvania  15213-3891                                     
    //  jrs@cs.cmu.edu                                                           
    //                                                                           
    //  This file contains C implementation of algorithms for exact addition     
    //    and multiplication of floating-point numbers, and predicates for       
    //    robustly performing the orientation and incircle tests used in         
    //    computational geometry.  The algorithms and underlying theory are      
    //    described in Jonathan Richard Shewchuk.  "Adaptive Precision Floating- 
    //    Point Arithmetic and Fast Robust Geometric Predicates."  Technical     
    //    Report CMU-CS-96-140, School of Computer Science, Carnegie Mellon      
    //    University, Pittsburgh, Pennsylvania, May 1996.  (Submitted to         
    //    Discrete & Computational Geometry.)                                    
    //                                                                           
    //  This file, the paper listed above, and other information are available   
    //    from the Web page http://www.cs.cmu.edu/~quake/robust.html .           
    //                                                                           
    //-------------------------------------------------------------------------


    /// <summary>
    /// Implements the four geometric predicates described by Shewchuck, and implemented in predicates.c.
    /// For each predicate, exports a ~Fast version that is a non-robust implementation directly with double arithmetic, 
    /// an ~Exact version which completed the full calculation in exact arithmetic, and the preferred version which
    /// implements the adaptive routines returning the correct sign and an approximate value.
    /// </summary>
    internal static class GeometricPredicates
    {
        #region Error bounds
        // epsilon is equal to Math.Pow(2.0, -53) and is the largest power of 
        // two that 1.0 + epsilon = 1.0.
        // NOTE: Don't confuse this with double.Epsilon.
        const double epsilon = 1.1102230246251565E-16;

        // Error bounds for orientation and incircle tests.
        const double resulterrbound = (3.0 + 8.0 * epsilon) * epsilon;
        const double ccwerrboundA = (3.0 + 16.0 * epsilon) * epsilon;
        const double ccwerrboundB = (2.0 + 12.0 * epsilon) * epsilon;
        const double ccwerrboundC = (9.0 + 64.0 * epsilon) * epsilon * epsilon;

        #endregion

        #region Orient2D
        // <summary>
        // Non-robust approximate 2D orientation test.
        // </summary>
        // <param name="pa">array with x and y coordinates of pa.</param>
        // <param name="pb">array with x and y coordinates of pb.</param>
        // <param name="pc">array with x and y coordinates of pc.</param>
        // <returns>a positive value if the points pa, pb, and pc occur
        // in counterclockwise order; a negative value if they occur in 
        // clockwise order; and zero if they are collinear. 
        // The result is also a rough aproximation of twice the signed 
        // area of the triangle defined by the three points.</returns>
        // <remarks>The implementation computed the determinant using simple double arithmetic.</remarks>


        // <summary>
        // Adaptive, robust 2D orientation test.
        // </summary>
        // <param name="pa">array with x and y coordinates of pa.</param>
        // <param name="pb">array with x and y coordinates of pb.</param>
        // <param name="pc">array with x and y coordinates of pc.</param>
        // <returns>a positive value if the points pa, pb, and pc occur
        // in counterclockwise order; a negative value if they occur in 
        // clockwise order; and zero if they are collinear. 
        // The result is also an aproximation of twice the signed 
        // area of the triangle defined by the three points.</returns>
        internal static double Orient2D(Vec2 pa, Vec2 pb, Vec2 pc)
        {
            double detleft, detright, det;
            double detsum, errbound;

            detleft = (pa.X - pc.X) * (pb.Y - pc.Y);
            detright = (pa.Y - pc.Y) * (pb.X - pc.X);
            det = detleft - detright;

            if (detleft > 0.0)
            {
                if (detright <= 0.0)
                {
                    return det;
                }
                else
                {
                    detsum = detleft + detright;
                }
            }
            else if (detleft < 0.0)
            {
                if (detright >= 0.0)
                {
                    return det;
                }
                else
                {
                    detsum = -detleft - detright;
                }
            }
            else
            {
                return det;
            }

            errbound = ccwerrboundA * detsum;
            if ((det >= errbound) || (-det >= errbound))
            {
                return det;
            }

            return Orient2DAdapt(pa, pb, pc, detsum);
        }

        // Internal adaptive continuation
        internal static double Orient2DAdapt(Vec2 pa, Vec2 pb, Vec2 pc, double detsum)
        {
            double acx, acy, bcx, bcy;
            double acxtail, acytail, bcxtail, bcytail;
            double detleft, detright;
            double detlefttail, detrighttail;
            double det, errbound;
            double[] B = new double[4];
            double[] C1 = new double[8];
            double[] C2 = new double[12];
            double[] D = new double[16];
            double B3;
            int C1length, C2length, Dlength;
            double[] u = new double[4];
            double u3;
            double s1, t1;
            double s0, t0;

            acx = pa.X - pc.X;
            bcx = pb.X - pc.X;
            acy = pa.Y - pc.Y;
            bcy = pb.Y - pc.Y;

            EA.TwoProduct(acx, bcy, out detleft, out detlefttail);
            EA.TwoProduct(acy, bcx, out detright, out detrighttail);

            EA.TwoTwoDiff(detleft, detlefttail, detright, detrighttail,
                        out B3, out B[2], out B[1], out B[0]);
            B[3] = B3;

            det = EA.Estimate(4, B);
            errbound = ccwerrboundB * detsum;
            if ((det >= errbound) || (-det >= errbound))
            {
                return det;
            }

            EA.TwoDiffTail(pa.X, pc.X, acx, out acxtail);
            EA.TwoDiffTail(pb.X, pc.X, bcx, out bcxtail);
            EA.TwoDiffTail(pa.Y, pc.Y, acy, out acytail);
            EA.TwoDiffTail(pb.Y, pc.Y, bcy, out bcytail);

            if ((acxtail == 0.0) && (acytail == 0.0)
                && (bcxtail == 0.0) && (bcytail == 0.0))
            {
                return det;
            }

            errbound = ccwerrboundC * detsum + resulterrbound * Math.Abs(det);
            det += (acx * bcytail + bcy * acxtail)
                - (acy * bcxtail + bcx * acytail);
            if ((det >= errbound) || (-det >= errbound))
            {
                return det;
            }

            EA.TwoProduct(acxtail, bcy, out s1, out s0);
            EA.TwoProduct(acytail, bcx, out t1, out t0);
            EA.TwoTwoDiff(s1, s0, t1, t0, out u3, out u[2], out u[1], out u[0]);
            u[3] = u3;
            C1length = EA.FastExpansionSumZeroElim(4, B, 4, u, C1);

            EA.TwoProduct(acx, bcytail, out s1, out s0);
            EA.TwoProduct(acy, bcxtail, out t1, out t0);
            EA.TwoTwoDiff(s1, s0, t1, t0, out u3, out u[2], out u[1], out u[0]);
            u[3] = u3;
            C2length = EA.FastExpansionSumZeroElim(C1length, C1, 4, u, C2);

            EA.TwoProduct(acxtail, bcytail, out s1, out s0);
            EA.TwoProduct(acytail, bcxtail, out t1, out t0);
            EA.TwoTwoDiff(s1, s0, t1, t0, out u3, out u[2], out u[1], out u[0]);
            u[3] = u3;
            Dlength = EA.FastExpansionSumZeroElim(C2length, C2, 4, u, D);

            return (D[Dlength - 1]);
        }

        #endregion
    }
}