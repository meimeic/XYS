using System;
using XYS.Lis.Core;
using XYS.Lis.Fill;
using XYS.Lis.Export;
using XYS.Lis.Export.PDF;
using XYS.Lis.Export.Json;
using XYS.Lis.Handler;
namespace XYS.Lis.Repository.Hierarchy
{
    public class DefaultReporter : Reporter
    {
        #region
        private static readonly string m_strategyName = "defaultstrategy";
        #endregion
        
        #region
        public DefaultReporter()
            : base("defaultReporter", m_strategyName)
        {
        }
        #endregion

        #region
        //private void BuildInReportFillMap()
        //{
        //    this.ClearFiller();
        //    IReportFiller filler = new ReportFillByDB();
        //    this.AddFiller(filler);
        //    this.DefaultFill = filler;
        //}
        //private void BuildInReportExportMap()
        //{
        //    this.ClearExport();
        //    IReportExport export;
        //    export = new Export2PDF();
        //    this.AddExport(export);
        //    this.DefaultExport = export;
        //    export = new Export2Json();
        //    this.AddExport(export);
        //}
        //private void BuildInHandler()
        //{
        //    this.ClearHandler();
        //    this.AddHandler(new HeaderDefaultHandler());
        //}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
        #endregion
    }
}
