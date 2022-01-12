#ifndef FRONTEND_SYMBOL_HPP
#define FRONTEND_SYMBOL_HPP

#include <standard/common.hpp>

enum struct SymbolKind : uint32
{
    Function,
};

struct Symbol
{
    SymbolKind kind;
};

#endif