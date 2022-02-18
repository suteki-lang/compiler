#include <modules/global.hpp>
#include <modules/global.hpp>

int su_global_exit_code(int code)
{
	return code;
}

int main()
{
	return su_global_exit_code(13);
}