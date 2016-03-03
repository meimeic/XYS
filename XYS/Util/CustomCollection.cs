using System;
using System.Collections;
using System.Collections.Generic;

namespace XYS.Util
{
    public class CustomCollection<T> : IList<T>, ICloneable
    {
        #region 枚举接口
        public interface ICustomCollectionEnumerator<T>
        {
            T Current { get; }
            bool MoveNext();
            void Reset();
        }
        #endregion

        #region 字段
        private static readonly int DEFAULT_CAPACITY = 6;
        private T[] m_array;//元素容器
        private int m_count = 0; //元素个数
        private int m_version = 0;
        #endregion

        #region 构造函数
        public CustomCollection()
        {
            m_array = new T[DEFAULT_CAPACITY];
        }
        public CustomCollection(int capacity)
        {
            m_array = new T[capacity];
        }
        public CustomCollection(CustomCollection<T> c)
        {
            m_array = new T[c.Count];
            AddRange(c);
        }
        public CustomCollection(T[] a)
        {
            m_array = new T[a.Length];
            AddRange(a);
        }
        public CustomCollection(ICollection<T> col)
        {
            m_array = new T[col.Count];
            AddRange(col);
        }
        protected internal enum Tag
        {
            Default
        }
        protected internal CustomCollection(Tag tag)
        {
            m_array = null;
        }
        #endregion

        #region ICollection<T>实现
        public int ICollection<T>.Count
        {
            get { return this.Count; }
        }
        public bool ICollection<T>.IsReadOnly
        {
            get { return this.IsReadOnly; }
        }

        public void ICollection<T>.Add(T item)
        {
            this.Add(item);
        }
        public void ICollection<T>.Clear()
        {
            this.Clear();
        }
        public bool ICollection<T>.Contains(T item)
        {
            return this.Contains(item);
        }
        public void ICollection<T>.CopyTo(T[] array, int start)
        {
            this.CopyTo(array, start);
        }
        public virtual bool ICollection<T>.Remove(T item)
        {
            return this.Remove(item);
        }
        #endregion

        #region 实现枚举IEnumerable<T>接口
        public IEnumerator<T> IEnumerator<T>.GetEnumerator()
        {
            return (IEnumerator<T>)this.GetEnumerator();
        }
        #endregion

