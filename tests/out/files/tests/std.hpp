#ifndef FILES_TESTS_STD_HPP
#define FILES_TESTS_STD_HPP

extern "C" signed int puts(const signed char *msg);
extern "C" void exit(signed int status);
extern "C" void *malloc(signed long size);
extern "C" void *realloc(void *ptr, signed long size);
extern "C" void free(void *ptr);

#endif