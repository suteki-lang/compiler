#include <frontend/logger.hpp>
#include <standard/io.hpp>

// Error message
void Logger::error(string message)
{
    had_error = true;
    write(ConsoleStream::Error, "[", path, "] Error: ", message, "\n");
}

// Error at token
void Logger::error(Token &token, string message)
{
    had_error = true;

    write(ConsoleStream::Error, "[", path, ":", token.line, ":", token.column, "] Error");

    if (token.kind == TokenKind::End)
        write(ConsoleStream::Error, " at end");
    else if (token.kind == TokenKind::Error)
        write(ConsoleStream::Error, ": ");
    else
        write(ConsoleStream::Error, " at '", token.to_string(), "': ");

    writeln(ConsoleStream::Error, message);
}