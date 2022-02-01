namespace Suteki
{
    enum PrimitiveKind
    {
        Void,
        Bool,
        String,

        UByte,
        UShort,
        UInt,
        ULong,
        Byte,
        Short,
        Int,
        Long,

        Single,
        Double,
    }

    class TypePrimitive : Type
    {
        public override bool IsVoid()
        {
            return (Kind == PrimitiveKind.Void);
        }

        public override bool IsBasic()
        {
            return (Kind != PrimitiveKind.Void);
        }

        public override bool IsInteger()
        {
            return (Kind >= PrimitiveKind.UByte && Kind <= PrimitiveKind.Long);
        }

        public override bool IsUnsigned()
        {
            return (Kind >= PrimitiveKind.UByte && Kind <= PrimitiveKind.ULong);
        }

        public override bool IsFloat()
        {
            return (Kind == PrimitiveKind.Single || Kind == PrimitiveKind.Double);
        }

        public override bool IsBool()
        {
            return (Kind == PrimitiveKind.Bool);
        }

        public override bool IsString()
        {
            return (Kind == PrimitiveKind.String);
        }

        public override bool CanCastTo(Type type)
        {
            if (IsVoid())
                return false;

            return type.IsBasic();
        }

        public override bool IsIdentical(Type other)
        {
            if (IsVoid() && other.IsVoid())
                return true;

            if (IsInteger() && other.IsInteger())
                return true;

            if (IsFloat() && other.IsFloat())
                return true;

            if (IsBool() && other.IsBool())
                return true;

            if (IsString() && other.IsString())
                return true;

            // string == const byte *
            if (IsString() && other.IsPointer() &&
                other.GetDerefedType().IsConst() && 
                other.GetDerefedType().GetDeconstedType().Kind == PrimitiveKind.Byte)
                return true;

            return false;
        }

        public override int GetSize()
        {
            switch (Kind)
            {
                case PrimitiveKind.UByte:
                case PrimitiveKind.Byte:
                case PrimitiveKind.Bool:
                    return 1;

                case PrimitiveKind.UShort:
                case PrimitiveKind.Short:
                    return 2;

                case PrimitiveKind.UInt:
                case PrimitiveKind.Int:
                    return 4;

                case PrimitiveKind.ULong:
                case PrimitiveKind.Long:
                    return 8;

                case PrimitiveKind.String:
                    return 8;

                case PrimitiveKind.Single:
                    return 4;

                case PrimitiveKind.Double:
                    return 8;

                default:
                    return 0;
            }
        }

        public override string GetName()
        {
            return Kind.ToString().ToLower();
        }
    }
}