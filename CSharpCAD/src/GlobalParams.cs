namespace CSharpCAD;
public static partial class CSCAD
{
    public static class GlobalParams
    {
        private static string _units = "mm";
        public static string Units {
            get {
                return _units;
            }
            set {
                switch(value) {
                    case "mm":
                    case "in":
                        _units = value;
                        break;
                    default:
                        throw new ArgumentException("Units can only be set to one of: \"mm\", \"in\"");
                }
            }
        }
        // Handles enabling debug checking like geometry validation.
        public static bool CheckingEnabled = false; // LATER should default to false
    }
}

