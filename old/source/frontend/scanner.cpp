#include <frontend/scanner.hpp>

#include <unordered_map>

// Keywords
static std::unordered_map<string, TokenKind> keywords =
{
    { "export", TokenKind::Export },
    { "import", TokenKind::Import },

    { "return", TokenKind::Return },
};

// Set the source
void Scanner::set(string _source)
{
    source = _source;
    start  = 0;
    end    = 0;
    line   = 1;
    column = 1;
}

// Is character an identifier?
static bool is_identifier(char character)
{
    return (character >= 'A' && character <= 'Z') || 
           (character >= 'a' && character <= 'z') ||
           (character == '_');
}

// Is character a number?
static bool is_number(char character)
{
    return (character >= '0' && character <= '9');
}

// Advance the scanner
char Scanner::advance(void)
{
    ++column;
    return source[end++];
}

// Skip whitespace
void Scanner::skip_whitespace(void)
{
    for (;;)
    {
        switch (source[end])
        {
            case '\n':
            {
                line   += 1;
                column  = 0;
            }

            case ' ':
            case '\t':
            case '\r':
            {
                advance();
                break;
            }

            default:
                return;
        }
    }
}

// Make token
TokenKind Scanner::make_token(TokenKind kind, TokenData data)
{
    current.kind   = kind;
    current.data   = data;
    current.line   = line;
    current.column = column;

    return kind;
}

TokenKind Scanner::make_token(TokenKind kind)
{
    current.kind   = kind;
    current.data   = std::string(source).substr(start, end - start).c_str();
    current.line   = line;
    current.column = column;

    return kind;
}

// Make error token
TokenKind Scanner::make_token(string message)
{
    current.kind   = TokenKind::Error;
    current.data   = message;
    current.line   = line;
    current.column = column;

    return TokenKind::Error;
}

// Make string token
TokenKind Scanner::make_string_token(void)
{
    while (source[end] != '"' && source[end] != '\0')
        advance();

    if (source[end] == '\0')
        return make_token("Unterminated string.");

    advance();
    return make_token(TokenKind::String);
}

// Make identifier token
TokenKind Scanner::make_identifier_token(void)
{
    while (is_identifier(source[end]) || is_number(source[end]))
        advance();

    // Find keyword
    std::string keyword = std::string(source).substr(start, end - start);

    if (keywords.find(keyword.c_str()) != keywords.end())
        return make_token(keywords[keyword.c_str()], keyword.c_str());

    return make_token(TokenKind::Identifier, keyword.c_str());
}

// Make number
TokenKind Scanner::make_number_token(void)
{
    while (is_number(source[end]))
        advance();

    if (source[end] == '.')
    {
        advance();

        while (is_number(source[end]))
            advance();
    }

    return make_token(TokenKind::Number, std::stod(source + start));
}

// Scan token
TokenKind Scanner::scan(void)
{
    skip_whitespace();

    start    = end;
    previous = current;

    if (!source[end])
        return make_token(TokenKind::End, "<EOF>");

    int8 character = advance();

    if (is_identifier(character))
        return make_identifier_token();

    if (is_number(character))
        return make_number_token();

    switch (character)
    {
        case '(':
            return make_token(TokenKind::LeftParenthesis);

        case ')':
            return make_token(TokenKind::RightParenthesis);

        case '{':
            return make_token(TokenKind::LeftBrace);

        case '}':
            return make_token(TokenKind::RightBrace);

        case ',':
            return make_token(TokenKind::Comma);

        case ';':
            return make_token(TokenKind::Semicolon);

        case '.':
            return make_token(TokenKind::Dot);

        case '"':
            return make_string_token();
    }

    return make_token("Unexpected character.");
}