using System;

using XYS.Lis.Core;
using XYS.Lis.Export;

namespace XYS.Lis.Export.HTML
{
    public class Export2Html : ReportExportSkeleton
    {
        private static readonly ExportTag DEFAULT_EXPORT = ExportTag.HTML;
        public Export2Html()
            : this("Export2Html")
        {
 
        }
        public Export2Html(string name)
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
