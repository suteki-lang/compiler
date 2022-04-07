namespace Suteki.Compiler;

using System.Collections.Generic;

/// <summary>
/// The base class for all types.
/// </summary>
public class Type
{
    /// <summary>
    /// Is type null?
    /// </summary>
    public virtual bool IsNull() => false;

    /// <summary>
    /// Is type void?
    /// </summary>
    public virtual bool IsVoid() => false;
    
    /// <summary>
    /// Is type a primitive, function, pointer...?
    /// </summary>
    public virtual bool IsBasic() => false;

    /// <summary>
    /// Is type an integer?
    /// </summary>
    public virtual bool IsInteger() => false;

    /// <summary>
    /// Is type an unsigned integer?
    /// </summary>
    public virtual bool IsUnsigned() => false;

    /// <summary>
    /// Is type a float?
    /// </summary>
    public virtual bool IsFloat() => false;

    /// <summary>
    /// Is type a boolean?
    /// </summary>
    public virtual bool IsBool() => false;

    /// <summary>
    /// Is type constant?
    /// </summary>
    public virtual bool IsConst() => false;

    /// <summary>
    /// Is type a function?
    /// </summary>
    public virtual bool IsFunction() => false;

    /// <summary>
    /// Is type a pointer?
    /// </summary>
    public virtual bool IsPointer() => false;

    /// <summary>
    /// Is type a string?
    /// </summary>
    public virtual bool IsString() => false;

    /// <summary>
    /// Can type be casted to another type?
    /// </summary>
    /// <param name="type">The type to be checked.</param>
    public virtual bool CanCastTo(Type type) => false;

    /// <summary>
    /// Is other type identical to this type?
    /// </summary>
    /// <param name="other">The type to be checked.</param>
    /// <returns></returns>
    public virtual bool IsIdentical(Type other) => false;

    /// <summary>
    /// Get size of the type.
    /// </summary>
    public virtual int GetSize() => 0;

    /// <summary>
    /// Get return type from function type only and check parameters types.
    /// </summary>
    /// <param name="parameters">The parameters to be checked.</param>
    public virtual Type GetReturnType(List<Type> parameters) => null;

    /// <summary>
    /// Remove the pointer from type.
    /// </summary>
    public virtual Type GetDerefedType() => null;

    /// <summary>
    /// Remove the constant from type.
    /// </summary>
    public virtual Type GetDeconstedType() => null;

    /// <summary>
    /// Get name of the type.
    /// </summary>
    public virtual string GetName() => "";
}