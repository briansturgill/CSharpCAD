namespace CSharpCAD;

public static partial class CSCAD
{
    /*
     * Original source from quickhull3d (https://github.com/mauriciopoppe/quickhull3d)
     * Copyright (c) 2015 Mauricio Poppe
     *
     * Adapted to JSCAD by Jeff Gay
     */

    internal class QuickHull
    {
        // merge types
        // non convex with respect to the large face
        const int MERGE_NON_CONVEX_WRT_LARGER_FACE = 1;
        const int MERGE_NON_CONVEX = 2;

        internal double tolerance;
        internal int nFaces;
        internal int nPoints;
        internal List<QHFace> faces;
        internal List<QHFace> newFaces;
        internal QHVertexList claimed;
        internal QHVertexList unclaimed;
        internal List<QHVertex> vertices;
        internal List<QHFace> discardedFaces;
        internal List<int> vertexPointIndices;

        internal QuickHull(Vec3[] points)
        {
            if (points.Length < 4)
            {
                throw new ArgumentException("Cannot build a 3D hull out of less than 4 unique points.");
            }

            this.tolerance = -1;

            // buffers
            this.nFaces = 0;
            this.nPoints = points.Length;


            this.faces = new List<QHFace>();
            this.newFaces = new List<QHFace>();
            // helpers
            //
            // var `a`, `b` be `Face` instances
            // var `v` be points wrapped as instance of `Vertex`
            //
            //     [v, v, ..., v, v, v, ...]
            //      ^             ^
            //      |             |
            //  a.outside     b.outside
            //
            this.claimed = new QHVertexList();
            this.unclaimed = new QHVertexList();

            // vertices of the hull(internal representation of points)
            this.vertices = new List<QHVertex>(points.Length);
            for (var i = 0; i < points.Length; i += 1)
            {
                this.vertices.Add(new QHVertex(points[i], i));
            }
            this.discardedFaces = new List<QHFace>();
            this.vertexPointIndices = new List<int>();
        }

        internal void AddVertexToFace(QHVertex vertex, QHFace face)
        {
            vertex.face = face;
            if (face.outside is null)
            {
                this.claimed.Add(vertex);
            }
            else
            {
                this.claimed.InsertBefore(face.outside, vertex);
            }
            face.outside = vertex;
        }

        /*
         * Removes `vertex` for the `claimed` list of vertices, it also makes sure
         * that the link from `face` to the first vertex it sees in `claimed` is
         * linked correctly after the removal
         *
         * @param {Vertex} vertex
         * @param {Face} face
         */
        internal void RemoveVertexFromFace(QHVertex vertex, QHFace face)
        {
            if (vertex == face.outside)
            {
                // fix face.outside link
                if (vertex.next is not null && vertex.next.face == face)
                {
                    // face has at least 2 outside vertices, move the `outside` reference
                    face.outside = vertex.next;
                }
                else
                {
                    // vertex was the only outside vertex that face had
                    face.outside = null;
                }
            }
            this.claimed.Remove(vertex);
        }

        /*
         * Removes all the visible vertices that `face` is able to see which are
         * stored in the `claimed` vertext list
         *
         * @param {Face} face
         * @return {Vertex|undefined} If face had visible vertices returns
         * `face.outside`, otherwise undefined
         */
        internal QHVertex? RemoveAllVerticesFromFace(QHFace face)
        {
            if (face.outside is not null)
            {
                // pointer to the last vertex of this face
                // [..., outside, ..., end, outside, ...]
                //          |           |      |
                //          a           a      b
                var end = face.outside;
                while (end is not null && end.next is not null && end.next.face == face)
                {
                    end = end.next;
                }
                if (end is not null) this.claimed.RemoveChain(face.outside, end);
                //                            b
                //                       [ outside, ...]
                //                            |  removes this link
                //     [ outside, ..., end ] -┘
                //          |           |
                //          a           a
                if (end is not null) end.next = null;
            }
            return face.outside;
        }

