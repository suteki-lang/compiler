#ifndef SCANNER_HPP
#define SCANNER_HPP

#include "token.hpp"

struct Scanner
{
    uint32_t    start;
    uint32_t    end;
    uint32_t    line;
    uint32_t    column;
    Token       previous;
    Token       current;
    std::string source;

    // Advance the scanner
    char advance(void);

    // Skip whitespace
    void skip_whitespace(void);

    // Make token
    TokenKind make_token(TokenKind kind, TokenData data);
    TokenKind make_token(TokenKind kind);

    // Make error token
    TokenKind make_token(std::string message);

    // Make string token
    TokenKind make_string_token(void);

    // Make identifier token
    TokenKind make_identifier_token(void);

    // Make number
    TokenKind make_number_token(void);

    // Set the source
    void set(std::string source);

    // Scan token
    TokenKind scan(void);
};

#endif