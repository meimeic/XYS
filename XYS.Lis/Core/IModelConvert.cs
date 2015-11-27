using System;
using System.Collections.Generic;

using XYS.Model;

namespace XYS.Lis.Core
{
    public interface IModelConvert
    {
        bool Convert2Export(ILisReportElement reportElement, ILisExportElement exportElement);
        bool Convert2Export(List<ILisReportElement> reportElementList, List<ILisExportElement> exportElementList,ReportElementTag elementTag);
    }
}
