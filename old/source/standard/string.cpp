#include <standard/string.hpp>

// Get string length
uint64 string_length(string content)
{
    string base = content;

    while (*content++);

    return (static_cast<uint64>(content - base) - 1);
}

// Are both strings equals?
bool string_equals(string a, string b, uint64 length)
{
    // Get length
    if (!length)
    {
        length = string_length(a);

        if (length != string_length(b))
            return false;
    }

    // Compare every character
    for (uint64 index = 0; index < length; ++index)
    {
        if (a[index] != b[index])
            return false;
    }

    return true;
}