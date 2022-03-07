namespace Suteki.Compiler;

/// <summary>
/// A structure that represents a location of the file.
/// </summary>
public sealed class FileLocation
{
    /// <summary>
    /// The line of the file.
    /// </summary>
    public int Line;

    /// <summary>
    /// The column of the line.
    /// </summary>
    public int Column;

    /// <summary>
    /// The content of the line.
    /// </summary>
    public string Content;

    /// <summary>
    /// The length of the value.
    /// </summary>
    public int Length;

    /// <summary>
    /// Constructs the <see cref="FileLocation"/> class.
    /// </summary>
    /// <param name="line"   >The line of the file.         </param>
    /// <param name="column" >The column of the file.       </param>
    /// <param name="content">The content of the line.      </param>
    /// <param name="length">The length of the offset value.</param>
    public FileLocation(int line, int column, string content, int length)
    {
        Line    = line;
        Column  = column;
        Content = content;
        Length  = length;
    }
}