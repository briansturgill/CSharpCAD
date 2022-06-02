namespace CSharpCAD;

/*
 * Original source from quickhull3d (https://github.com/mauriciopoppe/quickhull3d)
 * Copyright (c) 2015 Mauricio Poppe
 *
 * Adapted to JSCAD by Jeff Gay
 */

public static partial class CSCAD
{
    internal class QHFace
    {
      #nullable disable
        internal const int VISIBLE = 0;
        internal const int NON_CONVEX = 1;
        internal const int DELETED = 2;

        private Vec3 normal;
        internal Vec3 centroid;
        private double offset;
        internal QHVertex outside;
        internal int mark;
        internal QHHalfEdge edge;
        internal int nVertices;
        internal double area;

        internal QHFace()
        {
            this.normal = new Vec3();
            this.centroid = new Vec3();
            // signed distance from face to the origin
            this.offset = 0;
            // pointer to the a vertex in a double linked list this face can see
            this.outside = null;
            this.mark = VISIBLE;
            this.edge = null;
            this.nVertices = 0;
        }

        internal QHHalfEdge GetEdge(int i)
        {
            var it = this.edge;
            while (i > 0)
            {
                it = it.next;
                i -= 1;
            }
            while (i < 0)
            {
                it = it.prev;
                i += 1;
            }
            return it;
        }

        internal void ComputeNormal()
        {
            var e0 = this.edge;
            var e1 = e0.next;
            var e2 = e1.next;
            var v2 = e1.Head().point.Subtract(e0.Head().point);
            var v1 = new Vec3();


            this.nVertices = 2;
            this.normal = new Vec3(0, 0, 0);
            while (e2 != e0)
            {
                v1 =  v2;
                v2 = e2.Head().point.Subtract(e0.Head().point);
                this.normal = this.normal.Add(v1.Cross(v2));
                e2 = e2.next;
                this.nVertices += 1;
            }
            this.area = this.normal.Length();
            // normalize the vector, since we've already calculated the area
            // it's cheaper to scale the vector using this quantity instead of
            // doing the same operation again
            this.normal = this.normal.Scale(1.0 / this.area);
        }

        internal void ComputeNormalMinArea(double minArea)
        {
            this.ComputeNormal();
            if (this.area < minArea)
            {
                // compute the normal without the longest edge
                QHHalfEdge maxEdge = null;
                var maxSquaredLength = 0.0;
                var edge = this.edge;
                // find the longest edge (in length) in the chain of edges
                do
                {
                    var lengthSquared = edge.LengthSquared();
                    if (lengthSquared > maxSquaredLength)
                    {
                        maxEdge = edge;
                        maxSquaredLength = lengthSquared;
                    }
                    edge = edge.next;
                } while (edge != this.edge);

                var p1 = maxEdge.Tail().point;
                var p2 = maxEdge.Head().point;
                var maxVector = p2.Subtract(p1);
                var maxLength = Math.Sqrt(maxSquaredLength);
                // maxVector is normalized after this operation
                maxVector = maxVector.Scale(1.0 / maxLength);
                // compute the projection of maxVector over this face normal
                var maxProjection = this.normal.Dot(maxVector);
                // subtract the quantity maxEdge adds on the normal
                maxVector = maxVector.Scale(-maxProjection);
                this.normal = this.normal.Add(maxVector);
                // renormalize `this.normal`
                this.normal = this.normal.Normalize();
            }
        }

        internal void ComputeCentroid()
        {
            this.centroid = new Vec3(0, 0, 0);
            var edge = this.edge;
            do
            {
                this.centroid = this.centroid.Add(edge.Head().point);
                edge = edge.next;
            } while (edge != this.edge);
            this.centroid = this.centroid.Scale(1 / (double)this.nVertices);
        }

        internal void ComputeNormalAndCentroid(double? minArea = null)
        {
            if (minArea is not null)
            {
                this.ComputeNormalMinArea((double)minArea);
            }
            else
            {
                this.ComputeNormal();
            }
            this.ComputeCentroid();
            this.offset = this.normal.Dot(this.centroid);
        }

        internal double DistanceToPlane(Vec3 point)
        {
            return this.normal.Dot(point) - this.offset;
        }

