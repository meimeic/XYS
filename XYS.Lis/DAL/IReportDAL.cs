using System;
using System.Collections.Generic;

using XYS.Lis.Core;
namespace XYS.Lis.DAL
{
    public interface IReportDAL
    {
        void Fill(ILisReportElement element, string sql);
        void FillList(List<ILisReportElement> elementList, Type elementType, string sql);
    }
}
