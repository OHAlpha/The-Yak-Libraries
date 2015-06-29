using System;
using OAlphaCollections;

namespace JSON
{

    /**
     * The JSONObject class is intended for manipulating string objects according to the JSON Specification.
     * Instances can be created from strings using the JSONParser, or empty instances can be created from constructors and can be
     * manipulated using the add, set and remove functions. The primitive types are wrapper into the classes specified in Wrapper.cs.
     */
    public class JSONObject : JSONElement
    {
        private Map<string, JSONElement> values = new SinglyLinkedMap<string, JSONElement>();

        /**
         * Creates an empty instance.
         */
        public JSONObject()
        {
        }

        /**
         * Initializes the instance with key/value pairs.
         */ 
        public JSONObject(string[] keys, object[] values)
        {
            int n = keys.Length;
            if (values.Length < n)
                n = values.Length;
            for (int i = 0; i < n; i++)
                try
                {
                    this.values.Put(keys[i], JSONElement.ToJSONElement(values[i]));
                }
                catch (Exception ex)
                {
                    throw new Exception("object, " + values[i].ToString() + 
                        ", corresponding to key, " + keys[i] + 
                        ", is of invalid type: " + values[i].GetType().FullName, ex);
                }
        }

        /**
         * Creates an exact copy of this instance. Values are shared between the copies. To clone the values, use CloneComplete().
         */
        public JSONObject Clone()
        {
            JSONObject clone = new JSONObject();
            clone.values.PutAll(values);
            return clone;
        }

        /**
         * Creates an exact copy of this instance. Values are also cloned. To share the values, use Clone().
         */
        public JSONObject CloneComplete()
        {
            JSONObject clone = new JSONObject();
            for (Iterator<string> it = values.KeySet().Iterate(); it.HasNext(); )
            {
                string key = it.Next();
                clone.AddObject(key, values.Get(key).GetValue());
            }
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

        public bool HasKey(string key)
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

        public bool IsBool(string key)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            Type type = val.GetType();
            return type.Equals(typeof(BoolValue));
        }

        public bool IsInt(string key)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            Type type = val.GetType();
            return type.Equals(typeof(IntValue));
        }

        public bool IsFloat(string key)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            Type type = val.GetType();
            return type.Equals(typeof(FloatValue));
        }

        public bool IsString(string key)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            Type type = val.GetType();
            return type.Equals(typeof(StringValue));
        }

        public bool IsArray(string key)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            Type type = val.GetType();
            return type.Equals(typeof(ArrayValue));
        }

        public bool IsJSON(string key)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            Type type = val.GetType();
            return type.Equals(typeof(JSONObject));
        }

        public bool IsNull(string key)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            Type type = val.GetType();
            return type.Equals(typeof(NullValue));
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
            if (val == null)
                throw new NullReferenceException("value pointed to by key, \"" + key + "\", is null");
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
            if (val == null)
                throw new NullReferenceException("value pointed to by key, \"" + key + "\", is null");
            if (val.GetType().Equals(typeof(IntValue)))
                return ((IntValue)val).value;
            else
                throw new InvalidCastException("value pointed to by key, \"" + key + "\", is not an int");
        }

        public double GetFloat(int index)
        {
            throw new Exception("method not implemented");
        }

        public double GetFloat(string key)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            Object val = values.Get(key);
            if (val == null)
                throw new NullReferenceException("value pointed to by key, \"" + key + "\", is null");
            if (val.GetType().Equals(typeof(FloatValue)))
                return ((FloatValue)val).value;
            else
                throw new InvalidCastException("value pointed to by key, \"" + key + "\", is not a double");
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
            if (val == null)
                throw new NullReferenceException("value pointed to by key, \"" + key + "\", is null");
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
            if (val == null)
                throw new NullReferenceException("value pointed to by key, \"" + key + "\", is null");
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
            if (val == null)
                throw new NullReferenceException("value pointed to by key, \"" + key + "\", is null");
            if (val.GetType().Equals(typeof(JSONObject)))
                return (JSONObject)val;
            else
                throw new InvalidCastException("value pointed to by key, \"" + key + "\", is not a JSON");
        }

        public void SetObjectAt(int index, object v)
        {
            throw new Exception("method not implemented");
        }

        public void SetObject(string key, object v)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            JSONElement element = JSONElement.ToJSONElement(v);
            if (element != null)
                values.Put(key, element);
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

        public void SetFloatAt(int index, double v)
        {
            throw new Exception("method not implemented");
        }

        public void SetFloat(string key, double v)
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

        public ArrayValue SetEmptyArray(string key)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            ArrayValue arr = new ArrayValue(); ;
            values.Put(key, arr);
            return arr;
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

        public JSONObject SetEmptyJSON(string key)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            JSONObject json = new JSONObject();
            values.Put(key, json);
            return json;
        }

        public void SetNullAt(int index)
        {
            throw new Exception("method not implemented");
        }

        public void SetNull(string key)
        {
            if (!values.ContainsKey(key))
                throw new ArgumentException("key, \"" + key + "\", does not exist");
            values.Put(key, new NullValue());
        }

        public void AddObjectAt(int index, string key, object v)
        {
            throw new Exception("method not implemented");
        }

        public void AddBoolAt(int index, string key, bool v)
        {
            throw new Exception("method not implemented");
        }

        public void AddIntAt(int index, string key, int v)
        {
            throw new Exception("method not implemented");
        }

        public void AddFloatAt(int index, string key, double v)
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

        public void AddNullAt(int index, string key)
        {
            throw new Exception("method not implemented");
        }

        public void AddObject(string key, object v)
        {
            JSONElement element = JSONElement.ToJSONElement(v);
            if( element != null )
                values.Put(key, element);
        }

        public void AddBool(string key, bool v)
        {
            values.Put(key, new BoolValue(v));
        }

        public void AddInt(string key, int v)
        {
            values.Put(key, new IntValue(v));
        }

        public void AddFloat(string key, double v)
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

        public ArrayValue AddEmptyArray(string key)
        {
            ArrayValue arr = new ArrayValue();
            values.Put(key, arr);
            return arr;
        }

        public void AddJSON(string key, JSONObject v)
        {
            values.Put(key, v);
        }

        public JSONObject AddEmptyJSON(string key)
        {
            JSONObject json = new JSONObject();
            values.Put(key, json);
            return json;
        }

        public void AddNull(string key)
        {
            values.Put(key, new NullValue());
        }

        override
        public string ToString()
        {
            string str = "{";
            int i = 0;
            for (Iterator<Entry<string, JSONElement>> it = values.EntrySet().Iterate(); it.HasNext(); )
            {
                Entry<string, JSONElement> e = it.Next();
                str += (i > 0 ? "," : "") + e.GetKey() + "=" + e.GetValue().ToString();
                i++;
            }
            return str + "}";
        }

        override public object GetValue()
        {
            return CloneComplete();
        }

    }

}