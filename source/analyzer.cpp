#include "config.hpp"

static ExpressionKind primitive_as_kind[] =
{
    ExpressionKind::Void,
    ExpressionKind::Bool,
    ExpressionKind::String,

    ExpressionKind::Integer,
    ExpressionKind::Integer,
    ExpressionKind::Integer,
    ExpressionKind::Integer,
    ExpressionKind::Integer,
    ExpressionKind::Integer,
    ExpressionKind::Integer,
    ExpressionKind::Integer,

    ExpressionKind::Floating,
    ExpressionKind::Floating,
};

// Compare types
static bool compare(ExpressionKind destination, ExpressionKind source)
{
    if (source == ExpressionKind::Integer && destination == ExpressionKind::Floating)
        return true;

    return (source == destination);
}

// Analyze
ExpressionKind Node::analyze(Input *input)
{
    return ExpressionKind::Void;
}

// Analyze
ExpressionKind NodeBinary::analyze(Input *input)
{
    return ExpressionKind::Void;
}

// Analyze
ExpressionKind NodeUnary::analyze(Input *input)
{
    return ExpressionKind::Void;
}

// Analyze
ExpressionKind NodeNumber::analyze(Input *input)
{
    if ((static_cast<long>(value) - value) == 0)
        return ExpressionKind::Integer;
    else
        return ExpressionKind::Floating;
}

// Analyze
ExpressionKind NodePrimitive::analyze(Input *input)
{
    return primitive_as_kind[kind];
}

// Analyze
ExpressionKind NodeFunction::analyze(Input *input)
{
    input->current_function = this;
    block->analyze(input);

    return ExpressionKind::Void;
}

// Analyze
ExpressionKind NodeParameter::analyze(Input *input)
{
    return type->analyze(input);
}

// Analyze
ExpressionKind NodeBlock::analyze(Input *input)
{
    for (Node *statement : statements)
        statement->analyze(input);

    return ExpressionKind::Void;
}

// Analyze
ExpressionKind NodeReturn::analyze(Input *input)
{
    ExpressionKind type          = ((!value) ? ExpressionKind::Void : value->analyze(input));
    ExpressionKind function_type = input->current_function->type->analyze(input);
    
    if (!compare(function_type, type))
        input->logger.error(start, "Return type does not match function return type.");

    return ExpressionKind::Void;
}