using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Xml;

namespace XYS.Util
{
    [Serializable]
    public class PropertiesDictionary : ReadOnlyPropertiesDictionary, ISerializable, IDictionary
    {
        #region 构造函数
        public PropertiesDictionary()
        {
        }
        public PropertiesDictionary(ReadOnlyPropertiesDictionary propertiesDictionary)
            : base(propertiesDictionary)
        {
        }
        private PropertiesDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region 重写ReadOnlyPropertiesDictionary方法
        public override object this[string key]
        {
            get { return InnerHashtable[key]; }
            set { InnerHashtable[key] = value; }
        }
        public void Remove(string key)
        {
            InnerHashtable.Remove(key);
        }
        #endregion

        #region IDictionary接口实现
        public IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return InnerHashtable.GetEnumerator();
        }
        public void IDictionary.Remove(object key)
        {
            InnerHashtable.Remove(key);
        }
        public bool IDictionary.Contains(object key)
        {
            return InnerHashtable.Contains(key);
        }
        public override void Clear()
        {
            InnerHashtable.Clear();
        }
        public void IDictionary.Add(object key, object value)
        {
            if (!(key is string))
            {
                throw new ArgumentException("key must be a string", "key");
            }
            InnerHashtable.Add(key, value);
        }
        public bool IDictionary.IsReadOnly
        {
            get { return false; }
        }
        public object IDictionary.this[object key]
        {
            get
            {
                if (!(key is string))
                {
                    throw new ArgumentException("key must be a string", "key");
                }
                return InnerHashtable[key];
            }
            set
            {
                if (!(key is string))
                {
                    throw new ArgumentException("key must be a string", "key");
                }
                InnerHashtable[key] = value;
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
            get { return false; }
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
        object ICollection.SyncRoot
        {
            get { return InnerHashtable.SyncRoot; }
        }
        #endregion

        #region IEnumerable接口实现
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)InnerHashtable).GetEnumerator();
        }
        #endregion
    }
}
