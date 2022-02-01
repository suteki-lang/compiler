namespace Suteki
{
    class TypeConst : Type
    {
        public Type Type;

        public override bool IsVoid    () => Type.IsVoid();
        public override bool IsBasic   () => Type.IsBasic();
        public override bool IsInteger () => Type.IsInteger();
        public override bool IsUnsigned() => Type.IsUnsigned();
        public override bool IsFloat   () => Type.IsFloat();
        public override bool IsBool    () => Type.IsBool();
        public override bool IsConst   () => true;
        public override bool IsPointer () => Type.IsPointer();

        public override bool CanCastTo(Type type)
        {
            if (type.IsConst())
                return Type.CanCastTo(type.GetDeconstedType());
            else
                return Type.CanCastTo(type);
        }

        public override bool IsIdentical(Type other)
        {
            if (other.IsConst())
                return Type.IsIdentical(other.GetDeconstedType());
            else
                return Type.IsIdentical(other);
        }

        public override int GetSize() => Type.GetSize();

        public override Type GetDerefedType  () => Type.GetDerefedType();
        public override Type GetDeconstedType() => Type;

        public override string GetName() => $"c{Type.GetName()}";
    }
}