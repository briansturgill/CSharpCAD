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
    /// <summary>The View and Save (see CADViewerDoNotSendSaves below) commands will send their geometries to CADViewer.</summary>
    public static bool CADViewerEnabled = true;
    /// <summary>The Save command will not send its geometries to CADViewer.</summary>
    public static bool CADViewerDoNotSendSaves = false;
    /// <summary>CADViewer IP Address (remember there is no security).</summary>
    public static string CADViewerUrl = "http://127.0.0.1:8037";
    ///<summary>Enables special (time consuming) debug checking like geometry validation.</summary>
    public static bool CheckingEnabled = false;
}