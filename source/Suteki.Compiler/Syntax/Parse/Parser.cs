namespace Suteki.Compiler;

using System.Collections.Generic;

/// <summary>
/// Parses all input's sources into AST nodes.
/// </summary>
public sealed class Parser
{
    /// <summary>
    /// All the primitive types.
    /// </summary>
    public Dictionary<string, TypePrimitive> PrimitiveTypes = 
       new Dictionary<string, TypePrimitive>
    {
        { "void",    new TypePrimitive(PrimitiveKind.Void)   },
        { "bool",    new TypePrimitive(PrimitiveKind.Bool)   },
        { "string",  new TypePrimitive(PrimitiveKind.String) },

        { "ubyte",   new TypePrimitive(PrimitiveKind.UByte)  },
        { "ushort",  new TypePrimitive(PrimitiveKind.UShort) },
        { "uint",    new TypePrimitive(PrimitiveKind.UInt)   },
        { "ulong",   new TypePrimitive(PrimitiveKind.ULong)  },
        { "uword",   new TypePrimitive(PrimitiveKind.UWord)  },

        { "sbyte",   new TypePrimitive(PrimitiveKind.SByte)  },
        { "sshort",  new TypePrimitive(PrimitiveKind.SShort) },
        { "sint",    new TypePrimitive(PrimitiveKind.SInt)   },
        { "slong",   new TypePrimitive(PrimitiveKind.SLong)  },
        { "sword",   new TypePrimitive(PrimitiveKind.SWord)  },

        { "byte",    new TypePrimitive(PrimitiveKind.SByte)  },
        { "short",   new TypePrimitive(PrimitiveKind.SShort) },
        { "int",     new TypePrimitive(PrimitiveKind.SInt)   },
        { "long",    new TypePrimitive(PrimitiveKind.SLong)  },
        { "word",    new TypePrimitive(PrimitiveKind.SWord)  },

        { "single",  new TypePrimitive(PrimitiveKind.Single) },
        { "double",  new TypePrimitive(PrimitiveKind.Double) },
    };

    /// <summary>
    /// The current input being parsed.
    /// </summary>
    public Input Input;

    /// <summary>
    /// Get the input's scanner.
    /// </summary>
    public Scanner Scanner
    {
        get
        {
            return Input.Scanner;
        }
    }

    /// <summary>
    /// Get the input's AST nodes.
    /// </summary>
    public List<Node> Nodes
    {
        get
        {
            return Input.Nodes;
        }
    }

    /// <summary>
    /// Get the previous input's scanner token.
    /// </summary>
    public Token Previous
    {
        get
        {
            return Input.Scanner.Previous;
        }
    }

    /// <summary>
    /// Get the current input's scanner token.
    /// </summary>
    public Token Current
    {
        get
        {
            return Input.Scanner.Current;
        }
    }

    /// <summary>
    /// Advance current token.
    /// </summary>
    private void Advance()
    {
        for (;;)
        {
            // Is current token an error?
            if (Scanner.Next() != TokenKind.Error)
                break;

            // If so, add error diagnostic
            Input.Error(Current.Location, Current.Content);
        }
    }

