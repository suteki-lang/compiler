namespace Suteki.Compiler;

/// <summary>
/// A node that represents a function parameter.
/// </summary>
public class NodeFunctionParameter : Node
{
    /// <summary>
    /// The type of the parameter.
    /// </summary>
    public Node Type;

    /// <summary>
    /// The name of the parameter.
    /// </summary>
    public Token Name;

    /// <summary>
    /// Constructs a <see cref="NodeIdentifierName"/> class.
    /// </summary>
    /// <param name="type">The type of the parameter.</param>
    /// <param name="name">The name of the parameter.</param>
    public NodeFunctionParameter(Node type, Token name)
    {
        Type = type;
        Name = name;
    }

    /// <summary>
    /// Get the name of a node.
    /// </summary>
    public override string GetName() => Name.Content;

    /// <summary>
    /// Accepts the visitor.
    /// </summary>
    public override T AcceptVisitor<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}