namespace Suteki.Compiler;

/// <summary>
/// Visits all the AST nodes.
/// </summary>
public interface ASTVisitor<T>
{
    /// <summary>
    /// Visits the node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public T Visit(Node node)
    {
        return node.AcceptVisitor(this);
    }
}