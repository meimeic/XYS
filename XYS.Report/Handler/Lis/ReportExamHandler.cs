using System;
using System.Collections.Generic;

using XYS.Util;
using XYS.Model;
using XYS.Report.Util;
using XYS.Report.Model.Lis;
namespace XYS.Report.Handler.Lis
{
    public class ReportExamHandler : ReportHandlerSkeleton
    {
        #region 静态变量
        public static readonly string m_defaultHandlerName = "ReportExamHandler";
        #endregion

        #region 构造函数
        public ReportExamHandler()
            : this(m_defaultHandlerName)
        {
        }
        public ReportExamHandler(string name)
            : base(name)
        {
        }
        #endregion
        
        #region 实现父类抽象方法
        protected override bool OperateElement(IReportElement element)
        {
            //exam 元素级别处理
            ReportExamElement ree = element as ReportExamElement;
            if (ree != null)
            {
                //处理代码
                if (ree.SectionNo == 10)
                {
                    if (ree.FormMemo != null)
                    {
                        ree.FormMemo = ree.FormMemo.Replace(";", SystemInfo.NewLine);
                    }
                }
                return true;
            }
            return false;
        }
        protected override bool OperateReport(ReportReportElement report)
        {
            //顶级处理
            return OperateExam(report);
        }
        #endregion

        #region 内部处理逻辑
        protected virtual bool OperateExam(ReportReportElement rre)
        {
            //根据exam元素进行处理
            //rre.SectionNo = rre.ReportExam.SectionNo;
            //rre.ParItemName = rre.ReportExam.ParItemName;
            //rre.ReceiveDateTime = rre.ReportExam.ReceiveDateTime;
            //rre.CollectDateTime = rre.ReportExam.CollectDateTime;
            //rre.InceptDateTime = rre.ReportExam.InceptDateTime;
            //rre.CheckDateTime = rre.ReportExam.CheckDateTime;
            //rre.SecondeCheckDateTime = rre.ReportExam.SecondeCheckDateTime;
            //rre.TestDateTime = rre.ReportExam.TestDateTime;

            //设置签名图片
            if (!string.IsNullOrEmpty(rre.Checker))
            {
                rre.CheckerImage = LisPUser.GetSignImage(rre.Checker);
            }
            if (string.IsNullOrEmpty(rre.Technician))
            {
                rre.TechnicianImage = LisPUser.GetSignImage(rre.Technician);
            }
            return OperateElement(rre.ReportExam);
        }
        #endregion
    }
}
