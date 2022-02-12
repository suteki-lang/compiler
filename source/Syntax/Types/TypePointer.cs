namespace Suteki
{
    class TypePointer : Type
    {
        public Type Base;

        public override bool IsNull   () => Base.IsNull();
        public override bool IsBasic  () => true;
        public override bool IsPointer() => true;

        public override bool CanCastTo(Type type)
        {
            if (type.IsPointer() && Base.IsConst() && !type.GetDerefedType().IsConst())
                return false;

            return type.IsBasic();
        }

        public override bool IsIdentical(Type other)
        {
            // const byte * == string
            if (Base.IsConst() && (Base.GetDeconstedType().Kind == PrimitiveKind.SByte ||
                Base.GetDeconstedType().Kind == PrimitiveKind.UByte) &&
                other.IsString())
                return true;

            if (other.IsNull())
                return true;

            return other.IsPointer() && Base.IsIdentical(other.GetDerefedType());
        }

        public override int GetSize() => 8;

        public override Type GetDerefedType() => Base;
    }
}