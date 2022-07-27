namespace CSharpCADTests;

#nullable disable

using static Geom2Booleans;

[TestFixture]
public class SplayTreeTestsContains
{
    //static bool WriteTests = false;

    int comparator(int? a, int? b)
    {
        if (a is null)
        {
            return -1;
        }
        if (b is null)
        {
            return 1;
        }
        return ((int)a) - ((int)b);
    }

    //SplayTree<int, int>.STComparator<int?> comparator = _comparator;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ContainsFalseIfTreeEmpty()
    {
        var tree = new SplayTree<int?, int>(comparator);
        Assert.IsFalse(tree.contains(1));
    }


    [Test]
    public void ContainsReturnsWhetherTreeContainsNode()
    {
        var tree = new SplayTree<int?, int>(comparator);
        Assert.IsFalse(tree.contains(1));
        Assert.IsFalse(tree.contains(2));
        Assert.IsFalse(tree.contains(3));
        tree.insert(3, 0);
        tree.insert(1, 0);
        tree.insert(2, 0);
        Assert.IsTrue(tree.contains(1));
        Assert.IsTrue(tree.contains(2));
        Assert.IsTrue(tree.contains(3));
    }


    [Test]
    public void ContainsFalseWhenExpectedParentHasNoChildren()
    {
        var tree = new SplayTree<int?, int>(comparator);
        tree.insert(2, 0);
        Assert.IsFalse(tree.contains(1));
        Assert.IsFalse(tree.contains(3));
    }

}
