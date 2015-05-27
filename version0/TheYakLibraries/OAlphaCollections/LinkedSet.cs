using System;

namespace OAlphaCollections
{

    public class SinglyLinkedSet<T> : Set<T>
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
                SingleLink<T> p = null, c = head;
                while (c != null)
                {
                    if (e == null && c.GetValue() == null || e.Equals(c.GetValue()))
                        return;
                    p = c;
                    c = c.GetNext();
                }
                tail = p.SetNewNext(e);
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
            if (size == 0)
                return false;
            SingleLink<T> c = head;
            while (head != null)
            {
                if (e == null && c.GetValue() == null || e.Equals(c.GetValue()))
                    return true;
                c = c.GetNext();
            }
            return false;
        }

        public bool Remove(T e)
        {
            if (head == null)
                return false;
            else
            {
                if (e == null && head.GetValue() == null || e.Equals(head.GetValue()))
                {
                    head = head.GetNext();
                    size--;
                    return true;
                }
                else
                {
                    SingleLink<T> p = head, c = head.GetNext();
                    while (c != null)
                    {
                        if (e == null && c.GetValue() == null || e.Equals(c.GetValue()))
                        {
                            p.SetNext(c.GetNext());
                            size--;
                            return true;
                        }
                        p = c;
                        c = c.GetNext();
                    }
                    return false;
                }
            }
        }

        public bool AddAll(Collection<T> es)
        {
            if (es.IsEmpty())
                return false;
            int s = size;
            for (Iterator<T> it = es.Iterate(); it.HasNext(); )
                Add(it.Next());
            return s != size;
        }

        public bool RemoveAll(Collection<T> es)
        {
            if (es.IsEmpty())
                return false;
            int r = 0;
            for (Iterator<T> it = es.Iterate(); it.HasNext(); )
            {
                T e = it.Next();
                if (Contains(e))
                {
                    r++;
                    Remove(e);
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
            return new SinglyLinkedListIterator<T>(head);
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