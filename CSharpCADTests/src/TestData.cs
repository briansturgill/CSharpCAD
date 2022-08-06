namespace CSharpCADTests;

public static class TestData
{
    public static void Make(string tag, List<List<Vec3>> data)
    {
        NUnit.Framework.TestContext.Error.WriteLine($"WARNING: write test data for {tag}.");
        StringBuilder sb = new StringBuilder();

        var indent_level = 0;

        void indent()
        {
            indent_level++;
        }
        void dedent()
        {
            indent_level--;
        }
        void tab()
        {
            for (var i = 0; i < indent_level; i++) sb.Append("  ");
        }

        sb.Append("namespace CSharpCADTests;\n");
        sb.Append("\n");
        sb.Append("using NUnit.Framework;\n");
        sb.Append("\n");
        sb.Append("public static partial class UnitTestData {\n");
        indent();
        tab(); sb.Append($"public static List<List<Vec3>> {tag} = new List<List<Vec3>> {{\n");
        indent();
        for (var i = 0; i < data.Count; i++)
        {
            var sub = data[i];
            indent();
            tab(); sb.Append("new List<Vec3> {\n");
            for (var j = 0; j < sub.Count; j++)
            {
                var pts = sub[j];
                indent();
                tab(); sb.Append($"new Vec3({pts.X}, {pts.Y}, {pts.Z}){(j < sub.Count - 1 ? "," : "")}\n");
                dedent();
            }
            dedent();
            tab(); sb.Append($"}}{(i < data.Count - 1 ? ", " : "")}\n");
        }
        dedent();
        tab(); sb.Append("};\n");

        dedent();
        sb.Append("}\n");

        System.IO.File.WriteAllText($"../../../src/unit_test_data/{tag}.cs", sb.ToString());
    }

    public static void Make(string tag, Vec2[] data)
    {
        NUnit.Framework.TestContext.Error.WriteLine($"WARNING: write test data for {tag}.");
        StringBuilder sb = new StringBuilder();

        var indent_level = 0;

        void indent()
        {
            indent_level++;
        }
        void dedent()
        {
            indent_level--;
        }
        void tab()
        {
            for (var i = 0; i < indent_level; i++) sb.Append("  ");
        }

        sb.Append("namespace CSharpCADTests;\n");
        sb.Append("\n");
        sb.Append("using NUnit.Framework;\n");
        sb.Append("\n");
        sb.Append("public static partial class UnitTestData {\n");
        indent();
        tab(); sb.Append($"public static Vec2[] {tag} = new Vec2[] {{\n");
        indent();
        for (var i = 0; i < data.Length; i++)
        {
            var sub = data[i];
            var pts = data[i];
            tab(); sb.Append($"new Vec2({pts.X}, {pts.Y}){(i < data.Length - 1 ? "," : "")}\n");
        }
        dedent();
        tab(); sb.Append("};\n");
        dedent();
        sb.Append("}\n");

        System.IO.File.WriteAllText($"../../../src/unit_test_data/{tag}.cs", sb.ToString());
    }

    public static void Make(string tag, Vec2[][] data)
    {
        NUnit.Framework.TestContext.Error.WriteLine($"WARNING: write test data for {tag}.");
        StringBuilder sb = new StringBuilder();

        var indent_level = 0;

        void indent()
        {
            indent_level++;
        }
        void dedent()
        {
            indent_level--;
        }
        void tab()
        {
            for (var i = 0; i < indent_level; i++) sb.Append("  ");
        }

        sb.Append("namespace CSharpCADTests;\n");
        sb.Append("\n");
        sb.Append("using NUnit.Framework;\n");
        sb.Append("\n");
        sb.Append("public static partial class UnitTestData {\n");
        indent();
        tab(); sb.Append($"public static Vec2[][] {tag} = new Vec2[][] {{\n");
        indent();
        for (var i = 0; i < data.Length; i++)
        {
            var inner = data[i];
            tab(); sb.Append($"new Vec2[] {{\n");
            indent();
            for (var j = 0; j < inner.Length; j++)
            {
                var pts = inner[j];
                tab(); sb.Append($"new Vec2({pts.X}, {pts.Y}){(j < inner.Length - 1 ? "," : "")}\n");
            }
            dedent();
            tab(); sb.Append($"}}{(i < inner.Length - 1 ? "," : "")}\n");
        }
        dedent();
        tab(); sb.Append("};\n");
        dedent();
        sb.Append("}\n");

        System.IO.File.WriteAllText($"../../../src/unit_test_data/{tag}.cs", sb.ToString());
    }
}