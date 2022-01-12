#ifndef UTILS_HPP
#define UTILS_HPP

#include <string>
#include <fstream>
#include <sstream>

// Read file from path
// from: https://stackoverflow.com/a/116220
// note: edited a little to fit my style.
inline std::string read_file(std::string path)
{
    std::ostringstream buffer;
    std::ifstream      input(path);

    buffer << input.rdbuf();
    return buffer.str();
}

// Replace all
// from: https://stackoverflow.com/a/24315631
// note: edited a little to fit my style.
inline std::string replace_all(std::string source, std::string from, std::string to)
{
    size_t start_pos = 0;

    while ((start_pos = source.find(from, start_pos)) != std::string::npos) 
    {
        source.replace(start_pos, from.length(), to);
        start_pos += to.length();
    }

    return source;
}

#endif