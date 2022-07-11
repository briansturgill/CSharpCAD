namespace CSharpCADTests;

// Compare two numeric values for near equality.
// the given test is fails if the numeric values are outside the given epsilon
static class Helpers
{
    public static bool NearlyEqual(double a, double b, double epsilon)
    {
        if (a == b)
        { // shortcut, also handles infinities and NaNs
            return true;
        }

        var absA = Math.Abs(a);
        var absB = Math.Abs(b);
        var diff = Math.Abs(a - b);
        if (double.IsNaN(diff))
        {
            Assert.Fail($"Difference of {a}, {b} is not a number");
            return false;
        }
        if (a == 0.0 || b == 0.0 || diff < double.Epsilon)
        {
            // a or b is zero or both are extremely close to it
            // relative error is less meaningful here
            if (diff > (epsilon * double.Epsilon))
            {
                Assert.Fail($"Near zero numbers {a}, {b} outside of epsilon");
                return false;
            }
        }
        // use relative error
        var relative = (diff / Math.Min((absA + absB), double.MaxValue));
        if (relative > epsilon)
        {
            Assert.Fail($"Numbers {a}, {b} outside of epsilon");
            return false;
        }
        return true;
    }

    public static bool CompareArrays<T>(T[] a, T[] b)
    {
        if (a.Length != b.Length)
        {
            return false;
        }
        for (var i = 0; i < a.Length; i++)
        {
            if (!a[i]!.Equals(b[i]))
            {
                return false;
            }
        }
        return true;
    }

    public static bool CompareArraysNEVec2(Vec2[] a, Vec2[] b)
    {
        if (a.Length != b.Length)
        {
            return false;
        }
        for (var i = 0; i < a.Length; i++)
        {
            if (!a[i]!.IsNearlyEqual(b[i]))
            {
                return false;
            }
        }
        return true;
    }

    public static bool CompareArraysNEVec3(Vec3[] a, Vec3[] b)
    {
        if (a.Length != b.Length)
        {
            return false;
        }
        for (var i = 0; i < a.Length; i++)
        {
            if (!a[i]!.IsNearlyEqual(b[i]))
            {
                return false;
            }
        }
        return true;
    }

    public static bool CompareListsNEVec2(List<Vec2> a, List<Vec2> b)
    {
        if (a.Count != b.Count)
        {
            return false;
        }
        for (var i = 0; i < a.Count; i++)
        {
            if (!a[i]!.IsNearlyEqual(b[i]))
            {
                return false;
            }
        }
        return true;
    }

    public static bool CompareListOfListsNEVec2(List<List<Vec2>> a, List<List<Vec2>> b)
    {
        if (a.Count != b.Count)
        {
            return false;
        }
        for (var i = 0; i < a.Count; i++)
        {
            if (a[i].Count != b[i].Count) {
                    throw new NUnit.Framework.AssertionException(
                        $"Inner counts differ: a[{i}].Count={a[i].Count}, b[{i}.Count={b[i].Count}");
            }
            for (var j = 0; j < a[i].Count; j++)
            {
                if (!a[i][j]!.IsNearlyEqual(b[i][j]))
                {
                    throw new NUnit.Framework.AssertionException(
                        $"Values differ: {a[i][j]}, {b[i][j]} at indices {i}, {j}");
                    //return false;
                }
            }
        }
        return true;
    }

    public static bool CompareListsNEVec3(List<Vec3> a, List<Vec3> b)
    {
        if (a.Count != b.Count)
        {
            return false;
        }
        for (var i = 0; i < a.Count; i++)
        {
            if (!a[i]!.IsNearlyEqual(b[i]))
            {
                throw new NUnit.Framework.AssertionException($"Values differ: {a[i]}, {b[i]} at index {i}");
                //return false;
            }
        }
        return true;
    }

    public static bool CompareListOfListsNEVec3(List<List<Vec3>> a, List<List<Vec3>> b)
    {
        if (a.Count != b.Count)
        {
            throw new NUnit.Framework.AssertionException(
                $"Outer counts differ: a.Count={a.Count}, b.Count={b.Count}");
        }
        for (var i = 0; i < a.Count; i++)
        {
            if (a[i].Count != b[i].Count) {
                    throw new NUnit.Framework.AssertionException(
                        $"Inner counts differ: a[{i}].Count={a[i].Count}, b[{i}].Count={b[i].Count}");
            }
            for (var j = 0; j < a[i].Count; j++)
            {
                if (!a[i][j]!.IsNearlyEqual(b[i][j]))
                {
                    throw new NUnit.Framework.AssertionException(
                        $"Values differ: {a[i][j]}, {b[i][j]} at indices {i}, {j}");
                    //return false;
                }
            }
        }
        return true;
    }

    public static bool CompareLists<T>(List<T> a, List<T> b)
    {
        if (a.Count != b.Count)
        {
            throw new NUnit.Framework.AssertionException(
                $"Counts differ: a.Count={a.Count}, b.Count={b.Count}");
        }
        for (var i = 0; i < a.Count; i++)
        {
            if (!a[i]!.Equals(b[i]))
            {
                return false;
            }
        }
        return true;
    }

    public static bool CompareListOfLists<T>(List<List<T>> a, List<List<T>> b)
    {
        if (a.Count != b.Count)
        {
            throw new NUnit.Framework.AssertionException(
                $"Outer counts differ: a.Count={a.Count}, b.Count={b.Count}");
        }
        for (var i = 0; i < a.Count; i++)
        {
            if (!CompareLists<T>(a[i], b[i]))
            {
                return false;
            }
        }
        return true;
    }

    public static double DegToRad(double degrees) => degrees * 0.017453292519943295;
    public static void PrintListOfLists<T>(string tag, List<List<T>> ll1, List<List<T>> ll2)
    {
        var len = Math.Min(ll1.Count, ll2.Count);
        Console.WriteLine($"{tag}: Count: ${ll1.Count}, Count: ${ll2.Count}, showing: {len}");
        for (var i = 0; i < len; i++) {
            PrintList<T>($"{tag}[{i}]", ll1[i], ll2[i]);
        }
    }
    public static void PrintList<T>(string tag, List<T> l1, List<T> l2)
    {
        var len = Math.Min(l1.Count, l2.Count);
        Console.WriteLine($"{tag}: Count: ${l1.Count}, Count: ${l2.Count}, showing: {len}");
        for (var i = 0; i < l1.Count; i++) {
            Console.WriteLine($"{tag}[{i}]: {l1[i]}, {l2[i]}");
        }
    }

    public static void Log(string msg)
    {
        NUnit.Framework.TestContext.Error.WriteLine(msg);
    }
}