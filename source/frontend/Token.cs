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
        public object    Data;
        public int       Line;
        public int       Column;
    }
}