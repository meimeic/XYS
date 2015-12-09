using System;
using System.Collections.Generic;

using XYS.Model;
using XYS.Lis.Core;
using XYS.Lis.Service.Models.Report;

namespace XYS.Lis.Service.Common
{
    public interface IModelConvert
    {
        bool Convert2Export(ILisReportElement reportElement, IReportModel exportElement);
        bool Convert2Export(List<ILisReportElement> reportElementList, List<IReportModel> exportElementList, ReportElementTag elementTag);
    }
}
