namespace CSharpCAD;


// Internally we also use System.ArgumentException
public class ValidationException : Exception {
    public ValidationException(string message) : base(message) {}
}