#ifndef FILES_TESTS_STDC_HPP
#define FILES_TESTS_STDC_HPP

extern "C" int puts(const char *msg);
extern "C" void exit(int status);
extern "C" void *malloc(unsigned long size);
extern "C" void *realloc(void *ptr, unsigned long size);
extern "C" void free(void *ptr);

#endif