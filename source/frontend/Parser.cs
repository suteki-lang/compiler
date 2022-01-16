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

                Logger.Error(Current, Current.Data.ToString());
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

        // Parse module name
        private string ParseModuleName()
        {
            string result = "";

            for (;;)
            {
                if (Match(TokenKind.Identifier))
                    result += Previous.Data.ToString();
                else
                {
                    if (result != "")
                    {
                        Current.Data = result;
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
            // Set input module
            string moduleName = ParseModuleName();

            if (CurrentInput.Module != "")
            {
                Previous.Data = moduleName;
                Logger.Error(Previous, "This file was already exported.");
            }

            CurrentInput.Module = moduleName;

            // Optional semicolon
            Match(TokenKind.Semicolon);
        }

        // Parse import
        private void ParseImport()
        {
            // Make node
            NodeImport node            = new NodeImport();
                       node.Start      = Current;
                       node.ModuleName = ParseModuleName();

            Nodes.Add(node);

            // Optional semicolon
            Match(TokenKind.Semicolon);
        }

        // Parse type
        private Node ParseType()
        {
            string typeName = Previous.Data.ToString();

            if (Types.ContainsKey(typeName))
            {
                NodePrimitive node      = new NodePrimitive();
                              node.Kind = Types[typeName].Kind;

                return node;
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
            NodeReturn node       = new NodeReturn();
                       node.Start = Previous;

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
        private Node ParseCall(Token name)
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
            Token name = Previous;
            
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
            NodeBlock node = new NodeBlock();

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
            if (CurrentProperty != PropertyKind.Extern)
            {
                Consume(TokenKind.LeftBrace, "Expected '{'.");
                node.Block = ParseBlock();
            }
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
            foreach (Input input in Config.Inputs)
            {
                foreach (Node node in input.Nodes)
                {
                    if (!input.SymbolsAreRegistered)
                        node.RegisterSymbols(input);
                    
                    node.CheckSymbols(input);
                    node.TypeCheck(input);
                    node.Optimize(input);
                }

                input.SymbolsAreRegistered = true;
            }
        }
    }
}