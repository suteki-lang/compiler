namespace Suteki.Compiler;

/// <summary>
/// Forward declaration pass.
/// </summary>
public class Emitter : ASTVisitor<string>
{
    /// <summary>
    /// The current input being visited.
    /// </summary>
    public Input Input;

    /// <summary>
    /// The amount of tabs to emit.
    /// </summary>
    public int Tabs;

    /// <summary>
    /// The current input's C++ header.
    /// </summary>
    private string Header 
    {
        get => Input.Output.Header;
        set 
        {
            Input.Output.Header = value;
        }
    }

    /// <summary>
    /// The current input's C++ source.
    /// </summary>
    private string Source 
    {
        get => Input.Output.Source;
        set 
        {
            Input.Output.Source = value;
        }
    }

    /// <summary>
    /// Get primitive type as C type.
    /// </summary>
    /// <param name="kind">The primitive kind.</param>
    private string GetCType(PrimitiveKind kind)
    {
        string[] cTypes =
        {
            null,
            "void ",
            "bool ",
            "string ",

            "uint8_t ",
            "uint16_t ",
            "uint32_t ",
            "uint64_t ",
            "uint64_t ",

            "int8_t ",
            "int16_t ",
            "int32_t ",
            "int64_t ",
            "int64_t ",

            "float ",
            "double ",
        };

        return cTypes[((int)kind)];
    }

    /// <summary>
    /// Get the amount of tabs to emit.
    /// </summary>
    private string GetTabs()
    {
        string tabs = "";

        for (int i = 0; i < Tabs; ++i)
            tabs += '\t';

        return tabs;
    }

    /// <summary>
    /// Emits C++ output from all inputs.
    /// </summary>
    public void Start()
    {
        foreach (Input input in Config.Inputs)
        {
            Input = input;
            Tabs  = 0;

            foreach (Node node in input.Nodes)
                node.AcceptVisitor(this);

            System.Console.WriteLine($"[{input.Path.Replace(".su", ".hpp")}]");
            System.Console.Write(Header);

            System.Console.WriteLine();

            System.Console.WriteLine($"[{input.Path.Replace(".su", ".cpp")}]");
            System.Console.Write(Source);
        }
    }

    /// <summary>
    /// Visits the node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public string Visit(Node node)
    {
        return node.AcceptVisitor(this);
    }

    /// <summary>
    /// Visits binary expression node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public string Visit(NodeBinary node)
    {
        return (Visit(node.Left) + ' ' + node.Operator.Content + ' ' + Visit(node.Right));
    }

    /// <summary>
    /// Visits block of statements node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public string Visit(NodeBlock node) 
    {
        ++Tabs;

        foreach (Node statement in node.Statements)
            Visit(statement);

        --Tabs;
        return null;
    }

    /// <summary>
    /// Visits bool literal node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public string Visit(NodeBool node)
    {
        return node.Value.Content;
    }

    /// <summary>
    /// Visits float literal node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public string Visit(NodeFloat node)
    {
        return node.Value.Content;
    }

    /// <summary>
    /// Visits function declaration node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public string Visit(NodeFunctionDeclaration node)
    {
        // Generate function head
        string functionHead = "";

        functionHead += Visit(node.Type);
        functionHead += node.Name.Content; // TODO: mangle this
        functionHead += '(';

        for (int index = 0; index < node.Parameters.Count; ++index)
        {
            functionHead += Visit(node.Parameters[index]);

            if (index != (node.Parameters.Count - 1))
                functionHead += ", ";
        }

        functionHead += ')';

        // Emit header output
        Header += (functionHead + ";\n");

        // Emit source output
        Source += functionHead;
        Source += "\n{\n";
        Visit(node.Body);
        Source += "}\n";

        return null;
    }

    /// <summary>
    /// Visits function parameter node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public string Visit(NodeFunctionParameter node)
    {
        return (Visit(node.Type) + node.Name.Content);
    }

    /// <summary>
    /// Visits grouping expression node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public string Visit(NodeGrouping node) 
    {
        return ('(' + Visit(node.Expression) + ')');
    }

    /// <summary>
    /// Visits import node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public string Visit(NodeImport node)
    {
        return null;
    }

    /// <summary>
    /// Visits integer literal node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public string Visit(NodeInteger node)
    {
        return node.Value.Content;
    }

    /// <summary>
    /// Visits null literal node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public string Visit(NodeNull node)
    {
        return "nullptr";
    }

    /// <summary>
    /// Visits pointer type node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public string Visit(NodePointerType node) 
    {
        return (Visit(node.Base) + '*');
    }

    /// <summary>
    /// Visits primitive type node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public string Visit(NodePrimitiveType node) 
    {
        return GetCType(node.Kind);
    }

    /// <summary>
    /// Visits return statement node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public string Visit(NodeReturn node)
    {
        // Emit tabs
        Source += GetTabs();

        // Emit return statement
        Source += "return";

        if (node.Expression != null)
        {
            Source += ' ';
            Source += Visit(node.Expression);
        }

        Source += ";\n";

        return null;
    }

    /// <summary>
    /// Visits string node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public string Visit(NodeString node) 
    {
        return node.Value.Content;
    }

    /// <summary>
    /// Visits unary expression node.
    /// </summary>
    /// <param name="node">The node to be visited.</param>
    public string Visit(NodeUnary node) 
    {
        return (node.Operator.Content + Visit(node.Operand));
    }
}