#include <modules/global.hpp>
#include <modules/std.hpp>

int binary()
{
	return (70 - 1);
}

int unary()
{
	return -1;
}

int main()
{
	unary();
	binary();
	return binary();
}