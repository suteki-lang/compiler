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

        LeftParenthesis,
        RightParenthesis,
        LeftBrace,
        RightBrace,
        Comma,
        Semicolon,
        Dot,

        Export,
        Import,
        
        Extern,

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
    }
}