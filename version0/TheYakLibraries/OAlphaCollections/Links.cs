using System;

namespace OAlphaCollections
{

    public class SingleLink<T>
    {

        private T value = default(T);

        private SingleLink<T> next = null;

        public T GetValue()
        {
            return value;
        }

        public SingleLink<T> GetNext()
        {
            return next;
        }

        public bool HasNext()
        {
            return next != null;
        }

        public T SetValue(T v)
        {
            T o = value;
            value = v;
            return o;
        }

        public SingleLink<T> SetNext(SingleLink<T> n)
        {
            SingleLink<T> o = next;
            next = n;
            return o;
        }

        public SingleLink<T> SetNewNext(T v)
        {
            next = new SingleLink<T>();
            next.value = v;
            return next;
        }

    }

    class SinglyLinkedListIterator<T> : ListIterator<T>
    {

        private int index;

        private SingleLink<SingleLink<T>> stack;

        private SingleLink<T> next;

        public SinglyLinkedListIterator(SingleLink<T> head)
        {
            index = -1;
            stack = null;
            next = head;
        }

        public bool HasNext()
        {
            return next != null;
        }

        public T Next()
        {
            if (next == null)
                return default(T);
            T o = next.GetValue();
            SingleLink<SingleLink<T>> s = new SingleLink<SingleLink<T>>();
            s.SetValue(next);
            s.SetNext(stack);
            next = next.GetNext();
            index++;
            return o;
        }

        public bool HasPrevious()
        {
            return stack != null;
        }

        public T Previous()
        {
            if (stack == null)
                return default(T);
            next = stack.GetValue();
            stack = stack.GetNext();
            index--;
            return next.GetValue();
        }

        public int nextIndex()
        {
            return next == null ? -1 : index + 1;
        }

        public int previousIndex()
        {
            return index;
        }

    }

}