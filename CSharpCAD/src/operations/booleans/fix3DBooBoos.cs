namespace CSharpCAD;

public static partial class CSCAD
{
    internal static Poly3[] Fix3DBooBoos(string tag, List<Poly3> _polys)
    {
        _polys = Retessellate(_polys);

        var ret = _polys.ToArray();

        tag = ""; // Suppresses logging.
        MakePointsStable(tag, ret);

        return ret;
    }
}