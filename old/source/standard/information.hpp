#ifndef STANDARD_INFORMATION_HPP
#define STANDARD_INFORMATION_HPP

/*
    this file will have informations about:

    - which compiler we're using
    - which os we're compiling
    - exit codes
    - debug mode?
    
    etc.
*/

/*
    This macro is only defined if the program
    is in debug mode.
*/
#define DEBUG_MODE

#ifdef  DEBUG_MODE
#define DEBUG_WRITE(content) writeln("Debug >> ", content);
#elif
#define DEBUG_WRITE(content)
#endif

enum struct ExitCode
{
    /*
        These errors can be throw by anything.
    */
    Success,
    Failure,

    /*
        These errors can be throw by:

        system.hpp/
            memory_allocate
            memory_reallocate
    */
    MemoryAllocationError,
    MemoryReAllocationError,
};

#endif