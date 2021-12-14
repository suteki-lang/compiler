#include "input.hpp"
#include "utils.hpp"

// Constructor
Input::Input(std::string _path)
{
    tabs        = 0;
    path        = _path;
    source      = read_file(_path);
    logger.path = _path;
}

// Generate tabs
void Input::generate_tabs(void)
{
    for (uint32_t i = 0; i < tabs; ++i)
        source_output += '\t';
}

// Semantic analysis
bool Input::analyze(void)
{
    for (Node *node : nodes)
        node->analyze(this);

    return !logger.had_error;
}

// Generate C++ code
void Input::generate(void)
{
    tabs = 0;

    for (Node *node : nodes)
        node->generate(this);
}