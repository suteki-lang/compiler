#ifndef FRONTEND_LOGGER_HPP
#define FRONTEND_LOGGER_HPP

#include <frontend/token.hpp>

struct Logger
{
    bool   had_error;
    string path;

    // Constructor
    Logger(void)
    {
        path      = "";
        had_error = false;
    }

    // Error message
    void error(string message);

    // Error at token
    void error(Token &token, string message);
};

#endif