namespace Suteki.Compiler;

using System.Collections.Generic;

/// <summary>
/// A node that represents a function declaration.
/// </summary>
public class NodeFunctionDeclaration : Node
{
    /// <summary>
    /// The type of the function.
    /// </summary>
    public Node Type;

    /// <summary>
    /// The name of the function.
    /// </summary>
    public Token Name;

    /// <summary>
    /// The function parameters.
    /// </summary>
    public List<Node> Parameters = new List<Node>();

    /// <summary>
    /// The body of the function.
    /// </summary>
    public Node Body;

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