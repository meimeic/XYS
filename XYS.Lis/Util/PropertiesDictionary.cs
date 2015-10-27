using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Xml;

namespace XYS.Lis.Util
{
    [Serializable]
    public class PropertiesDictionary : ReadOnlyPropertiesDictionary, ISerializable, IDictionary
    {
        #region
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

        #region
        override public object this[string key]
        {
            get { return InnerHashtable[key]; }
            set { InnerHashtable[key] = value; }
        }
        public void Remove(string key)
        {
            InnerHashtable.Remove(key);
        }
        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return InnerHashtable.GetEnumerator();
        }
        void IDictionary.Remove(object key)
        {
            InnerHashtable.Remove(key);
        }
        bool IDictionary.Contains(object key)
        {
            return InnerHashtable.Contains(key);
        }
        public override void Clear()
        {
            InnerHashtable.Clear();
        }
        void IDictionary.Add(object key, object value)
        {
            if (!(key is string))
            {
                throw new ArgumentException("key must be a string", "key");
            }
            InnerHashtable.Add(key, value);
        }
        bool IDictionary.IsReadOnly
        {
            get { return false; }
        }
        object IDictionary.this[object key]
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
        
        #region Implementation of ICollection
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

        #region Implementation of IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)InnerHashtable).GetEnumerator();
        }

        #endregion
    }
}
