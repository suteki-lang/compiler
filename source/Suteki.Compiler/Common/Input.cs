namespace Suteki.Compiler;

using System.Collections.Generic;

/// <summary>
/// A class that represents an input.
/// </summary>
public sealed class Input
{
    /// <summary>
    /// The path to the file.
    /// </summary>
    public string Path;

    /// <summary>
    /// The source of the file.
    /// </summary>
    public string Source;

    /// <summary>
    /// A list of all <see cref="Diagnotic"/>s the file have.
    /// </summary>
    public List<Diagnostic> Diagnostics;

    /// <summary>
    /// The <see cref="Scanner"/> of the file.
    /// </summary>
    public Scanner Scanner;

    /// <summary>
    /// Constructs a <see cref="Input"/> class.
    /// </summary>
    /// <param name="path"  >The path to the file.  </param>
    /// <param name="source">The source of the file.</param>
    public Input(string path, string source)
    {
        Path        = path;
        Source      = source;
        Diagnostics = new List<Diagnostic>();
        Scanner     = new Scanner(source);
    }

    /// <summary>
    /// Adds error <see cref="Diagnostic"/>.
    /// </summary>
    /// <param name="location">The location of the error.</param>
    /// <param name="message" >The message of the error. </param>
    public void Error(FileLocation location, string message)
    {
        Diagnostics.Add(new Diagnostic(DiagnosticKind.Error, location, message));
    }

    /// <summary>
    /// Adds warning <see cref="Diagnostic"/>.
    /// </summary>
    /// <param name="location">The location of the warning.</param>
    /// <param name="message" >The message of the warning. </param>
    public void Warning(FileLocation location, string message)
    {
        Diagnostics.Add(new Diagnostic(DiagnosticKind.Warning, location, message));
    }

    /// <summary>
    /// Adds information <see cref="Diagnostic"/>.
    /// </summary>
    /// <param name="location">The location of the information.</param>
    /// <param name="message" >The message of the information. </param>
    public void Information(FileLocation location, string message)
    {
        Diagnostics.Add(new Diagnostic(DiagnosticKind.Information, location, message));
    }
}