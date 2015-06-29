using System;

namespace OAlphaCollections
{
    public class SimpleEntry<K, V> : Entry<K, V>
    {

        private K key;

        private V value;

        public SimpleEntry()
        {
            key = default(K);
            value = default(V);
        }

        public SimpleEntry(K key, V value)
        {
            this.key = key;
            this.value = value;
        }

        public K GetKey()
        {
            return key;
        }

        public K SetKey(K key)
        {
            K o = this.key;
            this.key = key;
            return o;
        }

        public V GetValue()
        {
            return value;
        }

        public V SetValue(V value)
        {
            V o = this.value;
            this.value = value;
            return o;
        }

        override
        public bool Equals(Object o)
        {
            if (o.GetType().Equals(typeof(Entry<K, V>)))
            {
                Entry<K, V> e = (Entry<K, V>)o;
                K k = e.GetKey();
                if (k == null && key != null || !k.Equals(key))
                    return false;
                V v = e.GetValue();
                if (v == null && value != null || !v.Equals(value))
                    return false;
                return true;
            }
            return false;
        }

        override
            public int GetHashCode()
        {
            return (key.Equals(default(K)) ? 0 : key.GetHashCode()) + (value.Equals(default(V)) ? 0 : value.GetHashCode());
        }

    }

}
