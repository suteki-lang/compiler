#ifndef SUTEKI_STRING_HPP
#define SUTEKI_STRING_HPP

#include <string.h>

// NOTE: this is temporary

struct string
{
    const char   *content;
    unsigned int  length;

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