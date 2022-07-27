namespace CSharpCAD;

internal static partial class Geom2Booleans
{
    internal static bool equals(Vec2 p1, Vec2 p2)
    {
      return Math.Abs(p1.X - p2.X) <= C.EPS && Math.Abs(p1.Y - p2.Y) <= C.EPS;
      /*
        if (p1.X == p2.X)
        {
            if (p1.Y == p2.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
        */
    }

    // const EPSILON = 1e-9;
    // const abs = Math.abs;
    // TODO https://github.com/w8r/martinez/issues/6#issuecomment-262847164
    // Precision problem.
    //
    // module.exports = function equals(p1, p2) {
    //   return abs(p1[0] - p2[0]) <= EPSILON && abs(p1[1] - p2[1]) <= EPSILON;
    // };

}