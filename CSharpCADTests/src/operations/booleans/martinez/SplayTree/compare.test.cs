namespace CSharpCADTests;

#nullable disable

using static Geom2Booleans;

[TestFixture]
public class SplayTreeTestsComparator
{
    //static bool WriteTests = false;

    [SetUp]
    public void Setup()
    {
    }

    public double[] shuffle(double[] array)
    {
        int currentIndex = array.Length;
        double temporaryValue;
        int randomIndex;
        var rand = new System.Random();
        while (0 != currentIndex)
        {
            randomIndex = (int)Math.Floor(rand.NextDouble() * currentIndex);
            currentIndex -= 1;
            temporaryValue = array[currentIndex];
            array[currentIndex] = array[randomIndex];
            array[randomIndex] = temporaryValue;
        }
        return array;
    }

    [Test]
    public void CustomComparator()
    {
        var tree = new SplayTree<int, double>((int a, int b) => b - a);
        tree.insert(2, 2);
        tree.insert(1, 1);
        tree.insert(3, 3);
        Assert.AreEqual(tree.size, 3);
        Assert.AreEqual(tree.min(), 3);
        Assert.AreEqual(tree.max(), 1);
        tree.remove(3);
        Assert.AreEqual(tree.size, 2);
        Assert.AreEqual(tree.root.key, 2);
        Assert.AreEqual(tree.root.left, null);
        Assert.AreEqual(tree.root.right.key, 1);
    }

    [Test]
    public void CustomKeys()
    {
        int comparator(double a, double b) => (int)(a - b);
        var tree = new SplayTree<double, double>(comparator);
        var objects = new double[10];
        for (var i = 0; i < objects.Length; i++)
        {
            objects[i] = Math.Pow(i, 2);
        }
        shuffle(objects);

        foreach (var o in objects)
        {
            tree.insert(o, o);
        }

        var ikeys = tree.keys();
        ikeys.Sort();
        Array.Sort(objects);
        for (var i = 0; i < ikeys.Count; i++)
        {
            Assert.AreEqual(ikeys[i], objects[i]);
        }
    }
}