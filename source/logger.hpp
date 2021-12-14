#ifndef LOGGER_HPP
#define LOGGER_HPP

#include "token.hpp"

#include <string>

struct Logger
{
    bool        had_error;
    std::string path;

    // Constructor
    Logger(void)
    {
        path      = "";
        had_error = false;
    }

    // Error message
    void error(std::string message);

    // Error at token
    void error(Token &token, std::string message);
};

#endif