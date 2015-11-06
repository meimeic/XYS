using System;
using System.Collections.Generic;
using XYS.Lis.Model;
using XYS.Lis.Export;
using XYS.Lis.Core;
namespace XYS.Lis.Export.Json
{
    public class Export2Json:ReportExportSkeleton
    {
        private static readonly ExportTag DEFAULT_EXPORT=ExportTag.JSON;
        public Export2Json()
            : this("Export2Json")
        {
        }
        public Export2Json(string name)
            : base(name)
        {
            this.ExportTag = DEFAULT_EXPORT;
        }
        protected override string InnerElementExport(ILisReportElement reportElement)
        {
            throw new NotImplementedException();
        }

        protected override string InnerElementsExport(List<ILisReportElement> elementList)
        {
            throw new NotImplementedException();
        }

        protected override string InnerReportExport(ReportReportElement rre)
        {
            throw new NotImplementedException();
        }
    }
}
