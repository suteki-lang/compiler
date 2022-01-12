#ifndef STANDARD_STRING_HPP
#define STANDARD_STRING_HPP

#include <standard/common.hpp>
#include <standard/system.hpp>
#include <standard/io.hpp>

// Get string length
extern uint64 string_length(string content);

// Are both strings equals?
extern bool string_equals(string a, string b, uint64 length = 0);

struct String
{
    int8   *content;
    uint64  length;
    uint64  capacity;

    // Initialize empty string
    String(void)
    {
        length   = 0;
        capacity = 256;
        content  = memory_allocate<int8>(capacity);
    }

    // Initialize string with value
    String(string value)
    {
        length   = 0;
        capacity = 256;

        uint64 value_length = string_length(value);

        while (capacity < value_length)
            capacity *= 2;

        content = memory_allocate<int8>(capacity);

        while (*value)
            content[length++] = *value++;
    }

    // Add string
    void operator+=(string value)
    {
        uint64 value_length = string_length(value);

        // Check for capacity
        while ((length + value_length) >= capacity)
        {
            // Increase capacity
            capacity *= 2;
            content   = memory_reallocate<int8>(content, capacity);
        }

        // Add value to content
        while (*value)
            content[length++] = *value++;
    }

    // Add String
    void operator+=(const String &other)
    {
        // Check for capacity
        while ((length + other.length) >= capacity)
        {
            // Increase capacity
            capacity *= 2;
            content   = memory_reallocate<int8>(content, capacity);
        }

        // Add string to content
        for (uint64 index  = 0; index < other.length; ++index)
            content[length++] = other.content[index];
    }

    // Add character
    void operator+=(int8 character)
    {
        // Check for capacity
        if ((length + 1) >= capacity)
        {
            // Increase capacity
            capacity *= 2;
            content   = memory_reallocate<int8>(content, capacity);
        }

        // Add character to content
        content[length++] = character;
    }

    // Compare with string
    bool operator==(string other)
    {
        return string_equals(content, other);
    }

    // Compare with String
    bool operator==(const String &other)
    {
        return (length == other.length && string_equals(content, other.content, length));
    }

    // Get string
    operator string(void)
    {
        *this += '\0';
        return content;
    }
};

#endif