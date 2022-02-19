#include <modules/global.hpp>
#include <modules/libc.hpp>
#include <modules/global.hpp>

string su_global_hello()
{
	return "hello!";
}

void *su_global_alloc()
{
	return nullptr;
}

int main()
{
	if (su_global_alloc() == nullptr)
	{
		puts("null");
		return 1;
	}
	else 
		return 69;
	if (1.23 == 1)
		puts("true");
	else if (false)
		puts("ok");
	else 
	{
		su_global_hello();
		puts("k");
	}
	puts(su_global_hello());
	return 0;
}