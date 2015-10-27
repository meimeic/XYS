using System;
using System.Collections;

namespace XYS.Lis.Util
{
    public class NullEnumerator : IEnumerator
    {
        #region Private Instance Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NullEnumerator" /> class. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// Uses a private access modifier to enforce the singleton pattern.
        /// </para>
        /// </remarks>
        private NullEnumerator()
        {
        }

        #endregion Private Instance Constructors

        #region Public Static Properties

        /// <summary>
        /// Get the singleton instance of the <see cref="NullEnumerator" />.
        /// </summary>
        /// <returns>The singleton instance of the <see cref="NullEnumerator" />.</returns>
        /// <remarks>
        /// <para>
        /// Gets the singleton instance of the <see cref="NullEnumerator" />.
        /// </para>
        /// </remarks>
        public static NullEnumerator Instance
        {
            get { return s_instance; }
        }

        #endregion Public Static Properties

        #region Implementation of IEnumerator

        public object Current
        {
            get { throw new InvalidOperationException(); }
        }

        public bool MoveNext()
        {
            return false;
        }

        public void Reset()
        {
        }

        #endregion Implementation of IEnumerator

        #region Private Static Fields

        private readonly static NullEnumerator s_instance = new NullEnumerator();

        #endregion Private Static Fields
    }
}
