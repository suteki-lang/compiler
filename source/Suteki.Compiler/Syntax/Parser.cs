namespace Suteki.Compiler;

using System.Collections.Generic;

public class Parser
{
    public Input        CurrentInput;
    public PropertyKind CurrentProperty;

    public Dictionary<string, UserType> Types = new Dictionary<string, UserType>
    {
        { "void",    new UserType(PrimitiveKind.Void)    },
        { "bool",    new UserType(PrimitiveKind.Bool)    },
        { "string",  new UserType(PrimitiveKind.String)  },

        { "ubyte",   new UserType(PrimitiveKind.UByte)   },
        { "ushort",  new UserType(PrimitiveKind.UShort)  },
        { "uint",    new UserType(PrimitiveKind.UInt)    },
        { "ulong",   new UserType(PrimitiveKind.ULong)   },
        { "uword",   new UserType(PrimitiveKind.UWord)   },

        { "sbyte",   new UserType(PrimitiveKind.SByte)   },
        { "sshort",  new UserType(PrimitiveKind.SShort)  },
        { "sint",    new UserType(PrimitiveKind.SInt)    },
        { "slong",   new UserType(PrimitiveKind.SLong)   },
        { "sword",   new UserType(PrimitiveKind.SWord)   },

        { "byte",    new UserType(PrimitiveKind.SByte)   },
        { "short",   new UserType(PrimitiveKind.SShort)  },
        { "int",     new UserType(PrimitiveKind.SInt)    },
        { "long",    new UserType(PrimitiveKind.SLong)   },
        { "word",    new UserType(PrimitiveKind.SWord)   },

        { "single",  new UserType(PrimitiveKind.Single)  },
        { "double",  new UserType(PrimitiveKind.Double)  },
    };

    // Get scanner
    public Scanner Scanner
    {
        get 
        {
            return CurrentInput.Scanner;
        }
    }

    // Get logger
    public Logger Logger
    {
        get
        {
            return CurrentInput.Logger;
        }
    }

    // Get nodes
    public List<Node> Nodes
    {
        get
        {
            return CurrentInput.Nodes;
        }
    }

    // Get previous token
    public Token Previous
    {
        get
        {
            return Scanner.Previous;
        }
    }

    // Get current token
    public Token Current
    {
        get
        {
            return Scanner.Current;
        }
    }

    // Advance current token
    public void Advance()
    {
        for (;;)
        {
            if (Scanner.Next() != TokenKind.Error)
                break;

            Logger.Error(Current, Current.Content);
        }
    }

    // Match current token?
    public bool Match(TokenKind expected)
    {
        if (Current.Kind == expected)
        {
            Advance();
            return true;
        }

        return false;
    }

    // Consume token
    public void Consume(TokenKind expected, string message)
    {
        if (Match(expected))
            return;

        Logger.Error(Current, message);
    }

    // Parse qualified name
    public Node ParseQualifiedName(Node left)
    {
        Node name;

        Consume(TokenKind.Identifier, "Expected identifier.");
        name = new NodeQualifiedName(left, new NodeIdentifierName(Previous));

        if (Match(TokenKind.Dot))
            return ParseQualifiedName(name);

        return name;
    }

    // Parse name
    public Node ParseName()
    {
        Node left;

        Consume(TokenKind.Identifier, "Expected identifier.");
        left = new NodeIdentifierName(Previous);

        if (Match(TokenKind.Dot))
            return ParseQualifiedName(left);

        return left;
    }

    // Parse name
    public Node ParseName(Token token)
    {
        Node left = new NodeIdentifierName(token);

        if (Match(TokenKind.Dot))
            return ParseQualifiedName(left);

        return left;
    }

    // Parse module name
    public string ParseModuleName()
    {
        string result = "";

        for (;;)
        {
            if (Match(TokenKind.Identifier))
                result += Previous.Content;
            else
            {
                if (result != "")
                {
                    Current.Content = result;
                    Logger.Error(Current, "Invalid module name.");
                }
                else
                    Logger.Error(Current, "Expected module name.");
            }

            if (!Match(TokenKind.Dot))
                break;

            result += '.';
        }

        return result;
    }

