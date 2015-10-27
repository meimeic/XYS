using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Xml;
namespace XYS.Lis.Util
{
    [Serializable] 
    public class ReadOnlyPropertiesDictionary : ISerializable, IDictionary
    {
        #region
        private Hashtable m_hashtable = new Hashtable();
        #endregion
        #region
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
                // The keys are stored as Xml encoded names
                InnerHashtable[XmlConvert.DecodeName(entry.Name)] = entry.Value;
            }
        }
        #endregion
        
        #region
        protected Hashtable InnerHashtable
        {
            get { return m_hashtable; }
        }
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

        #region Implementation of ISerializable
        [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (DictionaryEntry entry in InnerHashtable)
            {
                string entryKey = entry.Key as string;
                object entryValue = entry.Value;

                // If value is serializable then we add it to the list
                if (entryKey != null && entryValue != null && entryValue.GetType().IsSerializable)
                {
                    // Store the keys as an Xml encoded local name as it may contain colons (':') 
                    // which are NOT escaped by the Xml Serialization framework.
                    // This must be a bug in the serialization framework as we cannot be expected
                    // to know the implementation details of all the possible transport layers.
                    info.AddValue(XmlConvert.EncodeLocalName(entryKey), entryValue);
                }
            }
        }
        
        #endregion Implementation of ISerializable

        #region Implementation of IDictionary
        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return InnerHashtable.GetEnumerator();
        }
        void IDictionary.Remove(object key)
        {
            throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
        }
        bool IDictionary.Contains(object key)
        {
            return InnerHashtable.Contains(key);
        }
        public virtual void Clear()
        {
            throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
        }
        void IDictionary.Add(object key, object value)
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

        #region Implementation of ICollection

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

        #region Implementation of IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)InnerHashtable).GetEnumerator();
        }
        #endregion
    }
}
