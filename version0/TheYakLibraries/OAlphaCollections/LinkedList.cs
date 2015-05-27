using System;

namespace OAlphaCollections
{

    public class SinglyLinkedList<T> : List<T>
    {

        private int size = 0;

        private SingleLink<T> head = null, tail = null;

        public void Clear()
        {
            if (head == null)
                return;
            tail = null;
            while (head != null)
            {
                SingleLink<T> t = head;
                head = head.GetNext();
                t.SetValue(default(T));
                t.SetNext(null);
            }
        }

        public void Add(T e)
        {
            if (head == null)
            {
                head = tail = new SingleLink<T>();
                head.SetValue(e);
                size++;
            }
            else
            {
                tail = tail.SetNewNext(e);
                size++;
            }
        }

        public int Size()
        {
            return size;
        }

        public bool IsEmpty()
        {
            return size == 0;
        }

        public bool Contains(T e)
        {
            return IndexOf(e) > -1;
        }

        public bool Remove(T e)
        {
            if (head == null)
                return false;
            else
            {
                int i = IndexOf(e);
                if (i < -1)
                {
                    RemoveAt(i);
                    return true;
                }
                else
                    return false;
            }
        }

        public bool AddAll(Collection<T> es)
        {
            if (es.IsEmpty())
                return false;
            for (Iterator<T> it = es.Iterate(); it.HasNext(); )
                Add(it.Next());
            return true;
        }

        public bool RemoveAll(Collection<T> es)
        {
            if (es.IsEmpty())
                return false;
            int r = 0;
            for (Iterator<T> it = es.Iterate(); it.HasNext(); )
            {
                T e = it.Next();
                int i = IndexOf(e);
                if (i > -1)
                {
                    r++;
                    RemoveAt(i);
                }
            }
            return r > 0;
        }

        public bool RetainAll(Collection<T> es)
        {
            int r = 0;
            SingleLink<T> p = null, c = head;
            while (c != null)
            {
                if (es.Contains(c.GetValue()))
                {
                    if (p == null)
                        head = head.GetNext();
                    else
                        p.SetNext(c.GetNext());
                    size--;
                    r++;
                }
                p = c;
                c = c.GetNext();
            }
            return r > 0;
        }

        public Iterator<T> Iterate()
        {
            return IterateListAt(0);
        }

        public T SetAt(int i, T e)
        {
            if (i >= size || i < 0)
                return default(T);
            SingleLink<T> c = head;
            for (int j = 0; j < i; j++)
                c = c.GetNext();
            T o = c.GetValue();
            c.SetValue(e);
            return o;
        }

        public T GetAt(int i)
        {
            if (i >= size || i < 0)
                return default(T);
            SingleLink<T> c = head;
            for (int j = 0; j < i; j++)
                c = c.GetNext();
            return c.GetValue();
        }

        public bool AddAt(T e, int i)
        {
            if (i > size)
                return false;
            if (i == size)
            {
                Add(e);
                return true;
            }
            SingleLink<T> c = head;
            for (int j = 1; j < i; j++)
                c = c.GetNext();
            SingleLink<T> n = c.GetNext();
            SingleLink<T> t = c.SetNewNext(e);
            t.SetNext(n);
            return true;
        }

        public int IndexOf(T e)
        {
            if (size == 0)
                return -1;
            int i = 0;
            SingleLink<T> c = head;
            while (head != null)
            {
                if (e == null && c.GetValue() == null || e.Equals(c.GetValue()))
                    return i;
                c = c.GetNext();
            }
            return -1;
        }

        public int LastIndexOf(T e)
        {
            if (size == 0)
                return -1;
            int m = -1;
            int i = 0;
            SingleLink<T> c = head;
            while (head != null)
            {
                if (e == null && c.GetValue() == null || e.Equals(c.GetValue()))
                    m = i;
                c = c.GetNext();
            }
            return m;
        }

        public T RemoveAt(int i)
        {
            if (head == null)
                return default(T);
            T o;
            if (i == 0)
            {
                o = head.GetValue();
                head = head.GetNext();
            }
            SingleLink<T> p = head, c = head.GetNext();
            for (int j = 1; j < i; j++)
            {
                p = c;
                c = c.GetNext();
            }
            o = c.GetValue();
            p.SetNext(c.GetNext());
            size--;
            return o;
        }

        public ListIterator<T> IterateList()
        {
            return IterateListAt(0);
        }

        public ListIterator<T> IterateListAt(int i)
        {
            return new SinglyLinkedListIterator<T>(head);
        }

        public bool AddAllAt(Collection<T> es, int i)
        {
            if (i > size)
                return false;
            if (i == size)
            {
                AddAll(es);
                return true;
            }
            SingleLink<T> c = head;
            for (int j = 1; j < i; j++)
                c = c.GetNext();
            SingleLink<T> n = c.GetNext();
            SingleLink<T> t = c;
            for (Iterator<T> it = es.Iterate(); it.HasNext(); )
                t = t.SetNewNext(it.Next());
            t.SetNext(n);
            return true;
        }

        public T[] ToArray()
        {
            T[] arr = new T[size];
            int i = 0;
            SingleLink<T> c = head;
            while (c != null)
            {
                arr[i++] = c.GetValue();
                c = c.GetNext();
            }
            return arr;
        }

        public U[] ToArray<U>(ElementConverter<T, U> con)
        {
            U[] arr = new U[size];
            int i = 0;
            SingleLink<T> c = head;
            while (c != null)
            {
                arr[i++] = con.Convert(c.GetValue());
                c = c.GetNext();
            }
            return arr;
        }

    }

}