using System;
using System.Collections;
namespace XYS.Lis.Util
{
    [Serializable]
    public sealed class EmptyCollection : ICollection
    {
        #region Private Instance Constructors

        private EmptyCollection()
        {
        }

        #endregion Private Instance Constructors

        #region Public Static Properties

        public static EmptyCollection Instance
        {
            get { return s_instance; }
        }

        #endregion Public Static Properties

        #region Implementation of ICollection

        public void CopyTo(System.Array array, int index)
        {
            // copy nothing
        }

        public bool IsSynchronized
        {
            get { return true; }
        }

        public int Count
        {
            get { return 0; }
        }

        public object SyncRoot
        {
            get { return this; }
        }

        #endregion Implementation of ICollection

        #region Implementation of IEnumerable

        public IEnumerator GetEnumerator()
        {
            return NullEnumerator.Instance;
        }

        #endregion Implementation of IEnumerable

        #region Private Static Fields

        private readonly static EmptyCollection s_instance = new EmptyCollection();

        #endregion Private Static Fields
    }
}
