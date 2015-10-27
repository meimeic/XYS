using System;

namespace XYS.Lis.Config
{
    [AttributeUsage(AttributeTargets.Assembly)]
    [Serializable]
    public class RepositoryAttribute:Attribute
    {
        #region Public Instance Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryAttribute" /> class.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Default constructor.
        /// </para>
        /// </remarks>
        public RepositoryAttribute()
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="RepositoryAttribute" /> class 
        /// with the name of the repository.
        /// </summary>
        /// <param name="name">The name of the repository.</param>
        /// <remarks>
        /// <para>
        /// Initialize the attribute with the name for the assembly's repository.
        /// </para>
        /// </remarks>
        public RepositoryAttribute(string name)
        {
            m_name = name;
        }

        #endregion Public Instance Constructors

        #region Public Instance Properties

        /// <summary>
        /// Gets or sets the name of the logging repository.
        /// </summary>
        /// <value>
        /// The string name to use as the name of the repository associated with this
        /// assembly.
        /// </value>
        /// <remarks>
        /// <para>
        /// This value does not have to be unique. Several assemblies can share the
        /// same repository. They will share the logging configuration of the repository.
        /// </para>
        /// </remarks>
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        public Type RepositoryType
        {
            get { return m_repositoryType; }
            set { m_repositoryType = value; }
        }

        #endregion Public Instance Properties

        #region Private Instance Fields

        private string m_name = null;
        private Type m_repositoryType = null;

        #endregion Private Instance Fields
    }
}
