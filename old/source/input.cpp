#include <input.hpp>
#include <utils.hpp>

// Constructor
Input::Input(std::string _path)
{
    tabs        = 0;
    path        = _path;
    source      = read_file(_path);
    logger.path = _path.c_str();
}

// Generate tabs
void Input::generate_tabs(void)
{
    for (uint32 i = 0; i < tabs; ++i)
        source_output += '\t';
}

// Register symbols
void Input::register_symbols(void)
{
    if (!registered_symbols)
    {
        for (Node *node : nodes)
            node->register_symbols(this);

        registered_symbols = true;
    }
}

// Semantic analysis
bool Input::analyze(void)
{
    // Register symbols
    register_symbols();

    // Check symbols
    for (Node *node : nodes)
        node->check_symbols(this);

    // Type checking
    for (Node *node : nodes)
        node->typecheck(this);

    return !logger.had_error;
}

// Generate C++ code
void Input::generate(void)
{
    tabs = 0;

    for (Node *node : nodes)
        node->generate(this);
}