    // Parse module
    public void ParseModule()
    {
        string moduleName = ParseModuleName();

        // Add module
        if (!Config.Modules.ContainsKey(moduleName))
            Config.Modules.Add(moduleName, new Module(moduleName));

        // Set input module
        if (CurrentInput.Module != null)
        {
            Previous.Content = moduleName;
            Logger.Error(Previous, "This file was already exported.");
        }

        CurrentInput.Module = Config.Modules[moduleName];

        // Expect semicolon
        Consume(TokenKind.Semicolon, "Expected ';' after module name.");
    }

    // Parse import
    public void ParseImport()
    {
        // Make node
        NodeImport node = new NodeImport()
        {
            ModuleName = ParseName(),
            IsPublic   = (CurrentProperty == PropertyKind.Public)
        };

        Nodes.Add(node);

        // Expect semicolon
        Consume(TokenKind.Semicolon, "Expected ';' after module name.");

        // Reset property
        CurrentProperty = PropertyKind.None;
    }

    // Parse type
    public Node ParseType()
    {
        Node node = null;

        // Parse constant type
        if (Previous.Kind == TokenKind.Const)
        {
            Token previous = Previous;
            Advance();

            node = new NodeConst()
            {
                Token = previous
            };
        }

        // Parse primitive type
        if (Types.ContainsKey(Previous.Content))
        {
            NodePrimitive primitive = new NodePrimitive()
            {
                PrimitiveKind = Types[Previous.Content].Kind
            };

            if (node != null)
            {
                NodeConst constNode      = ((NodeConst)node);
                          constNode.Type = primitive;
            }
            else
                node = primitive;
        }

        // Parse pointer type
        while (Match(TokenKind.Star))
        {
            node = new NodePointer()
            {
                PointsTo = node
            };
        }

        return node;
    }

    // Parse call
    public Node ParseCall(Node callee, bool isExpression)
    {
        // Make node
        NodeCall node = new NodeCall()
        {
            Callee       = callee,
            IsExpression = isExpression
        };

        // Parse function parameters
        if (!Match(TokenKind.RightParenthesis))
        {
            do
                node.Parameters.Add(ParseExpression());
            while (Match(TokenKind.Comma));

            Consume(TokenKind.RightParenthesis, "Expected ')' after function call parameter(s).");
        }

        return node;
    }

    // Parse identifier expression
    public Node ParseIdentifierExpression()
    {
        Node name = ParseName(Previous);
        
        if (Match(TokenKind.LeftParenthesis))
            return ParseCall(name, true);
        
        return ParseName(Previous);
    }

    // Parse primary expression
    public Node ParsePrimaryExpression()
    {
        if (Match(TokenKind.Float))
            return new NodeFloat(Previous);

        if (Match(TokenKind.Integer))
            return new NodeInteger(Previous);

        if (Match(TokenKind.String))
            return new NodeString(Previous);

        if (Match(TokenKind.Bool))
            return new NodeBool(Previous);

        if (Match(TokenKind.Null))
            return new NodeNull(Previous);

        if (Match(TokenKind.LeftParenthesis))
        {
            Node expression = ParseExpression();

            Consume(TokenKind.RightParenthesis, "Expected ')' after expression.");
            Node node = new NodeGrouping(expression);

            while (Match(TokenKind.LeftParenthesis))
                node = ParseCall(node, true);

            return node;
        }

        if (Match(TokenKind.Identifier))
            return ParseIdentifierExpression();

        Logger.Error(Current, "Expected expression.");
        return null;
    }

    // Parse unary expression
    public Node ParseUnaryExpression()
    {
        if (Match(TokenKind.Minus))
        {
            OperatorKind op      = Token.ToOperatorKind(Previous.Kind);
            Node         operand = ParseUnaryExpression();

            return new NodeUnary(op, operand);
        }

        return ParsePrimaryExpression();
    }

    // Parse factor expression
    public Node ParseFactorExpression()
    {
        Node expression = ParseUnaryExpression();

        while (Match(TokenKind.Slash) || Match(TokenKind.Star))
        {
            OperatorKind op    = Token.ToOperatorKind(Previous.Kind);
            Node         right = ParseUnaryExpression();

            expression = new NodeBinary(expression, op, right);
        }

        return expression;
    }

