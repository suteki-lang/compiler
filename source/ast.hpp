#ifndef AST_HPP
#define AST_HPP

#include "token.hpp"
#include "type.hpp"

// Forward declare the Input structure.
struct Input;

enum OperatorKind : unsigned int
{
    
};

struct Node
{
    // Analyze
    virtual ExpressionKind analyze(Input *input);

    // Generate C++ code
    virtual void generate(Input *input);

    // To string
    virtual std::string to_string(void);
};

struct NodeBinary : Node
{
    Node         *left;
    Node         *right;
    OperatorKind  kind;

    // Analyze
    ExpressionKind analyze(Input *input);

    // Generate C++ code
    void generate(Input *input);
};

struct NodeUnary : Node
{
    Node         *operand;
    OperatorKind  kind; 

    // Analyze
    ExpressionKind analyze(Input *input);

    // Generate C++ code
    void generate(Input *input);
};

struct NodeNumber : Node
{
    double value;

    // Constructor
    NodeNumber(double value) : value(value) {}

    // Analyze
    ExpressionKind analyze(Input *input);

    // Generate C++ code
    void generate(Input *input);
};

struct NodePrimitive : Node
{
    PrimitiveKind kind;

    // Constructor
    NodePrimitive(PrimitiveKind kind) : kind(kind) {}

    // Analyze
    ExpressionKind analyze(Input *input);

    // To string
    std::string to_string(void);
};

struct NodeImport : Node
{
    std::string name;
    std::string path;
    Token       start;

    // Analyze
    ExpressionKind analyze(Input *input);

    // Generate C++ code
    void generate(Input *input);
};

struct NodeFunction : Node
{
    Node                *type;
    Node                *block;
    Token                name;
    std::vector<Node *>  parameters;

    // Analyze
    ExpressionKind analyze(Input *input);

    // Generate C++ code
    void generate(Input *input);
};

struct NodeParameter : Node
{
    Node  *type;
    Token  name;

    // Analyze
    ExpressionKind analyze(Input *input);

    // To string
    std::string to_string(void);
};

struct NodeBlock : Node
{
    std::vector<Node *> statements;

    // Analyze
    ExpressionKind analyze(Input *input);

    // Generate C++ code
    void generate(Input *input);
};

struct NodeReturn : Node
{
    Node  *value;
    Token  start;

    // Analyze
    ExpressionKind analyze(Input *input);

    // Generate C++ code
    void generate(Input *input);
};

#endif