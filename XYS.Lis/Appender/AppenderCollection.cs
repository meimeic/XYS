using System;
using System.Collections;
namespace XYS.Lis.Appender
{
    public class AppenderCollection : ICollection, IList, IEnumerable, ICloneable
    {
        #region
        private static readonly int DEFAULT_CAPACITY = 16;
        public static readonly AppenderCollection EmptyCollection = ReadOnly(new AppenderCollection(0));
        #endregion

        #region 私有实例字段
        private IAppender[] m_array;
        private int m_count = 0;
        private int m_version = 0;
        #endregion

        #region 构造函数
        public AppenderCollection()
        {
            this.m_array = new IAppender[DEFAULT_CAPACITY];
        }
        public AppenderCollection(int capacity)
        {
            this.m_array = new IAppender[capacity];
        }
        public AppenderCollection(AppenderCollection c)
        {
            m_array = new IAppender[c.Count];
            AddRange(c);
        }
        public AppenderCollection(IAppender[] a)
        {
            m_array = new IAppender[a.Length];
            AddRange(a);
        }
        public AppenderCollection(ICollection col)
        {
            m_array = new IAppender[col.Count];
            AddRange(col);
        }
        internal protected AppenderCollection(Tag tag)
        {
            m_array = null;
        }
        #endregion

        #region 静态包装类
        public static AppenderCollection ReadOnly(AppenderCollection list)
        {
            if (list == null) throw new ArgumentNullException("list");

            return new ReadOnlyAppenderCollection(list);
        }
        #endregion

        #region 实现ICollection接口
        public virtual int Count
        {
            get { return this.m_count; }
        }
        public virtual bool IsSynchronized
        {
            get { return m_array.IsSynchronized; }
        }
        public virtual object SyncRoot
        {
            get { return m_array.SyncRoot; }
        }
        public virtual void CopyTo(IAppender[] array)
        {
            this.CopyTo(array, 0);
        }
        public virtual void CopyTo(IAppender[] array, int start)
        {
            if (m_count > array.GetUpperBound(0) + 1 - start)
            {
                throw new System.ArgumentException("Destination array was not long enough.");
            }
            Array.Copy(m_array, 0, array, start, m_count);
        }
        void ICollection.CopyTo(Array array, int start)
        {
            if (m_count > 0)
            {
                Array.Copy(m_array, 0, array, start, m_count);
            }
        }
        #endregion

        #region 接口Ilist 实现
        public virtual IAppender this[int index]
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
        public virtual int Add(IAppender item)
        {
            if (m_count == m_array.Length)
            {
                EnsureCapacity(m_count + 1);
            }

            m_array[m_count] = item;
            m_version++;
            return m_count++;
        }
        public virtual void Clear()
        {
            ++m_version;
            m_array = new IAppender[DEFAULT_CAPACITY];
            m_count = 0;
        }
        public virtual bool Contains(IAppender item)
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
        public virtual int IndexOf(IAppender item)
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
        public virtual void Insert(int index, IAppender item)
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
        public virtual void Remove(IAppender item)
        {
            int i = IndexOf(item);
            if (i < 0)
            {
                throw new System.ArgumentException("Cannot remove the specified item because it was not found in the specified Collection.");
            }

            ++m_version;
            RemoveAt(i);
        }
        public virtual void RemoveAt(int index)
        {
            ValidateIndex(index); // throws
            m_count--;
            if (index < m_count)
            {
                Array.Copy(m_array, index + 1, m_array, index, m_count - index);
            }
            // We can't set the deleted entry equal to null, because it might be a value type.
            // Instead, we'll create an empty single-element array of the right type and copy it 
            // over the entry we want to erase.
            IAppender[] temp = new IAppender[1];
            Array.Copy(temp, 0, m_array, m_count, 1);
            m_version++;
        }
        public virtual bool IsFixedSize
        {
            get { return false; }
        }
        public virtual bool IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region 实现 接口IEnumerator 接口
        public virtual IAppenderCollectionEnumerator GetEnumerator()
        {
            return new Enumerator(this);
        }
        #endregion

        #region 实现ICloneable接口
        public virtual object Clone()
        {
            AppenderCollection newCol = new AppenderCollection(m_count);
            Array.Copy(m_array, 0, newCol.m_array, 0, m_count);
            newCol.m_count = m_count;
            newCol.m_version = m_version;

            return newCol;
        }
        #endregion

