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

       #region 构造函数
       public ReportPatientHandler()
           : this(m_defaultHandlerName)
       { }
       public ReportPatientHandler(string name)
           : base(name)
       { }
       #endregion

       #region 实现父类抽象方法
       protected override bool OperateElement(ILisReportElement element)
       {
           //二级操作
           ReportPatientElement rpe = element as ReportPatientElement;
           if (rpe != null)
           {
               return true;
           }
           return false;
       }
       protected override bool OperateReport(ReportReportElement rre)
       {
           //顶级操作

           //List<IReportElement> patientList = rre.GetReportItem(typeof(ReportPatientElement).Name);
           //OperateElementList(patientList, typeof(ReportPatientElement));
           //if (patientList.Count > 0)
           //{
           //    ReportPatientElement rpe = patientList[0] as ReportPatientElement;
           //    rre.ClinicType = rpe.ClinicType;
           //    return true;
           //}
           //return false;
           return OperatePatient(rre);
       }
       #endregion

        #region
       protected virtual bool OperatePatient(ReportReportElement rre)
       {
           //一级操作
           return OperateElement(rre.ReportPatient);
       }
        #endregion
    }
}
