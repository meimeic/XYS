using System;
using System.Collections.Generic;

using XYS.Lis.Core;
using XYS.Lis.Export;
using XYS.Lis.Model;

namespace XYS.Lis.Export.XML
{
    public class Report2XML:ReportExportSkeleton
    {
        private static readonly ExportTag DEFAULT_EXPORT = ExportTag.XML;
        public Report2XML()
            : this("Report2XML")
        {
        }
        public Report2XML(string name)
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
