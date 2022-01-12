#ifndef FRONTEND_SCANNER_HPP
#define FRONTEND_SCANNER_HPP

#include <frontend/token.hpp>

struct Scanner
{
    uint32 start;
    uint32 end;
    uint32 line;
    uint32 column;
    Token  previous;
    Token  current;
    string source;

    // Advance the scanner
    char advance(void);

    // Skip whitespace
    void skip_whitespace(void);

    // Make token
    TokenKind make_token(TokenKind kind, TokenData data);
    TokenKind make_token(TokenKind kind);

    // Make error token
    TokenKind make_token(string message);

    // Make string token
    TokenKind make_string_token(void);

    // Make identifier token
    TokenKind make_identifier_token(void);

    // Make number
    TokenKind make_number_token(void);

    // Set the source
    void set(string source);

    // Scan token
    TokenKind scan(void);
};

#endif