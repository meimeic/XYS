using XYS.Lis.Core;
using XYS.Lis.Export;
namespace XYS.Lis.Appender
{
    public class AppenderImpl:AppenderSkeleton
    {
        protected override string Append(ILisReportElement reportElement,ExportTag exportTag)
        {
            return this.RenderExport(reportElement, exportTag);
        }
    }
}
