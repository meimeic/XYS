﻿//using System;
//using System.Collections;

//namespace XYS.Lis.Core
//{
//    public class ElementTypeCollection : ICollection, IList, IEnumerable, ICloneable
//    {

//        #region 定义了一个枚举接口
//        public interface IElementTypeCollectionEnumerator
//        {
//            ElementType Current { get; }
//            bool MoveNext();
//            void Reset();
//        }
//        #endregion

//        private static readonly int DEFAULT_CAPACITY = 6;

//        #region 私有变量 用于实现elementtype 集合

//        private ElementType[] m_array;//元素容器
//        private int m_count = 0; //元素个数
//        private int m_version = 0;

//        #endregion

//        #region 静态包装
//        public static ElementTypeCollection ReadOnly(ElementTypeCollection list)
//        {
//            if (list == null)
//            {
//                throw new ArgumentNullException("list");
//            }
//            return new ReadOnlyElementTypeCollection(list);
//        }
//        #endregion

//        #region 构造函数
//        //默认初始化容器大小
//        public ElementTypeCollection()
//        {
//            m_array = new ElementType[DEFAULT_CAPACITY];
//        }

//        public ElementTypeCollection(int capacity)
//        {
//            m_array = new ElementType[capacity];
//        }

//        public ElementTypeCollection(ElementTypeCollection c)
//        {
//            m_array = new ElementType[c.Count];
//            AddRange(c);
//        }
//        public ElementTypeCollection(ElementType[] a)
//        {
//            m_array = new ElementType[a.Length];
//            AddRange(a);
//        }
//        public ElementTypeCollection(ICollection col)
//        {
//            m_array = new ElementType[col.Count];
//            AddRange(col);
//        }
//        /// </summary>
//        protected internal enum Tag
//        {
//            Default
//        }
//        protected internal ElementTypeCollection(Tag tag)
//        {
//            m_array = null;
//        }
//        #endregion

//        #region Operations (type-safe ICollection)

//        public virtual int Count
//        {
//            get { return m_count; }
//        }

//        public virtual void CopyTo(ElementType[] array)
//        {
//            this.CopyTo(array, 0);
//        }

//        public virtual void CopyTo(ElementType[] array, int start)
//        {
//            if (m_count > array.GetUpperBound(0) + 1 - start)
//            {
//                throw new System.ArgumentException("Destination array was not long enough.");
//            }
//            Array.Copy(m_array, 0, array, start, m_count);
//        }

//        public virtual bool IsSynchronized
//        {
//            get { return m_array.IsSynchronized; }
//        }

//        public virtual object SyncRoot
//        {
//            get { return m_array.SyncRoot; }
//        }

//        #endregion

//        #region Operations (type-safe IList)

//        public virtual ElementType this[int index]
//        {
//            get
//            {
//                ValidateIndex(index); // throws
//                return m_array[index];
//            }
//            set
//            {
//                ValidateIndex(index); // throws
//                ++m_version;
//                m_array[index] = value;
//            }
//        }

//        public virtual int Add(ElementType item)
//        {
//            if (m_count == m_array.Length)
//            {
//                EnsureCapacity(m_count + 1);
//            }

//            m_array[m_count] = item;
//            m_version++;
//            return m_count++;
//        }

//        public virtual void Clear()
//        {
//            ++m_version;
//            m_array = new ElementType[DEFAULT_CAPACITY];
//            m_count = 0;
//        }

//        public virtual object Clone()
//        {
//            ElementTypeCollection newCol = new ElementTypeCollection(m_count);
//            Array.Copy(m_array, 0, newCol.m_array, 0, m_count);
//            newCol.m_count = m_count;
//            newCol.m_version = m_version;
//            return newCol;
//        }

//        public virtual bool Contains(ElementType item)
//        {
//            for (int i = 0; i != m_count; ++i)
//            {
//                if (m_array[i].Equals(item))
//                {
//                    return true;
//                }
//            }
//            return false;
//        }

//        public virtual int IndexOf(ElementType item)
//        {
//            for (int i = 0; i != m_count; ++i)
//            {
//                if (m_array[i].Equals(item))
//                {
//                    return i;
//                }
//            }
//            return -1;
//        }

//        public virtual void Insert(int index, ElementType item)
//        {
//            ValidateIndex(index, true); // throws
//            if (m_count == m_array.Length)
//            {
//                EnsureCapacity(m_count + 1);
//            }
//            if (index < m_count)
//            {
//                Array.Copy(m_array, index, m_array, index + 1, m_count - index);
//            }
//            m_array[index] = item;
//            m_count++;
//            m_version++;
//        }

