using System;
using OAlphaCollections;

namespace JSON
{

    public class ArrayValue : JSONElement
    {
        private List<JSONElement> value = new SinglyLinkedList<JSONElement>();

        public ArrayValue()
        {
        }

        public ArrayValue(object[] value)
        {
            for (int i = 0; i < value.Length; i++)
                this.value.Add(JSONElement.ToJSONElement(value[i]));
        }

        public int GetLength()
        {
            return value.Size();
        }

        public int GetType(int index)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            Object val = value.GetAt(index);
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
            else if (type.Equals(typeof(NullValue)))
                return VALUE_TYPE_NULL;
            else
                return -1;
        }

        public bool IsBool(int index)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            Object val = value.GetAt(index);
            Type type = val.GetType();
            return type.Equals(typeof(BoolValue));
        }

        public bool IsInt(int index)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            Object val = value.GetAt(index);
            Type type = val.GetType();
            return type.Equals(typeof(IntValue));
        }

        public bool IsFloat(int index)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            Object val = value.GetAt(index);
            Type type = val.GetType();
            return type.Equals(typeof(FloatValue));
        }

        public bool IsString(int index)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            Object val = value.GetAt(index);
            Type type = val.GetType();
            return type.Equals(typeof(StringValue));
        }

        public bool IsArray(int index)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            Object val = value.GetAt(index);
            Type type = val.GetType();
            return type.Equals(typeof(ArrayValue));
        }

        public bool IsJSON(int index)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            Object val = value.GetAt(index);
            Type type = val.GetType();
            return type.Equals(typeof(JSONObject));
        }

        public bool IsNull(int index)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            Object val = value.GetAt(index);
            Type type = val.GetType();
            return type.Equals(typeof(NullValue));
        }

        public bool GetBool(int index)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            Object val = value.GetAt(index);
            if (val.GetType().Equals(typeof(BoolValue)))
                return ((BoolValue)val).value;
            else
                throw new InvalidCastException("value at " + index + " is not a bool");
        }

        public int GetInt(int index)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            Object val = value.GetAt(index);
            if (val.GetType().Equals(typeof(IntValue)))
                return ((IntValue)val).value;
            else
                throw new InvalidCastException("value at " + index + " is not an int");
        }

        public double GetFloat(int index)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            Object val = value.GetAt(index);
            if (val.GetType().Equals(typeof(FloatValue)))
                return ((FloatValue)val).value;
            else
                throw new InvalidCastException("value at " + index + " is not a double");
        }

        public string GetString(int index)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            Object val = value.GetAt(index);
            if (val.GetType().Equals(typeof(StringValue)))
                return ((StringValue)val).value;
            else
                throw new InvalidCastException("value at " + index + " is not a string");
        }

        public ArrayValue GetArray(int index)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            Object val = value.GetAt(index);
            if (val.GetType().Equals(typeof(ArrayValue)))
                return (ArrayValue)val;
            else
                throw new InvalidCastException("value at " + index + " is not an array");
        }

        public JSONObject GetJSON(int index)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            Object val = value.GetAt(index);
            if (val.GetType().Equals(typeof(JSONObject)))
                return (JSONObject)val;
            else
                throw new InvalidCastException("value at " + index + " is not a JSON");
        }

        public void SetObject(int index, object v)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            JSONElement element = JSONElement.ToJSONElement(v);
            if (element != null)
                value.SetAt(index, element);
        }

        public void SetBool(int index, bool v)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            Object val = value.GetAt(index);
            if (val.GetType().Equals(typeof(BoolValue)))
                ((BoolValue)val).value = v;
            else
                value.SetAt(index, new BoolValue(v));
        }

        public void SetInt(int index, int v)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            Object val = value.GetAt(index);
            if (val.GetType().Equals(typeof(IntValue)))
                ((IntValue)val).value = v;
            else
                value.SetAt(index, new IntValue(v));
        }

        public void SetFloat(int index, double v)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            Object val = value.GetAt(index);
            if (val.GetType().Equals(typeof(FloatValue)))
                ((FloatValue)val).value = v;
            else
                value.SetAt(index, new FloatValue(v));
        }

        public void SetString(int index, string v)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            Object val = value.GetAt(index);
            if (val.GetType().Equals(typeof(StringValue)))
                ((StringValue)val).value = v;
            else
                value.SetAt(index, new StringValue(v));
        }

        public void SetArray(int index, Object[] v)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            value.SetAt(index, new ArrayValue(v));
        }

        public ArrayValue SetEmptyArray(int index)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            ArrayValue arr = new ArrayValue();
            value.SetAt(index, arr);
            return arr;
        }

        public void SetJSON(int index, JSONObject v)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            value.SetAt(index, v);
        }

        public JSONObject SetEmptyJSON(int index, JSONObject v)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            JSONObject json = new JSONObject();
            value.SetAt(index, json);
            return json;
        }

        public void SetNull(int index)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            value.SetAt(index, new NullValue());
        }

        public void SetEmptyJSON(int index)
        {
            if (index < 0 || index >= value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + (value.Size() - 1) + " inclusively");
            Object val = value.GetAt(index);
            value.SetAt(index, new JSONObject());
        }

        public void AddObjectAt(int index, object v)
        {
            JSONElement element = JSONElement.ToJSONElement(v);
            if (element != null)
                value.AddAt(element, index);
        }

        public void AddBoolAt(int index, bool v)
        {
            if (index < 0 || index > value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + value.Size() + " inclusively");
            value.AddAt(new BoolValue(v), index);
        }

        public void AddIntAt(int index, int v)
        {
            if (index < 0 || index > value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + value.Size() + " inclusively");
            value.AddAt(new IntValue(v), index);
        }

        public void AddFloatAt(int index, double v)
        {
            if (index < 0 || index > value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + value.Size() + " inclusively");
            value.AddAt(new FloatValue(v), index);
        }

        public void AddStringAt(int index, string v)
        {
            if (index < 0 || index > value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + value.Size() + " inclusively");
            value.AddAt(new StringValue(v), index);
        }

        public void AddArrayAt(int index, Object[] v)
        {
            if (index < 0 || index > value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + value.Size() + " inclusively");
            value.AddAt(new ArrayValue(v), index);
        }

        public ArrayValue AddEmptyArrayAt(int index)
        {
            if (index < 0 || index > value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + value.Size() + " inclusively");
            ArrayValue arr = new ArrayValue();
            value.AddAt(arr, index);
            return arr;
        }

        public void AddJSONAt(int index, JSONObject v)
        {
            if (index < 0 || index > value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + value.Size() + " inclusively");
            value.AddAt(v, index);
        }

        public JSONObject AddEmptyJSONAt(int index)
        {
            if (index < 0 || index > value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + value.Size() + " inclusively");
            JSONObject json = new JSONObject();
            value.AddAt(json, index);
            return json;
        }

        public void AddNullAt(int index)
        {
            if (index < 0 || index > value.Size())
                throw new IndexOutOfRangeException("index, " + index + ", must between 0 and " + value.Size() + " inclusively");
            value.AddAt(new NullValue(), index);
        }

        public void AddObject(object v)
        {
            JSONElement element = JSONElement.ToJSONElement(v);
            if( element != null )
                value.Add(element);
        }

        public void AddBool(bool v)
        {
            value.Add(new BoolValue(v));
        }

        public void AddInt(int v)
        {
            value.Add(new IntValue(v));
        }

        public void AddFloat(double v)
        {
            value.Add(new FloatValue(v));
        }

        public void AddString(string v)
        {
            value.Add(new StringValue(v));
        }

        public void AddArray(Object[] v)
        {
            value.Add(new ArrayValue(v));
        }

        public ArrayValue AddEmptyArray()
        {
            ArrayValue arr = new ArrayValue();
            value.Add(arr);
            return arr;
        }

        public void AddJSON(JSONObject v)
        {
            value.Add(v);
        }

        public JSONObject AddEmptyJSON()
        {
            JSONObject json = new JSONObject();
            value.Add(json);
            return json;
        }

        public void AddNull()
        {
            value.Add(new NullValue());
        }

        override
            public string ToString()
        {
            string str = "[";
            for (int i = 0; i < value.Size(); i++)
                str += (i > 0 ? "," : "") + value.GetAt(i).ToString();
            return str + "]";
        }

        override public object GetValue()
        {
            object[] arr = new object[value.Size()];
            int i = 0;
            for (Iterator<JSONElement> it = value.Iterate(); it.HasNext(); )
                arr[i++] = it.Next().GetValue();
            return arr;
        }
    }

}