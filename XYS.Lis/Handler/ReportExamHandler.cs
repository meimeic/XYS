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
        protected override bool OperateElement(IReportElement element)
        {
            if (element.ElementTag == ReportElementTag.Report)
            {
                ReportReportElement rre = element as ReportReportElement;
                return OperateExamList(rre);
            }
            if (element.ElementTag == ReportElementTag.Exam)
            {
                ReportExamElement ree = element as ReportExamElement;
                return OperateExam(ree);
            }
            return true;
        }
        #endregion

        #region 内部处理逻辑
        protected virtual bool OperateExamList(ReportReportElement rre)
        {
            List<IReportElement> examList = rre.GetReportItem(typeof(ReportExamElement).Name);
            OperateElementList(examList, typeof(ReportExamElement));
            if (examList.Count > 0)
            {
                ReportExamElement ree = examList[0] as ReportExamElement;
                rre.SectionNo = ree.SectionNo;
                rre.ParItemName = ree.ParItemName;
                rre.ReceiveDateTime = ree.ReceiveDateTime;
                rre.CollectDateTime = ree.CollectDateTime;
                rre.InceptDateTime = ree.InceptDateTime;
                rre.TestDateTime = ree.TestDateTime;
                rre.CheckDateTime = ree.CheckDateTime;
                rre.SecondeCheckDateTime = ree.SecondeCheckDateTime;
                //设置签名图片
                if (!string.IsNullOrEmpty(ree.Checker))
                {
                    rre.CheckerImage = LisPUser.GetSignImage(ree.Checker);
                }
                if (string.IsNullOrEmpty(ree.Technician))
                {
                    rre.TechnicianImage = LisPUser.GetSignImage(ree.Technician);
                }
                return true;
            }
            return false;
        }
        protected virtual bool OperateExam(ReportExamElement ree)
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
        #endregion
    }
}
