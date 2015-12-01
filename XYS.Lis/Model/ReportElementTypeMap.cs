using System;
using System.Collections;

using XYS.Model;
using XYS.Lis.Core;
namespace XYS.Lis.Model
{
   public class ReportElementTypeMap
   {
       #region
       private Hashtable m_mapName2ElementType;
       #endregion
       public ReportElementTypeMap()
       {
           this.m_mapName2ElementType = new Hashtable(6);
       }
       public void Clear()
       {
           this.m_mapName2ElementType.Clear();
       }
       public ReportElementType this[string elementName]
       {
           get 
           {
               lock (this)
               {
                   return this.m_mapName2ElementType[elementName] as ReportElementType;
               }
           }
       }
       public ReportElementTypeCollection AllElementTypes
       {
           get
           {
               lock (this)
               {
                   return new ReportElementTypeCollection(this.m_mapName2ElementType.Values);
               }
           }
       }
       public int Count
       {
           get { return this.m_mapName2ElementType.Count; }
       }
       public void Add(ReportElementType elementType)
       {
           if (elementType == null)
           {
               throw new ArgumentNullException("elementType");
           }
           lock (this)
           {
               this.m_mapName2ElementType[elementType.ElementName] = elementType;
           }
       }

   }
}
