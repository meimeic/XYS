using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Lis.Core;
namespace XYS.Lis.DAL
{
    public interface ILisReportDAL
    {
        void Fill(ILisReportElement element, Hashtable equalTable);
        void FillList(List<ILisReportElement> elementList, Type elementType, Hashtable equalTable);
    }
}
