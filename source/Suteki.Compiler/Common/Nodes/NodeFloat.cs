namespace Suteki.Compiler;

/// <summary>
/// A node that represents a float.
/// </summary>
public class NodeFloat : Node
{
    /// <summary>
    /// The token of the float.
    /// </summary>
    public Token Value;

    /// <summary>
    /// Constructs a <see cref="NodeFloat"/> class.
    /// </summary>
    /// <param name="location">The location of the node.</param>
    /// <param name="value"   >The float token.         </param>
    public NodeFloat(FileLocation location, Token value)
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