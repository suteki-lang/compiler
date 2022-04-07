namespace Suteki.Compiler;

/// <summary>
/// A node that represents a boolean.
/// </summary>
public class NodeBool : Node
{
    /// <summary>
    /// The token of the boolean.
    /// </summary>
    public Token Value;

    /// <summary>
    /// Constructs a <see cref="NodeBool"/> class.
    /// </summary>
    /// <param name="location">The location of the node.</param>
    /// <param name="value"   >The boolean token.       </param>
    public NodeBool(FileLocation location, Token value)
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