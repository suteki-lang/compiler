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

        LeftParenthesis,
        RightParenthesis,
        LeftBrace,
        RightBrace,
        Comma,
        Semicolon,
        Dot,

        Export,
        Import,

        True,
        False,

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