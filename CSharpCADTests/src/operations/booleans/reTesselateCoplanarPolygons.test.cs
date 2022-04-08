namespace CSharpCADTests;

[TestFixture]
public class ReTessellateCoplanerPolygonsTests
{
    [SetUp]
    public void Setup()
    {
    }

    private static Poly3 translatePoly3(Vec3 offsets, Poly3 polygon)
    {
        var matrix = Mat4.FromTranslation(offsets);
        return polygon.Transform(matrix);
    }

    private static Poly3 rotatePoly3(Vec3 angles, Poly3 polygon)
    {
        var matrix = Mat4.FromTaitBryanRotation((angles.x * 0.017453292519943295), (angles.y * 0.017453292519943295), (angles.z * 0.017453292519943295));
        return polygon.Transform(matrix);
    }

    [Test]
    public void TestRCPShouldMergeCoplanarPolygons()
    {
        var polyA = new Poly3(new Vec3[]{new Vec3(-5, -5, 0), new Vec3(5, -5, 0),
          new Vec3(5, 5, 0), new Vec3(-5, 5, 0)});
        var polyB = new Poly3(new Vec3[] { new Vec3(5, -5, 0), new Vec3(8, 0, 0), new Vec3(5, 5, 0) });
        var polyC = new Poly3(new Vec3[] { new Vec3(-5, 5, 0), new Vec3(-8, 0, 0), new Vec3(-5, -5, 0) });
        var polyD = new Poly3(new Vec3[] { new Vec3(-5, 5, 0), new Vec3(5, 5, 0), new Vec3(0, 8, 0) });
        var polyE = new Poly3(new Vec3[] { new Vec3(5, -5, 0), new Vec3(-5, -5, 0), new Vec3(0, -8, 0) });

        // combine polygons in each direction
        var obs = ReTessellateCoplanarPolygons(new List<Poly3> { polyA, polyB });
        Assert.IsTrue(obs.Count == 1);
        obs = ReTessellateCoplanarPolygons(new List<Poly3> { polyA, polyC });
        Assert.IsTrue(obs.Count == 1);
        obs = ReTessellateCoplanarPolygons(new List<Poly3> { polyA, polyD });
        Assert.IsTrue(obs.Count == 1);
        obs = ReTessellateCoplanarPolygons(new List<Poly3> { polyA, polyE });
        Assert.IsTrue(obs.Count == 1);

        // combine several polygons in each direction
        obs = ReTessellateCoplanarPolygons(new List<Poly3> { polyB, polyA, polyC });
        Assert.IsTrue(obs.Count == 1);
        obs = ReTessellateCoplanarPolygons(new List<Poly3> { polyC, polyA, polyB });
        Assert.IsTrue(obs.Count == 1);

        obs = ReTessellateCoplanarPolygons(new List<Poly3> { polyD, polyA, polyE });
        Assert.IsTrue(obs.Count == 1);
        obs = ReTessellateCoplanarPolygons(new List<Poly3> { polyE, polyA, polyD });
        Assert.IsTrue(obs.Count == 1);

        // combine all polygons
        obs = ReTessellateCoplanarPolygons(new List<Poly3> { polyA, polyB, polyC, polyD, polyE });
        Assert.IsTrue(obs.Count == 1);

        // now rotate everything and do again
        var polyH = rotatePoly3(new Vec3(-45, -45, -45), polyA);
        var polyI = rotatePoly3(new Vec3(-45, -45, -45), polyB);
        var polyJ = rotatePoly3(new Vec3(-45, -45, -45), polyC);
        var polyK = rotatePoly3(new Vec3(-45, -45, -45), polyD);
        var polyL = rotatePoly3(new Vec3(-45, -45, -45), polyE);

        obs = ReTessellateCoplanarPolygons(new List<Poly3> { polyH, polyI, polyJ, polyK, polyL });
        Assert.IsTrue(obs.Count == 1);

        // now translate everything and do again
        polyH = translatePoly3(new Vec3(-15, -15, -15), polyA);
        polyI = translatePoly3(new Vec3(-15, -15, -15), polyB);
        polyJ = translatePoly3(new Vec3(-15, -15, -15), polyC);
        polyK = translatePoly3(new Vec3(-15, -15, -15), polyD);
        polyL = translatePoly3(new Vec3(-15, -15, -15), polyE);

        obs = ReTessellateCoplanarPolygons(new List<Poly3> { polyH, polyI, polyJ, polyK, polyL });
        Assert.IsTrue(obs.Count == 1);
    }
}
