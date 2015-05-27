using System;
using System.Text;
using OAlphaCollections;

namespace JSON
{

    class StringBuilderConverter : ElementConverter<StringBuilder, string>
    {

        public string Convert(StringBuilder e)
        {
            return e.ToString();
        }

    }

    class ParseSpace
    {
        public char[] arr;
        public int index;
        public Object result;

        public ParseSpace(string str)
        {
            arr = str.ToCharArray();
            index = 1;
            result = null;
        }

        public ParseSpace(ParseSpace space)
        {
            arr = space.arr;
            index = space.index;
            result = null;
        }
    }

    public class JSONParser
    {

        private JSONParser()
        {
        }

        public static JSONObject ParseString(string str)
        {
            if (!str.StartsWith("{"))
                throw new FormatException("valid JSON string mu st begin with '{'");
            ParseSpace space = new ParseSpace(str);
            space = parseStringInternal(space);
            return (JSONObject)space.result;
        }

        private static ParseSpace parseStringInternal(ParseSpace space)
        {
            List<StringBuilder> keys = new SinglyLinkedList<StringBuilder>();
            List<Object> values = new SinglyLinkedList<Object>();
            int oindex = space.index;
            try
            {
                while (space.arr[space.index] != '}')
                {
                    while (wspace(space.arr[space.index]))
                        space.index++;
                    StringBuilder key = new StringBuilder();
                    while (letter(space.arr[space.index]))
                    {
                        key.Append(space.arr[space.index]);
                        space.index++;
                    }
                    keys.Add(key);
                    //UnityEngine.Debug.Log ("parsed key: " + key.ToString ());
                    while (wspace(space.arr[space.index]))
                        space.index++;
                    if (space.arr[space.index] != '=')
                    {
                        int i = space.index;
                        space.index = oindex;
                        throw new FormatException("invalid character at " + i + ". expecting '='");
                    }
                    space.index++;
                    while (wspace(space.arr[space.index]))
                        space.index++;
                    //UnityEngine.Debug.Log ("parsing value for key, " + key.ToString ());
                    ParseSpace value = new ParseSpace(space);
                    value = getValue(value);
                    //UnityEngine.Debug.Log ("parsed value for key, " + key.ToString () + ": " + value.result);
                    values.Add(value.result);
                    space.index = value.index;
                    while (wspace(space.arr[space.index]))
                        space.index++;
                    if (space.arr[space.index] != ',' && space.arr[space.index] != '}')
                    {
                        int i = space.index;
                        space.index = oindex;
                        throw new FormatException("invalid character at " + i + ". expecting ',' or '}'");
                    }
                    else if (space.arr[space.index] == ',')
                    {
                        //UnityEngine.Debug.Log ("read comma. expexting next key-value pair");
                        space.index++;
                        while (wspace(space.arr[space.index]))
                            space.index++;
                    }

                }
            }
            catch (IndexOutOfRangeException e)
            {
                throw new IndexOutOfRangeException(new String(space.arr, oindex, space.index - 1), e);
            }
            space.index++;
            space.result = new JSONObject(keys.ToArray(new StringBuilderConverter()), values.ToArray());
            return space;
        }

        private static bool letter(char c)
        {
            if (c >= 'a' && c <= 'z')
                return true;
            else if (c >= 'A' && c <= 'Z')
                return true;
            else
                return false;
        }

        private static bool numeric(char c)
        {
            if ((c >= '0' && c <= '9') || c == '.')
                return true;
            else
                return false;
        }

        private static bool wspace(char c)
        {
            if (c == ' ' || c == '\t' || c == '\r' || c == '\n')
                return true;
            else
                return false;
        }

