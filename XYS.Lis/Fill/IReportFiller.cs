using System;
using System.Collections;
using System.Collections.Generic;

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
        void Fill(ILisReportElement reportElement, ReportKey key);
        void Fill(List<ILisReportElement> reportElementList, ReportKey key, ReportElementTag elementTag);

        // void AddSection2ElementTypes(int sectionNo, ReportElementTypeCollection elementTypeCollection);
    }
}
