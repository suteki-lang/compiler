#ifndef FRONTEND_PARSER_HPP
#define FRONTEND_PARSER_HPP

#include <frontend/scanner.hpp>
#include <frontend/type.hpp>
#include <input.hpp>

#include <unordered_map>

struct Parser
{
    bool     had_error;
    Scanner  scanner;
    Input   *current_input;

    std::unordered_map<string, Type> types;

    // Error message
    void error(string message);

    // Error at token
    void error(Token &token, string message);

    // Advance scanner token
    void advance(void);

    // Match current token kind?
    bool match(TokenKind expected);

    // Consume token
    void consume(TokenKind expected, string message);

    // Parse expression
    Node *parse_expression(void);

    // Parse return statement
    Node *parse_return(void);

    // Parse statement
    Node *parse_statement(void);

    // Parse block of statements
    Node *parse_block(void);

    // Parse type
    Node *parse_type(void);

    // Parse function declaration
    void parse_function_declaration(Node *type);

    // Parse module name
    String parse_module_name(void);

    // Parse export
    void parse_export(void);

    // Parse import
    void parse_import(void);

    // Parse declaration
    void parse_declaration(void);

    // Parse tokens into AST nodes
    bool parse(void);

    // Start parsing
    bool start(void);
};

#endif