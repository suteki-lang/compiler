namespace Suteki.Compiler;

using System.Collections.Generic;

/// <summary>
/// A class that represents a global configuration.
/// </summary>
public sealed class Config
{
    /// <summary>
    /// All the user inputs.
    /// </summary>
    public static List<Input> Inputs = new List<Input>();

    /// <summary>
    /// Is <see langword="true"/> if the compiler had any fatal error.
    /// </summary>
    public static bool HasFatalErrors = false;
}