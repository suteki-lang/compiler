#ifndef FRONTEND_AST_HPP
#define FRONTEND_AST_HPP

#include <standard/common.hpp>
#include <standard/list.hpp>
#include <frontend/token.hpp>
#include <frontend/type.hpp>

// Forward declare the Input structure.
struct Input;

enum OperatorKind : uint32
{
    
};

struct Node
{
    // Register symbols
    virtual void register_symbols(Input *input);

    // Check symbols
    virtual void check_symbols(Input *input);

    // Type checking
    virtual ExpressionKind typecheck(Input *input);

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

    // Type checking
    ExpressionKind typecheck(Input *input);

    // Generate C++ code
    void generate(Input *input);
};

struct NodeUnary : Node
{
    Node         *operand;
    OperatorKind  kind; 

    // Type checking
    ExpressionKind typecheck(Input *input);

    // Generate C++ code
    void generate(Input *input);
};

struct NodeNumber : Node
{
    float64 value;

    // Constructor
    NodeNumber(double value) : value(value) {}

    // Type checking
    ExpressionKind typecheck(Input *input);

    // Generate C++ code
    void generate(Input *input);
};

struct NodePrimitive : Node
{
    PrimitiveKind kind;

    // Constructor
    NodePrimitive(PrimitiveKind kind) : kind(kind) {}

    // Type checking
    ExpressionKind typecheck(Input *input);

    // To string
    std::string to_string(void);
};

struct NodeImport : Node
{
    String name;
    String path;
    Token  start;

    // Register symbols
    void register_symbols(Input *input);

    // Type checking
    ExpressionKind typecheck(Input *input);

    // Generate C++ code
    void generate(Input *input);
};

struct NodeFunction : Node
{
    Node         *type;
    Node         *block;
    Token         name;
    List<Node *>  parameters;

    // Register symbols
    void register_symbols(Input *input);

    // Type checking
    ExpressionKind typecheck(Input *input);

    // Generate C++ code
    void generate(Input *input);
};

struct NodeParameter : Node
{
    Node  *type;
    Token  name;

    // Type checking
    ExpressionKind typecheck(Input *input);

    // To string
    std::string to_string(void);
};

struct NodeBlock : Node
{
    List<Node *> statements;

    // Type checking
    ExpressionKind typecheck(Input *input);

    // Generate C++ code
    void generate(Input *input);
};

struct NodeReturn : Node
{
    Node  *value;
    Token  start;

    // Type checking
    ExpressionKind typecheck(Input *input);

    // Generate C++ code
    void generate(Input *input);
};

#endif