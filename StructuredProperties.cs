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

        public STypedValue(EValueType type, object value)
        {
            m_Type = type;
            m_Value = value;
        }
    }

    public struct SKeyedValue
    {
        public string m_Key;
        public STypedValue m_TypedValue;
    };

    public class StructuredProperties
    {
        private SKeyedValue[] m_KeyedValues;
        public static readonly object InvalidValue = new object();

        public StructuredProperties(SKeyedValue[] keyed_values)
        {
            m_KeyedValues = keyed_values;
        }

        public int Count
        {
            get { return m_KeyedValues.Length; }
        }

        public SKeyedValue GetKeyedValue(int index)
        {
            return m_KeyedValues[index];
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
            return (StructuredProperties)TryGetMember(key_);
        }

        void AddKeyedValue(SKeyedValue keyed_value)
        {
            SKeyedValue[] new_array = new SKeyedValue[m_KeyedValues.Length + 1];
            Array.Copy(m_KeyedValues, 0, new_array, 0, m_KeyedValues.Length);
            new_array[m_KeyedValues.Length] = keyed_value;
            m_KeyedValues = new_array;
        }

        public StructuredProperties AddChild(string key_)
        {
            StructuredProperties child = new StructuredProperties(new SKeyedValue[0]);

            SKeyedValue keyed_value = new SKeyedValue();
            keyed_value.m_TypedValue = new STypedValue(EValueType.Parent, child);
            keyed_value.m_Key = key_;

            AddKeyedValue(keyed_value);

            return child;
        }

        public StructuredProperties GetOrAddChild(string key_)
        {
            int index_child = FindMember(key_);
            if (index_child >= 0)
                return GetChild(key_);

            return AddChild(key_);
        }

        void SetValue(string key_, EValueType value_type, object value)
        {
            int index = FindMember(key_);
            if (index >= 0)
                m_KeyedValues[index].m_TypedValue.m_Value = value;
            else
            {
                SKeyedValue keyed_value = new SKeyedValue();
                keyed_value.m_Key = key_;
                keyed_value.m_TypedValue = new STypedValue(value_type, value);
                AddKeyedValue(keyed_value);
            }
        }

        public void SetFloat(string key_, float value)
        {
            SetValue(key_, EValueType.Float, value);
        }

        public void SetInteger(string key_, int value)
        {
            SetFloat(key_, (float)value);
        }

        public void SetString(string key_, string value)
        {
            SetValue(key_, EValueType.String, value);
        }

        public void RemoveChild(string key_)
        {
            int index_child = FindMember(key_);
            if (index_child < 0)
                return;
            SKeyedValue[] new_array = new SKeyedValue[m_KeyedValues.Length - 1];
            Array.Copy(m_KeyedValues, 0, new_array, 0, index_child);
            Array.Copy(m_KeyedValues, index_child + 1, new_array, index_child, m_KeyedValues.Length - (index_child + 1));
            m_KeyedValues = new_array;
        }

        // Useful for MoveUp and similar.
        public bool SwapEntries(string key1, string key2, bool keep_names)
        {
            int index1 = FindMember(key1);
            int index2 = FindMember(key2);
            if (index1 < 0 || index2 < 0)
                return false;

            SKeyedValue keyed_value1 = m_KeyedValues[index1];
            SKeyedValue keyed_value2 = m_KeyedValues[index2];
            m_KeyedValues[index1] = keyed_value2;
            m_KeyedValues[index2] = keyed_value1;

            if (keep_names)
            {
                m_KeyedValues[index1].m_Key = key1;
                m_KeyedValues[index2].m_Key = key2;
            }

            return true;
        }

        public bool Rename(string key_from, string key_to)
        {
            int index = FindMember(key_from);
            if (index < 0)
                return false;
            m_KeyedValues[index].m_Key = key_to;
            return true;
        }
    }
}
