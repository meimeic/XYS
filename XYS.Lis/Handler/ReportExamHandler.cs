using System;
using System.Collections.Generic;

using XYS.Lis.Core;
using XYS.Lis.Model;
using XYS.Lis.Util;
namespace XYS.Lis.Handler
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
        protected override bool OperateElement(ILisReportElement element)
        {
            ReportExamElement ree = element as ReportExamElement;
            if (ree != null)
            {
                //此处可以添加判断是否删除代码

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
            return OperateExam(report);
        }
        #endregion

        #region 内部处理逻辑
        protected virtual bool OperateExam(ReportReportElement rre)
        {
            rre.SectionNo = rre.ReportExam.SectionNo;
            rre.ParItemName = rre.ReportExam.ParItemName;

            rre.ReceiveDateTime = rre.ReportExam.ReceiveDateTime;
            rre.CollectDateTime = rre.ReportExam.CollectDateTime;
            rre.InceptDateTime = rre.ReportExam.InceptDateTime;
            rre.CheckDateTime = rre.ReportExam.CheckDateTime;
            rre.SecondeCheckDateTime = rre.ReportExam.SecondeCheckDateTime;
            rre.TestDateTime = rre.ReportExam.TestDateTime;

            //设置签名图片
            if (!string.IsNullOrEmpty(rre.ReportExam.Checker))
            {
                rre.CheckerImage = LisPUser.GetSignImage(rre.ReportExam.Checker);
            }
            if (string.IsNullOrEmpty(rre.ReportExam.Technician))
            {
                rre.TechnicianImage = LisPUser.GetSignImage(rre.ReportExam.Technician);
            }

            return OperateElement(rre.ReportExam);
        }
        #endregion
    }
}
