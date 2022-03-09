namespace Suteki.Compiler;

using System.Collections.Generic;

/// <summary>
/// A function type.
/// </summary>
public class TypeFunction : Type
{
    /// <summary>
    /// The function return type.
    /// </summary>
    public Type Return;

    /// <summary>
    /// All the function parameters types.
    /// </summary>
    public List<Type> Parameters = new List<Type>();

    /// <summary>
    /// Is type a function?
    /// </summary>
    public override bool IsFunction() => true;

    /// <summary>
    /// Is other type identical to this type?
    /// </summary>
    /// <param name="other">The type to be checked.</param>
    /// <returns></returns>
    public override bool IsIdentical(Type other)
    {
        // Make sure other type is a function
        if (!other.IsFunction())
            return false;

        // Get return type
        Type returnType = other.GetReturnType(Parameters);

        // Compare return types
        if (returnType != null)
            return Return.IsIdentical(returnType);
        else    
            return false;
    }

    /// <summary>
    /// Get size of the type.
    /// </summary>
    public override int GetSize() => Config.PointerSize;

    /// <summary>
    /// Get return type from function type only and check parameters types.
    /// </summary>
    /// <param name="parameters">The parameters to be checked.</param>
    public override Type GetReturnType(List<Type> parameters)
    {
        // Check for parameters count
        if (Parameters.Count != parameters.Count)
            return null;

        // Compare parameters types
        for (int index = 0; index < Parameters.Count; ++index)
        {
            if (!Parameters[index].IsIdentical(parameters[index]))
                return null;
        }

        return Return;
    }
}