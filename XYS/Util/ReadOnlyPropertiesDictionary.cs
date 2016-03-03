using System;
using System.Xml;
using System.Collections;
using System.Runtime.Serialization;
namespace XYS.Util
{
    [Serializable]
    public class ReadOnlyPropertiesDictionary : ISerializable, IDictionary
    {
        #region 字段
        private readonly Hashtable m_hashtable = new Hashtable();
        #endregion

        #region 构造函数
        public ReadOnlyPropertiesDictionary()
        {
        }
        public ReadOnlyPropertiesDictionary(ReadOnlyPropertiesDictionary propertiesDictionary)
        {
            foreach (DictionaryEntry entry in propertiesDictionary)
            {
                InnerHashtable.Add(entry.Key, entry.Value);
            }
        }
        protected ReadOnlyPropertiesDictionary(SerializationInfo info, StreamingContext context)
        {
            foreach (SerializationEntry entry in info)
            {
                InnerHashtable[XmlConvert.DecodeName(entry.Name)] = entry.Value;
            }
        }
        #endregion

        #region 实例属性
        protected Hashtable InnerHashtable
        {
            get { return m_hashtable; }
        }
        #endregion

        #region 实例方法
        public string[] GetKeys()
        {
            string[] keys = new String[InnerHashtable.Count];
            InnerHashtable.Keys.CopyTo(keys, 0);
            return keys;
        }
        public virtual object this[string key]
        {
            get { return InnerHashtable[key]; }
            set { throw new NotSupportedException("This is a Read Only Dictionary and can not be modified"); }
        }
        public bool Contains(string key)
        {
            return InnerHashtable.Contains(key);
        }
        #endregion

        #region ISerializable接口实现
        [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (DictionaryEntry entry in InnerHashtable)
            {
                string entryKey = entry.Key as string;
                object entryValue = entry.Value;
                if (entryKey != null && entryValue != null && entryValue.GetType().IsSerializable)
                {
                    info.AddValue(XmlConvert.EncodeLocalName(entryKey), entryValue);
                }
            }
        }
        #endregion Implementation of ISerializable

        #region IDictionary接口实现
       public IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return InnerHashtable.GetEnumerator();
        }
        public void IDictionary.Remove(object key)
        {
            throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
        }
        public bool IDictionary.Contains(object key)
        {
            return InnerHashtable.Contains(key);
        }
        public virtual void Clear()
        {
            throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
        }
        public void IDictionary.Add(object key, object value)
        {
            throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
        }
        
        bool IDictionary.IsReadOnly
        {
            get { return true; }
        }
        object IDictionary.this[object key]
        {
            get
            {
                if (!(key is string)) throw new ArgumentException("key must be a string");
                return InnerHashtable[key];
            }
            set
            {
                throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
            }
        }
        ICollection IDictionary.Values
        {
            get { return InnerHashtable.Values; }
        }
        ICollection IDictionary.Keys
        {
            get { return InnerHashtable.Keys; }
        }
        bool IDictionary.IsFixedSize
        {
            get { return InnerHashtable.IsFixedSize; }
        }
        #endregion

        #region ICollection接口实现
        void ICollection.CopyTo(Array array, int index)
        {
            InnerHashtable.CopyTo(array, index);
        }
        
        bool ICollection.IsSynchronized
        {
            get { return InnerHashtable.IsSynchronized; }
        }
        public int Count
        {
            get { return InnerHashtable.Count; }
        }
        object ICollection.SyncRoot
        {
            get { return InnerHashtable.SyncRoot; }
        }
        #endregion

        #region IEnumerable接口实现
        public IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)InnerHashtable).GetEnumerator();
        }
        #endregion
    }
}
