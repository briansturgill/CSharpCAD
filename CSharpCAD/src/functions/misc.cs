namespace CSharpCAD;

public class Opts : Dictionary<string, object>
{
    public Opts() : base(0) { }
    public Opts(int capacity) : base(capacity) { }
    private void err(string key, string expectedType)
    {
        throw new ArgumentException($"Option value for \"{key}\" must be of type: {expectedType}.");
    }

    public List<List<int>> GetListOfListOfInt(string key, List<List<int>> _default)
    {
        if (!this.ContainsKey(key))
        {
            return _default;
        }
        var o = this[key];
        List<List<int>> ret = _default;
        if (o.GetType() == typeof(List<List<int>>))
        {
            ret = (List<List<int>>)o;
        }
        else
        {
            err(key, "List<List<int>>");
        }
        return ret;
    }

    public List<Vec3> GetListOfVec3(string key, List<Vec3> _default)
    {
        if (!this.ContainsKey(key))
        {
            return _default;
        }
        var o = this[key];
        List<Vec3> ret = _default;
        if (o.GetType() == typeof(List<Vec3>))
        {
            ret = (List<Vec3>)o;
        }
        else
        {
            err(key, "List<Vec3>");
        }
        return ret;
    }

    public List<Vec2> GetListOfVec2(string key, List<Vec2> _default)
    {
        if (!this.ContainsKey(key))
        {
            return _default;
        }
        var o = this[key];
        List<Vec2> ret = _default;
        if (o.GetType() == typeof(List<Vec2>))
        {
            ret = (List<Vec2>)o;
        }
        else
        {
            err(key, "List<Vec2>");
        }
        return ret;
    }

    public List<Color> GetListOfColor(string key, List<Color> _default)
    {
        if (!this.ContainsKey(key))
        {
            return _default;
        }
        var o = this[key];
        List<Color> ret = _default;
        if (o.GetType() == typeof(List<Color>))
        {
            ret = (List<Color>)o;
        }
        else
        {
            err(key, "List<Color>");
        }
        return ret;
    }

    public int GetInt(string key, int _default)
    {
        if (!this.ContainsKey(key))
        {
            return _default;
        }
        var o = this[key];
        int ret = _default;
        if (o.GetType() == typeof(int))
        {
            ret = (int)o;
        }
        else
        {
            err(key, "int");
        }
        return ret;
    }

    public bool GetBool(string key, bool _default)
    {
        if (!this.ContainsKey(key))
        {
            return _default;
        }
        var o = this[key];
        bool ret = _default;
        if (o.GetType() == typeof(bool))
        {
            ret = (bool)o;
        }
        else
        {
            err(key, "bool (true or false)");
        }
        return ret;
    }

    public string GetString(string key, string _default)
    {
        if (!this.ContainsKey(key))
        {
            return _default;
        }
        var o = this[key];
        string ret = _default;
        if (o.GetType() == typeof(string))
        {
            ret = (string)o;
        }
        else
        {
            err(key, "string");
        }
        return ret;
    }

    public double GetDouble(string key, double _default)
    {
        if (!this.ContainsKey(key))
        {
            return _default;
        }
        var o = this[key];
        double ret = _default;
        if (o.GetType() == typeof(int))
        {
            ret = (int)o;
        }
        else if (o.GetType() == typeof(double))
        {
            ret = (double)o;
        }
        else
        {
            err(key, "double or int");
        }
        return ret;
    }

    public Vec2 GetVec2(string key, (double, double) _default)
    {
        var (x, y) = _default;
        if (!this.ContainsKey(key))
        {
            return new Vec2(x, y);
        }
        var o = this[key];
        var t = o.GetType();
        if (t == typeof((double, double)))
        {
            var (_x, _y) = ((double, double))o;
            x = _x;
            y = _y;
        }
        else if (t == typeof((double, int)))
        {
            var (_x, _y) = ((double, int))o;
            x = _x;
            y = _y;
        }
        else if (t == typeof((int, double)))
        {
            var (_x, _y) = ((int, double))o;
            x = _x;
            y = _y;
        }
        else if (t == typeof((int, int)))
        {
            var (_x, _y) = ((int, int))o;
            x = _x;
            y = _y;
        }
        else
        {
            err(key, "tuple (double or int, double or int)");
        }
        return new Vec2(x, y);
    }

    public Vec3 GetVec3(string key, (double, double, double) _default)
    {
        var (x, y, z) = _default;
        if (!this.ContainsKey(key))
        {
            return new Vec3(x, y, z);
        }
        var o = this[key];
        var t = o.GetType();
        if (t == typeof((double, double, double)))
        {
            var (_x, _y, _z) = ((double, double, double))o;
            x = _x;
            y = _y;
            z = _z;
        }
        else if (t == typeof((double, double, int)))
        {
            var (_x, _y, _z) = ((double, double, int))o;
            x = _x;
            y = _y;
            z = _z;
        }
        else if (t == typeof((double, int, int)))
        {
            var (_x, _y, _z) = ((double, int, int))o;
            x = _x;
            y = _y;
            z = _z;
        }
        else if (t == typeof((double, int, double)))
        {
            var (_x, _y, _z) = ((double, int, double))o;
            x = _x;
            y = _y;
            z = _z;
        }
        else if (t == typeof((int, double, int)))
        {
            var (_x, _y, _z) = ((int, double, int))o;
            x = _x;
            y = _y;
            z = _z;
        }
        else if (t == typeof((int, double, double)))
        {
            var (_x, _y, _z) = ((int, double, double))o;
            x = _x;
            y = _y;
            z = _z;
        }
        else if (t == typeof((int, int, double)))
        {
            var (_x, _y, _z) = ((int, int, double))o;
            x = _x;
            y = _y;
            z = _z;
        }
        else if (t == typeof((int, int, int)))
        {
            var (_x, _y, _z) = ((int, int, int))o;
            x = _x;
            y = _y;
            z = _z;
        }
        else
        {
            err(key, "tuple (double or int, double or int, double or int)");
        }
        return new Vec3(x, y, z);
    }
}

public static partial class CSCAD
{
    public static void Echo(params object[] objs)
    {
        var first = true;
        foreach (var o in objs)
        {
            if (!first)
            {
                Console.Write(" ");
            }
            Console.Write(o.ToString());
            first = false;
        }
        Console.WriteLine("");
    }

    public static void Save(string file, Geometry g, bool binary = true)
    {
        if (file.EndsWith(".svg"))
        {
            SerializeToSVG(file, g);
        }
        else if (file.EndsWith(".amf"))
        {
            SerializeToAMF(file, g);
        }
        else if (file.EndsWith(".stl"))
        {
            if (binary)
            {
                SerializeToSTLBinary(file, g);
            }
            else
            {
                SerializeToSTLText(file, g);
            }
        }
        else
        {
            throw new ArgumentException($"Sorry but the output file type of \"{file}\" is not one we understand!");
        }
    }

    private static double rezero(double v) => Math.Abs(v) < C.NEPS ? 0 : v;

    public static double sin(double angle)
    {
        return rezero(Math.Sin(angle));
    }

    public static double cos(double angle)
    {
        return rezero(Math.Cos(angle));
    }
}