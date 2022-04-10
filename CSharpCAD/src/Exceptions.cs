namespace CSharpCAD;


// Internally we also use System.ArgumentException

/// <summary>Thrown by internal validation checks.</summary>
public class ValidationException : Exception {
    ///
    public ValidationException(string message) : base(message) {}
}