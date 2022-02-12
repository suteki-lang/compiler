namespace Suteki
{
    enum TokenKind
    {
        Error,
        End,

        Identifier,
        Integer,
        Float,
        String,
        Bool,
        Null,

        LeftParenthesis,
        RightParenthesis,
        LeftBrace,
        RightBrace,
        Comma,
        Semicolon,
        Dot,
        Star,
        Plus,
        Minus,
        Slash,

        Module,
        Import,
        
        Extern,

        Const,

        Return,
    }

    class Token
    {
        public TokenKind Kind;
        public string    Content;
        public int       Line;
        public int       Column;

        // Create token with same line information and kind but different content
        public static Token From(Token previous, string content)
        {
            Token token = new Token();

            token.Kind    = previous.Kind;
            token.Content = content;
            token.Line    = previous.Line;
            token.Column  = previous.Column;

            return token;
        }

        // Convert TokenKind into OperatorKind
        public static OperatorKind ToOperatorKind(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Plus:
                    return OperatorKind.Add;

                case TokenKind.Minus:
                    return OperatorKind.Subtract;

                case TokenKind.Slash:
                    return OperatorKind.Divide;

                case TokenKind.Star:
                    return OperatorKind.Multiply;
                
                default:
                    return OperatorKind.None;
            }
        }
    }
}