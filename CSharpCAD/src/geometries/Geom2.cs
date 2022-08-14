namespace CSharpCAD;

/// <summary>Represents a 2D geometry consisting of an array of sides.</summary>
public class Geom2 : Geometry
{
    private NRTree nrtree;
    private Vec2[][]? outlines = null;
    private Side[]? sides = null;
    private Mat4 transforms;
    private (Vec2, Vec2)? boundingBox = null;
    private bool needsTransform = false;
    ///
    public Mat4 Transforms { get => this.transforms; }
    ///
    public Color? Color = null;

    /// <summary>Is this a 2D geometry object?</summary>
    public override bool Is2D => true;

    /// <summary>Is this a 3D geometry object?</summary>
    public override bool Is3D => false;

    /// <summary>Is this geometry empty?</summary>
    public bool IsEmpty => nrtree.NodeCount == 0;

    /// <summary>Empty constructor.</summary>
    public Geom2()
    {
        this.nrtree = new NRTree();
        this.transforms = new Mat4();
    }

    // Internal constructor.
    internal Geom2(NRTree nrtree, Mat4? transforms = null, Color? Color = null, bool needsTransform = false)
    {
        this.nrtree = nrtree;
        this.transforms = transforms ?? new Mat4();
        this.Color = Color;
        this.needsTransform = needsTransform;
        if (GlobalParams.CheckingEnabled)
        {
            this.CheckValid();
        }
    }

    // Internal constructor.
    // Will hopefully become obsolete.
    internal Geom2(Side[] sides, Mat4? transforms = null, Color? Color = null, bool needsTransform = false)
    {
        this.nrtree = SidesToNRTree(sides);
        this.transforms = transforms ?? new Mat4();
        this.Color = Color;
        this.needsTransform = needsTransform;
        if (GlobalParams.CheckingEnabled)
        {
            this.CheckValid();
        }
    }

    /**
     * <summary>Create a new 2D geometry from the given points.</summary>
     * <remarks>
     * The direction (rotation) of the points is not relevant,
     * as the points can define a convex or a concave polygon.
     * The geometry must not self intersect, i.e. the sides cannot cross.
     * </remarks>
     */
    public Geom2(List<Vec2> points)
    {
        var length = points.Count;
        if (length < 3)
        {
            throw new ArgumentException("The given points must define a closed geometry with three or more points.");
        }
        this.nrtree = new NRTree();
        this.nrtree.Insert(points.ToArray());
        this.transforms = new Mat4();
        if (GlobalParams.CheckingEnabled)
        {
            this.CheckValid();
        }
    }

    /**
     * <summary>Create a new 2D geometry from the given points.</summary>
     * <remarks>
     * The direction (rotation) of the points is not relevant,
     * as the points can define a convex or a concave polygon.
     * The geometry must not self intersect, i.e. the sides cannot cross.
     * </remarks>
     */
    internal Geom2(Vec2[] points)
    {
        var length = points.Length;
        if (length < 3)
        {
            throw new ArgumentException("The given points must define a closed geometry with three or more points.");
        }
        this.nrtree = new NRTree();
        this.nrtree.Insert(points);
        this.transforms = new Mat4();
        if (GlobalParams.CheckingEnabled)
        {
            this.CheckValid();
        }
    }

    /// <summary>Check if this geometry is equal to the given geometry.</summary>
    public bool Equals(Geom2 gg)
    {
        if (!this.nrtree.Equals(gg.nrtree))
        {
            return false;
        }
        if (this.transforms != gg.transforms)
        {
            return false;
        }
        return this.Color == gg.Color;
    }

    /// <summary>Check if this vector is equal to the given vector.</summary>
    public static bool operator ==(Geom2 a, Geom2 b)
    {
        return a.Equals(b);
    }

    /// <summary>Check if this vector is not equal to the given vector.</summary>
    public static bool operator !=(Geom2 a, Geom2 b) => !(a == b);

