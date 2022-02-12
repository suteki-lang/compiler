#include <modules/global.hpp>
#include <modules/std.hpp>

void su_global_writeln(string message)
{
	puts(message);
}

int main()
{
	su_global_writeln("hello!");
	return 0;
}