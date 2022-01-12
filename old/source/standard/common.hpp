#ifndef STANDARD_COMMON_HPP
#define STANDARD_COMMON_HPP

#include <standard/information.hpp>

/*
    TYPE DEFINITIONS

    Some compilers will use 'long long' to make a 64-bit integer
    and some will use 'long', so that's why I'm doing these type
    definitions below.

    Float types don't need that, but I'm also doing it to fit the styling.

    String is just so it's short to write instead of writing 'const char *'.

    TODO (ryaangu): check for compiler and use 'long long' or 'long'.
*/

using uint8  = unsigned char;
using uint16 = unsigned short;
using uint32 = unsigned int;
using uint64 = unsigned long;

using int8  = char;
using int16 = short;
using int32 = int;
using int64 = long;

using float32 = float;
using float64 = double;

using string = const char *;

#endif