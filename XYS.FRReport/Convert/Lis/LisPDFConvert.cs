namespace XYS.FRReport.Convert.Lis
{
    public class LisPDFConvert:FRConvertSkeleton
    {
        private static readonly string m_converterName = "LisPDFConvert";

        public LisPDFConvert()
            : this(m_converterName)
        {
        }
        public LisPDFConvert(string name)
            : base(name)
        {
        }

        protected override void InnerElementConvert(XYS.Model.IReportElement report, Model.IFRExportElement export)
        {
            throw new System.NotImplementedException();
        }

        protected override void AfterConvert(Model.IFRExportElement export)
        {
            throw new System.NotImplementedException();
        }
    }
}
