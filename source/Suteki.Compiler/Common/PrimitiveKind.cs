namespace Suteki.Compiler;

/// <summary>
/// The kind of a <see cref="TypePrimitive"/>.
/// </summary>
public enum PrimitiveKind
{
    Null,
    Void,
    Bool,
    String,

    UByte,
    UShort,
    UInt,
    ULong,
    UWord,
    SByte,
    SShort,
    SInt,
    SLong,
    SWord,

    Single,
    Double,
}