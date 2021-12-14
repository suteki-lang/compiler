#ifndef TOKEN_HPP
#define TOKEN_HPP

#include <cstdint>
#include <variant>
#include <string>

enum class TokenKind : unsigned int
{
    Error,
    End,

    String,
    Number,
    Identifier,

    LeftParenthesis,
    RightParenthesis,
    LeftBrace,
    RightBrace,
    Comma,
    Semicolon,
    Dot,

    Export,
    Import,

    Return,
};

using TokenData = std::variant<double, std::string>;

struct Token
{
    TokenKind kind;
    uint32_t  line;
    uint32_t  column;
    TokenData data;

    // Get data as string
    std::string to_string(void);
};

#endif