//        public virtual void Remove(ElementType item)
//        {
//            int i = IndexOf(item);
//            if (i < 0)
//            {
//                throw new System.ArgumentException("Cannot remove the specified item because it was not found in the specified Collection.");
//            }
//            ++m_version;
//            RemoveAt(i);
//        }

//        public virtual void RemoveAt(int index)
//        {
//            ValidateIndex(index); // throws
//            m_count--;
//            if (index < m_count)
//            {
//                Array.Copy(m_array, index + 1, m_array, index, m_count - index);
//            }
//            ElementType[] temp = new ElementType[1];
//            Array.Copy(temp, 0, m_array, m_count, 1);
//            m_version++;
//        }
//        public virtual bool IsFixedSize
//        {
//            get { return false; }
//        }
//        public virtual bool IsReadOnly
//        {
//            get { return false; }
//        }
//        #endregion

//        #region Operations (type-safe IEnumerable)

//        public virtual IElementTypeCollectionEnumerator GetEnumerator()
//        {
//            return new Enumerator(this);
//        }

//        #endregion

//        #region Public helpers (just to mimic some nice features of ArrayList)

//        public virtual int Capacity
//        {
//            get
//            {
//                return m_array.Length;
//            }
//            set
//            {
//                if (value < m_count)
//                {
//                    value = m_count;
//                }
//                if (value != m_array.Length)
//                {
//                    if (value > 0)
//                    {
//                        ElementType[] temp = new ElementType[value];
//                        Array.Copy(m_array, 0, temp, 0, m_count);
//                        m_array = temp;
//                    }
//                    else
//                    {
//                        m_array = new ElementType[DEFAULT_CAPACITY];
//                    }
//                }
//            }
//        }

//        public virtual int AddRange(ElementTypeCollection x)
//        {
//            if (m_count + x.Count >= m_array.Length)
//            {
//                EnsureCapacity(m_count + x.Count);
//            }
//            Array.Copy(x.m_array, 0, m_array, m_count, x.Count);
//            m_count += x.Count;
//            m_version++;
//            return m_count;
//        }

//        public virtual int AddRange(ElementType[] x)
//        {
//            if (m_count + x.Length >= m_array.Length)
//            {
//                EnsureCapacity(m_count + x.Length);
//            }

//            Array.Copy(x, 0, m_array, m_count, x.Length);
//            m_count += x.Length;
//            m_version++;

//            return m_count;
//        }

//        public virtual int AddRange(ICollection col)
//        {
//            if (m_count + col.Count >= m_array.Length)
//            {
//                EnsureCapacity(m_count + col.Count);
//            }
//            foreach (object item in col)
//            {
//                Add((ElementType)item);
//            }
//            return m_count;
//        }

//        public virtual void TrimToSize()
//        {
//            this.Capacity = m_count;
//        }

//        #endregion

//        #region Implementation (helpers)

//        private void ValidateIndex(int i)
//        {
//            ValidateIndex(i, false);
//        }

//        private void ValidateIndex(int i, bool allowEqualEnd)
//        {
//            int max = (allowEqualEnd) ? (m_count) : (m_count - 1);
//            if (i < 0 || i > max)
//            {
//                throw new ArgumentOutOfRangeException("i", (object)i, "Index was out of range. Must be non-negative and less than the size of the collection. [" + (object)i + "] Specified argument was out of the range of valid values.");
//            }
//        }

//        private void EnsureCapacity(int min)
//        {
//            int newCapacity = ((m_array.Length == 0) ? DEFAULT_CAPACITY : m_array.Length * 2);
//            if (newCapacity < min)
//            {
//                newCapacity = min;
//            }

//            this.Capacity = newCapacity;
//        }

//        #endregion

//        #region Implementation (ICollection)

//        void ICollection.CopyTo(Array array, int start)
//        {
//            Array.Copy(m_array, 0, array, start, m_count);
//        }

//        #endregion

//        #region Implementation (IList)

//        object IList.this[int i]
//        {
//            get { return (object)this[i]; }
//            set { this[i] = (ElementType)value; }
//        }

//        int IList.Add(object x)
//        {
//            return this.Add((ElementType)x);
//        }