    // Parse term expression
    public Node ParseTermExpression()
    {
        Node expression = ParseFactorExpression();

        while (Match(TokenKind.Minus) || Match(TokenKind.Plus))
        {
            OperatorKind op    = Token.ToOperatorKind(Previous.Kind);
            Node         right = ParseFactorExpression();

            expression = new NodeBinary(expression, op, right);
        }

        return expression;
    }

    // Parse comparison expression
    public Node ParseComparisonExpression()
    {
        return ParseTermExpression();
    }

    // Parse equality expression
    public Node ParseEqualityExpression()
    {
        Node expression = ParseComparisonExpression();

        while (Match(TokenKind.EqualEqual))
        {
            OperatorKind op    = Token.ToOperatorKind(Previous.Kind);
            Node         right = ParseComparisonExpression();

            expression = new NodeBinary(expression, op, right);
        }

        return expression;
    }

    // Parse expression
    public Node ParseExpression()
    {
        return ParseEqualityExpression();
    }

    // Parse return statement
    public Node ParseReturn()
    {
        // Make node
        NodeReturn node = new NodeReturn()
        {
            Token = Previous
        };

        // Parse expression
        if (Match(TokenKind.Semicolon))
            node.Expression = null;
        else
        {
            node.Expression = ParseExpression();

            // Expect semicolon
            Consume(TokenKind.Semicolon, "Expected ';' after expression.");
        }

        return node;
    }

    // Parse identifier statement
    public Node ParseIdentifierStatement()
    {
        Node name = ParseName(Previous);
        
        if (Match(TokenKind.LeftParenthesis))
        {
            Node node = ParseCall(name, true);

            while (Match(TokenKind.LeftParenthesis))
                node = ParseCall(node, true);

            // Expect semicolon
            ((NodeCall)node).IsExpression = false;
            Consume(TokenKind.Semicolon, "Expected ';' after call.");
            return node;
        }

        Logger.Error(Previous, "Unexpected token.");
        return null;
    }

    // Parse if statement
    public Node ParseIf()
    {
        NodeIf node = new NodeIf()
        {
            Token = Previous
        };

        Consume(TokenKind.LeftParenthesis, "Expected '(' after 'if'.");
        node.Condition = ParseExpression();
        Consume(TokenKind.RightParenthesis, "Expected ')' after condition.");

        node.ThenBody = ParseStatement();
        node.ElseBody = null;

        if (Match(TokenKind.Else))
            node.ElseBody = ParseStatement();

        return node;
    }

    // Parse statement
    public Node ParseStatement()
    {
        Advance();

        switch (Previous.Kind)
        {
            case TokenKind.Return:
                return ParseReturn();

            case TokenKind.Identifier:
                return ParseIdentifierStatement();

            case TokenKind.If:
                return ParseIf();

            case TokenKind.LeftBrace:
                return ParseBlock();

            case TokenKind.LeftParenthesis:
            {
                Node expression = ParseExpression();

                Consume(TokenKind.RightParenthesis, "Expected ')' after expression.");
                Node nodeG = new NodeGrouping(expression);
                Node node  = nodeG;

                while (Match(TokenKind.LeftParenthesis))
                    node = ParseCall(node, true);

                // Expect semicolon
                if (node != nodeG)
                    ((NodeCall)node).IsExpression = false;
                Consume(TokenKind.Semicolon, "Expected ';' after call.");
                return node;
            }

            default:
            {
                Logger.Error(Previous, "Unexpected token.");
                return null;
            }
        }
    }

    // Parse block of statements
    public Node ParseBlock()
    {
        Token start = Previous;

        // Make node
        NodeBlock node = new NodeBlock()
        {
            Token = start
        };

        // Parse statements
        if (!Match(TokenKind.RightBrace))
        {
            while (!Match(TokenKind.RightBrace) && !Match(TokenKind.End))
            {
                Node statementNode = ParseStatement();

                if (statementNode != null)
                    node.Statements.Add(statementNode);
            }

            if (Previous.Kind == TokenKind.End)
                Logger.Error(start, "Expected '}' after statement(s).");
        }

        return node;
    }

