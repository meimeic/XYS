using XYS.Lis.Core;
namespace XYS.Lis.Model
{
   public abstract class LisPatientElement:ILisPatientElement
   {
       #region 私有字段
       private readonly string m_patientElementName;
       private readonly long m_patientElementValue;
       #endregion

       #region 受保护的构造方法
       protected LisPatientElement(string name,long value)
       {
           m_patientElementName = name;
           m_patientElementValue = value;
       }
       #endregion

       #region 实现ILisPatientElement接口
       public string PatientElementName
        {
            get { return m_patientElementName; }
        }
        public long PatientElementValue
        {
            get { return m_patientElementValue; }
        }
       #endregion
    }
}
