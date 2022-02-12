#include <modules/global.hpp>
#include <modules/stdc.hpp>

void su_global_writeln(string message)
{
	puts("Looping..");
	puts(message);
	su_global_writeln(message);
}

int main()
{
	su_global_writeln("hello!");
	return 0;
}