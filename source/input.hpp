#ifndef INPUT_HPP
#define INPUT_HPP

#include <string>
#include <vector>

#include "ast.hpp"
#include "logger.hpp"

struct Input
{
    uint32_t                  tabs;
    Logger                    logger;
    NodeFunction             *current_function;
    std::string               path;
    std::string               source;
    std::string               header_output;
    std::string               source_output;
    std::string               includes;
    std::string               module_name;
    std::vector<Node *>       nodes;
    
    // Constructor
    Input(std::string _path);

    // Generate tabs
    void generate_tabs(void);

    // Semantic analysis
    bool analyze(void);

    // Generate C++ code
    void generate(void);
};

#endif