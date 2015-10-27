using System;
using XYS.Lis.Core;
using XYS.Lis.Handler;
using XYS.Lis.Export;
namespace XYS.Lis.Appender
{
    public interface IAppender
    {
        string AppenderName { get; set; }
       
        void AddHandler(IReportHandler handler);
        void ClearHandler();
        void AddExport(IReportExport export);
        void ClearExport();
        void Close();
        string DoAppend(ILisReportElement reportElement,ExportTag exportTag);
    }
}
