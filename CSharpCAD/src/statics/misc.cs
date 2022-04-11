namespace CSharpCAD;

public static partial class CSCAD
{
    ///<summary>Print string representations of any object.</summary>
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

    ///<summary>Save a geometry object in a file suitable for printing, etc.</summary>
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
}