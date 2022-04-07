namespace Suteki.Compiler;

using System;

/// <summary>
/// Visits all the AST nodes.
/// </summary>
public interface ASTVisitor<T>
{
    /// <summary>
    /// Visits the node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public T Visit(Node node) => throw new NotImplementedException();

    /// <summary>
    /// Visits binary expression node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public T Visit(NodeBinary node) => throw new NotImplementedException();

    /// <summary>
    /// Visits block of statements node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public T Visit(NodeBlock node) => throw new NotImplementedException();

    /// <summary>
    /// Visits bool literal node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public T Visit(NodeBool node) => throw new NotImplementedException();

    /// <summary>
    /// Visits constant type node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public T Visit(NodeConstType node) => throw new NotImplementedException();

    /// <summary>
    /// Visits float literal node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public T Visit(NodeFloat node) => throw new NotImplementedException();

    /// <summary>
    /// Visits function declaration node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public T Visit(NodeFunctionDeclaration node) => throw new NotImplementedException();

    /// <summary>
    /// Visits function parameter node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public T Visit(NodeFunctionParameter node) => throw new NotImplementedException();

    /// <summary>
    /// Visits grouping expression node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public T Visit(NodeGrouping node) => throw new NotImplementedException();

    /// <summary>
    /// Visits identifier name node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public T Visit(NodeIdentifierName node) => throw new NotImplementedException();

    /// <summary>
    /// Visits import node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public T Visit(NodeImport node) => throw new NotImplementedException();

    /// <summary>
    /// Visits integer literal node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public T Visit(NodeInteger node) => throw new NotImplementedException();

    /// <summary>
    /// Visits null literal node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public T Visit(NodeNull node) => throw new NotImplementedException();

    /// <summary>
    /// Visits pointer type node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public T Visit(NodePointerType node) => throw new NotImplementedException();

    /// <summary>
    /// Visits primitive type node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public T Visit(NodePrimitiveType node) => throw new NotImplementedException();

    /// <summary>
    /// Visits qualified name node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public T Visit(NodeQualifiedName node) => throw new NotImplementedException();

    /// <summary>
    /// Visits return statement node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public T Visit(NodeReturn node) => throw new NotImplementedException();

    /// <summary>
    /// Visits string node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public T Visit(NodeString node) => throw new NotImplementedException();

    /// <summary>
    /// Visits unary expression node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public T Visit(NodeUnary node) => throw new NotImplementedException();
}