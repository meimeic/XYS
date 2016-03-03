using System;
using System.Collections.Generic;

using XYS.Model;
namespace XYS.Report.DAL
{
    public interface IReportDAL
    {
        void Fill(IReportElement element, string sql);
        void FillList(List<IReportElement> elementList, Type elementType, string sql);
    }
}
