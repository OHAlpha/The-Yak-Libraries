using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAlphaCollections
{
    public interface List<T> : Collection<T>
    {

        T GetAt(int i);

        T SetAt(int i, T e);

        bool AddAt(T e, int i);

        int IndexOf(T e);

        int LastIndexOf(T e);

        T RemoveAt(int i);

        ListIterator<T> IterateList();

        ListIterator<T> IterateListAt(int i);

        bool AddAllAt(Collection<T> es, int i);

    }

    public interface ListIterator<T> : Iterator<T>
    {

        bool HasPrevious();

        T Previous();

        int nextIndex();

        int previousIndex();

    }
}
