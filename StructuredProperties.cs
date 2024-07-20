using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveMix
{
    public enum EValueType
    {
        Float,
        String,
        Parent,
    }
    public struct STypedValue
    {
        public EValueType m_Type;
        public object m_Value;
    }

    public struct SKeyedValue
    {
        public string m_Key;
        public STypedValue m_TypedValue;
    };

    internal class StructuredProperties
    {
        private SKeyedValue[] m_KeyedValues;
        public static readonly object InvalidValue = new object();

        public StructuredProperties(SKeyedValue[] keyed_values)
        {
            m_KeyedValues = keyed_values;
        }

        public int FindMember(string key_)
        {
            for(int i = 0; i < m_KeyedValues.Length; i++)
            {
                if (key_ == m_KeyedValues[i].m_Key)
                    return i;
            }
            return -1;
        }

        public EValueType? GetMemberType(string key_)
        {
            int index = FindMember(key_);
            if (index < 0)
                return null;
            return m_KeyedValues[index].m_TypedValue.m_Type;
        }

        public object TryGetMember(string key_)
        {
            int index = FindMember(key_);
            if (index < 0)
                return InvalidValue;
            return m_KeyedValues[index].m_TypedValue.m_Value;
        }

        public int GetInteger(string key_, int default_value)
        {
            EValueType? type = GetMemberType(key_);
            if (!type.HasValue || type.Value != EValueType.Float)
                return default_value;
            object v = TryGetMember(key_);
            if (v == InvalidValue)
                return default_value;
            return (int)(float)v;
        }

        public float GetFloat(string key_, int default_value)
        {
            EValueType? type = GetMemberType(key_);
            if (!type.HasValue || type.Value != EValueType.Float)
                return default_value;
            object v = TryGetMember(key_);
            if (v == InvalidValue)
                return default_value;
            return (float)v;
        }

        public int GetInteger(string key_)
        {
            return (int)(float)TryGetMember(key_);
        }

        public float GetFloat(string key_)
        {
            return (float)TryGetMember(key_);
        }

        public string GetString(string key_)
        {
            return (string)TryGetMember(key_);
        }

        public StructuredProperties GetChild(string key_)
        {
            SKeyedValue[] children = (SKeyedValue[])TryGetMember(key_);
            return new StructuredProperties(children);
        }
    }
}
