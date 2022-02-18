using System;
using System.Collections.Generic;

namespace Suteki
{
    class Parser
    {
        private Input        CurrentInput;
        private PropertyKind CurrentProperty;

        private Dictionary<string, UserType> Types = new Dictionary<string, UserType>
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
        private Scanner Scanner
        {
            get 
            {
                return CurrentInput.Scanner;
            }
        }

        // Get logger
        private Logger Logger
        {
            get
            {
                return CurrentInput.Logger;
            }
        }

        // Get nodes
        private List<Node> Nodes
        {
            get
            {
                return CurrentInput.Nodes;
            }
        }

        // Get previous token
        private Token Previous
        {
            get
            {
                return Scanner.Previous;
            }
        }

        // Get current token
        private Token Current
        {
            get
            {
                return Scanner.Current;
            }
        }

        // Advance current token
        private void Advance()
        {
            for (;;)
            {
                if (Scanner.Next() != TokenKind.Error)
                    break;

                Logger.Error(Current, Current.Content);
            }
        }

        // Match current token?
        private bool Match(TokenKind expected)
        {
            if (Current.Kind == expected)
            {
                Advance();
                return true;
            }

            return false;
        }

        // Consume token
        private void Consume(TokenKind expected, string message)
        {
            if (Match(expected))
                return;

            Logger.Error(Current, message);
        }

        // Parse qualified name
        private Node ParseQualifiedName(Node left)
        {
            Node name;

            Consume(TokenKind.Identifier, "Expected identifier.");
            name = new NodeQualifiedName(left, new NodeIdentifierName(Previous));

            if (Match(TokenKind.Dot))
                return ParseQualifiedName(name);

            return name;
        }

        // Parse name
        private Node ParseName()
        {
            Node left;

            Consume(TokenKind.Identifier, "Expected identifier.");
            left = new NodeIdentifierName(Previous);

            if (Match(TokenKind.Dot))
                return ParseQualifiedName(left);

            return left;
        }

        // Parse name
        private Node ParseName(Token token)
        {
            Node left = new NodeIdentifierName(token);

            if (Match(TokenKind.Dot))
                return ParseQualifiedName(left);

            return left;
        }

        // Parse module name
        private string ParseModuleName()
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

