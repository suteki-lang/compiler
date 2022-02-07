#ifndef SUTEKI_STRING_HPP
#define SUTEKI_STRING_HPP

#include <runtime/common.hpp>

// NOTE: this is temporary

struct string
{
    const char *content;
    uint32_t    length;

    // Default Constructor
    string()
    {
        content = nullptr;
        length  = 0;
    }
    
    // Constructor
    string(const char *_content)
    {
        content = _content;
        length  = (strlen(_content) - 1);
    }

    // Get content
    operator const char *()
    {
        return content;
    }
};

#endif