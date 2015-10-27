using System;

using XYS.Lis.Core;
using XYS.Lis.Export;

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
    }
}
