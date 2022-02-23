namespace Suteki.Tester;

using Suteki.Compiler;

public static class ParserExtension
{
    // Parse single input
    public static void Parse(this Parser parser, Input input)
    {
        // Parse input into AST nodes
        parser.CurrentInput = input;
        parser.Advance();

        while (!parser.Match(TokenKind.End))
            parser.ParseDeclaration();

        // Don't analyze the nodes if there's an error
        if (Config.HadError)
            return;

        /*
            SEMANTIC ANALYSIS
        */

        // Register all symbols from input
        foreach (Node node in input.Nodes)
            node.RegisterSymbols(input);

        // Don't do the other pass if an error happened.
        if (Config.HadError)
            return;

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