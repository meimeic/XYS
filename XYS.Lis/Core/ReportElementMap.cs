using System;
using System.Collections;

namespace XYS.Lis.Core
{
   public class ReportElementMap
   {
       #region
       private Hashtable m_mapName2ElementType;
       #endregion
       public ReportElementMap()
       {
           this.m_mapName2ElementType = new Hashtable();
       }
       public void Clear()
       {
           this.m_mapName2ElementType.Clear();
       }
       public Type this[string name]
       {
           get 
           {
               if (name == null)
               {
                   throw new ArgumentNullException("name");
               }
               lock (this)
               {
                   return (Type)this.m_mapName2ElementType[name];
               }
           }
       }
       public void Add(string name, Type elementType)
       {
           lock (this)
           {
               this.m_mapName2ElementType[name] = elementType;
           }
       }

   }
}
