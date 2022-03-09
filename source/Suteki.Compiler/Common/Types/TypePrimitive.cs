namespace Suteki.Compiler;

/// <summary>
/// A primitive type.
/// </summary>
public class TypePrimitive : Type
{
    /// <summary>
    /// The kind of the primitive.
    /// </summary>
    public PrimitiveKind Kind;

    /// <summary>
    /// Constructs a <see cref="TypePrimitive"/> class.
    /// </summary>
    /// <param name="kind">The kind of the primitive.</param>
    public TypePrimitive(PrimitiveKind kind)
    {
        Kind = kind;
    }

    /// <summary>
    /// Is type null?
    /// </summary>
    public override bool IsNull() => (Kind == PrimitiveKind.Null);

    /// <summary>
    /// Is type void?
    /// </summary>
    public override bool IsVoid() => (Kind == PrimitiveKind.Void);
    
    /// <summary>
    /// Is type a primitive, function, pointer...?
    /// </summary>
    public override bool IsBasic()
    {
        return (Kind != PrimitiveKind.Void);
    }

    /// <summary>
    /// Is type an integer?
    /// </summary>
    public override bool IsInteger()
    {
        return (Kind >= PrimitiveKind.UByte && Kind <= PrimitiveKind.SWord);
    }

    /// <summary>
    /// Is type an unsigned integer?
    /// </summary>
    public override bool IsUnsigned()
    {
        return (Kind >= PrimitiveKind.UByte && Kind <= PrimitiveKind.UWord);
    }

    /// <summary>
    /// Is type a float?
    /// </summary>
    public override bool IsFloat()
    {
        return (Kind == PrimitiveKind.Single || Kind == PrimitiveKind.Double);
    }

    /// <summary>
    /// Is type a boolean?
    /// </summary>
    public override bool IsBool() => (Kind == PrimitiveKind.Bool);

    /// <summary>
    /// Is type a string?
    /// </summary>
    public override bool IsString() => (Kind == PrimitiveKind.String);

    /// <summary>
    /// Can type be casted to another type?
    /// </summary>
    /// <param name="type">The type to be checked.</param>
    public override bool CanCastTo(Type type)
    {
        // Allow casting to any basic type
        return type.IsBasic();
    }

    /// <summary>
    /// Is other type identical to this type?
    /// </summary>
    /// <param name="other">The type to be checked.</param>
    /// <returns></returns>
    public override bool IsIdentical(Type other)
    {
        // null == null
        if (IsNull() && other.IsNull())
            return true;

        // void == null
        if (IsVoid() && other.IsVoid())
            return true;

        // <any integer> == <any integer>
        if (IsInteger() && other.IsInteger())
            return true;
            
        // <any float> == <any float>
        if (IsFloat() && other.IsFloat())
            return true;

        // <any float> == <any integer>
        if (IsFloat() && other.IsInteger())
            return true;

        // bool == bool
        if (IsBool() && other.IsBool())
            return true;

        // string == string
        if (IsString() && other.IsString())
            return true;

        // No types matched.
        return false;
    }

    /// <summary>
    /// Get size of the type.
    /// </summary>
    public override int GetSize()
    {
        switch (Kind)
        {
            case PrimitiveKind.UByte:
            case PrimitiveKind.SByte:
            case PrimitiveKind.Bool:
                return 1;

            case PrimitiveKind.UShort:
            case PrimitiveKind.SShort:
                return 2;

            case PrimitiveKind.UInt:
            case PrimitiveKind.SInt:
            case PrimitiveKind.Single:
                return 4;

            case PrimitiveKind.ULong:
            case PrimitiveKind.SLong:
            case PrimitiveKind.String:
            case PrimitiveKind.Double:
            case PrimitiveKind.UWord:
            case PrimitiveKind.SWord:
            case PrimitiveKind.Null:
                return Config.PointerSize;

            default:
                return 0;
        }
    }
}