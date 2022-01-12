#include <compiler.hpp>
#include <config.hpp>

#include <standard/system.hpp>
#include <standard/string.hpp>

ExitCode program_init(void)
{
    g_inputs.push_back({ "tests/example.su" });
    g_inputs.push_back({ "tests/a.su"       });

    compile();
    return ExitCode::Success;
}