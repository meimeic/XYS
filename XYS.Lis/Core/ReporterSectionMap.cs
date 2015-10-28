using System;
using System.Collections;

namespace XYS.Lis.Core
{
   public class ReporterSectionMap
   {
       #region
       private Hashtable m_mapNo2ReporterSection;
       #endregion

       #region
       public ReporterSectionMap()
       {
           this.m_mapNo2ReporterSection = new Hashtable(100);
       }
       #endregion

       #region
       public void Clear()
       {
           this.m_mapNo2ReporterSection.Clear();
       }
       public ReporterSection this[int no]
       {
           get 
           {
               lock (this)
               {
                   return (ReporterSection)this.m_mapNo2ReporterSection[no];
               }
           }
       }
       public int Count
       {
           get { return this.m_mapNo2ReporterSection.Count; }
       }
       public ReporterSectionCollection AllReporterSection
       {
           get
           {
               lock (this)
               {
                   return new ReporterSectionCollection(this.m_mapNo2ReporterSection.Values);
               }
           }
       }
       public void Add(int sectionNo, string reporterName)
       {
           ReporterSection rs = new ReporterSection(sectionNo, reporterName);
           this.Add(rs);
       }
       public void Add(ReporterSection rs)
       {
           if(rs==null)
           {
               throw new ArgumentNullException("rs");
           }
           lock(this)
           {
               this.m_mapNo2ReporterSection.Add(rs.SectionNo,rs);
           }
       }
       #endregion
   }
}
