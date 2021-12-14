#include "parser.hpp"
#include "config.hpp"

#include <iostream>

// Error message
void Parser::error(std::string message)
{
    had_error = true;
    current_input->logger.error(message);
}

// Error at token
void Parser::error(Token &token, std::string message)
{
    had_error = true;
    current_input->logger.error(token, message);
}

// Advance scanner token
void Parser::advance(void)
{
    for (;;)
    {
        if (scanner.scan() != TokenKind::Error)
            return;

        error(scanner.current, scanner.current.to_string());
    }
}

// Match current token kind?
bool Parser::match(TokenKind expected)
{
    if (scanner.current.kind == expected)
    {
        advance();
        return true;
    }

    return false;
}

// Consume token
void Parser::consume(TokenKind expected, std::string message)
{
    if (match(expected))
        return;

    error(scanner.current, message);
}

// Parse expression
Node *Parser::parse_expression(void)
{
    if (match(TokenKind::Number))
        return new NodeNumber(std::get<double>(scanner.previous.data));

    return nullptr;
}

// Parse return statement
Node *Parser::parse_return(void)
{
    NodeReturn *node = new NodeReturn();
    node->start      = scanner.previous;

    if (match(TokenKind::Semicolon))
        node->value = nullptr;
    else
    {
        node->value = parse_expression();
        consume(TokenKind::Semicolon, "Expected ';' after return value.");
    }

    return node;
}

// Parse statement
Node *Parser::parse_statement(void)
{
    advance();

    switch (scanner.previous.kind)
    {
        case TokenKind::Return:
            return parse_return();

        default:
        {
            error(scanner.previous, "Expected statement.");
            return nullptr;
        }
    }
}

// Parse block of statements
Node *Parser::parse_block(void)
{
    NodeBlock *node  = new NodeBlock();
    Token      start = scanner.previous;

    if (!match(TokenKind::RightBrace))
    {
        while (!match(TokenKind::RightBrace) && !match(TokenKind::End))
            node->statements.push_back(parse_statement());

        if (scanner.previous.kind == TokenKind::End)
            error(start, "Expected '}' after function statement(s).");
    }

    return node;
}

// Parse type
Node *Parser::parse_type(void)
{
    std::string name = scanner.previous.to_string();

    if (types.find(name) != types.end())
    {
        Type &type = types[name];
        return new NodePrimitive(type.kind);
    }
    
    error(scanner.previous, "Expected type.");
    return nullptr;
}

// Parse function declaration
void Parser::parse_function_declaration(Node *type)
{
    NodeFunction *node = new NodeFunction();
    node->type         = type;

    consume(TokenKind::Identifier, "Expected function name.");
    node->name = scanner.previous;

    // Parse function parameters
    consume(TokenKind::LeftParenthesis, "Expected '(' after function name.");

    if (!match(TokenKind::RightParenthesis))
    {
        do
        {
            NodeParameter *parameter = new NodeParameter();

            // Parse parameter type
            advance();
            parameter->type = parse_type();

            // Parse parameter name
            consume(TokenKind::Identifier, "Expected function parameter name.");
            parameter->name = scanner.previous;

            // Add parameter
            node->parameters.push_back(parameter);
        }
        while (match(TokenKind::Comma));

        consume(TokenKind::RightParenthesis, "Expected ')' after function parameters.");
    }

    // Parse function block
    consume(TokenKind::LeftBrace, "Expected '{' after ')'.");
    node->block = parse_block();

    current_input->nodes.push_back(node);
}

// Parse declaration
void Parser::parse_declaration(void)
{
    advance();

    switch (scanner.previous.kind)
    {
        case TokenKind::Identifier:
        {
            Node *type = parse_type();

            if (type)
                parse_function_declaration(type);

            break;
        }

        default:
            error(scanner.previous, "Unexpected token.");
    }
}

// Parse tokens into AST nodes
bool Parser::parse(void)
{
    for (Input &input : g_inputs)
    {
        current_input = &input;

        scanner.set(input.source);
        advance();

        while (!match(TokenKind::End))
            parse_declaration();
    }

    return !had_error;
}

// Start parsing
bool Parser::start(void)
{
    // Initialize the types
    types["void"]   = { primitive_void   };
    types["bool"]   = { primitive_bool   };
    types["string"] = { primitive_string };

    types["ubyte"]  = { primitive_ubyte  };
    types["ushort"] = { primitive_ushort };
    types["uint"]   = { primitive_uint   };
    types["ulong"]  = { primitive_ulong  };
    types["byte"]   = { primitive_byte   };
    types["short"]  = { primitive_short  };
    types["int"]    = { primitive_int    };
    types["long"]   = { primitive_long   };

    types["single"] = { primitive_single };
    types["double"] = { primitive_double };

    // Initialize other parser stuff
    had_error = false;

    // Parse tokens into AST nodes
    if (!parse())
        return false;
    
    return true;
}