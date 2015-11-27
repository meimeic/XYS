using System;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;

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
        #region
        protected override string InnerExport(ILisExportElement exportElement)
        {
            return JsonConvert.SerializeObject(exportElement, Formatting.Indented);
        }

        protected override string InnerExport(List<ILisExportElement> exportElementList, ReportElementTag elementTag)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
