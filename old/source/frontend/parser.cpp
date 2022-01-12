#include <frontend/parser.hpp>
#include <config.hpp>

#include <iostream>

// Error message
void Parser::error(string message)
{
    had_error = true;
    current_input->logger.error(message);
}

// Error at token
void Parser::error(Token &token, string message)
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
void Parser::consume(TokenKind expected, string message)
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

    error(scanner.current, "Unexpected token.");
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
            error(scanner.previous, "Unexpected token.");
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
            node->statements.add(parse_statement());

        if (scanner.previous.kind == TokenKind::End)
            error(start, "Expected '}' after function statement(s).");
    }

    return node;
}

// Parse type
Node *Parser::parse_type(void)
{
    std::string name = scanner.previous.to_string();

    if (types.find(name.c_str()) != types.end())
    {
        Type &type = types[name.c_str()];
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
            node->parameters.add(parameter);
        }
        while (match(TokenKind::Comma));

        consume(TokenKind::RightParenthesis, "Expected ')' after function parameters.");
    }

    // Parse function block
    consume(TokenKind::LeftBrace, "Expected '{' after ')'.");
    node->block = parse_block();

    current_input->nodes.add(node);
}

// Parse module name
String Parser::parse_module_name(void)
{
    String name = "";

    for (;;)
    {
        if (match(TokenKind::Identifier))
            name += scanner.previous.to_string();
        else
        {
            scanner.current.data = name;
            error(scanner.current, "Invalid module name.");
        }

        if (!match(TokenKind::Dot))
            break;

        name += '.';
    }

    return name;
}

// Parse export
void Parser::parse_export(void)
{
    String module_name = parse_module_name();
    consume(TokenKind::Semicolon, "Expected ';' after module name.");

    // Set input module name
    current_input->module_name = module_name;
}

// Parse import
void Parser::parse_import(void)
{
    NodeImport *node = new NodeImport();
    node->start      = scanner.current;
    node->name       = parse_module_name();
    node->start.data = node->name;
    
    consume(TokenKind::Semicolon, "Expected ';' after module name.");
    current_input->nodes.add(node);
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

        case TokenKind::Export:
        {
            parse_export();
            break;
        }

        case TokenKind::Import:
        {
            parse_import();
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

        scanner.set(input.source.c_str());
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
    types["void"]   = { PrimitiveKind::Void   };
    types["bool"]   = { PrimitiveKind::Bool   };
    types["string"] = { PrimitiveKind::String };

    types["ubyte"]  = { PrimitiveKind::UByte  };
    types["ushort"] = { PrimitiveKind::UShort };
    types["uint"]   = { PrimitiveKind::UInt   };
    types["ulong"]  = { PrimitiveKind::ULong  };

    types["byte"]   = { PrimitiveKind::Byte  };
    types["short"]  = { PrimitiveKind::Short };
    types["int"]    = { PrimitiveKind::Int   };
    types["long"]   = { PrimitiveKind::Long  };

    types["single"] = { PrimitiveKind::Single };
    types["double"] = { PrimitiveKind::Double };

    // Initialize other parser stuff
    had_error = false;

    // Parse tokens into AST nodes
    if (!parse())
        return false;
    
    return true;
}