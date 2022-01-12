#ifndef INPUT_HPP
#define INPUT_HPP

#include <string>

#include <frontend/symbol.hpp>
#include <frontend/ast.hpp>
#include <frontend/logger.hpp>
#include <standard/list.hpp>

#include <unordered_map>
#include <vector>

struct Input
{
    bool   registered_symbols;
    uint32 tabs;
    Logger logger;

    NodeFunction *current_function;

    std::string path;
    std::string source;
    std::string header_output;
    std::string source_output;
    std::string includes;
    String module_name;

    List<Node *> nodes;

    std::unordered_map<std::string, Symbol> globals;
    
    // Constructor
    Input(std::string _path);

    // Generate tabs
    void generate_tabs(void);

    // Register symbols
    void register_symbols(void);

    // Semantic analysis
    bool analyze(void);

    // Generate C++ code
    void generate(void);
};

#endif