        /*
         * Removes all the visible vertices that `face` is able to see, additionally
         * checking the following:
         *
         * If `absorbingFace` doesn't exist then all the removed vertices will be
         * added to the `unclaimed` vertex list
         *
         * If `absorbingFace` exists then this method will assign all the vertices of
         * `face` that can see `absorbingFace`, if a vertex cannot see `absorbingFace`
         * it's added to the `unclaimed` vertex list
         *
         * @param {Face} face
         * @param {Face} [absorbingFace]
         */
        internal void DeleteFaceVertices(QHFace face, QHFace? absorbingFace = null)
        {
            var faceVertices = this.RemoveAllVerticesFromFace(face);
            if (faceVertices is not null)
            {
                if (absorbingFace is null)
                {
                    // mark the vertices to be reassigned to some other face
                    this.unclaimed.AddAll(faceVertices);
                }
                else
                {
                    // if there's an absorbing face try to assign as many vertices
                    // as possible to it

                    // the reference `vertex.next` might be destroyed on
                    // `this.addVertexToFace` (see VertexList#add), nextVertex is a
                    // reference to it
                    QHVertex? nextVertex;
                    for (var vertex = faceVertices; vertex is not null; vertex = nextVertex)
                    {
                        nextVertex = vertex.next;
                        var distance = absorbingFace.DistanceToPlane(vertex.point);

                        // check if `vertex` is able to see `absorbingFace`
                        if (distance > this.tolerance)
                        {
                            this.AddVertexToFace(vertex, absorbingFace);
                        }
                        else
                        {
                            this.unclaimed.Add(vertex);
                        }
                    }
                }
            }
        }

        /*
         * Reassigns as many vertices as possible from the unclaimed list to the new
         * faces
         *
         * @param {Faces[]} newFaces
         */
        internal void ResolveUnclaimedPoints(List<QHFace> newFaces)
        {
            // cache next vertex so that if `vertex.next` is destroyed it's still
            // recoverable
            var vertexNext = this.unclaimed.First();
            for (var vertex = vertexNext; vertex is not null; vertex = vertexNext)
            {
                vertexNext = vertex.next;
                var maxDistance = this.tolerance;
                QHFace? maxFace = null;
                for (var i = 0; i < newFaces.Count; i++)
                {
                    var face = newFaces[i];
                    if (face.mark == QHFace.VISIBLE)
                    {
                        var dist = face.DistanceToPlane(vertex.point);
                        if (dist > maxDistance)
                        {
                            maxDistance = dist;
                            maxFace = face;
                        }
                        if (maxDistance > 1000 * this.tolerance)
                        {
                            break;
                        }
                    }
                }

                if (maxFace is not null)
                {
                    ;
                    this.AddVertexToFace(vertex, maxFace);
                }
            }
        }

        /*
         * Computes the extremes of a tetrahedron which will be the initial hull
         *
         * @return {number[]} The min/max vertices in the x,y,z directions
         */
        internal (QHVertex[], QHVertex[]) ComputeExtremes()
        {
            var min = new double[3];
            var max = new double[3];

            // min vertex on the x,y,z directions
            var minVertices = new QHVertex[3];
            // max vertex on the x,y,z directions
            var maxVertices = new QHVertex[3];

            int i;

            // initially assume that the first vertex is the min/max
            for (i = 0; i < 3; i++)
            {
                minVertices[i] = maxVertices[i] = this.vertices[0];
            }
            // copy the coordinates of the first vertex to min/max
            min[0] = max[0] = this.vertices[0].point.X;
            min[1] = max[1] = this.vertices[0].point.Y;
            min[2] = max[2] = this.vertices[0].point.Z;


            // compute the min/max vertex on all 6 directions
            for (i = 1; i < this.vertices.Count; i++)
            {
                var vertex = this.vertices[i];
                var point = vertex.point;
                // update the min coordinates
                if (point.X < min[0])
                {
                    min[0] = point.X;
                    minVertices[0] = vertex;
                }
                if (point.Y < min[1])
                {
                    min[1] = point.Y;
                    minVertices[1] = vertex;
                }
                if (point.Z < min[2])
                {
                    min[2] = point.Z;
                    minVertices[2] = vertex;
                }
                // update the max coordinates
                if (point.X > max[0])
                {
                    max[0] = point.X;
                    maxVertices[0] = vertex;
                }
                if (point.Y > max[1])
                {
                    max[1] = point.Y;
                    maxVertices[1] = vertex;
                }
                if (point.Z > max[2])
                {
                    max[2] = point.Z;
                    maxVertices[2] = vertex;
                }
            }

            // compute epsilon
            this.tolerance = 3 * C.EPSILON * (
              Math.Max(Math.Abs(min[0]), Math.Abs(max[0])) +
              Math.Max(Math.Abs(min[1]), Math.Abs(max[1])) +
              Math.Max(Math.Abs(min[2]), Math.Abs(max[2]))
            );
            return (minVertices, maxVertices);
        }