//        bool IList.Contains(object x)
//        {
//            return this.Contains((ElementType)x);
//        }

//        int IList.IndexOf(object x)
//        {
//            return this.IndexOf((ElementType)x);
//        }

//        void IList.Insert(int pos, object x)
//        {
//            this.Insert(pos, (ElementType)x);
//        }

//        void IList.Remove(object x)
//        {
//            this.Remove((ElementType)x);
//        }

//        void IList.RemoveAt(int pos)
//        {
//            this.RemoveAt(pos);
//        }
//        #endregion

//        #region Implementation (IEnumerable)

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return (IEnumerator)(this.GetEnumerator());
//        }

//        #endregion

//        #region Nested enumerator class

//        private sealed class Enumerator : IEnumerator, IElementTypeCollectionEnumerator
//        {
//            #region Implementation (data)

//            private readonly ElementTypeCollection m_collection;
//            private int m_index;
//            private int m_version;

//            #endregion

//            #region Construction

//            internal Enumerator(ElementTypeCollection tc)
//            {
//                m_collection = tc;
//                m_index = -1;
//                m_version = tc.m_version;
//            }

//            #endregion

//            #region Operations (type-safe IEnumerator)

//            public ElementType Current
//            {
//                get { return m_collection[m_index]; }
//            }

//            public bool MoveNext()
//            {
//                if (m_version != m_collection.m_version)
//                {
//                    throw new System.InvalidOperationException("Collection was modified; enumeration operation may not execute.");
//                }

//                ++m_index;
//                return (m_index < m_collection.Count);
//            }

//            public void Reset()
//            {
//                m_index = -1;
//            }

//            #endregion

//            #region Implementation (IEnumerator)

//            object IEnumerator.Current
//            {
//                get { return this.Current; }
//            }
//            #endregion
//        }
//        #endregion

//        #region Nested Read Only Wrapper class
//        private sealed class ReadOnlyElementTypeCollection : ElementTypeCollection
//        {
//            #region Implementation (data)

//            private readonly ElementTypeCollection m_collection;

//            #endregion

//            #region Construction

//            internal ReadOnlyElementTypeCollection(ElementTypeCollection list)
//                : base(Tag.Default)
//            {
//                m_collection = list;
//            }

//            #endregion

//            #region Type-safe ICollection

//            public override void CopyTo(ElementType[] array)
//            {
//                m_collection.CopyTo(array);
//            }

//            public override void CopyTo(ElementType[] array, int start)
//            {
//                m_collection.CopyTo(array, start);
//            }
//            public override int Count
//            {
//                get { return m_collection.Count; }
//            }

//            public override bool IsSynchronized
//            {
//                get { return m_collection.IsSynchronized; }
//            }

//            public override object SyncRoot
//            {
//                get { return this.m_collection.SyncRoot; }
//            }

//            #endregion

//            #region Type-safe IList

//            public override ElementType this[int i]
//            {
//                get { return m_collection[i]; }
//                set { throw new NotSupportedException("This is a Read Only Collection and can not be modified"); }
//            }

//            public override int Add(ElementType x)
//            {
//                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
//            }

//            public override void Clear()
//            {
//                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
//            }

//            public override bool Contains(ElementType x)
//            {
//                return m_collection.Contains(x);
//            }

//            public override int IndexOf(ElementType x)
//            {
//                return m_collection.IndexOf(x);
//            }

//            public override void Insert(int pos, ElementType x)
//            {
//                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
//            }

//            public override void Remove(ElementType x)
//            {
//                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
//            }

//            public override void RemoveAt(int pos)
//            {
//                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
//            }

//            public override bool IsFixedSize
//            {
//                get { return true; }
//            }

//            public override bool IsReadOnly
//            {
//                get { return true; }
//            }

//            #endregion

//            #region Type-safe IEnumerable

//            public override IElementTypeCollectionEnumerator GetEnumerator()
//            {
//                return m_collection.GetEnumerator();
//            }

//            #endregion

//            #region Public Helpers

//            // (just to mimic some nice features of ArrayList)
//            public override int Capacity
//            {
//                get { return m_collection.Capacity; }
//                set { throw new NotSupportedException("This is a Read Only Collection and can not be modified"); }
//            }

//            public override int AddRange(ElementTypeCollection x)
//            {
//                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
//            }

//            public override int AddRange(ElementType[] x)
//            {
//                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
//            }

//            #endregion
//        }
//        #endregion
//    }
//}