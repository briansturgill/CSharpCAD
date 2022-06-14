namespace CSharpCAD;

/// <summary>Represents a 2D geometry consisting of an array of sides.</summary>
public class Geom2 : Geometry
{
    private Side[] sides;
    private Mat4 transforms;
    private (Vec2, Vec2)? boundingBox;
    private bool needsTransform;
    ///
    public Mat4 Transforms { get => this.transforms; }
    ///
    public Color? Color;

    /// <summary>Is this a 2D geometry object?</summary>
    public override bool Is2D => true;

    /// <summary>Is this a 3D geometry object?</summary>
    public override bool Is3D => false;

    /// <summary>Empty constructor.</summary>
    public Geom2()
    {
        this.sides = new Side[0];
        this.transforms = new Mat4();
        this.Color = null;
        this.needsTransform = false;
        this.boundingBox = null;
    }

    // Internal constructor.
    internal Geom2(Side[] sides, Mat4? transforms = null, Color? Color = null, bool needsTransform = false)
    {
        this.sides = sides;
        this.transforms = transforms ?? new Mat4();
        this.Color = Color;
        this.needsTransform = needsTransform;
        this.boundingBox = null;
        if (GlobalParams.CheckingEnabled)
        {
            this.Validate();
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
        // adjust length if the given points are closed by the same point
        if (points[0].IsNearlyEqual(points[length - 1])) { --length; }

        var sides = new Side[length];

        var prevpoint = points[length - 1];

        for (var i = 0; i < length; i++)
        {
            var point = points[i];
            sides[i] = new Side(prevpoint, point);
            prevpoint = point;
        }
        this.sides = sides;
        this.Color = null;
        this.transforms = new Mat4();
        this.needsTransform = false;
        this.boundingBox = null;
        if (GlobalParams.CheckingEnabled)
        {
            this.Validate();
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
    public Geom2(Vec2[] points)
    {
        var length = points.Length;
        if (length < 3)
        {
            throw new ArgumentException("The given points must define a closed geometry with three or more points.");
        }
        // adjust length if the given points are closed by the same point
        if (points[0].IsNearlyEqual(points[length - 1])) { --length; }

        var sides = new Side[length];

        var prevpoint = points[length - 1];

        for (var i = 0; i < length; i++)
        {
            var point = points[i];
            sides[i] = new Side(prevpoint, point);
            prevpoint = point;
        }
        this.sides = sides;
        this.Color = null;
        this.transforms = new Mat4();
        if (GlobalParams.CheckingEnabled)
        {
            this.Validate();
        }
    }

    /// <summary>Check if this geometry is equal to the given geometry.</summary>
    public bool Equals(Geom2 gg)
    {
        if (this.sides.Length != gg.sides.Length)
        {
            return false;
        }
        for (var i = 0; i < sides.Length; i++)
        {
            if (this.sides[i] != gg.sides[i])
            {
                return false;
            }
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
        return sides.GetHashCode() ^ transforms.GetHashCode() ^ Color.GetHashCode();
    }

    /// <summary>Standard C# override.</summary>
	public override string ToString()
    {
        var s = new StringBuilder();
        s.Append("Geom2(\n");
        foreach (var side in this.sides)
        {
            s.Append($"({side.v0}, {side.v1})\n");
        }
        s.Append($"{this.transforms}\n");
        s.Append($"{this.Color}\n");
        return s.ToString();
    }

    /// <summary>Used mostly for testing.</summary>
    public bool IsNearlyEqual(Geom2 gg)
    {
        if (this.sides.Length != gg.sides.Length)
        {
            return false;
        }
        for (var i = 0; i < sides.Length; i++)
        {
            var this_side = this.sides[i];
            var gg_side = gg.sides[i];
            if (!this_side.v0.IsNearlyEqual(gg_side.v0))
            {
                return false;
            }
            if (!this_side.v1.IsNearlyEqual(gg_side.v1))
            {
                return false;
            }
        }
        if (!this.transforms.IsNearlyEqual(gg.transforms))
        {
            return false;
        }
        return this.Color == gg.Color; // LATER is this wise?
    }



    /// <summary>Apply the transforms of the given geometry.</summary>
    /// <remarks>NOTE: This function must be called BEFORE exposing any data. See ToSides().</remarks>
    public Geom2 ApplyTransforms()
    {
        if (!this.needsTransform) return this;
        if (this.transforms.IsIdentity())
        {
            return this;
        }

        // apply transforms to each side
        for (var i = 0; i < this.sides.Length; i++)
        {
            var side = sides[i];
            var p0 = side.v0.Transform(this.transforms);
            var p1 = side.v1.Transform(this.transforms);
            this.sides[i] = new Side(p0, p1);
        }
        this.transforms = new Mat4();
        this.needsTransform = false;
        return this;
    }

    /// <summary>Measure the min and max bounds of the given (geom2) geometry.</summary>
    public (Vec2, Vec2) BoundingBox()
    {
        if (this.boundingBox is not null) return ((Vec2, Vec2))this.boundingBox;
        this.ApplyTransforms();
        if (sides.Length == 0)
        {
            return (new Vec2(), new Vec2());
        }
        var p = sides[0].v0;
        var min_x = p.X;
        var min_y = p.Y;
        var max_x = min_x;
        var max_y = min_y;

        foreach (var side in this.sides)
        {
            var p0 = side.v0;
            if (p0.X < min_x) min_x = p0.X;
            if (p0.Y < min_y) min_y = p0.Y;
            if (p0.X > max_x) max_x = p0.X;
            if (p0.Y > max_y) max_y = p0.Y;
        }

        var bb = (new Vec2(min_x, min_y), new Vec2(max_x, max_y));
        this.boundingBox = bb;
        return bb;
    }

    /// <summary>Return a clone of this geometry.</summary>
    public Geom2 Clone()
    {
        // Sides, transforms and Color are immutable, so don't need to be explicitly copied.
        return new Geom2(this.sides.ToArray(), this.transforms, this.Color, this.needsTransform);
    }

    /// <summary>Check that this geometry has only one connected path. (No cutouts.)</summary> 
    public bool HasOnlyOnePath { get => sides[0].v0 == sides[sides.Length - 1].v1; }

    /// <summary>Measure the epsilon of this geometry object.</summary>
    public double MeasureEpsilon()
    {
        var total = 0.0;
        var (min, max) = this.BoundingBox();
        total += max.X + max.Y;
        total -= min.X + min.Y;
        return C.EPS * total / 2; /*dimensions*/
    }

    /// <summary>Create the outline(s) of the given geometry.</summary>
    public List<List<Vec2>> ToOutlines(bool doTransformsFirst = true)
    {
        var vertexMap = new Dictionary<Vec2, List<Side>>();
        var edges = doTransformsFirst ? this.ToSides() : this.sides;
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
        vertexMap.Clear();
        return outlines;
    }

    /// <summary>Reverses the given geometry so that the sides are flipped in the opposite order.</summary>
    public Geom2 Reverse()
    {
        var oldsides = this.ToSides();
        var len = oldsides.Length;
        var newsides = new Side[len];
        for (var i = 0; i < len; i++)
        {
            var side = oldsides[i];
            newsides[len - i - 1] = new Side(side.v1, side.v0);
        }
        return new Geom2(newsides, new Mat4(), Color);
    }

    /**
     * <summary>Produces an array of points from the given geometry.</summary>
     * <remarks>
     * NOTE: The points returned do NOT define an order. Use toOutlines() for ordered points.
     * </remarks>
     */
    public Vec2[] ToPoints()
    {
        var sides = ToSides();
        var points = new List<Vec2>(sides.Length + 1);
        foreach (var side in sides)
        {
            var p0 = side.v0;
            points.Add(p0);
        }
        // due to the logic of fromPoints()
        // move the first point to the last
        if (points.Count > 0)
        {
            var first = points[0];
            points.Add(first);
            points.RemoveAt(0);
        }
        return points.ToArray();
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
        return this.ApplyTransforms().sides;
    }

    /**
     * <summary>Transform this geometry using the given matrix.</summary>
     * <remarks>
     * This is a lazy transform of the sides, as this function only adjusts the transforms.
     * The transforms are applied when accessing the sides via ToSides().
     * </remarks>
     */
    public Geom2 Transform(Mat4 matrix)
    {
        var transforms = matrix.Multiply(this.transforms);
        return new Geom2(this.sides.ToArray(), transforms, this.Color, needsTransform: true);
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
        // check for closedness
        this.ToOutlines(false);

        // check for self-edges
        foreach (var side in sides)
        {
            if (side.v0.IsNearlyEqual(side.v1))
            {
                throw new ValidationException($"Geom2 self-edge {side.v0}");
            }
        }

        // check transforms
        this.transforms.Validate();
        // LATER: check for self-intersecting
    }

}
