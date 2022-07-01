namespace CSharpCAD;

public static partial class CSCAD
{

    /// <summary>Represent a color in a space efficient manner.</summary>
    public readonly struct Color : IEquatable<Color>
    {
        /// <summary>Red</summary>
        public readonly byte r;
        /// <summary>Green</summary>
        public readonly byte g;
        /// <summary>Blue</summary>
        public readonly byte b;
        /// <summary>Alpha range is 0-255, with 255 meaning opaque.</summary>
        public readonly byte a;

        /// <summary>Construct from 3 RGB bytes.</summary>
        /// <remarks>With no arguments, construct the color "black".</remarks>
        public Color(byte r, byte g, byte b, byte a = 255)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        /// <summary>Construct from a CSS extended name or a hex specification (begins with) repeated 3 times.</summary>
        public Color(string color, byte alpha = (byte)255)
        {
            if (color[0] == '#')
            {
                (r, g, b) = CSCAD.HexToRGB(color);
                a = alpha;
            }
            else
            {
                (r, g, b) = CSCAD.ColorNameToRGB(color);
                a = alpha;
            }
        }

        /// <summary>Automatically convert a string to a Color.</summary>
        public static implicit operator Color(string colorString)
        {
            return new Color(colorString);
        }

        /// <summary>Automatically convert a tuple of 4 bytes to a Color.</summary>
        public static implicit operator Color((byte, byte, byte, byte) tuple)
        {
            var (r, g, b, a) = tuple;
            return new Color(r, g, b, a);
        }

        /// <summary>Automatically convert a tuple of 3 bytes to a Color.</summary>
        public static implicit operator Color((byte, byte, byte) tuple)
        {
            var (r, g, b) = tuple;
            return new Color(r, g, b);
        }

        /// <summary>Check if this Color is equal to the given Color.</summary>
        public bool Equals(Color gc)
        {
            return this.r == gc.r &&
                this.g == gc.g &&
                this.b == gc.b &&
                this.a == gc.a;
        }

        /// <summary>Check if this vector is equal to the given vector.</summary>
        public static bool operator ==(Color a, Color b)
        {
            return a.Equals(b);
        }

        /// <summary>Check if this Color is not equal to the given Color.</summary>
        public static bool operator !=(Color a, Color b) => !(a == b);

        /// <summary>Standard C# override.</summary>
        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Vec3 v = (Vec3)obj;
                return Equals(v);
            }
        }

        /// <summary>Standard C# override.</summary>
        public override int GetHashCode()
        {
            return r.GetHashCode() ^ g.GetHashCode() ^ b.GetHashCode() ^ a.GetHashCode();
        }

        /// <summary>Standard C# override.</summary>
        public override string ToString()
        {
            return $"rgba({r},{g},{b},{a})";
        }
    }

    private static Geom2 colorGeom2(Color color, Geom2 g)
    {
        Geom2 newgeom2 = g.Clone();
        newgeom2.Color = color;
        return newgeom2;
    }

    private static Geom3 colorGeom3(Color color, Geom3 g)
    {
        Geom3 newgeom3 = g.Clone();
        newgeom3.Color = color;
        return newgeom3;
    }

    /**
     * <summary>Assign the given color to the given objects.</summary>
     * <param name="color">Has 3 formats:
     *   A C# Tuple of RGB or RGBA color values, where each value is between 0 and 255.
     *   A string beginning with "#" followed by 6 hex digits representing RGB.
     *   A string that s one of the extended CSS color names.
     * </param>
     * <param name="obj">A 2D geometry object.</param>
     * <example>
     * Colorize((255, 0, 0), obj); // Colorizes obj brightest red. Fully opaque (alpha defaults to 255).
     * Colorize((255, 0, 0, 128), obj); // Colorizes obj brightest red, alpha at half opacity.
     * Colorize("#00FF00", obj); // Colorizes obj brightest green. Fully opaque (alpha defaults to "FF").
     * Colorize("#00FF0080", obj); // Colorizes obj brightest green, alpha at half opacity.
     * Colorize("salmon", obj); // Colorizes obj with the CSS color named "salmon". Fully opaque.
     * </example>
     * <group>Miscellaneous</group>
     */
    public static Geom2 Colorize(Color color, Geom2 obj)
    {
        return colorGeom2(color, obj);
    }

    /**
     * <summary>Assign the given color to the given objects.</summary>
     * <param name="color">Has 3 formats:
     *   A C# Tuple of RGB or RGBA color values, where each value is between 0 and 255.
     *   A string beginning with "#" followed by 6 hex digits representing RGB.
     *   A string that s one of the extended CSS color names.
     * </param>
     * <param name="obj">A 3D geometry object.</param>
     * <example>
     * Colorize((255, 0, 0), obj); // Colorizes obj brightest red. Fully opaque (alpha defaults to 255).
     * Colorize((255, 0, 0, 128), obj); // Colorizes obj brightest red, alpha at half opacity.
     * Colorize("#00FF00", obj); // Colorizes obj brightest green. Fully opaque (alpha defaults to "FF").
     * Colorize("#00FF0080", obj); // Colorizes obj brightest green, alpha at half opacity.
     * Colorize("salmon", obj); // Colorizes obj with the CSS color named "salmon". Fully opaque.
     * </example>
     * <group>Miscellaneous</group>
     */
    public static Geom3 Colorize(Color color, Geom3 obj)
    {
        return colorGeom3(color, obj);
    }

    // <summary>Converts a CSS color name to RGB color.</summary>
    internal static (byte, byte, byte) ColorNameToRGB(string colorName)
    {
        (byte, byte, byte) color;
        colorName = colorName.ToLower();
        if (!cssColors.TryGetValue(colorName, out color))
        {
            throw new ArgumentException($"Color \"{color}\" is not a known color");
        }
        return color;
    }

    private static Dictionary<string, (byte, byte, byte)> cssColors = new Dictionary<string, (byte, byte, byte)>();

    /// <summary>Get array of color names.</summary>
    /// <group>Miscellaneous</group>
    public static string[] GetColorNames()
    {
        var n = cssColors.Keys.ToArray();
        Array.Sort(n);
        return n;
    }

    static CSCAD()
    {
        // basic color keywords
        cssColors.Add("black", (0, 0, 0));
        cssColors.Add("silver", (192, 192, 192));
        cssColors.Add("gray", (128, 128, 128));
        cssColors.Add("white", (255, 255, 255));
        cssColors.Add("maroon", (128, 0, 0));
        cssColors.Add("red", (255, 0, 0));
        cssColors.Add("purple", (128, 0, 128));
        cssColors.Add("fuchsia", (255, 0, 255));
        cssColors.Add("green", (0, 128, 0));
        cssColors.Add("lime", (0, 255, 0));
        cssColors.Add("olive", (128, 128, 0));
        cssColors.Add("yellow", (255, 255, 0));
        cssColors.Add("navy", (0, 0, 128));
        cssColors.Add("blue", (0, 0, 255));
        cssColors.Add("teal", (0, 128, 128));
        cssColors.Add("aqua", (0, 255, 255));
        // extended color keywords
        cssColors.Add("aliceblue", (240, 248, 255));
        cssColors.Add("antiquewhite", (250, 235, 215));
        cssColors.Add("aquamarine", (127, 255, 212));
        cssColors.Add("azure", (240, 255, 255));
        cssColors.Add("beige", (245, 245, 220));
        cssColors.Add("bisque", (255, 228, 196));
        cssColors.Add("blanchedalmond", (255, 235, 205));
        cssColors.Add("blueviolet", (138, 43, 226));
        cssColors.Add("brown", (165, 42, 42));
        cssColors.Add("burlywood", (222, 184, 135));
        cssColors.Add("cadetblue", (95, 158, 160));
        cssColors.Add("chartreuse", (127, 255, 0));
        cssColors.Add("chocolate", (210, 105, 30));
        cssColors.Add("coral", (255, 127, 80));
        cssColors.Add("cornflowerblue", (100, 149, 237));
        cssColors.Add("cornsilk", (255, 248, 220));
        cssColors.Add("crimson", (220, 20, 60));
        cssColors.Add("cyan", (0, 255, 255));
        cssColors.Add("darkblue", (0, 0, 139));
        cssColors.Add("darkcyan", (0, 139, 139));
        cssColors.Add("darkgoldenrod", (184, 134, 11));
        cssColors.Add("darkgray", (169, 169, 169));
        cssColors.Add("darkgreen", (0, 100, 0));
        cssColors.Add("darkgrey", (169, 169, 169));
        cssColors.Add("darkkhaki", (189, 183, 107));
        cssColors.Add("darkmagenta", (139, 0, 139));
        cssColors.Add("darkolivegreen", (85, 107, 47));
        cssColors.Add("darkorange", (255, 140, 0));
        cssColors.Add("darkorchid", (153, 50, 204));
        cssColors.Add("darkred", (139, 0, 0));
        cssColors.Add("darksalmon", (233, 150, 122));
        cssColors.Add("darkseagreen", (143, 188, 143));
        cssColors.Add("darkslateblue", (72, 61, 139));
        cssColors.Add("darkslategray", (47, 79, 79));
        cssColors.Add("darkslategrey", (47, 79, 79));
        cssColors.Add("darkturquoise", (0, 206, 209));
        cssColors.Add("darkviolet", (148, 0, 211));
        cssColors.Add("deeppink", (255, 20, 147));
        cssColors.Add("deepskyblue", (0, 191, 255));
        cssColors.Add("dimgray", (105, 105, 105));
        cssColors.Add("dimgrey", (105, 105, 105));
        cssColors.Add("dodgerblue", (30, 144, 255));
        cssColors.Add("firebrick", (178, 34, 34));
        cssColors.Add("floralwhite", (255, 250, 240));
        cssColors.Add("forestgreen", (34, 139, 34));
        cssColors.Add("gainsboro", (220, 220, 220));
        cssColors.Add("ghostwhite", (248, 248, 255));
        cssColors.Add("gold", (255, 215, 0));
        cssColors.Add("goldenrod", (218, 165, 32));
        cssColors.Add("greenyellow", (173, 255, 47));
        cssColors.Add("grey", (128, 128, 128));
        cssColors.Add("honeydew", (240, 255, 240));
        cssColors.Add("hotpink", (255, 105, 180));
        cssColors.Add("indianred", (205, 92, 92));
        cssColors.Add("indigo", (75, 0, 130));
        cssColors.Add("ivory", (255, 255, 240));
        cssColors.Add("khaki", (240, 230, 140));
        cssColors.Add("lavender", (230, 230, 250));
        cssColors.Add("lavenderblush", (255, 240, 245));
        cssColors.Add("lawngreen", (124, 252, 0));
        cssColors.Add("lemonchiffon", (255, 250, 205));
        cssColors.Add("lightblue", (173, 216, 230));
        cssColors.Add("lightcoral", (240, 128, 128));
        cssColors.Add("lightcyan", (224, 255, 255));
        cssColors.Add("lightgoldenrodyellow", (250, 250, 210));
        cssColors.Add("lightgray", (211, 211, 211));
        cssColors.Add("lightgreen", (144, 238, 144));
        cssColors.Add("lightgrey", (211, 211, 211));
        cssColors.Add("lightpink", (255, 182, 193));
        cssColors.Add("lightsalmon", (255, 160, 122));
        cssColors.Add("lightseagreen", (32, 178, 170));
        cssColors.Add("lightskyblue", (135, 206, 250));
        cssColors.Add("lightslategray", (119, 136, 153));
        cssColors.Add("lightslategrey", (119, 136, 153));
        cssColors.Add("lightsteelblue", (176, 196, 222));
        cssColors.Add("lightyellow", (255, 255, 224));
        cssColors.Add("limegreen", (50, 205, 50));
        cssColors.Add("linen", (250, 240, 230));
        cssColors.Add("magenta", (255, 0, 255));
        cssColors.Add("mediumaquamarine", (102, 205, 170));
        cssColors.Add("mediumblue", (0, 0, 205));
        cssColors.Add("mediumorchid", (186, 85, 211));
        cssColors.Add("mediumpurple", (147, 112, 219));
        cssColors.Add("mediumseagreen", (60, 179, 113));
        cssColors.Add("mediumslateblue", (123, 104, 238));
        cssColors.Add("mediumspringgreen", (0, 250, 154));
        cssColors.Add("mediumturquoise", (72, 209, 204));
        cssColors.Add("mediumvioletred", (199, 21, 133));
        cssColors.Add("midnightblue", (25, 25, 112));
        cssColors.Add("mintcream", (245, 255, 250));
        cssColors.Add("mistyrose", (255, 228, 225));
        cssColors.Add("moccasin", (255, 228, 181));
        cssColors.Add("navajowhite", (255, 222, 173));
        cssColors.Add("oldlace", (253, 245, 230));
        cssColors.Add("olivedrab", (107, 142, 35));
        cssColors.Add("orange", (255, 165, 0));
        cssColors.Add("orangered", (255, 69, 0));
        cssColors.Add("orchid", (218, 112, 214));
        cssColors.Add("palegoldenrod", (238, 232, 170));
        cssColors.Add("palegreen", (152, 251, 152));
        cssColors.Add("paleturquoise", (175, 238, 238));
        cssColors.Add("palevioletred", (219, 112, 147));
        cssColors.Add("papayawhip", (255, 239, 213));
        cssColors.Add("peachpuff", (255, 218, 185));
        cssColors.Add("peru", (205, 133, 63));
        cssColors.Add("pink", (255, 192, 203));
        cssColors.Add("plum", (221, 160, 221));
        cssColors.Add("powderblue", (176, 224, 230));
        cssColors.Add("rosybrown", (188, 143, 143));
        cssColors.Add("royalblue", (65, 105, 225));
        cssColors.Add("saddlebrown", (139, 69, 19));
        cssColors.Add("salmon", (250, 128, 114));
        cssColors.Add("sandybrown", (244, 164, 96));
        cssColors.Add("seagreen", (46, 139, 87));
        cssColors.Add("seashell", (255, 245, 238));
        cssColors.Add("sienna", (160, 82, 45));
        cssColors.Add("skyblue", (135, 206, 235));
        cssColors.Add("slateblue", (106, 90, 205));
        cssColors.Add("slategray", (112, 128, 144));
        cssColors.Add("slategrey", (112, 128, 144));
        cssColors.Add("snow", (255, 250, 250));
        cssColors.Add("springgreen", (0, 255, 127));
        cssColors.Add("steelblue", (70, 130, 180));
        cssColors.Add("tan", (210, 180, 140));
        cssColors.Add("thistle", (216, 191, 216));
        cssColors.Add("tomato", (255, 99, 71));
        cssColors.Add("turquoise", (64, 224, 208));
        cssColors.Add("violet", (238, 130, 238));
        cssColors.Add("wheat", (245, 222, 179));
        cssColors.Add("whitesmoke", (245, 245, 245));
        cssColors.Add("yellowgreen", (154, 205, 50));
    }

    // <summary>Converts CSS color notations (string of hex values) to RGB values.</summary>
    internal static (byte, byte, byte) HexToRGB(string notation, byte alpha = (byte)255)
    {
        notation = notation.Replace("#", "");

        if (notation.Length < 6) throw new ArgumentException("The given color notation must contain 3 or more hex values");

        byte r = byte.Parse(notation.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
        byte g = byte.Parse(notation.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
        byte b = byte.Parse(notation.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
        return (r, g, b);
    }
}
