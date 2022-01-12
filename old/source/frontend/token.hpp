#ifndef FRONTEND_TOKEN_HPP
#define FRONTEND_TOKEN_HPP

#include <variant>
#include <string>

#include <standard/common.hpp>
#include <standard/string.hpp>

enum struct TokenKind : uint32
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

using TokenData = std::variant<double, String>;

struct Token
{
    TokenKind kind;
    uint32    line;
    uint32    column;
    TokenData data;

    // Get data as string
    string to_string(void);
};

#endif