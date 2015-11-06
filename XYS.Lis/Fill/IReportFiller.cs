using System;
using System.Collections;

using XYS.Model;
using XYS.Common;
using XYS.Lis.Core;
using XYS.Lis.DAL;
using XYS.Lis.Model;
namespace XYS.Lis.Fill
{
    public interface IReportFiller
    {
        string FillerName { get; }
        void AddSection2ElementTypes(int sectionNo, ReportElementTypeCollection elementTypeCollection);
        void Fill(ILisReportElement reportElement, ReportKey key);
        // ILisReportElement Fill(ReportKey key, ReportElementTag elementTag);
        void Fill(Hashtable reportElementTable, ReportKey key, ReportElementTag elementTag);
        //List<ILisReportElement> FillList(ReportKey key, ReportElementTag elementTag);
    }
}
