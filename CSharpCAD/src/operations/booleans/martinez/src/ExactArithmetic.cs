namespace CSharpCAD;

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
    /// Implements the exact floating-point described by Shewchuck, and implemented in predicates.c
    /// </summary>
    internal static class ExactArithmetic
    {
        // Only valid if |a| >= |b|
        // [MethodImplOptions(MethodImplOptions.AggressiveInlining)]
        public static void FastTwoSum(double a, double b, out double x, out double y)
        {
            x = a + b;
            FastTwoSumTail(a, b, x, out y);
        }
        // [MethodImplOptions(MethodImplOptions.AggressiveInlining)]
        public static void FastTwoSumTail(double a, double b, double x, out double y)
        {
            double bvirt = x - a;
            y = b - bvirt;
        }

        // [MethodImplOptions(MethodImplOptions.AggressiveInlining)]
        public static void TwoSum(double a, double b, out double x, out double y)
        {
            x = a + b;
            TwoSumTail(a, b, x, out y);
        }

        // [MethodImplOptions(MethodImplOptions.AggressiveInlining)]
        public static void TwoSumTail(double a, double b, double x, out double y)
        {
            double bvirt = x - a;
            double avirt = x - bvirt;
            double bround = b - bvirt;
            double around = a - avirt;
            y = around + bround;
        }

        // [MethodImplOptions(MethodImplOptions.AggressiveInlining)]
        public static void TwoDiff(double a, double b, out double x, out double y)
        {
            x = a - b;
            TwoDiffTail(a, b, x, out y);
        }

        // [MethodImplOptions(MethodImplOptions.AggressiveInlining)]
        public static void TwoDiffTail(double a, double b, double x, out double y)
        {
            double bvirt = a - x;
            double avirt = x + bvirt;
            double bround = bvirt - b;
            double around = a - avirt;
            y = around + bround;
        }

        // S. p18 with s = 27
        // [MethodImplOptions(MethodImplOptions.AggressiveInlining)]
        public static void Split(double a, out double ahi, out double alo)
        {
            // Set splitter used for Product (using s = 27)
            // Agrees with value calculated by EpsilonSplitter
            const double splitter = (1 << 27) + 1.0; // 2^ceiling(p / 2) + 1 (and p=53)
            double c = splitter * a;
            double abig = c - a;
            ahi = c - abig;
            alo = a - ahi;
        }

        // S. p19 with s = 27
        // [MethodImplOptions(MethodImplOptions.AggressiveInlining)]
        public static void TwoProduct(double a, double b, out double x, out double y)
        {
            x = a * b;
            TwoProductTail(a, b, x, out y);
        }

        // [MethodImplOptions(MethodImplOptions.AggressiveInlining)]
        public static void TwoProductTail(double a, double b, double x, out double y)
        {
            double ahi, alo, bhi, blo;

            Split(a, out ahi, out alo);
            Split(b, out bhi, out blo);
            double err1 = x - (ahi * bhi);
            double err2 = err1 - (alo * bhi);
            double err3 = err2 - (ahi * blo);
            y = (alo * blo) - err3;
        }

        // [MethodImplOptions(MethodImplOptions.AggressiveInlining)]
        public static void TwoOneDiff(double a1, double a0, double b, out double x2, out double x1, out double x0)
        {
            double _i;
            TwoDiff(a0, b, out _i, out x0);
            TwoSum(a1, _i, out x2, out x1);
        }

        // [MethodImplOptions(MethodImplOptions.AggressiveInlining)]
        public static void TwoTwoDiff(double a1, double a0, double b1, double b0, out double x3, out double x2, out double x1, out double x0)
        {
            double _j, _0;
            TwoOneDiff(a1, a0, b0, out _j, out _0, out x0);
            TwoOneDiff(_j, _0, b1, out x3, out x2, out x1);
        }

        // Algorithm Fast-Expansion-Sum-Zero-Elim
        //
        // Sum two expansions, elimiating zero components from the output expansion.
        //
        // h cannot be the same as e or f
        // Read this code together with fast_expansion_sum_zeroelim in predicates.c
        // There are some minor changes here, because we don't want to read outside the array bounds.
        public static int FastExpansionSumZeroElim(int elen, double[] e, int flen, double[] f, double[] h)
        {
            double Q;
            double Qnew;
            double hh;
            int eindex, findex, hindex;
            double enow, fnow;

            // We traverse the lists e and f together. moving from small to large magnitude
            // enow and fnow keep track of the current value in each list, 
            // and eindex and findex the current index
            enow = e[0];
            fnow = f[0];
            eindex = findex = 0;

            // First step is to assign to Q the entry with smaller magnitude
            if ((fnow > enow) == (fnow > -enow)) // if |fnow| >= |enow|
            {
                Q = enow;
                eindex++;

                // NOTE: The original prdicates.c code here would read past the array bound here (but never use the value).
                // Q = enow;
                // enow = e[++eindex]; <<< PROBLEM HERE

                // Instead I just increment the index, and do an extra read later.
                // Pattern is then to read both enow and fnow for every step
                // This adds some extra array evaluations, especially for long arrays, but removes one at the end of each array.
            }
            else
            {
                Q = fnow;
                findex++;
            }

            // Start adding entries into h, carrying Q
            hindex = 0;

            // Check whether we still have entries in both lists
            if ((eindex < elen) && (findex < flen))
            {
                // Note we have an extra 'unrolled' step here, where we are allowed to use FastTwoSum
                // This is becuase we know the next expansion entry is smaller than Q (according to how Q was picked above)
                enow = e[eindex];
                fnow = f[findex];
                // Pick smaller magnitude
                // if |fnow| >= |enow|
                if ((fnow > enow) == (fnow > -enow))
                {
                    // Add e and advance eindex
                    FastTwoSum(enow, Q, out Qnew, out hh);
                    eindex++;
                }
                else
                {
                    // Add f and advance findex
                    FastTwoSum(fnow, Q, out Qnew, out hh);
                    findex++;
                }
                Q = Qnew;
                if (hh != 0.0)
                {
                    h[hindex++] = hh;
                }
                // While we still have entries in both lists
                while ((eindex < elen) && (findex < flen))
                {
                    // Can no longer use FastTwoSum - use TwoSum
                    enow = e[eindex];
                    fnow = f[findex];
                    // Pick smaller magnitude
                    // if |fnow| >= |enow|
                    if ((fnow > enow) == (fnow > -enow))
                    {
                        TwoSum(Q, enow, out Qnew, out hh);
                        eindex++;
                    }
                    else
                    {
                        TwoSum(Q, fnow, out Qnew, out hh);
                        findex++;
                    }
                    Q = Qnew;
                    if (hh != 0.0)
                    {
                        h[hindex++] = hh;
                    }
                }
            }
            // Now we have exhausted one of the lists
            // For the rest, we just run along the list that has values left, 
            //    no more tests to try to pull from the correct list
            while (eindex < elen)
            {
                enow = e[eindex];
                TwoSum(Q, enow, out Qnew, out hh);
                eindex++;
                Q = Qnew;
                if (hh != 0.0)
                {
                    h[hindex++] = hh;
                }
            }
            while (findex < flen)
            {
                fnow = f[findex];
                TwoSum(Q, fnow, out Qnew, out hh);
                findex++;
                Q = Qnew;
                if (hh != 0.0)
                {
                    h[hindex++] = hh;
                }
            }
            if ((Q != 0.0) || (hindex == 0))
            {
                h[hindex++] = Q;
            }
            return hindex;
        }

        // Produce a one double estimate of an expansion's value
        // Also referred to as 'Approximate' in S.
        // This assumes e is sorted
        public static double Estimate(int elen, double[] e)
        {
            double Q;
            int eindex;

            Q = e[0];
            for (eindex = 1; eindex < elen; eindex++)
            {
                Q += e[eindex];
            }
            return Q;
        }

    }
}