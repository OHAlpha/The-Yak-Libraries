using System;
using OAlphaCollections;

namespace JSON
{

    public abstract class JSONElement
    {

        public static int VALUE_TYPE_BOOL = 0;
        public static int VALUE_TYPE_INT = 1;
        public static int VALUE_TYPE_FLOAT = 2;
        public static int VALUE_TYPE_STRING = 3;
        public static int VALUE_TYPE_ARRAY = 4;
        public static int VALUE_TYPE_JSON = 5;
        public static int VALUE_TYPE_NULL = 6;

        public abstract object GetValue();

        public static JSONElement ToJSONElement(object value)
        {
            if (value == null)
                return new NullValue();
            if (value.GetType().Equals(typeof(bool)))
                return new BoolValue((bool)value);
            if (value.GetType().Equals(typeof(int)))
                return new IntValue((int)value);
            if (value.GetType().Equals(typeof(float)))
                return new FloatValue((float)value);
            if (value.GetType().Equals(typeof(string)))
                return new StringValue((string)value);
            if (value.GetType().Equals(typeof(object[])))
                return new ArrayValue((object[])value);
            if (typeof(JSONElement).IsInstanceOfType(value))
                return (JSONElement)value;
            throw new Exception("invalid type");
        }

    }

    public class BoolValue : JSONElement
    {
        public bool value;

        public BoolValue(bool value)
        {
            this.value = value;
        }

        override
            public string ToString()
        {
            return "" + value;
        }

        override public object GetValue()
        {
            return value;
        }
    }

    public class IntValue : JSONElement
    {
        public int value;

        public IntValue(int value)
        {
            this.value = value;
        }

        override
            public string ToString()
        {
            return "" + value;
        }

        override public object GetValue()
        {
            return value;
        }
    }

    public class FloatValue : JSONElement
    {
        public double value;

        public FloatValue(double value)
        {
            this.value = value;
        }

        override
            public string ToString()
        {
            return "" + value;
        }

        override public object GetValue()
        {
            return value;
        }
    }

    public class StringValue : JSONElement
    {
        public string value;

        public StringValue(string value)
        {
            this.value = value;
        }

        override
            public string ToString()
        {
            return "\"" + value + "\"";
        }

        override public object GetValue()
        {
            return value;
        }
    }

    public class NullValue : JSONElement
    {

        override
            public string ToString()
        {
            return "null";
        }

        override public Object GetValue()
        {
            return null;
        }
    }

}