        #region 实现IEnumerable接口
        public IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)(this.GetEnumerator());
        }
        #endregion

        #region 实现IList<T>接口
        public T IList<T>.this[int index]
        {
            get { return this[index]; }
            set { this[index] = value; }
        }
        public int IList<T>.IndexOf(T item)
        {
            return this.IndexOf(item);
        }
        public void IList<T>.Insert(int index, T item)
        {
            this.Insert(index, item);
        }

        public void IList<T>.RemoveAt(int index)
        {
            this.RemoveAt(index);
        }
        #endregion

        #region 实现ICloneable接口
        public virtual object Clone()
        {
            CustomCollection<T> newCol = new CustomCollection<T>(m_count);
            Array.Copy(m_array, 0, newCol.m_array, 0, m_count);
            newCol.m_count = m_count;
            newCol.m_version = m_version;
            return newCol;
        }
        #endregion

        #region IEnumerable<T>接口对应
        public virtual ICustomCollectionEnumerator<T> GetEnumerator()
        {
            return new Enumerator<T>(this);
        }
        #endregion

        #region ICollcetion接口对应
        public virtual int Count
        {
            get { return m_count; }
        }
        public virtual bool IsReadOnly
        {
            get { return false; }
        }
        public virtual void Add(T item)
        {
            if (m_count == m_array.Length)
            {
                EnsureCapacity(m_count + 1);
            }
            m_array[m_count] = item;
            m_version++;
        }
        public virtual void Clear()
        {
            ++m_version;
            m_array = new T[DEFAULT_CAPACITY];
            m_count = 0;
        }
        public virtual bool Contains(T item)
        {
            for (int i = 0; i != m_count; ++i)
            {
                if (m_array[i].Equals(item))
                {
                    return true;
                }
            }
            return false;
        }
        public virtual void CopyTo(T[] array, int start)
        {
            if (m_count > array.GetUpperBound(0) + 1 - start)
            {
                throw new System.ArgumentException("Destination array was not long enough.");
            }
            Array.Copy(m_array, 0, array, start, m_count);
        }
        public virtual bool Remove(T item)
        {
            int i = IndexOf(item);
            if (i < 0)
            {
                throw new System.ArgumentException("Cannot remove the specified item because it was not found in the specified Collection.");
            }
            ++m_version;
            RemoveAt(i);
            return true;
        }
        #endregion

        #region IList接口对应
        public virtual T this[int index]
        {
            get
            {
                ValidateIndex(index); // throws
                return m_array[index];
            }
            set
            {
                ValidateIndex(index); // throws
                ++m_version;
                m_array[index] = value;
            }
        }
        public virtual int IndexOf(T item)
        {
            for (int i = 0; i != m_count; ++i)
            {
                if (m_array[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }
        public virtual void Insert(int index, T item)
        {
            ValidateIndex(index, true); // throws
            if (m_count == m_array.Length)
            {
                EnsureCapacity(m_count + 1);
            }
            if (index < m_count)
            {
                Array.Copy(m_array, index, m_array, index + 1, m_count - index);
            }
            m_array[index] = item;
            m_count++;
            m_version++;
        }
        public virtual void RemoveAt(int index)
        {
            ValidateIndex(index); // throws
            m_count--;
            if (index < m_count)
            {
                Array.Copy(m_array, index + 1, m_array, index, m_count - index);
            }
            T[] temp = new T[1];
            Array.Copy(temp, 0, m_array, m_count, 1);
            m_version++;
        }
        #endregion

        #region 实例属性
        public virtual bool IsFixedSize
        {
            get { return false; }
        }
        public virtual bool IsSynchronized
        {
            get { return m_array.IsSynchronized; }
        }
        public virtual object SyncRoot
        {
            get { return m_array.SyncRoot; }
        }
        public virtual int Capacity
        {
            get
            {
                return m_array.Length;
            }
            set
            {
                if (value < m_count)
                {
                    value = m_count;
                }
                if (value != m_array.Length)
                {
                    if (value > 0)
                    {
                        T[] temp = new T[value];
                        Array.Copy(m_array, 0, temp, 0, m_count);
                        m_array = temp;
                    }
                    else
                    {
                        m_array = new T[DEFAULT_CAPACITY];
                    }
                }
            }
        }
        #endregion

        #region 实例方法
        public virtual void CopyTo(T[] array)
        {
            this.CopyTo(array, 0);
        }
        public virtual int AddRange(T[] x)
        {
            if (m_count + x.Length >= m_array.Length)
            {
                EnsureCapacity(m_count + x.Length);
            }
            Array.Copy(x, 0, m_array, m_count, x.Length);
            m_count += x.Length;
            m_version++;
            return m_count;
        }
        public virtual int AddRange(CustomCollection<T> x)
        {
            if (m_count + x.Count >= m_array.Length)
            {
                EnsureCapacity(m_count + x.Count);
            }
            Array.Copy(x.m_array, 0, m_array, m_count, x.Count);
            m_count += x.Count;
            m_version++;
            return m_count;
        }
        public virtual int AddRange(ICollection<T> col)
        {
            if (m_count + col.Count >= m_array.Length)
            {
                EnsureCapacity(m_count + col.Count);
            }
            foreach (T item in col)
            {
                Add(item);
            }
            return m_count;
        }
        public virtual void TrimToSize()
        {
            this.Capacity = m_count;
        }
        #endregion

        #region 私有方法
        private void ValidateIndex(int i)
        {
            ValidateIndex(i, false);
        }
        private void ValidateIndex(int i, bool allowEqualEnd)
        {
            int max = (allowEqualEnd) ? (m_count) : (m_count - 1);
            if (i < 0 || i > max)
            {
                throw new ArgumentOutOfRangeException("i", (object)i, "Index was out of range. Must be non-negative and less than the size of the collection. [" + (object)i + "] Specified argument was out of the range of valid values.");
            }
        }
        private void EnsureCapacity(int min)
        {
            int newCapacity = ((m_array.Length == 0) ? DEFAULT_CAPACITY : m_array.Length * 2);
            if (newCapacity < min)
            {
                newCapacity = min;
            }
            this.Capacity = newCapacity;
        }
        #endregion

        #region 内部枚举类
        private sealed class Enumerator<T> : IEnumerator<T>, ICustomCollectionEnumerator<T>
        {
            private int m_index;
            private int m_version;
            private readonly CustomCollection<T> m_collection;

            internal Enumerator(CustomCollection<T> tc)
            {
                m_index = -1;
                m_collection = tc;
                m_version = tc.m_version;
            }
            public T Current
            {
                get { return m_collection[m_index]; }
            }
            public bool MoveNext()
            {
                if (m_version != m_collection.m_version)
                {
                    throw new System.InvalidOperationException("Collection was modified; enumeration operation may not execute.");
                }
                ++m_index;
                return (m_index < m_collection.Count);
            }
            public void Reset()
            {
                m_index = -1;
            }
            T IEnumerator<T>.Current
            {
                get { return this.Current; }
            }
        }
        #endregion

        #region 内部只读类
        private sealed class ReadOnlyCustomCollection<T> : CustomCollection<T>
        {
            #region
            private readonly CustomCollection<T> m_collection;
            #endregion

            #region 构造函数
            internal ReadOnlyCustomCollection(CustomCollection<T> list)
                : base(Tag.Default)
            {
                m_collection = list;
            }
            #endregion

            #region 重写ICollection<T>接口实现
            public override void CopyTo(T[] array)
            {
                m_collection.CopyTo(array);
            }
            public override void CopyTo(T[] array, int start)
            {
                m_collection.CopyTo(array, start);
            }

            public override int Count
            {
                get { return m_collection.Count; }
            }
            public override bool IsSynchronized
            {
                get { return m_collection.IsSynchronized; }
            }
            public override object SyncRoot
            {
                get { return this.m_collection.SyncRoot; }
            }
            #endregion

            #region 重写IList<T> 接口对应
            public override T this[int i]
            {
                get { return m_collection[i]; }
                set { throw new NotSupportedException("This is a Read Only Collection and can not be modified"); }
            }
            public override int Add(T x)
            {
                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
            }
            public override void Clear()
            {
                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
            }
            public override bool Contains(T x)
            {
                return m_collection.Contains(x);
            }
            public override int IndexOf(T x)
            {
                return m_collection.IndexOf(x);
            }
            public override void Insert(int pos, T x)
            {
                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
            }
            public override bool Remove(T x)
            {
                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
            }
            public override void RemoveAt(int pos)
            {
                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
            }
            #endregion

            #region
            public override bool IsFixedSize
            {
                get { return true; }
            }
            public override bool IsReadOnly
            {
                get { return true; }
            }
            #endregion

            #region
            public override ICustomCollectionEnumerator<T> GetEnumerator()
            {
                return m_collection.GetEnumerator();
            }
            #endregion

            #region Public Helpers
            // (just to mimic some nice features of ArrayList)
            public override int Capacity
            {
                get { return m_collection.Capacity; }
                set { throw new NotSupportedException("This is a Read Only Collection and can not be modified"); }
            }
            public override int AddRange(CustomCollection<T> x)
            {
                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
            }
            public override int AddRange(T[] x)
            {
                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
            }
            #endregion
        }
        #endregion
    }
}