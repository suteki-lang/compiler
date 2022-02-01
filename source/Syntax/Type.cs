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

        // Constructor
        public Type(PrimitiveKind kind)
        {
            Kind = kind;
        }

        // Compare types
        public static bool Compare(ExpressionKind destination, ExpressionKind source)
        {
            if (source == ExpressionKind.Integer && destination == ExpressionKind.Float)
                return true;

            if (source == ExpressionKind.Bool && (destination != ExpressionKind.Void &&
                                                  destination != ExpressionKind.String))
                return true;

            return (source == destination);
        }

        // Compare types
        public static bool Compare(ExpressionKind destination, ExpressionKind source, bool isPointer)
        {
            if (source == ExpressionKind.Integer && destination == ExpressionKind.Float)
                return true;

            if (source == ExpressionKind.Bool && (destination != ExpressionKind.Void &&
                                                  destination != ExpressionKind.String))
                return true;

            // NOTE: does this make sense?
            if (destination == ExpressionKind.Integer && isPointer && source == ExpressionKind.String)
                return true;

            return (source == destination);
        }
    }
}