        /*
         * Compues the initial tetrahedron assigning to its faces all the points that
         * are candidates to form part of the hull
         */
        internal void CreateInitialSimplex()
        {
            var vertices = this.vertices;
            var (min, max) = this.ComputeExtremes();
            QHVertex v2, v3;
            int i;

            // Find the two vertices with the greatest 1d separation
            // (max.x - min.x)
            // (max.y - min.y)
            // (max.z - min.z)
            var maxDistance = 0.0;
            var indexMax = 0;
            var distances = new double[] { max[0].point.X - min[0].point.X, max[1].point.Y - min[1].point.Y, max[2].point.Z - min[2].point.Z };
            for (i = 0; i < 3; i++)
            {
                var distance = distances[i];
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    indexMax = i;
                }
            }

            var v0 = min[indexMax];
            var v1 = max[indexMax];

            v3 = v2 = v1; // Make compiler quit complaining about uninitialized.

            // the next vertex is the one farthest to the line formed by `v0` and `v1`
            maxDistance = 0;
            for (i = 0; i < this.vertices.Count; i += 1)
            {
                var vertex = this.vertices[i];
                if (vertex != v0 && vertex != v1)
                {
                    var distance = PointLineDistance(vertex.point, v0.point, v1.point);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        v2 = vertex;
                    }
                }
            }

