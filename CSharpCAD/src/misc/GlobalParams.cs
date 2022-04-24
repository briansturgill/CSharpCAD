namespace CSharpCAD;
/// <summary>Contains settings of global importance to the library.</summary>
/// <static/>
public static class GlobalParams
{
    private static string _units = "mm";
    ///<summary>Units in the library are representing this real world unit.</summary>
    ///<returns>One of "mm" or "in" meaning millimeters or inches.</returns>
    public static string Units
    {
        get
        {
            return _units;
        }
        set
        {
            switch (value)
            {
                case "mm":
                case "in":
                    _units = value;
                    break;
                default:
                    throw new ArgumentException("Units can only be set to one of: \"mm\", \"in\"");
            }
        }
    }
    ///<summary>Enables special (time consuming) debug checking like geometry validation.</summary>
    public static bool CheckingEnabled = false; // LATER should default to false
}

