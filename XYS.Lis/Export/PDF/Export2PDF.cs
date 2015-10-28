using System;
using System.Collections.Generic;

using XYS.Lis.Core;
using XYS.Lis.Export;
using XYS.Lis.Model;
using XYS.Model;

namespace XYS.Lis.Export.PDF
{
    public class Export2PDF:ReportExportSkeleton
    {
        private static readonly ExportTag DEFAULT_EXPORT = ExportTag.PDF;

        #region
        public Export2PDF()
            : this("Export2PDF")
        {
        }
        public Export2PDF(string name)
            : base(name)
        {
            this.ExportTag = DEFAULT_EXPORT;
        }
        #endregion

        #region
        protected override string InnerElementExport(ILisReportElement reportElement)
        {
            throw new NotImplementedException();
        }

        protected virtual string GenderPDF(ReportReportElement rre)
        {
            return null;
        }
        #endregion

        protected override string InnerElementListExport(List<ILisReportElement> elementList)
        {
            throw new NotImplementedException();
        }

        protected override string InnerReportExport(ReportReportElement rre)
        {
            throw new NotImplementedException();
        }
    }
}
