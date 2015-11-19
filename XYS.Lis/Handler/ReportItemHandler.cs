using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Model;
using XYS.Lis.Core;
using XYS.Lis.Model;
using XYS.Lis.Util;
namespace XYS.Lis.Handler
{
    public class ReportItemHandler : ReportHandlerSkeleton
    {
        private static readonly string m_defaultHandlerName = "ReportItemHandler";

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

        #region 实现父类继承接口的抽象方法
        public override HandlerResult ReportOptions(ILisReportElement reportElement)
        {
            if (reportElement.ElementTag == ReportElementTag.ReportElement)
            {
                ReportReportElement rre = reportElement as ReportReportElement;
                if (rre != null)
                {
                    OperateReport(rre);
                }
                return HandlerResult.Continue;
            }
            else if (reportElement.ElementTag == ReportElementTag.ItemElement)
            {
                ReportItemElement rie = reportElement as ReportItemElement;
                if (rie != null)
                {
                    OperateItem(rie);
                }
                return HandlerResult.Continue;
            }
            else
            {
                return HandlerResult.Continue;
            }
        }
        public override HandlerResult ReportOptions(List<ILisReportElement> reportElementList, ReportElementTag elementTag)
        {
            if (elementTag == ReportElementTag.ReportElement || elementTag == ReportElementTag.ItemElement)
            {
                OperateElementList(reportElementList, elementTag);
                return HandlerResult.Continue;
            }
            else
            {
                return HandlerResult.Continue;
            }
        }
        #endregion

        #region 实现父类抽象方法
        protected override void OperateElement(ILisReportElement element, ReportElementTag elementTag)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 内部实例方法
        protected void OperateReport(ReportReportElement rre)
        {
            //List<ILisReportElement> itemElementList = rre.GetReportItem(ReportElementTag.ItemElement);
            //if (itemElementList.Count > 0)
            //{
            //    OperateElementList(itemElementList, ReportElementTag.ItemElement);
            //}
            //List<ILisReportElement> examElementList = rre.GetReportItem(ReportElementTag.ExamElement);
            //if (examElementList.Count > 0)
            //{

            //}
        }
        protected virtual void OperateItem(ReportItemElement rie)
        {

        }
        protected bool IsRemoveByItemNo(int itemNo)
        {
            return TestItem.GetHideItemNo.Contains(itemNo);
        }
        #endregion

        #region
        //public override HandlerResult ReportOptions(Hashtable reportElementTable, ReportElementTag elementTag)
        //{
        //    if (elementTag == ReportElementTag.ReportElement)
        //    {
        //        OperateReportTable(reportElementTable);
        //        return HandlerResult.Continue;
        //    }
        //    else if (elementTag == ReportElementTag.ItemElement)
        //    {
        //        OperateItems(reportElementTable);
        //        return HandlerResult.Continue;
        //    }
        //    else
        //    {
        //        return HandlerResult.Continue;
        //    }
        //}
        //protected virtual void OperateItems(List<ILisReportElement> itemList)
        //{
        //    if (itemList.Count > 0)
        //    {
        //        ReportItemElement item;
        //        for (int i = itemList.Count - 1; i >= 0; i--)
        //        {
        //            item = itemList[i] as ReportItemElement;
        //            //处理 删除数据等
        //            OperateItem(item);
        //        }
        //    }
        //}
        //protected virtual void OperateItems(Hashtable table)
        //{
        //    if (table.Count > 0)
        //    {
        //        ReportItemElement item;
        //        foreach (DictionaryEntry de in table)
        //        {
        //            item = de.Value as ReportItemElement;
        //            //处理 删除数据等
        //            if (item != null)
        //            {
        //                OperateItem(item);
        //            }
        //        }
        //    }
        //}
        #endregion
    }
}
