using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace XYS.Lis.Util
{
   public class ReportModelMap
   {
       #region
       private Hashtable m_mapNo2PdfModel;
       #endregion

       #region
       public ReportModelMap()
       {
           this.m_mapNo2PdfModel = new Hashtable();
       }
       #endregion

       #region
       public ReportModel this[int modelNo]
       {
           get
           {
               lock (this)
               {
                   return this.m_mapNo2PdfModel[modelNo] as ReportModel;
               }
           }
       }
       public ReportModelCollection AllPdfModel
       {
           get 
           {
               lock (this)
               {
                   return new ReportModelCollection(this.m_mapNo2PdfModel.Values);
               }
           }
       }
       public int Count
       {
           get { return this.m_mapNo2PdfModel.Count; }
       }
       #endregion

       #region
       public void Clear()
       {
           this.m_mapNo2PdfModel.Clear();
       }
       public void Add(ReportModel model)
       {
           if (model == null)
           {
               throw new ArgumentNullException("model");
           }
           lock (this)
           {
               this.m_mapNo2PdfModel.Add(model.ModelNo, model);
           }
       }
       #endregion
   }
}
