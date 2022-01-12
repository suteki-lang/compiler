#include <standard/system.hpp>

#include <stdlib.h>

// Entry point
int32 main(void)
{
    return static_cast<int32>(program_init());
}

// Exit the program
void program_exit(ExitCode code)
{
    exit(static_cast<int32>(code));
}

// Allocate heap memory
void *memory_allocate(uint64 size)
{
    void *memory = malloc(size);

    if (!memory)
        program_exit(ExitCode::MemoryAllocationError);

    return memory;
}

// Re-allocate heap memory
void *memory_reallocate(void *old, uint64 size)
{
    void *memory = realloc(old, size);

    if (!memory)
        program_exit(ExitCode::MemoryReAllocationError);

    return memory;
}

// Free heap memory
void memory_free(void *memory)
{
    free(memory);
}