        // Parse version identifier
        private string ParseVersionIdentifier()
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
                        Logger.Error(Current, "Invalid version identifier.");
                    }
                    else
                        Logger.Error(Current, "Expected version identifier.");
                }

                if (!Match(TokenKind.Dot))
                    break;

                result += '.';
            }

            return result;
        }

        // Parse module
        private void ParseModule()
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
        private void ParseImport()
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
        }

        // Parse type
        private Node ParseType()
        {
            bool   isConst  = false;
            Node   node     = null;

            if (Previous.Kind == TokenKind.Const)
            {
                isConst = true;
                Advance();
            }

            string typeName = Previous.Content;

            if (Types.ContainsKey(typeName))
            {
                node = new NodePrimitive()
                {
                    IsConst       = isConst,
                    PrimitiveKind = Types[typeName].Kind
                };
            }

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
        private Node ParseCall(Node name, bool isExpression)
        {
            // Make node
            NodeCall node = new NodeCall()
            {
                Name         = name,
                IsExpression = isExpression
            };

            // Parse function parameters
            if (!Match(TokenKind.RightParenthesis))
            {
                do
                {
                    node.Parameters.Add(ParseExpression());
                }
                while (Match(TokenKind.Comma));

                Consume(TokenKind.RightParenthesis, "Expected ')' after function call parameter(s).");
            }

            return node;
        }

        // Parse identifier expression
        private Node ParseIdentifierExpression()
        {
            Node name = ParseName(Previous);
            
            if (Match(TokenKind.LeftParenthesis))
                return ParseCall(name, true);
           
            return ParseName(Previous);
        }

        // Parse primary expression
        private Node ParsePrimaryExpression()
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
                return new NodeGrouping(expression);
            }

            if (Match(TokenKind.Identifier))
                return ParseIdentifierExpression();

            Logger.Error(Current, "Unexpected token.");
            return null;
        }

        // Parse unary expression
        private Node ParseUnaryExpression()
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
        private Node ParseFactorExpression()
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
        private Node ParseTermExpression()
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
        private Node ParseComparisonExpression()
        {
            return ParseTermExpression();
        }

        // Parse equality expression
        private Node ParseEqualityExpression()
        {
            return ParseComparisonExpression();
        }

        // Parse expression
        private Node ParseExpression()
        {
            return ParseEqualityExpression();
        }

        // Parse return statement
        private Node ParseReturn()
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
        private Node ParseIdentifierStatement()
        {
            Node name = ParseName(Previous);
            
            if (Match(TokenKind.LeftParenthesis))
            {
                Node node = ParseCall(name, false);

                // Expect semicolon
                Consume(TokenKind.Semicolon, "Expected ';' after ')'.");
                return node;
            }

            Logger.Error(Previous, "Unexpected token.");
            return null;
        }

        // Check version
        private void CheckVersion(NodeBlock block = null)
        {
            Consume(TokenKind.LeftParenthesis, "Expected '(' after 'version'.");
            string version = ParseVersionIdentifier();
            Consume(TokenKind.RightParenthesis, "Expected ')' after version identifier.");

            Consume(TokenKind.LeftBrace, "Expected '{' after ')'.");
            Token start = Previous;

            if (!Match(TokenKind.RightBrace))
            {
                // Check for version
                if (Config.Versions.Contains(version))
                {
                    // Parse everything
                    while (!Match(TokenKind.RightBrace) && !Match(TokenKind.End))
                    {
                        if (block != null)
                        {
                            Node statementNode = ParseStatement();

                            if (statementNode != null)
                                block.Statements.Add(statementNode);
                        }
                        else
                            ParseDeclaration();
                    }
                }
                else
                {
                    // Skip everything
                    while (!Match(TokenKind.RightBrace) && !Match(TokenKind.End))
                        Advance();
                }

                if (Previous.Kind == TokenKind.End)
                    Logger.Error(start, "Expected '}'.");
            }
        }

        // Parse statement
        private Node ParseStatement(NodeBlock block = null)
        {
            Advance();

            switch (Previous.Kind)
            {
                case TokenKind.Return:
                    return ParseReturn();

                case TokenKind.Identifier:
                    return ParseIdentifierStatement();
                
                case TokenKind.Version:
                {
                    CheckVersion(block);
                    return null;
                }

                default:
                {
                    Logger.Error(Previous, "Unexpected token.");
                    return null;
                }
            }
        }

        // Parse block of statements
        private Node ParseBlock()
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
                    Node statementNode = ParseStatement(node);

                    if (statementNode != null)
                        node.Statements.Add(statementNode);
                }

                if (Previous.Kind == TokenKind.End)
                    Logger.Error(start, "Expected '}' after statement(s).");
            }

            return node;
        }

        // Parse function declaration
        private void ParseFunction(Node type, Token name)
        {
            // Make node
            NodeFunction node          = new NodeFunction();
                         node.Property = CurrentProperty;
                         node.Type     = type;
                         node.Name     = name;

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
                        NodeParameter parameter      = new NodeParameter();
                                      parameter.Type = parameterType;
                                      parameter.Name = Previous;

                        node.Parameters.Add(parameter);
                    }
                    else
                        Logger.Error(Previous, "Expected type.");
                }
                while (Match(TokenKind.Comma));

                Consume(TokenKind.RightParenthesis, "Expected ')' after function parameter(s).");
            }

            // Parse function block
            if (Match(TokenKind.LeftBrace))
                node.Block = ParseBlock();
            else
            {
                node.Block = null;

                // Expect semicolon
                Consume(TokenKind.Semicolon, "Expected ';' after ')'.");
            }

            Nodes.Add(node);

            // Reset property
            CurrentProperty = PropertyKind.None;
        }

        // Parse identifier
        private void ParseIdentifier(Node type)
        {
            Consume(TokenKind.Identifier, "Expected identifier after type.");
            Token name = Previous;

            if (Match(TokenKind.LeftParenthesis))
                ParseFunction(type, name);
            else
                Logger.Error(Current, "Unexpected token.");
        }

        // Parse declaration
        private void ParseDeclaration()
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

                case TokenKind.Version:
                {
                    CheckVersion();
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
            Module globalModule = new Module("global");
            Config.Modules.Add("global", globalModule);

            // Register all global symbols from inputs
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

                // Don't do the other pass if an error happened.
                if (Config.HadError)
                    return;

                // Type checking
                foreach (Node node in input.Nodes)
                    node.TypeCheck(input);
            }
        }
    }
}