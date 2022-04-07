namespace Suteki.Compiler;

/// <summary>
/// A class that represents a symbol.
/// </summary>
public sealed class Symbol
{
    /// <summary>
    /// The input of where the symbol is.
    /// </summary>
    public Input Input;

    /// <summary>
    /// The type of the symbol.
    /// </summary>
    public Type Type;

    /// <summary>
    /// The name of the symbol.
    /// </summary>
    public string Name;

    /// <summary>
    /// The location of where the symbol was declared.
    /// </summary>
    public FileLocation Location;

    /// <summary>
    /// Constructs a <see cref="Symbol"/> class.
    /// </summary>
    /// <param name="module"  >The input of where the symbol is.</param>
    /// <param name="type"    >The type of the symbol.          </param>
    /// <param name="name"    >The name of the symbol.          </param>
    /// <param name="location">The location of the symbol.      </param>
    public Symbol(Input input, Type type, string name, FileLocation location)
    {
        Input    = input;
        Type     = type;
        Name     = name;
        Location = location;
    }
}