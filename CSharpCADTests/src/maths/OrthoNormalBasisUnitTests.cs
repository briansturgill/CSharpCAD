namespace CSharpCADTests;

[TestFixture]
public class OrthoNormalBasisTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    // test code for OrthoNormalBasis.GetCartesian()
    public void TestIt()
    {
        var axisnames = new string[] { "X", "Y", "Z", "-X", "-Y", "-Z" };
        var axisvectors = new Vec3[] { new Vec3(1, 0, 0), new Vec3(0, 1, 0), new Vec3(0, 0, 1),
            new Vec3(-1, 0, 0), new Vec3(0, -1, 0), new Vec3(0, 0, -1) };
        for (var axis1 = 0; axis1 < 3; axis1++)
        {
            for (var axis1inverted = 0; axis1inverted < 2; axis1inverted++)
            {
                var axis1name = axisnames[axis1 + 3 * axis1inverted];
                var axis1vector = axisvectors[axis1 + 3 * axis1inverted];
                for (var axis2 = 0; axis2 < 3; axis2++)
                {
                    if (axis2 != axis1)
                    {
                        for (var axis2inverted = 0; axis2inverted < 2; axis2inverted++)
                        {
                            var axis2name = axisnames[axis2 + 3 * axis2inverted];
                            var axis2vector = axisvectors[axis2 + 3 * axis2inverted];
                            var orthobasis = OrthoNormalBasis.GetCartesian(axis1name, axis2name);
                            var test1 = orthobasis.To3D(new Vec2(1, 0));
                            var test2 = orthobasis.To3D(new Vec2(0, 1));
                            var expected1 = new Vec3(axis1vector.X, axis1vector.Y, axis1vector.Z);
                            var expected2 = new Vec3(axis2vector.X, axis2vector.Y, axis2vector.Z);
                            var d1 = test1.Distance(expected1);
                            var d2 = test2.Distance(expected2);
                            if ((d1 > 0.01) || (d2 > 0.01))
                            {
                                Assert.Fail();
                            }
                        }
                    }
                }
            }
        }
        Assert.Pass();
    }
}