using System.Collections.Generic;

using XYS.Model;
using XYS.Util;
using XYS.FRReport.Model;
namespace XYS.FRReport.Convert
{
    public interface IFRConvert : IConvert
    {
        string ConverterName { get; }
        void convert(IReportElement report, IFRExportElement export);
        void convert(List<IReportElement> reportElements, List<IFRExportElement> exportElements);
    }
}