namespace Suteki.Compiler;

using System.Collections.Generic;

public class Type
{
    public PrimitiveKind Kind;

    public virtual bool IsNull     () => false;
    public virtual bool IsVoid     () => false;
    public virtual bool IsBasic    () => false;
    public virtual bool IsInteger  () => false;
    public virtual bool IsUnsigned () => false;
    public virtual bool IsFloat    () => false;
    public virtual bool IsBool     () => false;
    public virtual bool IsConst    () => false;
    public virtual bool IsFunction () => false;
    public virtual bool IsPointer  () => false;
    public virtual bool IsString   () => false;

    public virtual bool CanCastTo  (Type  type)                            => false;
    public virtual bool IsIdentical(Type other, bool isExpression = false) => false;

    public virtual int GetSize() => 0;

    public virtual Type GetReturnType   (List<Type> parameters) => null;
    public virtual Type GetDerefedType  ()                      => null;
    public virtual Type GetDeconstedType()                      => null;
}