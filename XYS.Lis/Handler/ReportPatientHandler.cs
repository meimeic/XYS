using System;
using System.Collections.Generic;

using XYS.Lis.Core;
using XYS.Lis.Model;
namespace XYS.Lis.Handler
{
   public class ReportPatientHandler:ReportHandlerSkeleton
    {
       #region 静态变量
       public static readonly string m_defaultHandlerName = "ReportPatientHandler";
       #endregion

       #region
       public ReportPatientHandler()
           : this(m_defaultHandlerName)
       { }
       public ReportPatientHandler(string name)
           : base(name)
       { }
       #endregion

       #region 实现父类抽象方法
       protected override bool OperateElement(IReportElement element)
       {
           if (element.ElementTag == ReportElementTag.Report)
           {
               ReportReportElement rre = element as ReportReportElement;
               return OperatePatientList(rre);
           }
           if (element.ElementTag == ReportElementTag.Patient)
           {
               ReportPatientElement rpe = element as ReportPatientElement;
               return OperatePatient(rpe);
           }
           return true;
       }
       #endregion

        #region
       protected virtual bool OperatePatientList(ReportReportElement rre)
       {
           List<IReportElement> patientList = rre.GetReportItem(typeof(ReportPatientElement).Name);
           OperateElementList(patientList, typeof(ReportPatientElement));
           if (patientList.Count > 0)
           {
               ReportPatientElement rpe = patientList[0] as ReportPatientElement;
               rre.ClinicType = rpe.ClinicType;
               return true;
           }
           return false;
       }
       protected virtual bool OperatePatient(ReportPatientElement rpe)
       {
           return true;
       }
        #endregion
    }
}
