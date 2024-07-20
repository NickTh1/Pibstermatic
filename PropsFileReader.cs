using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace WaveMix
{
    internal class PropsFileReader
    {
        static readonly char[] c_Delimeters = new char[]{ ' ', '\t', '=', ';'};

        class ParseContext
        {
            public string[] m_Lines = new string[0];
            public int m_IndexLine = 0;
        }

        static string SkipWhitespace(string str)
        {
            int index = 0;
            while(index < str.Length)
            {
                char ch = str[index];
                if (ch != ' ' & ch != '\t')
                    break;
                index++;
            }
            return str.Substring(index, str.Length - index);
        }

        static string GetLine(ParseContext context)
        {
            string line_ = context.m_Lines[context.m_IndexLine++];
            return SkipWhitespace(line_);
        }

        static string ParseToken(ref string str)
        {
            str = SkipWhitespace(str);
            if (str.Length == 0)
                return "";
            int index_end;
            if (str[0] == '"')
            {
                int index_end_quote = str.IndexOf('"');
                if (index_end_quote < 0)
                    return "";
                index_end = index_end_quote + 1;
            }
            else
            {
                int len = str.Substring(1).IndexOfAny(c_Delimeters);
                if (len >= 0)
                    index_end = 1 + len;
                else
                    index_end = str.Length;
            }
            string token = str.Substring(0, index_end);
            str = str.Substring(index_end, str.Length - index_end);
            return token;
        }

        static SKeyedValue[] ParseParentContent(ParseContext context)
        {
            List<SKeyedValue> children = new List<SKeyedValue>();
            while (true)
            {
                if (context.m_IndexLine >= context.m_Lines.Length)
                    break;
                string line_ = SkipWhitespace(context.m_Lines[context.m_IndexLine]);
                if (line_.StartsWith('}'))
                    break;
                SKeyedValue? child = ParseValue(context);
                if (!child.HasValue)
                    continue;

                children.Add(child.Value);
            }
            return children.ToArray();

        }

        static SKeyedValue[] ParseParent(ParseContext context)
        {
            while (true)
            {
                string line_ = GetLine(context);
                string token = ParseToken(ref line_);
                if (token.Length == 0)
                    continue;
                if (token.StartsWith(';'))
                    continue;
                if (token == "{")
                    break;
                throw new Exception("Failed to parse line:" + line_);
            }
            SKeyedValue[] children = ParseParentContent(context);

            string end = GetLine(context);
            if (end != "}")
                throw new Exception("Expected '}'");
            return children;
        }

        static SKeyedValue? ParseValue(ParseContext context)
        {
            string org_line = GetLine(context);
            string line_ = org_line;

            if (line_.Length == 0)
                return null;

            string key = ParseToken(ref line_);
            if (key.StartsWith(";"))
                return null;

            SKeyedValue keyed_value;
            keyed_value.m_Key = key;

            string eq = ParseToken(ref line_);
            if (eq != "=")
            {
                keyed_value.m_TypedValue.m_Type = EValueType.Parent;
                keyed_value.m_TypedValue.m_Value = ParseParent(context);
            } else
            {
                string str_value = ParseToken(ref line_);
                if (str_value.Length == 0)
                    throw new Exception("Failed to parse line:" + org_line);
                if (Char.IsNumber(str_value[0]))
                {
                    keyed_value.m_TypedValue.m_Type = EValueType.Float;
                    keyed_value.m_TypedValue.m_Value = float.Parse(str_value, CultureInfo.InvariantCulture);
                }
                else
                {
                    keyed_value.m_TypedValue.m_Type = EValueType.String;
                    if (str_value.StartsWith('"'))
                        keyed_value.m_TypedValue.m_Value = str_value.Substring(1, str_value.Length - 2);
                    else
                        keyed_value.m_TypedValue.m_Value = str_value;
                }
            }
            return keyed_value;
        }

        public static StructuredProperties ParsePropsFile(string text)
        {
            text = text.Replace("\n", "");

            ParseContext context = new ParseContext();
            context.m_Lines = text.Split('\r');

            SKeyedValue[] root_members = ParseParentContent(context);

            return new StructuredProperties(root_members);
        }

        public static StructuredProperties ReadPropsFile(string path)
        {
            using (StreamReader reader = File.OpenText(path))
            {
                string text = reader.ReadToEnd();

                return ParsePropsFile(text);
            }
        }
    }
}