        #region 公共虚方法
        public virtual int AddRange(AppenderCollection x)
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
        public virtual int AddRange(IAppender[] x)
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
        public virtual int AddRange(ICollection col)
        {
            if (m_count + col.Count >= m_array.Length)
            {
                EnsureCapacity(m_count + col.Count);
            }
            foreach (object item in col)
            {
                Add((IAppender)item);
            }
            return m_count;
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
                        IAppender[] temp = new IAppender[value];
                        Array.Copy(m_array, 0, temp, 0, m_count);
                        m_array = temp;
                    }
                    else
                    {
                        m_array = new IAppender[DEFAULT_CAPACITY];
                    }
                }
            }
        }
        public virtual void TrimToSize()
        {
            this.Capacity = m_count;
        }
        public virtual IAppender[] ToArray()
        {
            IAppender[] resultArray = new IAppender[m_count];
            if (m_count > 0)
            {
                Array.Copy(m_array, 0, resultArray, 0, m_count);
            }
            return resultArray;
        }
        #endregion

        #region 私有实例方法
        private void EnsureCapacity(int min)
        {
            int newCapacity = ((m_array.Length == 0) ? DEFAULT_CAPACITY : m_array.Length * 2);
            if (newCapacity < min)
            {
                newCapacity = min;
            }
            this.Capacity = newCapacity;
        }
        private void ValidateIndex(int i)
        {
            ValidateIndex(i, false);
        }
        private void ValidateIndex(int i, bool allowEqualEnd)
        {
            int max = (allowEqualEnd) ? (m_count) : (m_count - 1);
            if (i < 0 || i > max)
            {
                //抛出异常
                //throw log4net.Util.SystemInfo.CreateArgumentOutOfRangeException("i", (object)i, "Index was out of range. Must be non-negative and less than the size of the collection. [" + (object)i + "] Specified argument was out of the range of valid values.");
            }
        }
        #endregion

        #region Implementation (ICollection)

        #endregion

        #region Implementation (IList)

        object IList.this[int i]
        {
            get { return (object)this[i]; }
            set { this[i] = (IAppender)value; }
        }

        int IList.Add(object x)
        {
            return this.Add((IAppender)x);
        }

        bool IList.Contains(object x)
        {
            return this.Contains((IAppender)x);
        }

        int IList.IndexOf(object x)
        {
            return this.IndexOf((IAppender)x);
        }

        void IList.Insert(int pos, object x)
        {
            this.Insert(pos, (IAppender)x);
        }

        void IList.Remove(object x)
        {
            this.Remove((IAppender)x);
        }

        void IList.RemoveAt(int pos)
        {
            this.RemoveAt(pos);
        }

        #endregion

        #region Implementation (IEnumerable)

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)(this.GetEnumerator());
        }

        #endregion

        #region 公共虚属性
        #endregion

        #region 内部枚举类
        internal protected enum Tag
        {
            /// <summary>
            /// A value
            /// </summary>
            Default
        }
        #endregion

        #region 内部类
        private sealed class ReadOnlyAppenderCollection : AppenderCollection, ICollection
        {
            #region Implementation (data)

            private readonly AppenderCollection m_collection;

            #endregion

            #region Construction

            internal ReadOnlyAppenderCollection(AppenderCollection list)
                : base(Tag.Default)
            {
                m_collection = list;
            }
            #endregion

            #region Type-safe ICollection

            public override void CopyTo(IAppender[] array)
            {
                m_collection.CopyTo(array);
            }

            public override void CopyTo(IAppender[] array, int start)
            {
                m_collection.CopyTo(array, start);
            }

            void ICollection.CopyTo(Array array, int start)
            {
                ((ICollection)m_collection).CopyTo(array, start);
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

        }
        #endregion

        #region 内部类
        private sealed class Enumerator : IEnumerator, IAppenderCollectionEnumerator
        {
            #region Implementation (data)

            private readonly AppenderCollection m_collection;
            private int m_index;
            private int m_version;

            #endregion

            #region Construction

            /// <summary>
            /// Initializes a new instance of the <c>Enumerator</c> class.
            /// </summary>
            /// <param name="tc"></param>
            internal Enumerator(AppenderCollection tc)
            {
                m_collection = tc;
                m_index = -1;
                m_version = tc.m_version;
            }

            #endregion

            #region Operations (type-safe IEnumerator)

            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            public IAppender Current
            {
                get { return m_collection[m_index]; }
            }

            /// <summary>
            /// Advances the enumerator to the next element in the collection.
            /// </summary>
            /// <returns>
            /// <c>true</c> if the enumerator was successfully advanced to the next element; 
            /// <c>false</c> if the enumerator has passed the end of the collection.
            /// </returns>
            /// <exception cref="InvalidOperationException">
            /// The collection was modified after the enumerator was created.
            /// </exception>
            public bool MoveNext()
            {
                if (m_version != m_collection.m_version)
                {
                    throw new System.InvalidOperationException("Collection was modified; enumeration operation may not execute.");
                }
                ++m_index;
                return (m_index < m_collection.Count);
            }

            /// <summary>
            /// Sets the enumerator to its initial position, before the first element in the collection.
            /// </summary>
            public void Reset()
            {
                m_index = -1;
            }
            #endregion

            #region Implementation (IEnumerator)

            object IEnumerator.Current
            {
                get { return this.Current; }
            }

            #endregion
        }
        #endregion
        
        #region 内部接口
        public interface IAppenderCollectionEnumerator
        {
            IAppender Current { get; }
            bool MoveNext();
            void Reset();
        }
        #endregion

    }
}