            // the next vertex is the one farthest to the plane `v0`, `v1`, `v2`
            // normalize((v2 - v1) x (v0 - v1))
            var normal = GetPlaneNormal(v0.point, v1.point, v2.point);
            // distance from the origin to the plane
            var distPO = v0.point.Dot(normal);
            maxDistance = -1;
            for (i = 0; i < this.vertices.Count; i += 1)
            {
                var vertex = this.vertices[i];
                if (vertex != v0 && vertex != v1 && vertex != v2)
                {
                    var distance = Math.Abs(normal.Dot(vertex.point) - distPO);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        v3 = vertex;
                    }
                }
            }

            // initial simplex
            // Taken from http://everything2.com/title/How+to+paint+a+tetrahedron
            //
            //                              v2
            //                             ,|,
            //                           ,7``\'VA,
            //                         ,7`   |, `'VA,
            //                       ,7`     `\    `'VA,
            //                     ,7`        |,      `'VA,
            //                   ,7`          `\         `'VA,
            //                 ,7`             |,           `'VA,
            //               ,7`               `\       ,..ooOOTK` v3
            //             ,7`                  |,.ooOOT''`    AV
            //           ,7`            ,..ooOOT`\`           /7
            //         ,7`      ,..ooOOT''`      |,          AV
            //        ,T,..ooOOT''`              `\         /7
            //     v0 `'TTs.,                     |,       AV
            //            `'TTs.,                 `\      /7
            //                 `'TTs.,             |,    AV
            //                      `'TTs.,        `\   /7
            //                           `'TTs.,    |, AV
            //                                `'TTs.,\/7
            //                                     `'T`
            //                                       v1
            //
            var faces = new List<QHFace>();
            if (v3.point.Dot(normal) - distPO < 0)
            {
                // the face is not able to see the point so `planeNormal`
                // is pointing outside the tetrahedron
                faces.Add(QHFace.CreateTriangle(v0, v1, v2));
                faces.Add(QHFace.CreateTriangle(v3, v1, v0));
                faces.Add(QHFace.CreateTriangle(v3, v2, v1));
                faces.Add(QHFace.CreateTriangle(v3, v0, v2));

                // set the opposite edge
                for (i = 0; i < 3; i += 1)
                {
                    var j = (i + 1) % 3;
                    // join face[i] i > 0, with the first face
                    faces[i + 1].GetEdge(2).SetOpposite(faces[0].GetEdge(j));
                    // join face[i] with face[i + 1], 1 <= i <= 3
                    faces[i + 1].GetEdge(1).SetOpposite(faces[j + 1].GetEdge(0));
                }
            }
            else
            {
                // the face is able to see the point so `planeNormal`
                // is pointing inside the tetrahedron
                faces.Add(QHFace.CreateTriangle(v0, v2, v1));
                faces.Add(QHFace.CreateTriangle(v3, v0, v1));
                faces.Add(QHFace.CreateTriangle(v3, v1, v2));
                faces.Add(QHFace.CreateTriangle(v3, v2, v0));

                // set the opposite edge
                for (i = 0; i < 3; i += 1)
                {
                    var j = (i + 1) % 3;
                    // join face[i] i > 0, with the first face
                    faces[i + 1].GetEdge(2).SetOpposite(faces[0].GetEdge((3 - i) % 3));
                    // join face[i] with face[i + 1]
                    faces[i + 1].GetEdge(0).SetOpposite(faces[j + 1].GetEdge(1));
                }
            }

            // the initial hull is the tetrahedron
            for (i = 0; i < 4; i += 1)
            {
                this.faces.Add(faces[i]);
            }

            // initial assignment of vertices to the faces of the tetrahedron
            for (i = 0; i < vertices.Count; i++)
            {
                var vertex = vertices[i];
                if (vertex != v0 && vertex != v1 && vertex != v2 && vertex != v3)
                {
                    maxDistance = this.tolerance;
                    QHFace? maxFace = null;
                    for (var j = 0; j < 4; j += 1)
                    {
                        var distance = faces[j].DistanceToPlane(vertex.point);
                        if (distance > maxDistance)
                        {
                            maxDistance = distance;
                            maxFace = faces[j];
                        }
                    }

                    if (maxFace is not null)
                    {
                        this.AddVertexToFace(vertex, maxFace);
                    }
                }
            }
        }

        internal void ReindexFaceAndVertices()
        {
            // remove inactive faces
            var activeFaces = new List<QHFace>();
            for (var i = 0; i < this.faces.Count; i++)
            {
                var face = this.faces[i];
                if (face.mark == QHFace.VISIBLE)
                {
                    activeFaces.Add(face);
                }
            }
            this.faces = activeFaces;
        }

        internal List<List<int>> CollectFaces(bool skipTriangulation)
        {
            var faceIndices = new List<List<int>>();
            for (var i = 0; i < this.faces.Count; i += 1)
            {
                if (this.faces[i].mark != QHFace.VISIBLE)
                {
                    throw new Exception("Attempt to include a destroyed face in the hull");
                }
                var indices = this.faces[i].CollectIndices();
                if (skipTriangulation)
                {
                    faceIndices.Add(indices);
                }
                else
                {
                    for (var j = 0; j < indices.Count - 2; j += 1)
                    {
                        faceIndices.Add(new List<int> { indices[0], indices[j + 1], indices[j + 2] });
                    }
                }
            }
            return faceIndices;
        }

        /*
         * Finds the next vertex to make faces with the current hull
         *
         * - var `face` be the first face existing in the `claimed` vertex list
         *  - if `face` doesn't exist then return since there're no vertices left
         *  - otherwise for each `vertex` that face sees find the one furthest away
         *  from `face`
         *
         * @return {Vertex|undefined} Returns undefined when there're no more
         * visible vertices
         */
