namespace Suteki
{
    class Type
    {
        public PrimitiveKind Kind;

        public virtual bool IsVoid    () => false;
        public virtual bool IsBasic   () => false;
        public virtual bool IsInteger () => false;
        public virtual bool IsUnsigned() => false;
        public virtual bool IsFloat   () => false;
        public virtual bool IsBool    () => false;
        public virtual bool IsConst   () => false;
        public virtual bool IsPointer () => false;
        public virtual bool IsString  () => false;

        public virtual bool CanCastTo  (Type  type) => false;
        public virtual bool IsIdentical(Type other) => false;

        public virtual int GetSize() => 0;

        public virtual Type GetDerefedType  () => null;
        public virtual Type GetDeconstedType() => null;

        public virtual string GetName() => "";
    }
}