    // Parse function declaration
    public void ParseFunction(Node type, Token name)
    {
        // Make node
        NodeFunction node = new NodeFunction()
        {
            Property = CurrentProperty,
            Type     = type,
            Name     = name
        };

        // Parse function parameters
        if (!Match(TokenKind.RightParenthesis))
        {
            do
            {
                Advance();
                Node parameterType = ParseType();

                if (parameterType != null)
                {
                    Consume(TokenKind.Identifier, "Expected function name after type.");

                    // Make node
                    node.Parameters.Add(new NodeParameter()
                    {
                        Type = parameterType,
                        Name = Previous
                    });
                }
                else
                    Logger.Error(Previous, "Expected type.");
            }
            while (Match(TokenKind.Comma));

            Consume(TokenKind.RightParenthesis, "Expected ')' after function parameter(s).");
        }

        // Parse function block
        if (Match(TokenKind.LeftBrace))
            node.Body = ParseBlock();
        else if (Match(TokenKind.Arrow))
        {
            NodeBlock block = new NodeBlock();

            block.Statements.Add(new NodeReturn()
            {
                Token      = Previous,
                Expression = ParseExpression() 
            });

            node.Body = block;

            // Expect semicolon
            Consume(TokenKind.Semicolon, "Expected ';' after expression.");
        }
        else
        {
            node.Body = null;

            // Expect function to be extern
            if (CurrentProperty != PropertyKind.Extern)
                Logger.Error(Current, "Expected '{' after ')'.");
            else
                // Expect semicolon
                Consume(TokenKind.Semicolon, "Expected ';' after ')'.");
        }

        Nodes.Add(node);

        // Reset property
        CurrentProperty = PropertyKind.None;
    }

    // Parse identifier
    public void ParseIdentifier(Node type)
    {
        Consume(TokenKind.Identifier, "Expected identifier after type.");
        Token name = Previous;

        if (Match(TokenKind.LeftParenthesis))
            ParseFunction(type, name);
        else
            Logger.Error(Current, "Unexpected token.");
    }

    // Parse declaration
    public void ParseDeclaration()
    {
        Advance();

        switch (Previous.Kind)
        {
            case TokenKind.Public:
            {
                CurrentProperty = PropertyKind.Public;
                break;
            }

            case TokenKind.Private:
            {
                CurrentProperty = PropertyKind.Private;
                break;
            }

            case TokenKind.Extern:
            {
                CurrentProperty = PropertyKind.Extern;
                break;
            }

            case TokenKind.Module:
            {
                ParseModule();
                break;
            }

            case TokenKind.Import:
            {
                ParseImport();
                break;
            }

            case TokenKind.Const:
            case TokenKind.Identifier:
            {
                Node type = ParseType();

                if (type != null)
                    ParseIdentifier(type);
                else
                    Logger.Error(Previous, "Unexpected token.");

                break;
            }

            default:
                break;
        }
    }

    // Start parsing
    public void Start()
    {
        // Parse all source code into AST nodes
        foreach (Input input in Config.Inputs)
        {
            CurrentInput = input;
            Advance();

            while (!Match(TokenKind.End))
                ParseDeclaration();
        }

        // Don't analyze the nodes if there's an error
        if (Config.HadError)
            return;

        /*
            SEMANTIC ANALYSIS
        */

        // Make global module
        Module globalModule;

        if (!Config.Modules.ContainsKey("global"))
        {
            globalModule = new Module("global");
            Config.Modules.Add("global", globalModule);
        }
        else
            globalModule = Config.Modules["global"];
            
        // Register all symbols from inputs
        foreach (Input input in Config.Inputs)
        {
            // Use global module?
            if (input.Module == null)
                input.Module = globalModule;
            else
            {
                // Always import the global module
                input.Imports.Add(globalModule);
            }

            foreach (Node node in input.Nodes)
                node.RegisterSymbols(input);
        }

        // Don't do the other passes if an error happened.
        if (Config.HadError)
            return;

        foreach (Input input in Config.Inputs)
        {
            // Resolve symbols
            foreach (Node node in input.Nodes)
                node.ResolveSymbols(input);

            // Don't do the other passes if an error happened.
            if (Config.HadError)
                return;

            // Resolve types
            foreach (Node node in input.Nodes)
                node.ResolveTypes(input);

            // Type checking
            foreach (Node node in input.Nodes)
                node.TypeCheck(input);
        }
    }
}