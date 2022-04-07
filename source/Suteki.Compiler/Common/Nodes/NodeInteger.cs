namespace Suteki.Compiler;

/// <summary>
/// A node that represents an integer.
/// </summary>
public class NodeInteger : Node
{
    /// <summary>
    /// The token of the integer.
    /// </summary>
    public Token Value;

    /// <summary>
    /// Constructs a <see cref="NodeInteger"/> class.
    /// </summary>
    /// <param name="location">The location of the node.</param>
    /// <param name="value"   >The integer token.       </param>
    public NodeInteger(FileLocation location, Token value)
    {
        Location = location;
        Value    = value;
    }

    /// <summary>
    /// Accepts the visitor.
    /// </summary>
    public override T AcceptVisitor<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}