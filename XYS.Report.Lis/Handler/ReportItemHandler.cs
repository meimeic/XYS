using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;

using XYS.Report.Lis.Core;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public class ReportItemHandler : ReportHandlerSkeleton
    {
        #region 字段
        private static readonly string m_defaultHandlerName = "ReportItemHandler";
        private readonly List<int> m_convertItemList;
        #endregion

        #region 构造函数
        public ReportItemHandler()
            : this(m_defaultHandlerName)
        {
        }
        public ReportItemHandler(string handlerName)
            : base(handlerName)
        {
            this.m_convertItemList = new List<int>(20);
            InitConvertItemList();
        }
        #endregion

        #region 实现父类抽象方法
        protected override bool OperateReport(ReportReportElement report)
        {
            //报告级操作
            ReportItemElement rie = null;
            List<ILisReportElement> itemList = report.GetReportItem(typeof(ReportItemElement));
            ReportKVElement kv = new ReportKVElement();
            if (IsExist(itemList))
            {
                foreach (ILisReportElement item in itemList)
                {
                    rie = item as ReportItemElement;
                    if (rie == null)
                    {
                        continue;
                    }
                    //元素转换
                    if (Convert2KVElement(rie, kv))
                    {
                        continue;
                    }
                    //元素处理
                    if (!OperateItem(rie))
                    {
                        continue;
                    }
                    report.ReportItemList.Add(rie);
                }
                if (kv.Count > 0)
                {
                    List<ReportKVElement> kvList = GetReportKVList(report);
                    kvList.Add(kv);
                }
            }
            report.RemoveReportItem(typeof(ReportItemElement));
            return true;
        }
        #endregion

        #region item项转换成KV项
        private bool Convert2KVElement(ReportItemElement rie, ReportKVElement kv)
        {
            if (m_convertItemList.Contains(rie.ItemNo))
            {
                kv.Add(rie.ItemCName, rie.ItemResult);
                return true;
            }
            return false;
        }
        #endregion

         #region 检验项处理逻辑
        protected bool OperateItem(ReportItemElement rie)
        {
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

        #region 调用的方法
        private void InitConvertItemList()
        {
            m_convertItemList.Clear();

            m_convertItemList.Add(90009288);
            m_convertItemList.Add(90009289);
            m_convertItemList.Add(90009290);
            m_convertItemList.Add(90009291);
            m_convertItemList.Add(90009292);
            m_convertItemList.Add(90009293);
            m_convertItemList.Add(90009294);
            m_convertItemList.Add(90009295);
            m_convertItemList.Add(90009296);
            m_convertItemList.Add(90009297);
            m_convertItemList.Add(90009300);
            m_convertItemList.Add(90009301);

            m_convertItemList.Add(90008528);
            m_convertItemList.Add(90008797);
            m_convertItemList.Add(90008798);
            m_convertItemList.Add(90008799);
        }
        //private bool ItemConvert2Custom(ReportItemElement rie, List<ILisReportElement> customList)
        //{
        //    bool result = false;
        //    switch (rie.ItemNo)
        //    {
        //        case 90009288:     //血常规项目c8
        //        case 90009289:      //c9
        //        case 90009290:     //c10
        //        case 90009291:     //c11
        //        case 90009292:     //c12
        //        case 90009293:    //c13
        //        case 90009294:    //c14
        //        case 90009300:    //c0  
        //        case 90009295:    //c15
        //        case 90009296:    //c16
        //        case 90009297:   //c17
        //        case 90009301:   //c1
        //            result = true;
        //            AddItem2CustomList(rie, customList, "ManCustom");
        //            break;
        //        case 90008528:    //染色体
        //        case 90008797:
        //        case 90008798:
        //        case 90008799:
        //            result = true;
        //            AddItem2CustomList(rie, customList, "RanCustom");
        //            break;
        //        default:
        //            break;
        //    }
        //    return result;
        //}
        //private void AddItem2CustomList(ReportItemElement item, List<ILisReportElement> customElementList, string name)
        //{
        //    string propertyName;
        //    if (customElementList.Count == 0)
        //    {
        //        customElementList.Add(new ReportCustomElement());
        //    }
        //    ReportCustomElement rce = customElementList[0] as ReportCustomElement;
        //    if (rce != null)
        //    {
        //        rce.Name = name;
        //        propertyName = GetCustomPropertyName(item.ItemNo);
        //        SetCustomProperty(propertyName, item.ItemResult, rce);
        //    }
        //}
        //private void SetCustomProperty(string propertyName, object value, ReportCustomElement rce)
        //{
        //    try
        //    {
        //        PropertyInfo pro = rce.GetType().GetProperty(propertyName);
        //        if (pro != null)
        //        {
        //            pro.SetValue(rce, value, null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //private string GetCustomPropertyName(int itemNo)
        //{
        //    int m = itemNo % ReportCustomElement.COLUMN_COUNT;
        //    return "Column" + m;
        //}
        //private bool IsConvert2KVElement(ReportItemElement rie, List<IReportElement> kvList)
        //{
        //    bool result = false;
        //    ReportKVElement rkv = null;
        //    if (kvList!=null&&kvList.Count == 0)
        //    {
        //        kvList.Add(new ReportKVElement());
        //    }
        //    //
        //    rkv = kvList[0] as ReportKVElement;
        //    if (rkv == null)
        //    {
        //        return false;
        //    }
        //    switch (rie.ItemNo)
        //    {
        //        case 90009288:     //血常规项目c8
        //        case 90009289:      //c9
        //        case 90009290:     //c10
        //        case 90009291:     //c11
        //        case 90009292:     //c12
        //        case 90009293:    //c13
        //        case 90009294:    //c14
        //        case 90009300:    //c0  
        //        case 90009295:    //c15
        //        case 90009296:    //c16
        //        case 90009297:   //c17
        //        case 90009301:   //c1
        //            rkv.Name = "ManTable";
        //            Item2KVTable(rie, rkv.KVTable);
        //            result = true;
        //            break;
        //        case 90008528:    //染色体
        //        case 90008797:
        //        case 90008798:
        //        case 90008799:
        //            rkv.Name = "RanTable";
        //            Item2KVTable(rie, rkv.KVTable);
        //            result = true;
        //            break;
        //        default:
        //            break;
        //    }
        //    return result;
        //}
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
