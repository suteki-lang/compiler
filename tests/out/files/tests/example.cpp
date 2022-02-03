#include <modules/global.hpp>
#include <modules/std.hpp>

signed int main()
{
	puts("hi");
	malloc(1);
	free(nullptr);
}

signed long su_global_get_ptr_size()
{
	return 8;
}