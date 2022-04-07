namespace Suteki.Compiler;

/// <summary>
/// Type checking pass.
/// </summary>
public class TypeCheckPass : ASTVisitor<Type>
{
    /// <summary>
    /// The input being visited.
    /// </summary>
    public Input Input;

    /// <summary>
    /// The function we are inside.
    /// </summary>
    public NodeFunctionDeclaration Function;

    /// <summary>
    /// Constructs a <see cref="TypeCheckPass"/> class.
    /// </summary>
    /// <param name="input">The input to be visited.</param>
    public TypeCheckPass(Input input)
    {
        Input = input;
    }

    /// <summary>
    /// Visits the node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public Type Visit(Node node)
    {
        return node.AcceptVisitor(this);
    }

    /// <summary>
    /// Visits binary expression node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public Type Visit(NodeBinary node)
    {
        // Get the type of operands
        Type leftType  = Visit(node.Left);
        Type rightType = Visit(node.Right);

        // Check for both types
        if (!leftType.IsIdentical(rightType))
            Input.Error(node.Location, $"Invalid binary operation between '{leftType.GetName()}' and '{rightType.GetName()}'.");

        // Check for operation
        if (node.Operator.Kind == TokenKind.EqualEqual)
            return new TypePrimitive(PrimitiveKind.Bool);

        // Try returning the "highest" type
        if (leftType.IsString() || leftType.IsFloat())
            return leftType;

        return rightType;
    }

    /// <summary>
    /// Visits block of statements.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public Type Visit(NodeBlock node)
    {
        // Type check each statement
        foreach (Node statement in node.Statements)
            Visit(statement);
        
        return null;
    }

    /// <summary>
    /// Visits bool literal node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public Type Visit(NodeBool node)
    {
        return new TypePrimitive(PrimitiveKind.Bool);
    }

    /// <summary>
    /// Visits constant type node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public Type Visit(NodeConstType node) 
    {
        return new TypeConst(Visit(node.Type));
    }

    /// <summary>
    /// Visits float literal node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public Type Visit(NodeFloat node)
    {
        // TODO: make sure literal fits 32-bit
        return new TypePrimitive(PrimitiveKind.Single);
    }

    /// <summary>
    /// Visits function declaration node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public Type Visit(NodeFunctionDeclaration node)
    {
        // Set the function
        Function = node;

        // Type check the function body
        Visit(node.Body);

        // Reset the function
        Function = null; 
        
        return null;
    }

    /// <summary>
    /// Visits grouping expression node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public Type Visit(NodeGrouping node) 
    {
        return Visit(node.Expression);
    }

    /// <summary>
    /// Visits integer literal node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public Type Visit(NodeInteger node)
    {
        // TODO: make sure literal fits 32-bit
        return new TypePrimitive(PrimitiveKind.SInt);
    }

    /// <summary>
    /// Visits null literal node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public Type Visit(NodeNull node)
    {
        return new TypePrimitive(PrimitiveKind.Null);
    }

    /// <summary>
    /// Visits pointer type node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public Type Visit(NodePointerType node) 
    {
        return new TypePointer(Visit(node.Base));
    }

    /// <summary>
    /// Visits primitive type node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public Type Visit(NodePrimitiveType node) 
    {
        return new TypePrimitive(node.Kind);
    }

    /// <summary>
    /// Visits return statement.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public Type Visit(NodeReturn node)
    {
        // Get return value type
        Type valueType = (node.Expression != null)              ?
                          Visit(node.Expression)                :
                          new TypePrimitive(PrimitiveKind.Void) ;

        // Get function type
        Type functionType = Visit(Function.Type);

        // Check both types
        if (!functionType.IsIdentical(valueType))
            Input.Error(node.Expression.Location, $"Return value type '{valueType.GetName()}' doesn't match function type '{functionType.GetName()}'.");
        
        return null;
    }

    /// <summary>
    /// Visits string node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public Type Visit(NodeString node) 
    {
        return new TypePrimitive(PrimitiveKind.String);
    }

    /// <summary>
    /// Visits unary expression node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public Type Visit(NodeUnary node) 
    {
        // Get the type of operand
        Type operandType = Visit(node.Operand);

        // Check type
        if (!operandType.IsBool   () && !operandType.IsInteger() &&
            !operandType.IsPointer())
            Input.Error(node.Location, $"Invalid unary operation on '{operandType.GetName()}'.");

        // Check for operation
        if (node.Operator.Kind == TokenKind.Minus)
        {
            // TODO: make integer unsigned
        }

        return operandType;
    }
}