#include "logger.hpp"

#include <iostream>

// Error message
void Logger::error(std::string message)
{
    had_error = true;

    std::cout << '['         << path;
    std::cout << "] Error: " << message << std::endl;
}

// Error at token
void Logger::error(Token &token, std::string message)
{
    had_error = true;

    std::cout << '['        << path << ':';
    std::cout << token.line << ':'  << token.column;
    std::cout << "] Error";

    if (token.kind == TokenKind::End)
        std::cout << " at end: ";
    else if (token.kind == TokenKind::Error)
        std::cout << ": ";
    else
        std::cout << " at '" << token.to_string() << "': ";

    std::cout << message << std::endl;
}