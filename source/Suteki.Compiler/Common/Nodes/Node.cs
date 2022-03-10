namespace Suteki.Compiler;

/// <summary>
/// The base class for all AST nodes.
/// </summary>
public abstract class Node
{
    /// <summary>
    /// The location of the node in the file.
    /// </summary>
    public FileLocation Location;

    /// <summary>
    /// Accepts the visitor.
    /// </summary>
    public abstract T AcceptVisitor<T>(ASTVisitor<T> visitor);
}