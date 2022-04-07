namespace Suteki.Compiler;

/// <summary>
/// Forward declaration pass.
/// </summary>
public class ForwardDeclarePass : ASTVisitor<Node>
{
    /// <summary>
    /// The input being visited.
    /// </summary>
    public Input Input;

    /// <summary>
    /// Constructs a <see cref="ForwardDeclarePass"/> class.
    /// </summary>
    /// <param name="input">The input to be visited.</param>
    public ForwardDeclarePass(Input input)
    {
        Input = input;
    }
    
    /// <summary>
    /// Visits the node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public Node Visit(Node node)
    {
        return node.AcceptVisitor(this);
    }

    /// <summary>
    /// Visits function declaration node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public Node Visit(NodeFunctionDeclaration node)
    {
        // Make sure the function wasn't already declared.
        Symbol symbol = Input.FindSymbol(node.Name.Content);

        if (symbol != null)
        {
            Input.Error      (node.Name.Location, "this symbol was already declared.");
            symbol.Input.Note(symbol.Location,    "the symbol was declared here."    );
            return null;
        }

        // Add symbol to input module
        symbol = new Symbol(Input, null, node.Name.Content, node.Name.Location);
        Input.Module.Symbols.Add(node.Name.Content, symbol);

        return null;
    }

    /// <summary>
    /// Visits import node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public Node Visit(NodeImport node)
    {
        // Make sure the module exists
        if (!Config.Modules.ContainsKey(node.Module.GetName()))
        {
            Input.Error(node.Module.Location, "this module does not exists.");
            return null;
        }
        
        return null;
    }
}