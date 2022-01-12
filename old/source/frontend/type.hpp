#ifndef FRONTEND_TYPE_HPP
#define FRONTEND_TYPE_HPP

#include <standard/common.hpp>

enum struct ExpressionKind : uint32
{
    Void,
    Bool,
    Integer,
    Floating,
    String,
};

enum struct PrimitiveKind : uint32
{
    Void,
    Bool,
    String,

    UByte,
    UShort,
    UInt,
    ULong,
    
    Byte,
    Short,
    Int,
    Long,

    Single,
    Double,
};

struct Type
{
    PrimitiveKind kind;
};

#endif