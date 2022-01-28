using System;
using System.Collections.Generic;

namespace Suteki
{
    class Parser
    {
        private Input        CurrentInput;
        private PropertyKind CurrentProperty;

        private Dictionary<string, Type> Types = new Dictionary<string, Type>
        {
            { "void",   new Type(PrimitiveKind.Void)   },
            { "bool",   new Type(PrimitiveKind.Bool)   },
            { "string", new Type(PrimitiveKind.String) },

            { "ubyte",  new Type(PrimitiveKind.UByte)  },
            { "ushort", new Type(PrimitiveKind.UShort) },
            { "uint",   new Type(PrimitiveKind.UInt)   },
            { "ulong",  new Type(PrimitiveKind.ULong)  },

            { "byte",  new Type(PrimitiveKind.Byte)    },
            { "short", new Type(PrimitiveKind.Short)   },
            { "int",   new Type(PrimitiveKind.Int)     },
            { "long",  new Type(PrimitiveKind.Long)    },

            { "single", new Type(PrimitiveKind.Single) },
            { "double", new Type(PrimitiveKind.Double) },
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
        private string ParseModuleNameStr()
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

        // Parse export
        private void ParseExport()
        {
            string moduleName = ParseModuleNameStr();

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

            // Optional semicolon
            Match(TokenKind.Semicolon);
        }

        // Parse import
        private void ParseImport()
        {
            // Make node
            NodeImport node            = new NodeImport();
                       node.ModuleName = ParseName();

            Nodes.Add(node);

            // Optional semicolon
            Match(TokenKind.Semicolon);
        }

        // Parse type
        private Node ParseType()
        {
            string typeName = Previous.Content;

            if (Types.ContainsKey(typeName))
            {
                return new NodePrimitive()
                {
                    PrimitiveKind = Types[typeName].Kind
                };
            }

            return null;
        }

        // Parse expression
        private Node ParseExpression()
        {
            if (Match(TokenKind.Float))
                return new NodeFloat(Previous);

            if (Match(TokenKind.Integer))
                return new NodeInteger(Previous);

            if (Match(TokenKind.String))
                return new NodeString(Previous);

            if (Match(TokenKind.Bool))
                return new NodeBool(Previous);

            Logger.Error(Current, "Unexpected token.");
            return null;
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

                // Optional semicolon
                Match(TokenKind.Semicolon);
            }

            return node;
        }

        // Parse call
        private Node ParseCall(Node name)
        {
            // Make node
            NodeCall node      = new NodeCall();
                     node.Name = name;

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

            // Optional semicolon
            Match(TokenKind.Semicolon);

            return node;
        }

        // Parse identifier statement
        private Node ParseIdentifierStatement()
        {
            Node name = ParseName(Previous);
            
            if (Match(TokenKind.LeftParenthesis))
                return ParseCall(name);
            
            Logger.Error(Current, "Unexpected token.");
            return null;
        }

        // Parse statement
        private Node ParseStatement()
        {
            Advance();

            switch (Previous.Kind)
            {
                case TokenKind.Return:
                    return ParseReturn();

                case TokenKind.Identifier:
                    return ParseIdentifierStatement();

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
            Token start = Current;

            // Make node
            NodeBlock node = new NodeBlock()
            {
                Token = start
            };

            // Parse statements
            if (!Match(TokenKind.RightBrace))
            {
                while (!Match(TokenKind.RightBrace) && !Match(TokenKind.End))
                    node.Statements.Add(ParseStatement());

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

                // Optional semicolon
                Match(TokenKind.Semicolon);
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
                case TokenKind.Extern:
                {
                    CurrentProperty = PropertyKind.Extern;
                    break;
                }

                case TokenKind.Export:
                {
                    ParseExport();
                    break;
                }

                case TokenKind.Import:
                {
                    ParseImport();
                    break;
                }

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

            // Analyze the AST nodes
            // foreach (Input input in Config.Inputs)
            // {
            //     // Make a module
            //     if (input.Module == null)
            //         input.Module = new Module("");

            //     foreach (Node node in input.Nodes)
            //     {
            //         node.RegisterSymbols(input);
            //     }
            // }

            // foreach (Input input in Config.Inputs)
            // {
            //     foreach (Node node in input.Nodes)
            //     {
            //         node.ResolveSymbols(input);

            //         if (!Config.HadError)
            //         {
            //             node.TypeCheck(input);
            //             node.Optimize(input);
            //         }
            //     }
            // }
        }
    }
}