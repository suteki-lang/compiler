#include "compiler.hpp"
#include "parser.hpp"
#include "linker.hpp"
#include "config.hpp"

// Compile
bool compile(void)
{
    Parser parser;
    bool   had_error = false;

    // Parse source code
    if (!parser.start())
        return false;

    for (Input &input : g_inputs)
    {
        // Semantic analysis
        if (!input.analyze())
        {
            had_error = true;
            continue;
        }

        // Generate C++
        input.generate();
    }

    // An error happened?
    if (had_error)
        return false;

    // Link code
    if (!link())
        return false;

    return true;
}