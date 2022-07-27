namespace CSharpCADTests;

#nullable disable

using static Geom2Booleans;

[TestFixture]
public class MartinezTestsSweepLine
{
    //static bool WriteTests = false;

    List<List<List<Vec2>>> subject = new List<List<List<Vec2>>>
    {
      new List<List<Vec2>> {
        new List<Vec2>{
          new Vec2(20, -23.5),
          new Vec2(170, 74),
          new Vec2(226.5, -113.5),
          new Vec2(20, -23.5)
        }
      }
    };
    List<List<List<Vec2>>> clipping = new List<List<List<Vec2>>>
    {
      new List<List<Vec2>> {
        new List<Vec2>{
          new Vec2(54.5, -170.5),
          new Vec2(140.5, 33.5),
          new Vec2(239.5, -198),
          new Vec2(54.5, -170.5)
        }
      }
    };

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestSweepLine()
    {
      var s = subject[0];
      var c = clipping[0];

      var EF = new SweepEvent(s[0][0], true, new SweepEvent(s[0][2], false, null, false), true);
      var EG = new SweepEvent(s[0][0], true, new SweepEvent(s[0][1], false, null, false), true);

      var tree = new SplayTree<SweepEvent, int>(compareSegments);
      tree.insert(EF, 0);
      tree.insert(EG, 0);


      Assert.AreEqual(tree.find(EF).key, EF, "able to retrieve node");
      Assert.AreEqual(tree.minNode(tree.root).key, EF, "EF is at the begin");
      Assert.AreEqual(tree.maxNode(tree.root).key, EG, "EG is at the end");

      var it = tree.find(EF);

      Assert.AreEqual(tree.next(it).key, EG);

      it = tree.find(EG);

      Assert.AreEqual(tree.prev(it).key, EF);

      var DA = new SweepEvent(c[0][0], true, new SweepEvent(c[0][2], false, null, false), true);
      var DC = new SweepEvent(c[0][0], true, new SweepEvent(c[0][1], false, null, false), true);

      tree.insert(DA, 0);
      tree.insert(DC, 0);

      var begin = tree.minNode(tree.root);

      Assert.AreEqual(begin.key, DA, "DA");
      begin = tree.next(begin);
      Assert.AreEqual(begin.key, DC, "DC");
      begin = tree.next(begin);
      Assert.AreEqual(begin.key, EF, "EF");
      begin = tree.next(begin);
      Assert.AreEqual(begin.key, EG, "EG");
    }
}