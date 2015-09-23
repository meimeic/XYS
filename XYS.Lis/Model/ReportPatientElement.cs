using XYS.Model;
namespace XYS.Lis.Model
{
    internal class ReportPatientElement : AbstractPatientElement
    {
        #region 私有常量字段
        private const ReportElementType elementType = ReportElementType.PatientElement;
        #endregion

        #region 私有字段
        #endregion

        #region 公共属性
        #endregion
        #region 构造函数
        internal ReportPatientElement()
            : base(elementType)
        {
        }
        internal ReportPatientElement(ReportElementType elementType)
            : base(elementType)
        {
        }
        #endregion
    }
}
