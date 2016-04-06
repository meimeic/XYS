using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public class ReportItemHandler : ReportHandlerSkeleton
    {
        #region 字段
        private static readonly string m_defaultHandlerName = "ReportItemHandler";
        #endregion

        #region 构造函数
        public ReportItemHandler()
            : this(m_defaultHandlerName)
        {
        }
        public ReportItemHandler(string handlerName)
            : base(handlerName)
        {
        }
        #endregion

        #region 实现父类抽象方法
        protected override HandlerResult OperateReport(ReportReportElement report)
        {
            //报告级操作
            ReportItemElement rie = null;
            List<AbstractSubFillElement> itemList = report.GetReportItem(typeof(ReportItemElement));
            if (IsExist(itemList))
            {
                foreach (AbstractSubFillElement item in itemList)
                {
                    rie = item as ReportItemElement;
                    //元素处理
                    if (!OperateItem(rie))
                    {
                        continue;
                    }
                    report.ReportItemCollection.Add(rie);
                }
            }
            return new HandlerResult();
        }
        #endregion

        #region 检验项处理逻辑
        protected bool OperateItem(ReportItemElement rie)
        {
            if (rie == null)
            {
                return false;
            }
            if (ItemDelete(rie))
            {
                return false;
            }
            //不删除，检验项处理操作
            if (rie.ItemNo == 50004360 || rie.ItemNo == 50004370)
            {
                if (rie.RefRange != null)
                {
                    rie.RefRange = rie.RefRange.Replace(";", SystemInfo.NewLine);
                }
            }
            return true;
        }
        private bool ItemDelete(ReportItemElement rie)
        {
            return IsRemoveBySecret(rie.SecretGrade);
        }
        protected bool IsRemoveBySecret(int secretGrade)
        {
            if (secretGrade > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}