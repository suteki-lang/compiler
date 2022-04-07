namespace Suteki.Compiler;

using System.Collections.Generic;

/// <summary>
/// A class that represents a module.
/// </summary>
public sealed class Module
{
    /// <summary>
    /// The name of the module.
    /// </summary>
    public string Name;
    
    /// <summary>
    /// The symbol table of the module.
    /// </summary>
    public Dictionary<string, Symbol> Symbols;

    /// <summary>
    /// Constructs a <see cref="Module"/> class.
    /// </summary>
    /// <param name="name">The name of the module.</param>
    public Module(string name)
    {
        Name    = name;
        Symbols = new Dictionary<string, Symbol>();
    }
}