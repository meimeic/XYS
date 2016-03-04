using System;
using System.Collections;
namespace XYS.Report.Core
{
    public class LisSectionMap
    {
        #region
        private readonly Hashtable m_no2LisSectionTable;
        private static readonly int DEFAULT_CAPACITY = 20;
        #endregion

        #region
        public LisSectionMap()
            : this(DEFAULT_CAPACITY)
        { }
        public LisSectionMap(int c)
        {
            this.m_no2LisSectionTable = new Hashtable(c);
        }
        #endregion

        #region
        public void Clear()
        {
            this.m_no2LisSectionTable.Clear();
        }
        public LisSection this[int no]
        {
            get
            {
                lock (this)
                {
                    return this.m_no2LisSectionTable[no] as LisSection;
                }
            }
        }
        public int Count
        {
            get { return this.m_no2LisSectionTable.Count; }
        }
        public ICollection AllReporterSection
        {
            get
            {
                return this.m_no2LisSectionTable.Values;
            }
        }
        public void Add(int sectionNo)
        {
            LisSection ls = new LisSection(sectionNo);
            this.Add(ls);
        }
        public void Add(int sectionNo, string reporterName)
        {
            LisSection ls = new LisSection(sectionNo, reporterName);
            this.Add(ls);
        }
        public void Add(LisSection ls)
        {
            if (ls == null)
            {
                throw new ArgumentNullException("ls");
            }
            lock (this)
            {
                this.m_no2LisSectionTable[ls.SectionNo] = ls;
            }
        }
        #endregion
    }
}
