#include <config.hpp>

#include <iostream>

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

// Type checking
ExpressionKind Node::typecheck(Input *input)
{
    return ExpressionKind::Void;
}

// Type checking
ExpressionKind NodeBinary::typecheck(Input *input)
{
    return ExpressionKind::Void;
}

// Type checking
ExpressionKind NodeUnary::typecheck(Input *input)
{
    return ExpressionKind::Void;
}

// Type checking
ExpressionKind NodeNumber::typecheck(Input *input)
{
    if ((static_cast<int64>(value) - value) == 0)
        return ExpressionKind::Integer;
    else
        return ExpressionKind::Floating;
}

// Type checking
ExpressionKind NodePrimitive::typecheck(Input *input)
{
    return primitive_as_kind[(int32)kind];
}

// Type checking
ExpressionKind NodeImport::typecheck(Input *input)
{
    return ExpressionKind::Void;
}

// Type checking
ExpressionKind NodeFunction::typecheck(Input *input)
{
    input->current_function = this;
    block->typecheck(input);

    return ExpressionKind::Void;
}

// Type checking
ExpressionKind NodeParameter::typecheck(Input *input)
{
    return type->typecheck(input);
}

// Type checking
ExpressionKind NodeBlock::typecheck(Input *input)
{
    for (Node *statement : statements)
        statement->typecheck(input);

    return ExpressionKind::Void;
}

// Type checking
ExpressionKind NodeReturn::typecheck(Input *input)
{
    ExpressionKind type          = ((!value) ? ExpressionKind::Void : value->typecheck(input));
    ExpressionKind function_type = input->current_function->type->typecheck(input);
    
    if (!compare(function_type, type))
        input->logger.error(start, "Return type does not match function return type.");

    return ExpressionKind::Void;
}