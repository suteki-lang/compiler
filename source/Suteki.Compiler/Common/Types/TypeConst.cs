namespace Suteki.Compiler;

/// <summary>
/// A constant type.
/// </summary>
public class TypeConst : Type
{
    /// <summary>
    /// The type that is constant.
    /// </summary>
    public Type Type;

    /// <summary>
    /// Is type void?
    /// </summary>
    public override bool IsVoid() => Type.IsVoid();
    
    /// <summary>
    /// Is type a primitive, function, pointer...?
    /// </summary>
    public override bool IsBasic() => Type.IsBasic();

    /// <summary>
    /// Is type an integer?
    /// </summary>
    public override bool IsInteger() => Type.IsInteger();

    /// <summary>
    /// Is type an unsigned integer?
    /// </summary>
    public override bool IsUnsigned() => Type.IsUnsigned();

    /// <summary>
    /// Is type a float?
    /// </summary>
    public override bool IsFloat() => Type.IsFloat();

    /// <summary>
    /// Is type a boolean?
    /// </summary>
    public override bool IsBool() => Type.IsBool();

    /// <summary>
    /// Is type constant?
    /// </summary>
    public override bool IsConst() => true;

    /// <summary>
    /// Is type a pointer?
    /// </summary>
    public override bool IsPointer() => Type.IsPointer();

    /// <summary>
    /// Can type be casted to another type?
    /// </summary>
    /// <param name="type">The type to be checked.</param>
    public override bool CanCastTo(Type type)
    {
        Type typeToCheck = type;

        // Remove constant from type (if neeeded)
        if (type.IsConst())
            typeToCheck = type.GetDeconstedType();

        // Check if can cast
        return Type.CanCastTo(typeToCheck);
    }

    /// <summary>
    /// Is other type identical to this type?
    /// </summary>
    /// <param name="other">The type to be checked.</param>
    /// <returns></returns>
    public override bool IsIdentical(Type other)
    {
        Type typeToCheck = other;

        // Remove constant from type (if neeeded)
        if (other.IsConst())
            typeToCheck = other.GetDeconstedType();

        // Check if is identical
        return Type.IsIdentical(typeToCheck);
    }

    /// <summary>
    /// Get size of the type.
    /// </summary>
    public override int GetSize() => Type.GetSize();

    /// <summary>
    /// Remove the pointer from type.
    /// </summary>
    public override Type GetDerefedType() => Type.GetDerefedType();

    /// <summary>
    /// Remove the constant from type.
    /// </summary>
    public override Type GetDeconstedType() => Type;
}