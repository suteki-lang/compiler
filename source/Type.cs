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

    enum ExpressionKind 
    {
        Void,
        Bool,
        Integer,
        Float,
        String,
    }

    class Type
    {
        public PrimitiveKind Kind;

        // Type Constructor
        public Type(PrimitiveKind kind)
        {
            Kind = kind;
        }

        // Compare types
        public static bool Compare(ExpressionKind destination, ExpressionKind source)
        {
            if (source == ExpressionKind.Integer && destination == ExpressionKind.Float)
                return true;

            return (source == destination);
        }
    }
}