#nullable disable
        internal QHVertex NextVertexToAdd()
        {
            if (!this.claimed.IsEmpty())
            {
                QHVertex eyeVertex = null;
                QHVertex vertex;
                var maxDistance = 0.0;
                var eyeFace = this.claimed.First().face;
                for (vertex = eyeFace.outside; vertex is not null && vertex.face == eyeFace; vertex = vertex.next)
                {
                    var distance = eyeFace.DistanceToPlane(vertex.point);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        eyeVertex = vertex;
                    }
                }
                return eyeVertex;
            }
            return null;
        }
#nullable enable

        /*
         * Computes a chain of half edges in ccw order called the `horizon`, for an
         * edge to be part of the horizon it must join a face that can see
         * `eyePoint` and a face that cannot see `eyePoint`
         *
         * @param {number[]} eyePoint - The coordinates of a point
         * @param {HalfEdge} crossEdge - The edge used to jump to the current `face`
         * @param {Face} face - The current face being tested
         * @param {HalfEdge[]} horizon - The edges that form part of the horizon in
         * ccw order
         */
        internal void ComputeHorizon(Vec3 eyePoint, QHHalfEdge? crossEdge, QHFace face, List<QHHalfEdge> horizon)
        {
            // moves face's vertices to the `unclaimed` vertex list
            this.DeleteFaceVertices(face);


            face.mark = QHFace.DELETED;


            QHHalfEdge? edge;
            if (crossEdge is null)
            {
                edge = crossEdge = face.GetEdge(0);
            }
            else
            {
                // start from the next edge since `crossEdge` was already analyzed
                // (actually `crossEdge.opposite` was the face who called this method
                // recursively)
                edge = crossEdge.next;
            }

            // All the faces that are able to see `eyeVertex` are defined as follows
            //
            //       v    /
            //           / <== visible face
            //          /
            //         |
            //         | <== not visible face
            //
            //  dot(v, visible face normal) - visible face offset > this.tolerance
            //
            do
            {
                if (edge is null) break;
                var oppositeEdge = edge.opposite;
                if (oppositeEdge is null) break;
                var oppositeFace = oppositeEdge.face;
                if (oppositeFace.mark == QHFace.VISIBLE)
                {
                    if (oppositeFace.DistanceToPlane(eyePoint) > this.tolerance)
                    {
                        this.ComputeHorizon(eyePoint, oppositeEdge, oppositeFace, horizon);
                    }
                    else
                    {
                        horizon.Add(edge);
                    }
                }
                edge = edge.next;
            } while (edge != crossEdge);
        }

        /*
         * Creates a face with the points `eyeVertex.point`, `horizonEdge.tail` and
         * `horizonEdge.tail` in ccw order
         *
         * @param {Vertex} eyeVertex
         * @param {HalfEdge} horizonEdge
         * @return {HalfEdge} The half edge whose vertex is the eyeVertex
         */
        internal QHHalfEdge AddAdjoiningFace(QHVertex eyeVertex, QHHalfEdge horizonEdge)
        {
            // all the half edges are created in ccw order thus the face is always
            // pointing outside the hull
            // edges:
            //
            //                  eyeVertex.point
            //                       / \
            //                      /   \
            //                  1  /     \  0
            //                    /       \
            //                   /         \
            //                  /           \
            //          horizon.tail --- horizon.head
            //                        2
            //
            var face = QHFace.CreateTriangle(eyeVertex, horizonEdge.Tail(), horizonEdge.Head());
            this.faces.Add(face);
            // join face.getEdge(-1) with the horizon's opposite edge
            // face.getEdge(-1) = face.getEdge(2)
#nullable disable
            face.GetEdge(-1).SetOpposite(horizonEdge.opposite);
#nullable enable
            return face.GetEdge(0);
        }

        /*
         * Adds horizon.length faces to the hull, each face will be 'linked' with the
         * horizon opposite face and the face on the left/right
         *
         * @param {Vertex} eyeVertex
         * @param {HalfEdge[]} horizon - A chain of half edges in ccw order
         */
        internal void AddNewFaces(QHVertex eyeVertex, List<QHHalfEdge> horizon)
        {
            this.newFaces = new List<QHFace>();
            QHHalfEdge? firstSideEdge = null;
            QHHalfEdge? previousSideEdge = null;
            for (var i = 0; i < horizon.Count; i++)
            {
                var horizonEdge = horizon[i];
                // returns the right side edge
                var sideEdge = this.AddAdjoiningFace(eyeVertex, horizonEdge);
                if (firstSideEdge is null)
                {
                    firstSideEdge = sideEdge;
                }
                else
                {
                    // joins face.getEdge(1) with previousFace.getEdge(0)
                    if (sideEdge.next is not null && previousSideEdge is not null) sideEdge.next.SetOpposite(previousSideEdge);
                }
                this.newFaces.Add(sideEdge.face);
                previousSideEdge = sideEdge;
            }
            if (firstSideEdge is not null && firstSideEdge.next is not null && previousSideEdge is not null)
            {
                firstSideEdge.next.SetOpposite(previousSideEdge);
            }
        }

        /*
         * Computes the distance from `edge` opposite face's centroid to
         * `edge.face`
         *
         * @param {HalfEdge} edge
         * @return {number}
         * - A positive number when the centroid of the opposite face is above the
         *   face i.e. when the faces are concave
         * - A negative number when the centroid of the opposite face is below the
         *   face i.e. when the faces are convex
         */
        internal double OppositeFaceDistance(QHHalfEdge edge)
        {
#nullable disable
            return edge.face.DistanceToPlane(edge.opposite.face.centroid);
#nullable enable
        }

        /*
         * Merges a face with none/any/all its neighbors according to the strategy
         * used
         *
         * if `mergeType` is MERGE_NON_CONVEX_WRT_LARGER_FACE then the merge will be
         * decided based on the face with the larger area, the centroid of the face
         * with the smaller area will be checked against the one with the larger area
         * to see if it's in the merge range [tolerance, -tolerance] i.e.
         *
         *    dot(centroid smaller face, larger face normal) - larger face offset > -tolerance
         *
         * Note that the first check (with +tolerance) was done on `computeHorizon`
         *
         * If the above is not true then the check is done with respect to the smaller
         * face i.e.
         *
         *    dot(centroid larger face, smaller face normal) - smaller face offset > -tolerance
         *
         * If true then it means that two faces are non convex (concave), even if the
         * dot(...) - offset value is > 0 (that's the point of doing the merge in the
         * first place)
         *
         * If two faces are concave then the check must also be done on the other face
         * but this is done in another merge pass, for this to happen the face is
         * marked in a temporal NON_CONVEX state
         *
         * if `mergeType` is MERGE_NON_CONVEX then two faces will be merged only if
         * they pass the following conditions
         *
         *    dot(centroid smaller face, larger face normal) - larger face offset > -tolerance
         *    dot(centroid larger face, smaller face normal) - smaller face offset > -tolerance
         *
         * @param {Face} face
         * @param {number} mergeType - Either MERGE_NON_CONVEX_WRT_LARGER_FACE or
         * MERGE_NON_CONVEX
         */
        internal bool DoAdjacentMerge(QHFace face, int mergeType)
        {
            var edge = face.edge;
            var convex = true;
            var it = 0;
            do
            {
                if (it >= face.nVertices)
                {
                    throw new Exception("Merge recursion limit exceeded.");
                }
#nullable disable
                var oppositeFace = edge.opposite.face;
#nullable enable
                var merge = false;

                // Important notes about the algorithm to merge faces
                //
                // - Given a vertex `eyeVertex` that will be added to the hull
                //   all the faces that cannot see `eyeVertex` are defined as follows
                //
                //      dot(v, not visible face normal) - not visible offset < tolerance
                //
                // - Two faces can be merged when the centroid of one of these faces
                // projected to the normal of the other face minus the other face offset
                // is in the range [tolerance, -tolerance]
                // - Since `face` (given in the input for this method) has passed the
                // check above we only have to check the lower bound e.g.
                //
                //      dot(v, not visible face normal) - not visible offset > -tolerance
                //
                if (mergeType == MERGE_NON_CONVEX)
                {
                    if (this.OppositeFaceDistance(edge) > -this.tolerance ||
                        this.OppositeFaceDistance(edge.opposite) > -this.tolerance)
                    {
                        merge = true;
                    }
                }
                else
                {
                    if (face.area > oppositeFace.area)
                    {
                        if (this.OppositeFaceDistance(edge) > -this.tolerance)
                        {
                            merge = true;
                        }
                        else if (this.OppositeFaceDistance(edge.opposite) > -this.tolerance)
                        {
                            convex = false;
                        }
                    }
                    else
                    {
                        if (this.OppositeFaceDistance(edge.opposite) > -this.tolerance)
                        {
                            merge = true;
                        }
                        else if (this.OppositeFaceDistance(edge) > -this.tolerance)
                        {
                            convex = false;
                        }
                    }
                }

                if (merge)
                {
                    // when two faces are merged it might be possible that redundant faces
                    // are destroyed, in that case move all the visible vertices from the
                    // destroyed faces to the `unclaimed` vertex list
                    var discardedFaces = face.MergeAdjacentFaces(edge, new List<QHFace>());
                    for (var i = 0; i < discardedFaces.Count; i += 1)
                    {
                        this.DeleteFaceVertices(discardedFaces[i], face);
                    }
                    return true;
                }

                edge = edge.next;
                it += 1;
            } while (edge != face.edge);
            if (!convex)
            {
                face.mark = QHFace.NON_CONVEX;
            }
            return false;
        }

        /*
         * Adds a vertex to the hull with the following algorithm
         *
         * - Compute the `horizon` which is a chain of half edges, for an edge to
         *   belong to this group it must be the edge connecting a face that can
         *   see `eyeVertex` and a face which cannot see `eyeVertex`
         * - All the faces that can see `eyeVertex` have its visible vertices removed
         *   from the claimed VertexList
         * - A new set of faces is created with each edge of the `horizon` and
         *   `eyeVertex`, each face is connected with the opposite horizon face and
         *   the face on the left/right
         * - The new faces are merged if possible with the opposite horizon face first
         *   and then the faces on the right/left
         * - The vertices removed from all the visible faces are assigned to the new
         *   faces if possible
         *
         * @param {Vertex} eyeVertex
         */
        internal void AddVertexToHull(QHVertex eyeVertex)
        {
            var horizon = new List<QHHalfEdge>();


            if (this.unclaimed is not null) this.unclaimed.Clear();

            // remove `eyeVertex` from `eyeVertex.face` so that it can't be added to the
            // `unclaimed` vertex list
#nullable disable
            this.RemoveVertexFromFace(eyeVertex, eyeVertex.face);
#nullable enable
            this.ComputeHorizon(eyeVertex.point, null, eyeVertex.face, horizon);
            this.AddNewFaces(eyeVertex, horizon);

            // first merge pass
            // Do the merge with respect to the larger face
            for (var i = 0; i < this.newFaces.Count; i++)
            {
                var face = this.newFaces[i];
                if (face.mark == QHFace.VISIBLE)
                {
                    while (this.DoAdjacentMerge(face, MERGE_NON_CONVEX_WRT_LARGER_FACE)) { }
                }
            }

            // second merge pass
            // Do the merge on non convex faces (a face is marked as non convex in the
            // first pass)
            for (var i = 0; i < this.newFaces.Count; i++)
            {
                var face = this.newFaces[i];
                if (face.mark == QHFace.NON_CONVEX)
                {
                    face.mark = QHFace.VISIBLE;
                    while (this.DoAdjacentMerge(face, MERGE_NON_CONVEX)) { }
                }
            }

            // reassign `unclaimed` vertices to the new faces
            this.ResolveUnclaimedPoints(this.newFaces);
        }

        internal void Build()
        {
            QHVertex eyeVertex;
            this.CreateInitialSimplex();
            while ((eyeVertex = this.NextVertexToAdd()) is not null)
            {
                this.AddVertexToHull(eyeVertex);
            }
            this.ReindexFaceAndVertices();
        }
    }
}