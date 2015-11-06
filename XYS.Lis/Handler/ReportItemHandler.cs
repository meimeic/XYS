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

        public ReportItemHandler()
            : this(m_defaultHandlerName)
        {
        }
        public ReportItemHandler(string handlerName)
            : base(handlerName)
        {
        }
        #region
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
                ReportItemElement rcie = reportElement as ReportItemElement;
                if (rcie != null)
                {
                    OperateItem(rcie);
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
            if (elementTag == ReportElementTag.ReportElement)
            {
                OperateReportList(reportElementList);
                return HandlerResult.Continue;
            }
            else if (elementTag == ReportElementTag.ItemElement)
            {
                OperateItems(reportElementList);
                return HandlerResult.Continue;
            }
            else
            {
                return HandlerResult.Continue;
            }
        }
        public override HandlerResult ReportOptions(Hashtable reportElementTable, ReportElementTag elementTag)
        {
            if (elementTag == ReportElementTag.ReportElement)
            {
                OperateReportTable(reportElementTable);
                return HandlerResult.Continue;
            }
            else if (elementTag == ReportElementTag.ItemElement)
            {
                OperateItems(reportElementTable);
                return HandlerResult.Continue;
            }
            else
            {
                return HandlerResult.Continue;
            }
        }
        #endregion

        #region
        protected override void OperateReport(ReportReportElement rre)
        {
            Hashtable table = rre.ItemTable[ReportElementTag.ItemElement] as Hashtable;
            if (table != null)
            {
                OperateItems(table);
            }
        }
        #endregion
        
        #region
        protected virtual void OperateItems(List<ILisReportElement> itemList)
        {
            if (itemList.Count > 0)
            {
                ReportItemElement item;
                for (int i = itemList.Count - 1; i >= 0; i--)
                {
                    item = itemList[i] as ReportItemElement;
                    //处理 删除数据等
                    OperateItem(item);
                }
            }
        }
        protected virtual void OperateItems(Hashtable table)
        {
            if (table.Count > 0)
            {
                ReportItemElement item;
                foreach(DictionaryEntry de in table)
                {
                    item =de.Value as ReportItemElement;
                    //处理 删除数据等
                    if (item != null)
                    {
                        OperateItem(item);
                    }
                }
            }
        }
        protected virtual void OperateItem(ReportItemElement rcie)
        {
            //
        }
        protected bool IsRemoveByItemNo(int itemNo)
        {
            return TestItem.GetHideItemNo.Contains(itemNo);
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
