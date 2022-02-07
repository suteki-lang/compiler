#include <modules/global.hpp>
#include <modules/std.hpp>

string su_global_h()
{
	return "hello";
}

int main()
{
	su_global_h();
	puts("test");
}