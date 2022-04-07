namespace Suteki.Compiler;

/// <summary>
/// A pointer type.
/// </summary>
public class TypePointer : Type
{
    /// <summary>
    /// The base of the pointer.
    /// </summary>
    public Type Base;

    /// <summary>
    /// Constructs a <see cref="TypePointer"/> class.
    /// </summary>
    /// <param name="baseType">The base type.</param>
    public TypePointer(Type baseType)
    {
        Base = baseType;
    }

    /// <summary>
    /// Is type null?
    /// </summary>
    public override bool IsNull() => Base.IsNull();
    
    /// <summary>
    /// Is type a primitive, function, pointer...?
    /// </summary>
    public override bool IsBasic() => true;

    /// <summary>
    /// Is type a pointer?
    /// </summary>
    public override bool IsPointer() => true;

    /// <summary>
    /// Can type be casted to another type?
    /// </summary>
    /// <param name="type">The type to be checked.</param>
    public override bool CanCastTo(Type type)
    {
        // Get base from type pointer
        Type typeBase = type;

        if (type.IsConst())
            typeBase = type.GetDeconstedType();

        // Don't allow removing constant from type.
        if (type.IsPointer() && Base.IsConst() && !typeBase.IsConst())
            return false;        

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
        // Allow checking for null
        if (other.IsNull())
            return true;

        // Check if other is pointer and compare the base types
        return (other.IsPointer() && Base.IsIdentical(other.GetDerefedType()));
    }

    /// <summary>
    /// Get size of the type.
    /// </summary>
    public override int GetSize() => Config.PointerSize;

    /// <summary>
    /// Remove the pointer from type.
    /// </summary>
    public override Type GetDerefedType() => Base;

    /// <summary>
    /// Get name of the type.
    /// </summary>
    public override string GetName() => $"{Base.GetName()}*";
}