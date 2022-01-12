#ifndef STANDARD_IO_HPP
#define STANDARD_IO_HPP

#include <standard/any.hpp>
#include <initializer_list>

enum struct ConsoleStream : uint32
{
    Input,
    Output,
    Error,
};

// Write content to console
extern void write(ConsoleStream stream, const std::initializer_list<Any> &content);

template <typename... Ts> static inline void write(ConsoleStream stream, Ts... content)
{
    write(stream, { content... });
}

// Write content with line to console
extern void writeln(ConsoleStream stream, const std::initializer_list<Any> &content);

template <typename... Ts> static inline void writeln(ConsoleStream stream, Ts... content)
{
    writeln(stream, { content... });
}

#endif