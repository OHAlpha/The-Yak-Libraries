using System;

namespace OAlphaCollections
{
    public interface Map<K, V>
    {

        int Size();

        bool IsEmpty();

        V Put(K key, V value);

        V Get(K key);

        V Remove(K key);

        void Clear();

        bool PutAll(Map<K, V> es);

        bool ContainsKey(K key);

        bool ContainsValue(V value);

        Set<K> KeySet();

        Collection<V> Values();

        Set<Entry<K, V>> EntrySet();

    }

    public interface Entry<K, V>
    {

        K GetKey();

        K SetKey(K key);

        V GetValue();

        V SetValue(V value);

    }

}
