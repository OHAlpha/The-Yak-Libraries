using System;

namespace OAlphaCollections
{
    public interface Collection<T>
    {

        void Add(T e);

        int Size();

        bool IsEmpty();

        bool Contains(T e);

        bool Remove(T e);

        void Clear();

        bool AddAll(Collection<T> es);

        bool RemoveAll(Collection<T> es);

        bool RetainAll(Collection<T> es);

        Iterator<T> Iterate();

        T[] ToArray();

        U[] ToArray<U>(ElementConverter<T,U> c);

    }

    public interface Iterator<T>
    {

        bool HasNext();

        T Next();

    }

}
