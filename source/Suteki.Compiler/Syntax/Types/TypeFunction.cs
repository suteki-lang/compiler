namespace Suteki.Compiler;

using System.Collections.Generic;

public class TypeFunction : Type
{
    public List<Type> Parameters;
    public Type       Return;

    public override bool IsFunction() => true;

    public override bool CanCastTo(Type  type) => false;

    public override bool IsIdentical(Type other, bool isExpression = false)
    {
        // Make sure other type is a function
        if (!other.IsFunction())
            return false;

        // Get return type
        Type returnType = other.GetReturnType(Parameters);

        // Compare types
        if (returnType != null)
            return Return.IsIdentical(returnType);
        else    
            return false;
    }

    public override int GetSize() => Config.PointerSize;

    public override Type GetReturnType(List<Type> parameters)
    {
        // Check for parameter count
        if (Parameters.Count != parameters.Count)
            return null;

        // Compare parameter types
        for (int index = 0; index < Parameters.Count; ++index)
        {
            if (!Parameters[index].IsIdentical(parameters[index]))
                return null;
        }

        return Return;
    }
}