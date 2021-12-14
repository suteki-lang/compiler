#include "config.hpp"

// Get primitive type as string
static std::string primitive_as_string[] =
{
    "void",
    "bool",
    "std::string",

    "unsigned char",
    "unsigned short",
    "unsigned int",
    "unsigned long",

    "char",
    "short",
    "int",
    "long",

    "float",
    "double",
};

// Generate C++ code
void Node::generate(Input *input)
{
    
}

// To string
std::string Node::to_string(void)
{
    return "";
}

// Generate C++ code
void NodeBinary::generate(Input *input)
{
    
}

// Generate C++ code
void NodeUnary::generate(Input *input)
{
    
}

// Generate C++ code
void NodeNumber::generate(Input *input)
{
    long integer = value;

    if ((integer - value) == 0)
        input->source_output += std::to_string(integer);
    else
        input->source_output += std::to_string(value);
}

// To string
std::string NodePrimitive::to_string(void)
{
    return primitive_as_string[kind];
}

// Generate C++ code
void NodeImport::generate(Input *input)
{
    // Get path
    std::string module_path = "user/" + path.substr(0, path.length() - 3) + ".hpp";

    // Generate include
    input->includes += "#include <" + module_path + ">\n";
}

// Generate C++ code
void NodeFunction::generate(Input *input)
{
    int index = 0;

    // Generate function head
    std::string head;
    
    head += type->to_string() + ' ';
    head += name.to_string();
    head += '(';

    for (Node *parameter : parameters)
    {
        head += parameter->to_string();
        
        if ((++index) < parameters.size())
            head += ", ";
    }

    head += ')';
    
    // Generate header
    input->header_output += "\textern " + head + ";\n"; 

    // Generate source
    input->source_output += '\t' + head;
    input->source_output += "\t\n\t{\n";
    block->generate(input);
    input->source_output += "\t}\n";
}

// To string
std::string NodeParameter::to_string(void)
{
    return type->to_string() + ' ' + name.to_string();
}

// Generate C++ code
void NodeBlock::generate(Input *input)
{
    input->generate_tabs();
    ++input->tabs;

    for (Node *statement : statements)
    {
        input->generate_tabs();
        statement->generate(input);
    }

    --input->tabs;
}

// Generate C++ code
void NodeReturn::generate(Input *input)
{
    input->source_output += "\treturn";

    if (value)
    {
        input->source_output += ' ';
        value->generate(input);
    }

    input->source_output += ";\n";
}