    /// <summary>Standard C# override.</summary>
    public override bool Equals(object? obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            Geom2 v = (Geom2)obj;
            return Equals(v);
        }
    }

    /// <summary>Standard C# override.</summary>
    public override int GetHashCode()
    {
        return nrtree.GetHashCode() ^ transforms.GetHashCode() ^ Color.GetHashCode();
    }

    /// <summary>Standard C# override.</summary>
	public override string ToString()
    {
        var s = new StringBuilder();
        s.AppendLine("Geom2");
        s.Append(nrtree.ToString());
        s.Append($"{this.transforms}\n");
        s.Append($"{(this.Color is not null ? this.Color.ToString() : "Color is null")}\n");
        return s.ToString();
    }

    /// <summary>Apply the transforms of the given geometry.</summary>
    /// <remarks>NOTE: This function must be called BEFORE exposing any data.</remarks>
    public Geom2 ApplyTransforms()
    {
        if (!this.needsTransform) return this;
        if (this.transforms.IsIdentity())
        {
            return this;
        }

        this.nrtree.Transform(this.transforms);
        this.transforms = new Mat4();
        this.needsTransform = false;
        return this;
    }

    /// <summary>Measure the min and max bounds of the given (geom2) geometry.</summary>
    public (Vec2, Vec2) BoundingBox()
    {
        if (this.boundingBox is not null) return ((Vec2, Vec2))this.boundingBox;
        this.ApplyTransforms();

        var bb = this.nrtree.BoundingBox();
        this.boundingBox = bb;
        return bb;
    }

    /// <summary>Return a clone of this geometry.</summary>
    public Geom2 Clone()
    {
        // Transforms and Color are immutable, so don't need to be explicitly copied.
        return new Geom2(this.nrtree.Clone(), this.transforms, this.Color, this.needsTransform);
    }

    /// <summary>Check that this geometry has only one connected path. (No cutouts.)</summary> 
    public bool HasOnlyOneConvexPath() => this.nrtree.HasOnlyOneConvexPath();

    /// <summary>Measure the epsilon of this geometry object.</summary>
    public double MeasureEpsilon()
    {
        var total = 0.0;
        var (min, max) = this.BoundingBox();
        total += max.X + max.Y;
        total -= min.X + min.Y;
        return C.EPS * total / 2; /*dimensions*/
    }

    internal Vec2[] ToSinglePath()
    {
        return nrtree.Root.Contained[0].Points;
    }

    /// <summary>Create a special list for Triangulator, holes sorted by max X.</summary>
    public List<(Vec2[], Vec2[][])> ToEarcutNesting()
    {
        ApplyTransforms();
        return nrtree.ToEarcutNesting();
    }

    /// <summary>Create a list of list of lists of Vec2 where each middle list contains a shape, follow by its assigned holes.</summary>
    public List<List<List<Vec2>>> ToMultiPolygon()
    {
        ApplyTransforms();
        return nrtree.ToMultiPolygon();
    }

    /// <summary>Create a list of list of lists of Vec2 where each middle list contains a shape, follow by its assigned holes.</summary>
    public List<(Vec2[], Vec2[][])> ToShapesAndHoles()
    {
        ApplyTransforms();
        return nrtree.ToShapesAndHoles();
    }

    /// <summary>Create the outline(s) of the given geometry.</summary>
    /// <remarks>ToOutlines is more efficient that ToOutLinesLLV.</remarks>
    public List<List<Vec2>> ToOutlinesLLV()
    {
        ApplyTransforms();
        return nrtree.ToOutlinesLLV();
    }

    /// <summary>Create the outline(s) of the given geometry.</summary>
    public Vec2[][] ToOutlines()
    {
        ApplyTransforms();
        if (outlines is null)
        {
            outlines = nrtree.ToOutlines();
        }
        return outlines;
    }

    /// <summary>Create the outline(s) of the given geometry.</summary>
    internal NRTree SidesToNRTree(Side[] sides)
    {
        var vertexMap = new Dictionary<Vec2, List<Side>>();
        var edges = sides;
        foreach (var edge in edges)
        {
            List<Side>? sideslist;
            var v0 = edge.v0;
            var v1 = edge.v1;
            if (!(vertexMap.TryGetValue(v0, out sideslist)))
            {
                vertexMap[v0] = new List<Side>();
            }
            sideslist = vertexMap[v0];
            sideslist.Add(edge);
        }

        var outlines = new List<List<Vec2>>();
        while (true)
        {
            Vec2 startside_0 = new Vec2();
            Vec2 startside_1 = new Vec2();
            foreach (var vertex in vertexMap.Keys)
            {
                var edge_list = vertexMap[vertex];
                if (edge_list.Count == 0)
                {
                    vertexMap.Remove(vertex);
                    continue;
                }
                startside_0 = edge_list[0].v0;
                startside_1 = edge_list[0].v1;
                edge_list.RemoveAt(0);
                break;
            }
            if (vertexMap.Count == 0) break; // all starting sides have been visited

            var connectedVertexPoints = new List<Vec2>();
            var startvertex = startside_0;
            while (true)
            {
                connectedVertexPoints.Add(startside_0);
                var nextvertex = startside_1;
                if (nextvertex == startvertex) break; // the outline has been closed
                List<Side>? nextpossiblesides;
                var valuefound = vertexMap.TryGetValue(nextvertex, out nextpossiblesides);
                if (!valuefound)
                {
                    throw new ValidationException("The given geometry is not closed. verify proper construction");
                }
                var nextsideindex = -1;
                if (nextpossiblesides!.Count == 1)
                {
                    nextsideindex = 0;
                }
                else
                {
                    // more than one side starting at the same vertex
                    double bestangle = 0.0;
                    var startangle = startside_1.Subtract(startside_0).AngleDegrees();
                    for (var sideindex = 0; sideindex < nextpossiblesides.Count; sideindex++)
                    {
                        var nextpossibleside_0 = nextpossiblesides[sideindex].v0;
                        var nextpossibleside_1 = nextpossiblesides[sideindex].v1;
                        var nextangle = nextpossibleside_1.Subtract(nextpossibleside_0).AngleDegrees();
                        var angledif = nextangle - startangle;
                        if (angledif < -180) angledif += 360;
                        if (angledif >= 180) angledif -= 360;
                        if ((nextsideindex < 0) || (angledif > bestangle))
                        {
                            nextsideindex = sideindex;
                            bestangle = angledif;
                        }
                    }
                }
                var nextside = nextpossiblesides[nextsideindex];
                nextpossiblesides.RemoveAt(nextsideindex); // remove side from list
                if (nextpossiblesides.Count == 0)
                {
                    vertexMap.Remove(nextvertex);
                }
                startside_0 = nextside.v0;
                startside_1 = nextside.v1;
            } // inner loop

            // due to the logic of fromPoints()
            // move the first point to the last
            if (connectedVertexPoints.Count > 0)
            {
                connectedVertexPoints.Add(connectedVertexPoints[0]);
                connectedVertexPoints.RemoveAt(0);
            }
            outlines.Add(connectedVertexPoints);
        } // outer loop
        var nrt = new Geom2.NRTree();
        foreach (var outline in outlines)
        {
            nrt.Insert(outline.ToArray());
        }
        return nrt;
    }

    /// <summary>Reverses the given geometry so that the sides are flipped in the opposite order.</summary>
    public Geom2 Reverse()
    {
        // LATER does this ever make sense?
        return new Geom2(nrtree.Reverse(), transforms, Color, needsTransform);
    }

    /**
     * <summary>Produces an array of points from the given geometry.</summary>
     * <remarks>
     * NOTE: The points returned do NOT define an order. Use toOutlines() for ordered points.
     * </remarks>
     */
    public Vec2[] ToPoints()
    {
        this.ApplyTransforms();
        return nrtree.ToPoints();
    }

    /*
     * Produces an array of sides from the given geometry.
     *
     * The returned array should not be modified as the data is shared with the geometry.
     * NOTE: The sides returned do NOT define an order. Use toOutlines() for ordered points.
     *
     */
    internal Side[] ToSides()
    {
        if (this.sides is not null) return sides;
        var ls = new List<Side>();
        var outlines = ToOutlines();
        foreach (var outline in outlines)
        {
            var len = outline.Length;
            if (len > 1 && outline[0] == outline[len - 1]) len--;
            var prevpoint = outline[len - 1];
            for (var i = 0; i < len; i++)
            {
                var point = outline[i];
                ls.Add(new Side(prevpoint, point));
                prevpoint = point;
            }
        }
        sides = ls.ToArray();
        return sides;
    }

    /**
     * <summary>Transform this geometry using the given matrix.</summary>
     * <remarks>
     * This is a lazy transform of the sides, as this function only adjusts the transforms.
     * The transforms are applied when accessing the sides via ToOutlines() or other such retrieval.
     * The retrieval method calls ApplyTransforms().
     * </remarks>
     */
    public Geom2 Transform(Mat4 matrix)
    {
        var transforms = matrix.Multiply(this.transforms);
        return new Geom2(this.nrtree.Clone(), transforms, this.Color, needsTransform: true);
    }

    // Internal class.
    internal class Side : IEquatable<Side>
    {
        public readonly Vec2 v0;
        public readonly Vec2 v1;

        public Side()
        {
            this.v0 = new Vec2();
            this.v1 = new Vec2();
        }

        public Side(Vec2 v0, Vec2 v1)
        {
            this.v0 = v0;
            this.v1 = v1;
        }

        public bool Equals(Side? gs)
        {
            if (gs is null)
            {
                return false;
            }
            return this.v0 == gs.v0 && this.v1 == gs.v1;
        }

        public static bool operator ==(Side a, Side b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Side a, Side b) => !(a == b);

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Side s = (Side)obj;
                return Equals(s);
            }
        }

        public override int GetHashCode()
        {
            return v0.GetHashCode() ^ v1.GetHashCode();
        }

        public override string ToString() => $"Side({this.v0},{this.v1})";
    }

    internal class NRTreeNode
    {
        internal Vec2[] Points;
        internal Vec2 Min; // BoundingBox of points
        internal Vec2 Max;
        internal List<NRTreeNode> Contained;

        internal NRTreeNode(Vec2[] points)
        {
            Points = points;
            updateBbox();
            Contained = new List<NRTreeNode>();
        }

        private NRTreeNode(Vec2[] points, Vec2 min, Vec2 max, List<NRTreeNode> contained)
        {
            this.Points = points;
            this.Min = min;
            this.Max = max;
            var len = contained.Count;
            this.Contained = new List<NRTreeNode>(contained.Count);
            for (var i = 0; i < len; i++)
            {
                this.Contained.Add(contained[i].Clone());
            }
        }

        internal bool Equals(NRTreeNode gn)
        {
            var len = this.Points.Length;
            if (len != gn.Points.Length) return false;
            for (var i = 0; i < len; i++)
            {
                if (this.Points[i] != gn.Points[i]) return false;
            }
            return true;
        }

        internal NRTreeNode Clone()
        {
            return new NRTreeNode(Points.ToArray(), Min, Max, Contained);
        }

        internal void Transform(Mat4 mat)
        {
            var pts = Points;
            var len = pts.Length;
            for (var i = 0; i < len; i++)
            {
                pts[i] = pts[i].Transform(mat);
            }
            updateBbox();
        }

        private void updateBbox()
        {
            var poly = this.Points;
            if (poly.Length == 0) return;
            var min = poly[0];
            var max = poly[0];
            for (int i = 1; i < poly.Length; i++)
            {
                min = poly[i].Min(min);
                max = poly[i].Max(max);
            }
            this.Min = min;
            this.Max = max;
        }

        internal bool contains(NRTreeNode nrt)
        {
            return this.Min.X < nrt.Min.X && this.Min.Y < nrt.Min.Y && this.Max.X > nrt.Max.X && this.Max.Y > nrt.Max.Y;
        }
    }

    /*
        NRTree uses bounding boxes to sort a Geom2s outline into a nested tree.
        Nodes at level 0 are the exterior shapes.
        Nodes at level 1 are the first level holes.
        Nodes at level 2 are the first level of interior shapes.
        Nodes at level 3 are the second level of holes.
        ...
    */
    internal class NRTree // Nested Ring Tree
    {
        internal NRTreeNode Root;
        internal int NodeCount;
        internal NRTree()
        {
            Root = new NRTreeNode(new Vec2[0]);
            NodeCount = 0;
        }

        private NRTree(NRTreeNode root, int count)
        {
            Root = root;
            NodeCount = count;
        }

        internal bool Equals(NRTree gt)
        {
            if (this.NodeCount != gt.NodeCount) return false;
            return _equals(this.Root, gt.Root);
        }

        private bool _equals(NRTreeNode a, NRTreeNode b)
        {
            foreach (var an in a.Contained)
            {
                foreach (var bn in b.Contained)
                {
                    if (!an.Equals(bn)) return false;
                    if (!_equals(an, bn)) return false;
                }
            }
            return true;
        }

        internal bool HasOnlyOneConvexPath()
        {
            int turn(Vec2 v1, Vec2 v2, Vec2 v3)
            {
                var val = (v2.X - v1.X) * (v3.Y - v1.Y) - (v2.Y - v1.Y) * (v3.X - v1.X);
                if (Math.Abs(val) <= C.EPSILON)
                {
                    return 0;
                }
                return Math.Sign(val);
            }
            // Does this geometry have only one path?
            if (this.Root.Contained.Count != 1 || this.Root.Contained[0].Contained.Count != 0)
            {
                return false;
            }

            // Check if path is convex -- basically checks that all turns are in the same direction.
            // Important: This algoritm can be fooled by some self-intersecting paths.
            // However, as we don't allow self-intersecting paths...
            // Alas, detecting self-intersecting paths is slow.
            var s = 0;
            var pts = this.Root.Contained[0].Points;
            var len = pts.Length;
            for (int i = 0; i < len; i++)
            {
                var t = turn(pts[i], pts[(i + 1) % len], pts[(i + 2) % len]);
                if (s == 0) s = t;
                if (t != 0 && t != s) return false;
            }
            return true;
        }

        internal (Vec2, Vec2) BoundingBox()
        {
            // Because the first "row" is the exterior rings, we only need to aggregate their bounding boxes.
            var len = Root.Contained.Count;
            if (len == 0) return (new Vec2(), new Vec2());
            var min = this.Root.Contained[0].Min;
            var max = this.Root.Contained[0].Max;
            for (var i = 1; i < len; i++)
            {
                var n = Root.Contained[i];
                min = min.Min(n.Min);
                max = max.Max(n.Max);
            }
            return (min, max);
        }

        internal void Insert(Vec2[] ring)
        {
            _insert(this.Root, new NRTreeNode(ring));
            NodeCount++;
        }

        private void _insert(NRTreeNode parent, NRTreeNode child)
        {
            var len = parent.Contained.Count;
            for (var i = 0; i < len; i++)
            {
                var n = parent.Contained[i];
                if (n.contains(child))
                {
                    this._insert(n, child);
                    return;
                }
            }
            for (var i = 0; i < len; i++)
            {
                var n = parent.Contained[i];
                if (child.contains(n))
                {
                    parent.Contained.RemoveAt(i);
                    len--;
                    i--;
                    this._insert(child, n);
                    continue;
                }
            }
            parent.Contained.Add(child);
        }

        internal NRTree Reverse()
        {
            var clone = this.Clone();
            _reverse(clone.Root);
            return clone;
        }

        private void _reverse(NRTreeNode parent)
        {
            foreach (var n in parent.Contained)
            {
                Array.Reverse(n.Points);
                _reverse(n);
            }
        }

        internal void Transform(Mat4 mat)
        {
            _transform(this.Root, mat);
        }

        private void _transform(NRTreeNode parent, Mat4 mat)
        {
            foreach (var n in parent.Contained)
            {
                n.Transform(mat);
                _transform(n, mat);
            }
        }

        // Only for use by Bridge.cs in NPolyBool. MODIFIES existing data!
        internal void CorrectWindings()
        {
            _correctWindings(this.Root);
        }

        private void _correctWindings(NRTreeNode parent, int depth = 0)
        {
            foreach (var n in parent.Contained)
            {
                var winding = Winding(n.Points);
                if ((depth % 2 == 0 && winding == "cw") || (depth % 2 == 1 && winding == "ccw"))
                {
                    if (GlobalParams.DebugOutput) Console.WriteLine($"Correcting winding at depth: {depth}");
                    Array.Reverse(n.Points);
                }
                _correctWindings(n, depth + 1);
            }
        }

        internal Vec2[] ToPoints()
        {
            var lv = new List<Vec2>();
            _toPoints(this.Root, lv);
            return lv.ToArray();
        }

        private void _toPoints(NRTreeNode parent, List<Vec2> lv)
        {
            var len = parent.Contained.Count;
            var contained = parent.Contained;
            for (var i = 0; i < len; i++)
            {
                var n = contained[i];
                lv.AddRange(n.Points);
                _toPoints(n, lv);
            }
        }

        internal Vec2[][] ToOutlines()
        {
            var aav = new Vec2[this.NodeCount][];
            var aav_idx = 0;
            _toOutlines(this.Root, aav, ref aav_idx);
            return aav;
        }

        private void _toOutlines(NRTreeNode parent, Vec2[][] aav, ref int aav_idx)
        {
            var len = parent.Contained.Count;
            var contained = parent.Contained;
            for (var i = 0; i < len; i++)
            {
                var n = contained[i];
                aav[aav_idx++] = n.Points;
                _toOutlines(n, aav, ref aav_idx);
            }
        }

        internal List<(Vec2[], Vec2[][])> ToEarcutNesting()
        {
            var l = new List<(Vec2[], Vec2[][])>();
            _toEarcutNesting(this.Root, l);
            return l;
        }

        private void _toEarcutNesting(NRTreeNode parent, List<(Vec2[], Vec2[][])> l, int depth = 0)
        {
            foreach (var n in parent.Contained)
            {
                if (depth % 2 == 0)
                {
                    var shape = n.Points;
                    var holes = new Vec2[n.Contained.Count][];
                    var tmp = n.Contained.ToArray();
                    // Earcut requires greatest Max.X first.
                    int compareByMaxX(NRTreeNode a, NRTreeNode b)
                    {
                        var ret = Math.Sign(b.Max.X - a.Max.X);
                        if (ret == 0) ret = Math.Sign(b.Max.Y - a.Max.Y);
                        return ret;
                    };
                    Array.Sort(tmp, compareByMaxX);
                    var tmplen = tmp.Length;
                    for (var i = 0; i < tmplen; i++)
                    {
                        holes[i] = tmp[i].Points.ToArray();
                    }
                    l.Add((shape, holes));
                }
                _toEarcutNesting(n, l, depth + 1);
            }
        }

        internal List<List<List<Vec2>>> ToMultiPolygon()
        {
            var lllv = new List<List<List<Vec2>>>();
            _toMultiPolygon(this.Root, lllv);
            return lllv;
        }

        private void _toMultiPolygon(NRTreeNode parent, List<List<List<Vec2>>> lllv, int depth = 0)
        {
            foreach (var n in parent.Contained)
            {
                if (depth % 2 == 0)
                {
                    var llv = new List<List<Vec2>>(n.Contained.Count + 1);
                    llv.Add(n.Points.ToList());
                    var contained = n.Contained;
                    for (var i = 1; i < llv.Count; i++)
                    {
                        llv.Add(contained[i - 1].Points.ToList());
                    }
                    lllv.Add(llv);
                }
                _toMultiPolygon(n, lllv, depth + 1);
            }
        }

        internal List<(Vec2[], Vec2[][])> ToShapesAndHoles()
        {
            var l = new List<(Vec2[], Vec2[][])>();
            _toShapesAndHoles(this.Root, l);
            return l;
        }

        private void _toShapesAndHoles(NRTreeNode parent, List<(Vec2[], Vec2[][])> l, int depth = 0)
        {
            foreach (var n in parent.Contained)
            {
                if (depth % 2 == 0)
                {
                    var shape = n.Points;
                    var holes = new Vec2[n.Contained.Count][];
                    var tmp = n.Contained;
                    var tmplen = tmp.Count;
                    for (var i = 0; i < tmplen; i++)
                    {
                        holes[i] = tmp[i].Points.ToArray();
                    }
                    l.Add((shape, holes));
                }
                _toShapesAndHoles(n, l, depth + 1);
            }
        }

        internal List<List<Vec2>> ToOutlinesLLV()
        {
            var llv = new List<List<Vec2>>(this.NodeCount);
            _toOutlinesLLV(this.Root, llv);
            return llv;
        }

        private void _toOutlinesLLV(NRTreeNode parent, List<List<Vec2>> llv)
        {
            var len = parent.Contained.Count;
            var contained = parent.Contained;
            for (var i = 0; i < len; i++)
            {
                var n = contained[i];
                llv.Add(n.Points.ToList());
                _toOutlinesLLV(n, llv);
            }
        }

        internal NRTree Clone()
        {
            return new NRTree(this.Root.Clone(), this.NodeCount);
        }

        internal void Validate()
        {
            _validate(this.Root);
        }

        private void _validate(NRTreeNode parent, int depth = 0)
        {
            // check for self-edges
            foreach (var child in parent.Contained)
            {
                var len = child.Points.Length;
                for (var i = 0; i < len; i++)
                {
                    var j = i + 1 >= len ? 0 : i + 1;
                    if (child.Points[i].IsNearlyEqual(child.Points[j]))
                    {
                        throw new ValidationException($"Geom2 self-edge {child.Points[i]}");
                    }
                }
                var winding = Winding(child.Points);
                if (depth % 2 == 0)
                {
                    if (winding != "ccw")
                    {
                        throw new ValidationException($"Improper winding for shape: {winding}");
                    }
                }
                else
                {
                    if (winding != "cw")
                    {
                        throw new ValidationException($"Improper winding for hole: {winding}");
                    }
                }
                _validate(child, depth + 1);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"NRTree nodes={this.NodeCount}");
            _toString(this.Root, sb);
            return sb.ToString();
        }

        private void _toString(NRTreeNode parent, StringBuilder sb, int depth = 0)
        {
            foreach (var n in parent.Contained)
            {
                for (var i = 0; i <= depth; i++)
                {
                    sb.Append("    ");
                }
                if (depth % 2 == 0)
                {
                    sb.Append("shape ");
                }
                else
                {
                    sb.Append("hole  ");
                }
                foreach (var pt in n.Points)
                {
                    sb.Append($"{pt}");
                }
                sb.AppendLine();
                for (var i = 0; i <= depth; i++)
                {
                    sb.Append("    ");
                }
                sb.AppendLine($"-min={n.Min} max={n.Max}, {Winding(n.Points.ToList())}");
                _toString(n, sb, depth + 1);
            }
        }
    }

    /**
     * <summary>Determine if this object is a valid geom2.</summary>
     * <remarks>
     * Checks for closedness, self-edges, and valid data points.
     *
     * **If the geometry is not valid, an exception will be thrown with details of the geometry error.**
     * </remarks>
     */
    public override void Validate()
    {
        nrtree.Validate();

        // check transforms
        this.transforms.Validate();
        // LATER: check for self-intersecting
    }

    private void CheckValid()
    {
        try
        {
            this.Validate();
        }
        catch (ValidationException)
        {
            throw;
            /*
            Console.WriteLine($"Validation Exception: {e.Message}");
            var st = new StackTrace(e, true);
            var frame = st.GetFrame(st.FrameCount - 1);
            var method = frame?.GetMethod()?.ToString() ?? "unknown";
            Console.WriteLine($"  --At {frame.GetFileName()}:{frame.GetFileLineNumber()} {method}");
            */
        }
    }
}
