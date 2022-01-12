#ifndef STANDARD_SYSTEM_HPP
#define STANDARD_SYSTEM_HPP

#include <standard/information.hpp>
#include <standard/common.hpp>

// Initialize the program
extern ExitCode program_init(void);

// Exit the program
extern void program_exit(ExitCode code);

// TODO (ryaangu): Use virtual memory?

// Allocate heap memory
extern void *memory_allocate(uint64 size);

template <typename T> static inline T *memory_allocate(uint64 size)
{
    return reinterpret_cast<T *>(memory_allocate(size * sizeof(T)));
}

// Re-allocate heap memory
extern void *memory_reallocate(void *old, uint64 size);

template <typename T> static inline T *memory_reallocate(void *old, uint64 size)
{
    return reinterpret_cast<T *>(memory_reallocate(old, size * sizeof(T)));
}

// Free heap memory
extern void memory_free(void *memory);

#endif