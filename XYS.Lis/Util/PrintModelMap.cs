using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace XYS.Lis.Util
{
   public class PrintModelMap
   {
       #region
       private Hashtable m_mapNo2PdfModel;
       #endregion

       #region
       public PrintModelMap()
       {
           this.m_mapNo2PdfModel = new Hashtable(20);
       }
       #endregion

       #region
       public PrintModel this[int modelNo]
       {
           get
           {
               lock (this)
               {
                   return this.m_mapNo2PdfModel[modelNo] as PrintModel;
               }
           }
       }
       public PrintModelCollection AllModels
       {
           get 
           {
               lock (this)
               {
                   return new PrintModelCollection(this.m_mapNo2PdfModel.Values);
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
       public void Add(PrintModel model)
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
