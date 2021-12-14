#include "token.hpp"

// Get data as string
std::string Token::to_string(void)
{
    if (kind == TokenKind::Number)
        return std::to_string(std::get<double>(data));

    return std::get<std::string>(data);
}