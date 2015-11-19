using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Model;
using XYS.Lis.Model;
using XYS.Lis.Export;
using XYS.Lis.Core;

namespace XYS.Lis.Export.Json
{
    public class Export2Json : ReportExportSkeleton
    {
        private static readonly ExportTag DEFAULT_EXPORT = ExportTag.JSON;

        private readonly Hashtable m_tag2Separate;

        public Export2Json()
            : this("Export2Json")
        {
        }
        public Export2Json(string name)
            : base(name)
        {
            this.ExportTag = DEFAULT_EXPORT;
            this.m_tag2Separate = new Hashtable(6);
        }
        protected override string InnerElementExport(ILisReportElement reportElement)
        {
            throw new NotImplementedException();
        }

        protected override string InnerReportExport(ReportReportElement rre)
        {
            throw new NotImplementedException();
        }


        protected override string GetSeparateByTag(ReportElementTag elementTag)
        {
            string temp = this.m_tag2Separate[elementTag] as string;
            if (temp != null)
            {
                return temp;
            }
            else
            {
                return "";
            }
        }
    }
}