    /// <summary>
    /// Matches current token?
    /// </summary>
    /// <param name="expected">The expected token kind to be checked.</param>
    private bool Match(TokenKind expected)
    {
        // Current token kind is the same as expected kind?
        if (Current.Kind == expected)
        {
            Advance();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Consume current token if expected token matches.
    /// </summary>
    /// <param name="expected">The expected token kind to be checked.</param>
    /// <param name="message">The message of the error if the tokens didn't match.</param>
    private void Consume(TokenKind expected, string message)
    {
        // Matches?
        if (Match(expected))
            return;

        Input.Error(Current.Location, message);
    }

    /// <summary>
    /// Parse module declaration.
    /// </summary>
    private void ParseModuleDeclaration()
    {
        Token moduleToken = Previous;

        // Parse module name
        string moduleName = "";

        for (;;)
        {
            if (Match(TokenKind.Identifier))
                moduleName += Previous.Content;
            else
            {
                // Check for module name
                if (moduleName == "")
                {
                    Input.Error(Current.Location, "expected module name.");
                    break;
                }
                else
                {
                    Input.Error(Current.Location, "unexpected token.");
                    break;
                }
            }

            // Only keep parsing module name if there's a dot
            if (!Match(TokenKind.Dot))
                break;

            moduleName += '.';
        }

        // Consume semicolon
        Consume(TokenKind.Semicolon, "expected ';' after module declaration.");

        // Set input's module
        if (Input.Module == null)
        {
            // Add module (if needed)
            if (!Config.Modules.ContainsKey(moduleName))
                Config.Modules.Add(moduleName, new Module(moduleName));

            // Set module
            Input.Module = Config.Modules[moduleName];

            // Set module declaration token for error information
            Input.ModuleDeclarationLocation = moduleToken.Location;
        }
        else
        {
            Input.Error(moduleToken.Location, 
                $"this file was already exported as '{Input.Module.Name}' module.");

            Input.Note(Input.ModuleDeclarationLocation,
                $"the declaration of the module '{Input.Module.Name}' is here.");
        }
    }

    /// <summary>
    /// Parse import.
    /// </summary>
    private void ParseImport()
    {
        Token importToken = Previous;

        // Parse module name
        Node moduleName = ParseIdentifierName();

        // Consume semicolon
        Consume(TokenKind.Semicolon, "expected ';' after module name.");

        // Build import node
        NodeImport node = new NodeImport(importToken.Location, moduleName);

        // Add node
        Nodes.Add(node);
    }

    /// <summary>
    /// Parses qualified name.
    /// </summary>
    /// <param name="left">The left name node.</param>
    private Node ParseQualifiedName(Node left)
    {
        // Parse right name
        Consume(TokenKind.Identifier, "expected identifier.");
        Node right = new NodeIdentifierName(Previous.Location, Previous);

        // Make node location
        FileLocation location         = right.Location;
                     location.Length += (left.Location.Length + 1);

        // Build qualified name node
        Node name = new NodeQualifiedName(location, left, right);

        // Is it another qualified name?
        if (Match(TokenKind.Dot))
            return ParseQualifiedName(name);

        // Not another qualified name
        return name;
    }

    /// <summary>
    /// Parses identifier name.
    /// </summary>
    private Node ParseIdentifierName()
    {
        Consume(TokenKind.Identifier, "expected identifier.");
        Node name = new NodeIdentifierName(Previous.Location, Previous);

        // Is it a qualified name?
        if (Match(TokenKind.Dot))
            return ParseQualifiedName(name);

        // It's an identifier name
        return name;
    }

    /// <summary>
    /// Parses identifier name with a token.
    /// </summary>
    /// <param name="name">The token of the identifier.</param>
    private Node ParseIdentifierName(Token name)
    {
        Node nameNode = new NodeIdentifierName(name.Location, name);

        // Is it a qualified name?
        if (Match(TokenKind.Dot))
            return ParseQualifiedName(nameNode);

        // It's an identifier name
        return nameNode;
    }

    /// <summary>
    /// Parses type.
    /// </summary>
    private Node ParseType()
    {
        Node node = null;

        // Parse type name
        Node   name       = ParseIdentifierName(Previous);
        string nameString = name.GetName();

        // Check for primitive type
        if (PrimitiveTypes.ContainsKey(nameString))
            node = new NodePrimitiveType(name.Location, PrimitiveTypes[nameString].Kind);
        
        // Failed to parse type
        if (node == null)
            Input.Error(name.Location, "expected type.");
        else
        {
            // Check for pointer type
            while (Match(TokenKind.Star))
            {
                // Make node location
                FileLocation location        = node.Location;
                             location.Length = (location.Column - Previous.Location.Column) + 1;

                node = new NodePointerType(location, node);
            }
        }
        
        return node;
    }

    /// <summary>
    /// Parses primary expression.
    /// </summary>
    private Node ParsePrimaryExpression()
    {
        // Parse float literal?
        if (Match(TokenKind.Float))
            return new NodeFloat(Previous.Location, Previous);

        // Parse integer literal?
        if (Match(TokenKind.Integer))
            return new NodeInteger(Previous.Location, Previous);

        // Parse string literal?
        if (Match(TokenKind.String))
            return new NodeString(Previous.Location, Previous);

        // Parse bool literal?
        if (Match(TokenKind.Bool))
            return new NodeBool(Previous.Location, Previous);

        // Parse null literal?
        if (Match(TokenKind.Null))
            return new NodeNull(Previous.Location, Previous);

        // Parse expression grouping?
        if (Match(TokenKind.LeftParenthesis))
        {
            Token previous = Previous;

            Node expression = ParseExpression();
            Consume(TokenKind.RightParenthesis, "Expected ')' after expression.");

            // Make node location
            FileLocation location        = Previous.Location;
                         location.Length = (location.Column - previous.Location.Column) + 1;
            
            return new NodeGrouping(location, expression);
        }

        Input.Error(Current.Location, "unexpected token.");
        return null;
    }

    /// <summary>
    /// Parses unary expression.
    /// </summary>
    public Node ParseUnaryExpression()
    {
        // -
        if (Match(TokenKind.Minus))
        {
            Token op      = Previous;
            Node  operand = ParseUnaryExpression();

            // Make node location
            FileLocation location        = operand.Location;
                         location.Length = (operand.Location.Column - op.Location.Column) + 1; 

            return new NodeUnary(location, op, operand);
        }

        return ParsePrimaryExpression();
    }

    /// <summary>
    /// Parses factor expression.
    /// </summary>
    public Node ParseFactorExpression()
    {
        Node expression = ParseUnaryExpression();

        // /, *
        while (Match(TokenKind.Slash) || Match(TokenKind.Star))
        {
            Token op    = Previous;
            Node  right = ParseUnaryExpression();

            // Make node location
            FileLocation location        = right.Location;
                         location.Length = (right.Location.Column - expression.Location.Column) + 1;

            expression = new NodeBinary(location, expression, op, right);
        }

        return expression;
    }

    /// <summary>
    /// Parses term expression.
    /// </summary>
    public Node ParseTermExpression()
    {
        Node expression = ParseFactorExpression();

        // -, +
        while (Match(TokenKind.Minus) || Match(TokenKind.Plus))
        {
            Token op    = Previous;
            Node  right = ParseFactorExpression();

            // Make node location
            FileLocation location        = right.Location;
                         location.Length = (right.Location.Column - expression.Location.Column) + 1;

            expression = new NodeBinary(location, expression, op, right);
        }

        return expression;
    }

    /// <summary>
    /// Parses comparison expression.
    /// </summary>
    public Node ParseComparisonExpression()
    {
        return ParseTermExpression();
    }

    /// <summary>
    /// Parses equality expression.
    /// </summary>
    public Node ParseEqualityExpression()
    {
        Node expression = ParseComparisonExpression();

        // Calculate expression start column
        int startColumn = (expression.Location.Column - expression.Location.Length);

        // ==
        while (Match(TokenKind.EqualEqual))
        {
            Token op    = Previous;
            Node  right = ParseComparisonExpression();

            // Make node location
            FileLocation location        = right.Location;
                         location.Length = (right.Location.Column - startColumn);

            expression = new NodeBinary(location, expression, op, right);
        }

        return expression;
    }

    /// <summary>
    /// Parses expression.
    /// </summary>
    private Node ParseExpression()
    {
        return ParseEqualityExpression();
    }

    /// <summary>
    /// Parses return statement.
    /// </summary>
    private Node ParseReturn()
    {
        // Build return node
        NodeReturn node = new NodeReturn()
        {
            Location   = Previous.Location,
            Expression = null,
        };

        // Parse return expression
        if (!Match(TokenKind.Semicolon))
        {
            node.Expression = ParseExpression();

            // Consume semicolon
            Consume(TokenKind.Semicolon, "expected ';' after return expression.");
        }

        return node;
    }

    /// <summary>
    /// Parses statement.
    /// </summary>
    private Node ParseStatement()
    {
        Advance();

        // Check for token kind
        switch (Previous.Kind)
        {
            // Return statement
            case TokenKind.Return:
                return ParseReturn();

            // Unexpected token
            default:
            {
                Input.Error(Previous.Location, "unexpected token.");
                return null;
            }
        }
    }

    /// <summary>
    /// Parses block of statement(s).
    /// </summary>
    private Node ParseBlock()
    {
        // Build block node
        NodeBlock node = new NodeBlock();

        // Save block start
        Token start = Previous;

        // Parse statement(s)
        if (!Match(TokenKind.RightBrace))
        {
            while (!Match(TokenKind.RightBrace) && !Match(TokenKind.End))
                node.Statements.Add(ParseStatement());

            // Check for '}'
            if (Previous.Kind != TokenKind.RightBrace)
                Input.Error(start.Location, "expected '}' after function statement(s).");
        }

        return node;
    }

    /// <summary>
    /// Parses arrow expression.
    /// </summary>
    private Node ParseArrow()
    {
        // Build block node
        NodeBlock node = new NodeBlock();

        // Build return node
        NodeReturn returnNode = new NodeReturn()
        {
            Location   = Current.Location,
            Expression = null,
        };

        // Parse expression
        returnNode.Expression = ParseExpression();

        // Add return to block
        node.Statements.Add(returnNode);

        // Consume semicolon
        Consume(TokenKind.Semicolon, "expected ';' after arrow expression.");

        return node;
    }

    /// <summary>
    /// Parses function declaration.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="name"></param>
    private Node ParseFunctionDeclaration(Node type, Token name)
    {
        // Build the node
        NodeFunctionDeclaration node = new NodeFunctionDeclaration()
        {
            Type = type,
            Name = name,
        };

        // Parse function parameters
        if (!Match(TokenKind.RightParenthesis))
        {
            do
            {
                // Parse parameter type
                Advance(); // advance one token since the 
                           // type takes the previous identifier
                Node parameterType = ParseType();

                // Parse parameter name
                Consume(TokenKind.Identifier, "expected parameter name.");
                Token parameterName = Previous;

                // Build parameter node
                NodeFunctionParameter parameter = 
                    new NodeFunctionParameter(parameterType, parameterName);

                // Add parameter node to function parameters
                node.Parameters.Add(parameter);
            }
            while (Match(TokenKind.Comma));

            // Consume right parenthesis
            Consume(TokenKind.RightParenthesis, "Expected ')' after function parameter(s).");
        }

        // Parse function body
        if (Match(TokenKind.LeftBrace))
            node.Body = ParseBlock();
        else if (Match(TokenKind.Arrow))
            node.Body = ParseArrow();

        return node;
    }

    /// <summary>
    /// Try to parse a declaration.
    /// </summary>
    private void TryParsingDeclaration()
    {
        // Parse type
        Node type = ParseType();

        // Parse declaration name
        Consume(TokenKind.Identifier, "expected declaration name after type.");
        Token name = Previous;

        // Parse declaration
        if (Match(TokenKind.LeftParenthesis)) // function declaration?
            Nodes.Add(ParseFunctionDeclaration(type, name));
        else
            Input.Error(Current.Location, "expected declaration.");
    }

    /// <summary>
    /// Parse tokens into AST nodes.
    /// </summary>
    private void Parse()
    {
        Advance();

        // Check for token kind
        switch (Previous.Kind)
        {
            // Module declaration
            case TokenKind.Module:
            {
                ParseModuleDeclaration();
                break;
            }

            // Import
            case TokenKind.Import:
            {
                ParseImport();
                break;
            }
            
            // Declaration
            case TokenKind.Identifier:
            {
                TryParsingDeclaration();
                break;
            }

            // Unexpected token
            default:
            {
                Input.Error(Previous.Location, "unexpected token.");
                break;
            }
        }
    }

    /// <summary>
    /// Starts the parser.
    /// </summary>
    public void Start()
    {
        // Make global module
        Module globalModule = new Module("global");

        // Add global module
        Config.Modules.Add("global", globalModule);

        // Parse all input's sources into AST nodes
        foreach (Input input in Config.Inputs)
        {
            // Set current input to be parsed
            Input = input;

            // Start parsing until source end
            Advance();

            while (!Match(TokenKind.End) && !Config.HasFatalErrors)
                Parse();

            // Use global module if no module declaration was found
            if (Input.Module == null)
                Input.Module = globalModule;
        }

        /*
            SEMANTIC ANALYSIS
        */

        // Forward declaration pass
        foreach (Input input in Config.Inputs)
        {
            // NOTE: I can improve this to not create a class for each input
            ForwardDeclarePass pass = new ForwardDeclarePass(input);

            foreach (Node node in input.Nodes)
                pass.Visit(node);
        }

        // Type checking
        foreach (Input input in Config.Inputs)
        {
            // NOTE: I can improve this to not create a class for each input
            TypeCheckPass pass = new TypeCheckPass(input);

            foreach (Node node in input.Nodes)
                pass.Visit(node);
        }
    }
}