        /**
         * @private
         *
         * Connects two edges assuming that prev.Head().point == next.Tail().point
         *
         * @param {HalfEdge} prev
         * @param {HalfEdge} next
         */
        internal QHFace ConnectHalfEdges(QHHalfEdge prev, QHHalfEdge next)
        {
            QHFace discardedFace = null;
            if (prev.opposite.face == next.opposite.face)
            {
                // `prev` is remove a redundant edge
                var oppositeFace = next.opposite.face;
                QHHalfEdge oppositeEdge;
                if (prev == this.edge)
                {
                    this.edge = next;
                }
                if (oppositeFace.nVertices == 3)
                {
                    // case:
                    // remove the face on the right
                    //
                    //       /|\
                    //      / | \ the face on the right
                    //     /  |  \ --> opposite edge
                    //    / a |   \
                    //   *----*----*
                    //  /     b  |  \
                    //           ▾
                    //      redundant edge
                    //
                    // Note: the opposite edge is actually in the face to the right
                    // of the face to be destroyed
                    oppositeEdge = next.opposite.prev.opposite;
                    oppositeFace.mark = DELETED;
                    discardedFace = oppositeFace;
                }
                else
                {
                    // case:
                    //          t
                    //        *----
                    //       /| <- right face's redundant edge
                    //      / | opposite edge
                    //     /  |  ▴   /
                    //    / a |  |  /
                    //   *----*----*
                    //  /     b  |  \
                    //           ▾
                    //      redundant edge
                    oppositeEdge = next.opposite.next;
                    // make sure that the link `oppositeFace.edge` points correctly even
                    // after the right face redundant edge is removed
                    if (oppositeFace.edge == oppositeEdge.prev)
                    {
                        oppositeFace.edge = oppositeEdge;
                    }

                    //       /|   /
                    //      / | t/opposite edge
                    //     /  | / ▴  /
                    //    / a |/  | /
                    //   *----*----*
                    //  /     b     \
                    oppositeEdge.prev = oppositeEdge.prev.prev;
                    oppositeEdge.prev.next = oppositeEdge;
                }
                //       /|
                //      / |
                //     /  |
                //    / a |
                //   *----*----*
                //  /     b  ▴  \
                //           |
                //     redundant edge
                next.prev = prev.prev;
                next.prev.next = next;

                //       / \  \
                //      /   \->\
                //     /     \<-\ opposite edge
                //    / a     \  \
                //   *----*----*
                //  /     b  ^  \
                next.SetOpposite(oppositeEdge);

                oppositeFace.ComputeNormalAndCentroid();
            }
            else
            {
                // trivial case
                //        *
                //       /|\
                //      / | \
                //     /  |--> next
                //    / a |   \
                //   *----*----*
                //    \ b |   /
                //     \  |--> prev
                //      \ | /
                //       \|/
                //        *
                prev.next = next;
                next.prev = prev;
            }
            return discardedFace;
        }

        internal List<QHFace> MergeAdjacentFaces(QHHalfEdge adjacentEdge, List<QHFace> discardedFaces)
        {
            var oppositeEdge = adjacentEdge.opposite;
            var oppositeFace = oppositeEdge.face;


            discardedFaces.Add(oppositeFace);
            oppositeFace.mark = DELETED;

            // find the chain of edges whose opposite face is `oppositeFace`
            //
            //                ==>
            //      \         face         /
            //       * ---- * ---- * ---- *
            //      /     opposite face    \
            //                <==
            //
            var adjacentEdgePrev = adjacentEdge.prev;
            var adjacentEdgeNext = adjacentEdge.next;
            var oppositeEdgePrev = oppositeEdge.prev;
            var oppositeEdgeNext = oppositeEdge.next;

            // left edge
            while (adjacentEdgePrev.opposite.face == oppositeFace)
            {
                adjacentEdgePrev = adjacentEdgePrev.prev;
                oppositeEdgeNext = oppositeEdgeNext.next;
            }
            // right edge
            while (adjacentEdgeNext.opposite.face == oppositeFace)
            {
                adjacentEdgeNext = adjacentEdgeNext.next;
                oppositeEdgePrev = oppositeEdgePrev.prev;
            }
            // adjacentEdgePrev  \         face         / adjacentEdgeNext
            //                    * ---- * ---- * ---- *
            // oppositeEdgeNext  /     opposite face    \ oppositeEdgePrev

            // fix the face reference of all the opposite edges that are not part of
            // the edges whose opposite face is not `face` i.e. all the edges that
            // `face` and `oppositeFace` do not have in common
            QHHalfEdge edge;
            for (edge = oppositeEdgeNext; edge != oppositeEdgePrev.next; edge = edge.next)
            {
                edge.face = this;
            }

            // make sure that `face.edge` is not one of the edges to be destroyed
            // Note: it's important for it to be a `next` edge since `prev` edges
            // might be destroyed on `connectHalfEdges`
            this.edge = adjacentEdgeNext;

            // connect the extremes
            // Note: it might be possible that after connecting the edges a triangular
            // face might be redundant
            var discardedFace = this.ConnectHalfEdges(oppositeEdgePrev, adjacentEdgeNext);
            if (discardedFace is not null)
            {
                discardedFaces.Add(discardedFace);
            }
            discardedFace = this.ConnectHalfEdges(adjacentEdgePrev, oppositeEdgeNext);
            if (discardedFace is not null)
            {
                discardedFaces.Add(discardedFace);
            }

            this.ComputeNormalAndCentroid();
            // TODO: additional consistency checks
            return discardedFaces;
        }

        internal List<int> CollectIndices()
        {
            var indices = new List<int>();
            var edge = this.edge;
            do
            {
                indices.Add(edge.Head().index);
                edge = edge.next;
            } while (edge != this.edge);
            return indices;
        }

        internal static QHFace CreateTriangle(QHVertex v0, QHVertex v1, QHVertex v2, double minArea = 0)
        {
            var face = new QHFace();
            var e0 = new QHHalfEdge(v0, face);
            var e1 = new QHHalfEdge(v1, face);
            var e2 = new QHHalfEdge(v2, face);

            // join edges
            e0.next = e2.prev = e1;
            e1.next = e0.prev = e2;
            e2.next = e1.prev = e0;

            // main half edge reference
            face.edge = e0;
            face.ComputeNormalAndCentroid(minArea);
            return face;
        }
      #nullable enable
    }

}
