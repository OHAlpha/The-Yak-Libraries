using System;

namespace OAlphaCollections
{

    public class SinglyLinkedMap<K, V> : Map<K, V>
    {

        private int size = 0;

        private SingleLink<SimpleEntry<K, V>> head = null, tail = null;

        public void Clear()
        {
            if (head == null)
                return;
            tail = null;
            while (head != null)
            {
                SingleLink<SimpleEntry<K, V>> t = head;
                head = head.GetNext();
                SimpleEntry<K, V> e = t.GetValue();
                t.SetValue(null);
                t.SetNext(null);
                e.SetKey(default(K));
                e.SetValue(default(V));
            }
        }

        public V Put(K k, V v)
        {
            SimpleEntry<K, V> e = new SimpleEntry<K, V>(k, v);
            if (head == null)
            {
                head = tail = new SingleLink<SimpleEntry<K, V>>();
                head.SetValue(e);
                size++;
                return default(V);
            }
            else
            {
                SingleLink<SimpleEntry<K, V>> p = null, c = head;
                while (c != null)
                {
                    if (e == null && c.GetValue() == null || e.Equals(c.GetValue()))
                        return c.GetValue().SetValue(v);
                    p = c;
                    c = c.GetNext();
                }
                tail = p.SetNewNext(e);
                size++;
                return default(V);
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

        public bool ContainsKey(K k)
        {
            if (size == 0)
                return false;
            SingleLink<SimpleEntry<K, V>> c = head;
            while (head != null)
            {
                if (k == null && c.GetValue().GetKey() == null || k.Equals(c.GetValue().GetKey()))
                    return true;
                c = c.GetNext();
            }
            return false;
        }

        public bool ContainsValue(V v)
        {
            if (size == 0)
                return false;
            SingleLink<SimpleEntry<K, V>> c = head;
            while (head != null)
            {
                if (v == null && c.GetValue().GetValue() == null || v.Equals(c.GetValue().GetValue()))
                    return true;
                c = c.GetNext();
            }
            return false;
        }

        public V Remove(K k)
        {
            if (head == null)
                return default(V);
            else
            {
                SingleLink<SimpleEntry<K, V>> p = head, c = head.GetNext();
                while (c != null)
                {
                    if (k == null && c.GetValue().GetKey() == null || k.Equals(c.GetValue().GetKey()))
                    {
                        p.SetNext(c.GetNext());
                        size--;
                        return c.GetValue().GetValue();
                    }
                    p = c;
                    c = c.GetNext();
                }
                return default(V);
            }
        }

        public bool PutAll(Map<K, V> es)
        {
            if (es.IsEmpty())
                return false;
            int s = size;
            for (Iterator<Entry<K, V>> it = es.EntrySet().Iterate(); it.HasNext(); )
            {
                Entry<K, V> e = it.Next();
                Put(e.GetKey(), e.GetValue());
            }
            return s != size;
        }

        public V Get(K k)
        {
            if (size == 0)
                return default(V);
            SingleLink<SimpleEntry<K, V>> c = head;
            while (head != null)
            {
                if (k == null && c.GetValue().GetKey() == null || k.Equals(c.GetValue().GetKey()))
                    return c.GetValue().GetValue();
                c = c.GetNext();
            }
            return default(V);
        }

        public Set<K> KeySet()
        {
            SinglyLinkedSet<K> keySet = new SinglyLinkedSet<K>();
            SingleLink<SimpleEntry<K, V>> c = head;
            while (c != null)
            {
                keySet.Add(c.GetValue().GetKey());
                c = c.GetNext();
            }
            return keySet;
        }

        public Collection<V> Values()
        {
            SinglyLinkedList<V> values = new SinglyLinkedList<V>();
            SingleLink<SimpleEntry<K, V>> c = head;
            while (c != null)
            {
                values.Add(c.GetValue().GetValue());
                c = c.GetNext();
            }
            return values;
        }

        public Set<Entry<K, V>> EntrySet()
        {
            SinglyLinkedSet<Entry<K, V>> entrySet = new SinglyLinkedSet<Entry<K, V>>();
            SingleLink<SimpleEntry<K, V>> c = head;
            while (c != null)
            {
                entrySet.Add(c.GetValue());
                c = c.GetNext();
            }
            return entrySet;
        }

    }

}