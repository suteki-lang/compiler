#include <standard/io.hpp>

#include <stdio.h>

// TODO: Use POSIX or Linux syscalls.

// Get ConsoleStream as FILE
static FILE *get_stream(ConsoleStream stream)
{
    switch (stream)
    {
        case ConsoleStream::Input:
            return stdin;

        case ConsoleStream::Output:
            return stdout;

        case ConsoleStream::Error:
            return stderr;

        default:
            return nullptr;
    }
}

// Write Any to console
static void write(FILE *stream, const Any &value)
{
    switch (value.kind)
    {
        case AnyKind::Number:
        {
            fprintf(stream, "%f", value.as_number);
            break;
        }

        case AnyKind::String:
        {
            fprintf(stream, "%s", value.as_string);
            break;
        }
    }
}

// Write content to console
void write(ConsoleStream stream, const std::initializer_list<Any> &content)
{
    FILE *output_stream = get_stream(stream);
    
    for (const Any &value : content)
        write(output_stream, value);       
}

// Write content with line to console
void writeln(ConsoleStream stream, const std::initializer_list<Any> &content)
{
    FILE *output_stream = get_stream(stream);
    
    for (const Any &value : content)
        write(output_stream, value);      

    fprintf(output_stream, "\n");
}