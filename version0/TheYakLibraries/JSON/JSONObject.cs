using System;
using OAlphaCollections;

namespace JSON
{

    public class JSONObject : JSONElement
    {
        private Map<string, Object> values = new SinglyLinkedMap<string, Object>();

        public JSONObject()
        {
        }

        public JSONObject(string[] keys, Object[] values)
        {
            int n = keys.Length;
            if (values.Length < n)
                n = values.Length;
            for (int i = 0; i < n; i++)
                this.values.Put(keys[i], values[i]);
        }

        public JSONObject Clone()
        {
            JSONObject clone = new JSONObject();
            clone.values.PutAll(values);
            return clone;
        }

        public void Clear()
        {
            values.Clear();
        }

        public int Size()
        {
            return values.Size();
        }

        public int IndexOfKey(string key)
        {
            throw new Exception("method not implemented");
        }

        public bool hasKey(string key)
        {
            return values.ContainsKey(key);
        }

        public int GetType(int index)
        {
            throw new Exception("method not implemented");
        }

        public int GetType(string key)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            Type type = val.GetType();
            if (type.Equals(typeof(BoolValue)))
                return VALUE_TYPE_BOOL;
            else if (type.Equals(typeof(IntValue)))
                return VALUE_TYPE_INT;
            else if (type.Equals(typeof(FloatValue)))
                return VALUE_TYPE_FLOAT;
            else if (type.Equals(typeof(StringValue)))
                return VALUE_TYPE_STRING;
            else if (type.Equals(typeof(ArrayValue)))
                return VALUE_TYPE_ARRAY;
            else if (type.Equals(typeof(JSONObject)))
                return VALUE_TYPE_JSON;
            else
                return -1;
        }

        public bool GetBool(int index)
        {
            throw new Exception("method not implemented");
        }

        public bool GetBool(string key)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            if (val.GetType().Equals(typeof(BoolValue)))
                return ((BoolValue)val).value;
            else
                throw new InvalidCastException("value pointed to by key, \"" + key + "\", is not a bool");
        }

        public int GetInt(int index)
        {
            throw new Exception("method not implemented");
        }

        public int GetInt(string key)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            if (val.GetType().Equals(typeof(IntValue)))
                return ((IntValue)val).value;
            else
                throw new InvalidCastException("value pointed to by key, \"" + key + "\", is not an int");
        }

        public float GetFloat(int index)
        {
            throw new Exception("method not implemented");
        }

        public float GetFloat(string key)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            if (val.GetType().Equals(typeof(FloatValue)))
                return ((FloatValue)val).value;
            else
                throw new InvalidCastException("value pointed to by key, \"" + key + "\", is not a float");
        }

        public string GetString(int index)
        {
            throw new Exception("method not implemented");
        }

        public string GetString(string key)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            if (val.GetType().Equals(typeof(StringValue)))
                return ((StringValue)val).value;
            else
                throw new InvalidCastException("value pointed to by key, \"" + key + "\", is not a string");
        }

        public ArrayValue GetArray(int index)
        {
            throw new Exception("method not implemented");
        }

        public ArrayValue GetArray(string key)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            if (val.GetType().Equals(typeof(ArrayValue)))
                return (ArrayValue)val;
            else
                throw new InvalidCastException("value pointed to by key, \"" + key + "\", is not an array");
        }

        public JSONObject GetJSON(int index)
        {
            throw new Exception("method not implemented");
        }

        public JSONObject GetJSON(string key)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            if (val.GetType().Equals(typeof(JSONObject)))
                return (JSONObject)val;
            else
                throw new InvalidCastException("value pointed to by key, \"" + key + "\", is not a JSON");
        }

        public void SetBoolAt(int index, bool v)
        {
            throw new Exception("method not implemented");
        }

        public void SetBool(string key, bool v)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            if (val.GetType().Equals(typeof(BoolValue)))
                ((BoolValue)val).value = v;
            else
                values.Put(key, new BoolValue(v));
        }

        public void SetIntAt(int index, int v)
        {
            throw new Exception("method not implemented");
        }

        public void SetInt(string key, int v)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            if (val.GetType().Equals(typeof(IntValue)))
                ((IntValue)val).value = v;
            else
                values.Put(key, new IntValue(v));
        }

        public void SetFloatAt(int index, float v)
        {
            throw new Exception("method not implemented");
        }

        public void SetFloat(string key, float v)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            if (val.GetType().Equals(typeof(FloatValue)))
                ((FloatValue)val).value = v;
            else
                values.Put(key, new FloatValue(v));
        }

        public void SetStringAt(int index, string v)
        {
            throw new Exception("method not implemented");
        }

        public void SetString(string key, string v)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            if (val.GetType().Equals(typeof(StringValue)))
                ((StringValue)val).value = v;
            else
                values.Put(key, new StringValue(v));
        }

        public void SetArrayAt(int index, Object[] v)
        {
            throw new Exception("method not implemented");
        }

        public void SetArray(string key, Object[] v)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            values.Put(key, new ArrayValue(v));
        }

        public void SetEmptyArrayAt(int index)
        {
            throw new Exception("method not implemented");
        }

        public void SetEmptyArray(string key)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            values.Put(key, new ArrayValue());
        }

        public void SetJSONAt(int index, JSONObject v)
        {
            throw new Exception("method not implemented");
        }

        public void SetJSON(string key, JSONObject v)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            values.Put(key, v);
        }

        public void SetEmptyJSON(int index)
        {
            throw new Exception("method not implemented");
        }

        public void SetEmptyJSON(string key)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            values.Put(key, new JSONObject());
        }

        public void AddBoolAt(int index, string key, bool v)
        {
            throw new Exception("method not implemented");
        }

        public void AddIntAt(int index, string key, int v)
        {
            throw new Exception("method not implemented");
        }

        public void AddFloatAt(int index, string key, float v)
        {
            throw new Exception("method not implemented");
        }

        public void AddStringAt(int index, string key, string v)
        {
            throw new Exception("method not implemented");
        }

        public void AddArrayAt(int index, string key, Object[] v)
        {
            throw new Exception("method not implemented");
        }

        public void AddEmptyArrayAt(int index, string key)
        {
            throw new Exception("method not implemented");
        }

        public void AddJSONAt(int index, string key, JSONObject v)
        {
            throw new Exception("method not implemented");
        }

        public void AddEmptyJSONAt(int index, string key)
        {
            throw new Exception("method not implemented");
        }

        public void AddBool(string key, bool v)
        {
            values.Put(key, new BoolValue(v));
        }

        public void AddInt(string key, int v)
        {
            values.Put(key, new IntValue(v));
        }

        public void AddFloat(string key, float v)
        {
            values.Put(key, new FloatValue(v));
        }

        public void AddString(string key, string v)
        {
            values.Put(key, new StringValue(v));
        }

        public void AddArray(string key, Object[] v)
        {
            values.Put(key, new ArrayValue(v));
        }

        public void AddEmptyArray(string key)
        {
            values.Put(key, new ArrayValue());
        }

        public void AddJSON(string key, JSONObject v)
        {
            values.Put(key, v);
        }

        public void AddEmptyJSON(string key)
        {
            values.Put(key, new JSONObject());
        }

        override
        public string ToString()
        {
            string str = "{";
            int i = 0;
            for (Iterator<Entry<string, Object>> it = values.EntrySet().Iterate(); it.HasNext(); )
            {
                Entry<string, Object> e = it.Next();
                str += (i > 0 ? "," : "") + e.GetKey() + "=" + e.GetValue().ToString();
                i++;
            }
            return str + "}";
        }

    }

}