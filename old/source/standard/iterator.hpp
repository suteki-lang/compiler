#ifndef STANDARD_ITERATOR_HPP 
#define STANDARD_ITERATOR_HPP

template <typename T> struct Iterator
{
    T *data;

    Iterator()
    {
        data = nullptr;
    }

    Iterator(T *content)
    {
        data = content;
    }

    Iterator(const Iterator &iterator)
    {
        data = iterator.data;
    }

    T &operator*()
    {
        return *data;
    }

    T *operator->()
    {
        return &(*data);
    }

    Iterator &operator++()
    {
        ++data;
        return *this;
    }

    Iterator &operator--()
    {
        --data;
        return *this;
    }

    bool operator==(const Iterator &other)
    {
        return (data == other.data);
    }

    bool operator!=(const Iterator &other)
    {
        return (data != other.data);
    }
};

#endif