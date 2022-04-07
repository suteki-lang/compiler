namespace Suteki.Compiler;

/// <summary>
/// A class that represents a diagnostic.
/// </summary>
public sealed class Diagnostic
{
    /// <summary>
    /// The kind of the <see cref="Diagnostic"/>.
    /// </summary>
    public DiagnosticKind Kind;

    /// <summary>
    /// The path of the <see cref="Diagnostic"/>.
    /// </summary>
    public string Path;

    /// <summary>
    /// The location of the <see cref="Diagnostic"/>.
    /// </summary>
    public FileLocation Location;

    /// <summary>
    /// The content of the <see cref="Diagnostic"/>.
    /// </summary>
    public string Content;

    /// <summary>
    /// Constructs a <see cref="Diagnostic"/> class.
    /// </summary>
    /// <param name="kind"    >The kind of the diagnostic.    </param>
    /// <param name="path"    >The path of the diagnostic.    </param>
    /// <param name="location">The location of the diagnostic.</param>
    /// <param name="content" >The content of the diagnostic. </param>
    public Diagnostic(DiagnosticKind kind, string path, FileLocation location, string content)
    {
        Kind     = kind;
        Path     = path;
        Location = location;
        Content  = content;
    }
}