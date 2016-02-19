using System;
using System.Collections;

namespace XYS.Lis.Core
{
   public class ReportSectionMap
   {
       #region
       private Hashtable m_mapNo2ReporterSection;
       #endregion

       #region
       public ReportSectionMap()
       {
           this.m_mapNo2ReporterSection = new Hashtable(30);
       }
       #endregion

       #region
       public void Clear()
       {
           this.m_mapNo2ReporterSection.Clear();
       }
       public ReportSection this[int no]
       {
           get 
           {
               lock (this)
               {
                   return (ReportSection)this.m_mapNo2ReporterSection[no];
               }
           }
       }
       public int Count
       {
           get { return this.m_mapNo2ReporterSection.Count; }
       }
       public ReportSectionCollection AllReporterSection
       {
           get
           {
               lock (this)
               {
                   return new ReportSectionCollection(this.m_mapNo2ReporterSection.Values);
               }
           }
       }
       public void Add(int sectionNo, string reporterName)
       {
           ReportSection rs = new ReportSection(sectionNo, reporterName);
           this.Add(rs);
       }
       public void Add(ReportSection rs)
       {
           if (rs == null)
           {
               throw new ArgumentNullException("rs");
           }
           lock (this)
           {
               this.m_mapNo2ReporterSection[rs.SectionNo] = rs;
           }
       }
       #endregion
   }
}
