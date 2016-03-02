using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Lis.Core;
using XYS.Lis.Model;
using XYS.Lis.Export.Model;
namespace XYS.Lis.Export
{
    public class JsonExport : ReportExportSkeleton
    {
        private readonly static string m_defaultExportName = "JsonExport";
        public JsonExport()
            : this(m_defaultExportName)
        {
        }
        public JsonExport(string name)
            : base(name)
        {
        }
        protected override void ConvertGraph2Image(List<ILisReportElement> graphList, List<IExportElement> imageList)
        {
            throw new NotImplementedException();
        }

        protected override void AfterExport(ReportReport export)
        {

        }
    }
}
