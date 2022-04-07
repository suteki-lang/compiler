namespace Suteki.Compiler;

/// <summary>
/// A node that represents an unary expression.
/// </summary>
public class NodeUnary : Node
{
    /// <summary>
    /// The operand expression.
    /// </summary>
    public Node Operand;

    /// <summary>
    /// The operator.
    /// </summary>
    public Token Operator;

    /// <summary>
    /// Constructs a <see cref="NodeUnary"/> class.
    /// </summary>
    /// <param name="location">The location of the node.</param>
    /// <param name="op"      >The operator.            </param>
    /// <param name="operand" >The operand expression.  </param>
    public NodeUnary(FileLocation location, Token op, Node operand)
    {
        Location = location;
        Operator = op;
        Operand  = operand;
    }

    /// <summary>
    /// Accepts the visitor.
    /// </summary>
    public override T AcceptVisitor<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}