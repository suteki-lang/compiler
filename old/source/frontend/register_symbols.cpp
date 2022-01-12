#include <config.hpp>

#include <iostream>

// Find module
static Input *find_module(String name)
{
    for (Input &input : g_inputs)
    {
        if (input.module_name == name)
            return &input;
    }

    return nullptr;
}

// Register symbols
void Node::register_symbols(Input *input)
{

}

// Register symbols
void NodeImport::register_symbols(Input *input)
{
    // Find module
    Input *mod = find_module(name);

    if (!mod)
        input->logger.error(start, "This module does not exists.");
    
    // Add module symbols
    mod->register_symbols();

    for (auto &pair : mod->globals)
    {
        if (input->globals.find(pair.first) != input->globals.end())
            mod->logger.error("SYMBOL DECLARED.....");

        input->globals[pair.first] = pair.second;
    }
}

// Register symbol
void NodeFunction::register_symbols(Input *input)
{
    // Check for function redeclaration
    if (input->globals.find(name.to_string()) != input->globals.end())
        input->logger.error(name, "This function was already declared.");

    // Add function symbol
    input->globals[name.to_string()] = { SymbolKind::Function };
}