#ifndef FILES_TESTS_LIBC_HPP
#define FILES_TESTS_LIBC_HPP

#include <runtime/runtime.hpp>

extern "C" int puts(const char *str);
extern "C" void exit(int status);
extern "C" void *malloc(unsigned long size);
extern "C" void *realloc(void *ptr, unsigned long size);
extern "C" void free(void *ptr);

#endif