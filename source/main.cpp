#include "compiler.hpp"
#include "config.hpp"

int main(int argc, char **argv)
{
    g_inputs.push_back({ "tests/example.su" });

    compile();
    return 0;
}