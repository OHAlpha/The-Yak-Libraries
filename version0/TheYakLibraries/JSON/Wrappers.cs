using System;
using OAlphaCollections;

namespace JSON
{

    public class JSONElement
    {

        public static int VALUE_TYPE_BOOL = 0;
        public static int VALUE_TYPE_INT = 1;
        public static int VALUE_TYPE_FLOAT = 2;
        public static int VALUE_TYPE_STRING = 3;
        public static int VALUE_TYPE_ARRAY = 4;
        public static int VALUE_TYPE_JSON = 5;
        public static int VALUE_TYPE_NULL = 6;

    }

    public class BoolValue
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
    }

    public class IntValue
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
    }

    public class FloatValue
    {
        public float value;

        public FloatValue(float value)
        {
            this.value = value;
        }

        override
            public string ToString()
        {
            return "" + value;
        }
    }

    public class StringValue
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
    }

    public class NullValue
    {

        override
            public string ToString()
        {
            return "null";
        }
    }

}