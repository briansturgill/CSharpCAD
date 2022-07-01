namespace QBox;

public partial class QBox
{
    /// <summary>Box wall thickness.</summary>
    public double Wall { get => this.wall; }
    /// <summary>Box width (X direction).</summary>
    public double Width { get => this.w; }
    /// <summary>Box depth (Y direction).</summary>
    public double Depth { get => this.d; }
    /// <summary>Box height (Z direction).</summary>
    public double Height { get => this.h; }

    /// <summary>Box height (Z direction).</summary>
    public double LidHoleSize { get => this.lid_hole_size; }
    private double w;
    private double d;
    private double h;
    private double lid_h;
    private double lid_hole_z_offset;
    private double lid_inset;
    private double lid_hole_size;
    private bool lid_bolts_tapered;
    private double wall;
    private double fillet;
    private double segments;
    /**
     * <summary>Quickly make project boxes.</summary>
     * <remarks>
     * Wall should be at least 2 or it may not print well.
     * Fillet must be greater than or equal to 2*wall.
     *</remarks>
     * <param name="w">Inner box width (X).</param>
     * <param name="d">Inner box depth (Y).</param>
     * <param name="h">Inner box height (Z).</param>
     * <param name="lid_h">For filleted_shell_lid.</param>
     * <param name="lid_hole_z_offset" default="lid_h/2">Z distance down from top of box.</param>
     * <param name="lid_inset">So lid is not such a tight fit.</param>
     * <param name="lid_hole_size">Bolt size as a "M" bolt.</param>
     * <param name="lid_bolts_tapered">Do bolts have tapered heads?</param>
     * <param name="wall">Box/lid wall thickness.</param>
     * <param name="fillet">Fillet radius of xy plane fillet.</param>
     * <param name="segments">Segments in fillet.</param>
     */
    public QBox(double w, double d, double h, double lid_h = 15,
        double lid_hole_z_offset = -1, double lid_inset = 0.3, double lid_hole_size = 3,
        bool lid_bolts_tapered = true, double wall = 2, double fillet = 5, int segments = 50)
    {
        if (lid_hole_z_offset == -1)
        {
            lid_hole_z_offset = lid_h / 2;
        }
        this.w = w;
        this.d = d;
        this.h = h;
        this.lid_h = lid_h;
        this.lid_hole_z_offset = lid_hole_z_offset;
        this.lid_inset = lid_inset;
        this.lid_hole_size = lid_hole_size;
        this.lid_bolts_tapered = lid_bolts_tapered;
        this.wall = wall;
        this.fillet = fillet;
        this.segments = segments;
    }

    public Geom3 Left(double x, double y, params Geom3[] gobjs)
    {

        var outGobjs = new Geom3[gobjs.Length];
        for (var i = 0; i < gobjs.Length; i++)
        {
            outGobjs[i] = Translate((0, x, y), Rotate((-90, 180, -90), gobjs[i]));
        }
        return Union(outGobjs);
    }

    public Geom3 Right(double x, double y, params Geom3[] gobjs)
    {
        return Translate((this.w, this.d - x, y), Rotate((-90, 180, 90), Union(gobjs)));
    }

    public Geom3 Front(double x, double y, params Geom3[] gobjs)
    {
        return Translate((this.w - x, 0, y), Rotate((90, 0, 180), Union(gobjs)));
    }

    public Geom3 Back(double x, double y, params Geom3[] gobjs)
    {
        return Translate((x, d, y), Rotate((90, 0, 0), Union(gobjs)));
    }

    public Geom3 Bottom(double x, double y, params Geom3[] gobjs)
    {
        return Translate((x, y, 0), Union(gobjs));
    }
}