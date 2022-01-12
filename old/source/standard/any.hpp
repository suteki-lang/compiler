#ifndef STANDARD_ANY_HPP
#define STANDARD_ANY_HPP

#include <standard/common.hpp>

enum struct AnyKind : uint32
{
    Number,
    String,
};

struct Any
{
    union
    {
        float64 as_number;
        string  as_string;
    };

    AnyKind kind;

    // Number Constructor
    Any(float64 value)
    {
        as_number = value;
        kind      = AnyKind::Number;
    }

    // String Constructor
    Any(string value)
    {
        as_string = value;
        kind      = AnyKind::String;
    }

    // Get value as number
    operator float64(void)
    {
        return as_number;
    }

    // Get value as string
    operator string(void)
    {
        return as_string;
    }
};

#endif