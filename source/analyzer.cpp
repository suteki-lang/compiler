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

// Find module
static Input *find_module(std::string name)
{
    for (Input &input : g_inputs)
    {
        if (input.module_name == name)
            return &input;
    }

    return nullptr;
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
ExpressionKind NodeImport::analyze(Input *input)
{
    Input *module_input = find_module(name);

    if (!module_input)
        input->logger.error(start, "This module does not exists.");
    else
    {
        path = module_input->path;

        if (name == input->module_name)
            input->logger.error(start, "You can't import this module here.");
    }

    return ExpressionKind::Void;
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