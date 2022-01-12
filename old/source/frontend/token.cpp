#include <frontend/token.hpp>

// Get data as string
string Token::to_string(void)
{
    if (kind == TokenKind::Number)
        return std::to_string(std::get<double>(data)).c_str();

    return std::get<String>(data);
}