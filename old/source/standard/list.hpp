#ifndef STANDARD_LIST_HPP
#define STANDARD_LIST_HPP

#include <standard/system.hpp>
#include <standard/common.hpp>
#include <standard/iterator.hpp>

template <typename T> struct List
{
    T      *content;
    uint64  count;
    uint64  capacity;

    // Initialize the list
    List(void)
    {
        count    = 0;
        capacity = 64;
        content  = memory_allocate<T>(capacity);
    }

    // Add item to list
    uint64 add(const T &item)
    {
        // Check for capacity
        while ((count + 1) >= capacity)
        {
            // Increase capacity
            capacity *= 2;
            content   = memory_reallocate<T>(content, capacity);
        }

        // Add item
        content[count++] = item;
        return (count - 1);
    }

    // Get item from list
    T &get(uint64 index)
    {
        // NOTE: Do bounds checking?
        return content[index];
    }

    // Iterator stuff
    Iterator<T> begin(void)
    {
        return Iterator<T>(content);
    }

    Iterator<T> end(void)
    {
        return Iterator<T>(content + count);
    }
};

#endif