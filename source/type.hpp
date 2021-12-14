#ifndef TYPE_HPP
#define TYPE_HPP

enum class ExpressionKind : unsigned int
{
    Void,
    Bool,
    Integer,
    Floating,
    String,
};

enum PrimitiveKind : unsigned int
{
    primitive_void,
    primitive_bool,
    primitive_string,

    primitive_ubyte,
    primitive_ushort,
    primitive_uint,
    primitive_ulong,
    primitive_byte,
    primitive_short,
    primitive_int,
    primitive_long,

    primitive_single,
    primitive_double,
};

struct Type
{
    PrimitiveKind kind;
};

#endif