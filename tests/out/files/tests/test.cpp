#include <modules/global.hpp>
#include <modules/common.hpp>
#include <modules/libc.hpp>
#include <modules/global.hpp>

string su_global_test()
{
	return "private function test";
}

int main()
{
	puts(su_global_test());
	return 0;
}