        private static ParseSpace getValue(ParseSpace space)
        {
            int oindex = space.index;
            StringBuilder value = new StringBuilder();
            string valStr;
            if (space.arr[space.index] == 't')
            {
                //UnityEngine.Debug.Log ("read 't'. expecting true");
                space.index++;
                if (space.arr[space.index] != 'r')
                {
                    int i = space.index - 1;
                    space.index = oindex;
                    throw new FormatException("invalid character at " + i + ". expecting 'true'");
                }
                space.index++;
                if (space.arr[space.index] != 'u')
                {
                    int i = space.index - 2;
                    space.index = oindex;
                    throw new FormatException("invalid character at " + i + ". expecting 'true'");
                }
                space.index++;
                if (space.arr[space.index] != 'e')
                {
                    int i = space.index - 3;
                    space.index = oindex;
                    throw new FormatException("invalid character at " + i + ". expecting 'true'");
                }
                space.index++;
                space.result = new BoolValue(true);
            }
            else if (space.arr[space.index] == 'f')
            {
                //UnityEngine.Debug.Log ("read 'f'. expecting false");
                space.index++;
                if (space.arr[space.index] != 'a')
                {
                    int i = space.index - 1;
                    space.index = oindex;
                    throw new FormatException("invalid character at " + i + ". expecting 'false'");
                }
                space.index++;
                if (space.arr[space.index] != 'l')
                {
                    int i = space.index - 2;
                    space.index = oindex;
                    throw new FormatException("invalid character at " + i + ". expecting 'false'");
                }
                space.index++;
                if (space.arr[space.index] != 's')
                {
                    int i = space.index - 3;
                    space.index = oindex;
                    throw new FormatException("invalid character at " + i + ". expecting 'false'");
                }
                space.index++;
                if (space.arr[space.index] != 'e')
                {
                    int i = space.index - 4;
                    space.index = oindex;
                    throw new FormatException("invalid character at " + i + ". expecting 'false'");
                }
                space.index++;
                space.result = new BoolValue(false);
            }
            else if (numeric(space.arr[space.index]))
            {
                //UnityEngine.Debug.Log ("read digit. expecting number");
                while (numeric(space.arr[space.index]))
                {
                    value.Append(space.arr[space.index]);
                    space.index++;
                }
                valStr = value.ToString();
                space.result = valStr.Contains(".") ? (Object)new FloatValue(float.Parse(valStr)) : (Object)new IntValue(int.Parse(valStr));
            }
            else if (space.arr[space.index] == '\"')
            {
                //UnityEngine.Debug.Log ("read double quote. expecting string");
                space.index++;
                while (space.arr[space.index] != '\"' || space.arr[space.index - 1] == '\\')
                {
                    value.Append(space.arr[space.index]);
                    space.index++;
                }
                space.index++;
                space.result = new StringValue(value.ToString());
            }
            else if (space.arr[space.index] == '[')
            {
                //UnityEngine.Debug.Log ("read square bracket. expecting array");
                space.index++;
                List<Object> elements = new SinglyLinkedList<Object>();
                int index = 0;
                while (space.arr[space.index] != ']')
                {
                    while (wspace(space.arr[space.index]))
                        space.index++;
                    //UnityEngine.Debug.Log ("parsing element at index, " + index);
                    ParseSpace element = new ParseSpace(space);
                    element = getValue(element);
                    //UnityEngine.Debug.Log ("parsed element at index, " + index + ": " + element.result);
                    elements.Add(element.result);
                    space.index = element.index;
                    index++;
                    while (wspace(space.arr[space.index]))
                        space.index++;
                    if (space.arr[space.index] != ',' && space.arr[space.index] != ']')
                    {
                        int i = space.index;
                        space.index = oindex;
                        throw new FormatException("invalid character at " + i + ". expecting ',' or ']'");
                    }
                    else if (space.arr[space.index] == ',')
                    {
                        //UnityEngine.Debug.Log ("read comma. expexting next element");
                        space.index++;
                        while (wspace(space.arr[space.index]))
                            space.index++;
                    }
                }
                space.index++;
                space.result = new ArrayValue(elements.ToArray());
            }
            else if (space.arr[space.index] == '{')
            {
                //UnityEngine.Debug.Log ("read curly bracket. expecting json");
                space.index++;
                space = parseStringInternal(space);
            }
            else if (space.arr[space.index] == 'n')
            {
                //UnityEngine.Debug.Log ("read 'n'. expecting null");
                space.index++;
                if (space.arr[space.index] != 'u')
                {
                    int i = space.index - 1;
                    space.index = oindex;
                    throw new FormatException("invalid character at " + i + ". expecting 'null'");
                }
                space.index++;
                if (space.arr[space.index] != 'l')
                {
                    int i = space.index - 2;
                    space.index = oindex;
                    throw new FormatException("invalid character at " + i + ". expecting 'null'");
                }
                space.index++;
                if (space.arr[space.index] != 'l')
                {
                    int i = space.index - 3;
                    space.index = oindex;
                    throw new FormatException("invalid character at " + i + ". expecting 'null'");
                }
                space.index++;
                space.result = new NullValue();
            }
            else
            {
                int i = space.index;
                space.index = oindex;
                throw new FormatException("invalid character at " + i + ", expected token");
            }
            return space;
        }

    }

}