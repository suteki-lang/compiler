namespace Suteki.Compiler;

using System.Collections.Generic;

/// <summary>
/// Parses all input's sources into AST nodes.
/// </summary>
public sealed class Parser
{
    /// <summary>
    /// All the types (language and user).
    /// </summary>
    public Dictionary<string, TypePrimitive> Types = new Dictionary<string, TypePrimitive>
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
                    Input.Error(Current.Location, "Expected module name.");
                    break;
                }
                else
                {
                    Input.Error(Current.Location, "Unexpected token.");
                    break;
                }
            }

            // Only keep parsing module name if there's a dot
            if (!Match(TokenKind.Dot))
                break;

            moduleName += '.';
        }

        // Consume semicolon
        Consume(TokenKind.Semicolon, "Expected ';' after module declaration.");

        // Set input's module
        if (Input.Module == null)
        {
            // Add module (if needed)
            if (!Config.Modules.ContainsKey(moduleName))
                Config.Modules.Add(moduleName, new Module(moduleName));

            // Set module
            Input.Module = Config.Modules[moduleName];

            // Set module declaration token for error information
            Input.ModuleDeclarationToken = moduleToken;
        }
        else
        {
            Input.Error(moduleToken.Location, 
                $"This file was already exported as '{Input.Module.Name}' module.");

            Input.Note(Input.ModuleDeclarationToken.Location,
                "This file was exported here.");
        }
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

            // Unexpected token
            default:
            {
                Input.Error(Previous.Location, "Unexpected token.");
                break;
            }
        }
    }

    /// <summary>
    /// Starts the parser.
    /// </summary>
    public void Start()
    {
        // Parse all input's sources into AST nodes
        foreach (Input input in Config.Inputs)
        {
            // Set current input to be parsed
            Input = input;

            // Start parsing until source end
            Advance();

            while (!Match(TokenKind.End))
                Parse();
        }
    }
}