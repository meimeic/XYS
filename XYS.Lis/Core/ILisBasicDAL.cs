using System.Collections;
using System.Collections.Generic;
using XYS.Lis.Model;
namespace XYS.Lis.Core
{
   public interface ILisBasicDAL<T>
    {
       T Search(Hashtable equalTable);
       void Search(T t, Hashtable equalTable);
       List<T> SearchList(Hashtable equalTable);
       void SearchList(List<T> lt, Hashtable equalTable);
    }
}
