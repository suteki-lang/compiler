#include <modules/global.hpp>
#include <modules/libc.hpp>

const char *su_global_hello()
{
	return "H";
}

int main()
{
	puts(su_global_hello());
	if (true)
		return 10;
	return 0;
}