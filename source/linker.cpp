#include "linker.hpp"
#include "utils.hpp"
#include "config.hpp"

#include <iostream>
#include <filesystem>
#include <fstream>
#include <algorithm>

namespace fs         = std::filesystem;
using     fs_options = std::filesystem::copy_options;

// Get path
static std::string get_path(std::string path)
{
    std::string name = fs::path(path).stem();

    // Format the path
    path = replace_all(path, "../", "");
    path = path.substr(0, path.length() - name.length() - 3);
    path = "user/" + path;

    // Make the directory
    std::string directory = g_output + path;

    if (!fs::exists(directory))
        fs::create_directories(directory);

    return (path + name);
}

// Link
bool link(void)
{
    // Write suteki files
    std::string suteki_path = g_output + "suteki";

    if (!fs::exists(suteki_path))
        fs::create_directories(suteki_path);

    fs::copy(g_runtime, suteki_path, fs_options::update_existing | fs_options::recursive);

    // Write files
    for (Input &input : g_inputs)
    {
        std::string path        = get_path(input.path);
        std::string output_path = g_output + path;

        // Make guard name
        std::string guard_name = replace_all(path,       "/", "_");
                    guard_name = replace_all(guard_name, ".", "_"); // In case the file is named like: source.extended.su
        
        // bruh C++ has no std::string::to_upper() 
        std::transform(guard_name.begin(), guard_name.end(), guard_name.begin(), ::toupper);
    
        // Write header file
        std::ofstream header(output_path + ".hpp");

        // Header: Add guard
        header << "#ifndef " + guard_name + "\n";
        header << "#define " + guard_name + "\n\n";

        // Header: Add namespace
        header << "namespace User\n{\n";
        header << input.header_output;
        header << "}\n";

        // Header: End guard
        header << "\n#endif";

        // Write source file
        std::ofstream source(output_path + ".cpp");

        // Source: Add include
        source << "#include <" + path + ".hpp>\n\n";

        // Source: Add namespace
        source << "namespace User\n{\n";
        source << input.source_output;
        source << "}";

        // Close files
        header.close();
        source.close();
    }

    return true;
}