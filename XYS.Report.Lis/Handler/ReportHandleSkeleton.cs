using System;
using System.Collections.Generic;

using log4net;

using XYS.Report;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
     delegate void HandleErrorHandler(ReportReportElement report);
     delegate void HandleSuccessHandler(ReportReportElement report);
     public abstract class ReportHandleSkeleton : ILisReportHandle
     {
         #region 静态字段
         protected static ILog LOG = LogManager.GetLogger("LisReportHandle");
         #endregion

         #region 事件字段
         private event HandleErrorHandler m_handleErrorEvent;
         private event HandleSuccessHandler m_handleSuccessEvent;
         #endregion

         #region 构造函数
         protected ReportHandleSkeleton()
         {
         }
         #endregion

         #region 实现IReportHandler接口
         internal event HandleErrorHandler HandleErrorEvent
         {
             add { this.m_handleErrorEvent += value; }
             remove { this.m_handleErrorEvent -= value; }
         }
         internal event HandleSuccessHandler HandleSuccessEvent
         {
             add { this.m_handleSuccessEvent += value; }
             remove { this.m_handleSuccessEvent -= value; }
         }
         public abstract void ReportHandle(ReportReportElement report);
         #endregion

         #region 辅助方法
         protected bool IsExist(List<IFillElement> elementList)
         {
             if (elementList != null && elementList.Count > 0)
             {
                 return true;
             }
             return false;
         }
         protected void SetHandlerResult(HandleResult result, int code, string message = null, Type type = null, Exception ex = null)
         {
             result.ResultCode = code;
             result.Message = message;
             result.HandleType = type;
             result.Exception = ex;
         }
         #endregion

         #region 触发事件
         protected void OnHandleError(ReportReportElement report)
         {
             HandleErrorHandler handler = this.m_handleErrorEvent;
             if (handler != null)
             {
                 handler(report);
             }
         }
         protected void OnHandleSuccess(ReportReportElement report)
         {
             HandleSuccessHandler handler = this.m_handleSuccessEvent;
             if (handler != null)
             {
                 handler(report);
             }
         }
         #endregion
     }
}
