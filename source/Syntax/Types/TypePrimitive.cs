namespace Suteki
{
    enum PrimitiveKind
    {
        Null,
        Void,
        Bool,
        String,

        UByte,
        UShort,
        UInt,
        ULong,
        UWord,
        SByte,
        SShort,
        SInt,
        SLong,
        SWord,

        Single,
        Double,
    }

    class TypePrimitive : Type
    {
        public override bool IsNull()
        {
            return (Kind == PrimitiveKind.Null);
        }

        public override bool IsVoid()
        {
            return (Kind == PrimitiveKind.Void);
        }

        public override bool IsBasic()
        {
            return (Kind != PrimitiveKind.Void && Kind != PrimitiveKind.Null);
        }

        public override bool IsInteger()
        {
            return (Kind >= PrimitiveKind.UByte && Kind <= PrimitiveKind.SWord);
        }

        public override bool IsUnsigned()
        {
            return (Kind >= PrimitiveKind.UByte && Kind <= PrimitiveKind.UWord);
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
            if (IsNull() && other.IsNull())
                return true;

            if (IsVoid() && other.IsVoid())
                return true;

            if (IsInteger() && other.IsInteger())
                return true;

            if (IsFloat() && other.IsFloat())
                return true;

            if (IsFloat() && other.IsInteger())
                return true;

            if (IsBool() && other.IsBool())
                return true;

            if (IsBool() && other.IsInteger())
                return true;

            if (IsString() && other.IsString())
                return true;

            if (other.IsBool() && !IsVoid() && !IsPointer() && !IsString())
                return true;

            // string == const byte *
            if (IsString() && other.IsPointer() &&
                other.GetDerefedType().IsConst() && 
                other.GetDerefedType().GetDeconstedType().Kind == PrimitiveKind.SByte)
                return true;

            return false;
        }

        public override int GetSize()
        {
            switch (Kind)
            {
                case PrimitiveKind.UByte:
                case PrimitiveKind.SByte:
                case PrimitiveKind.Bool:
                    return 1;

                case PrimitiveKind.UShort:
                case PrimitiveKind.SShort:
                    return 2;

                case PrimitiveKind.UInt:
                case PrimitiveKind.SInt:
                case PrimitiveKind.Single:
                    return 4;

                case PrimitiveKind.ULong:
                case PrimitiveKind.SLong:
                case PrimitiveKind.String:
                case PrimitiveKind.Double:
                case PrimitiveKind.UWord:
                case PrimitiveKind.SWord:
                case PrimitiveKind.Null:
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