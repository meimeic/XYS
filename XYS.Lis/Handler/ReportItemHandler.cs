using System;
using System.Reflection;
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

        #region 实现父类抽象方法
        protected override bool OperateElement(ILisReportElement element, ReportElementTag elementTag)
        {
            if (elementTag == ReportElementTag.ReportElement)
            {
                ReportReportElement rre = element as ReportReportElement;
                return OperateItemList(rre);
            }
            if (elementTag == ReportElementTag.InfoElement)
            {
                ReportItemElement rie = element as ReportItemElement;
                return OperateItem(rie);
            }
            return true;
        }
        #endregion

        #region item项的内部处理逻辑
        protected virtual bool OperateItemList(ReportReportElement rre)
        {
            //item 处理
            ReportItemElement rie;
            List<ILisReportElement> itemElementList = rre.GetReportItem(ReportElementTag.ItemElement);
            List<ILisReportElement> kvList = rre.GetReportItem(ReportElementTag.KVElement);
            if (itemElementList.Count > 0)
            {
                for (int i = itemElementList.Count - 1; i >= 0; i--)
                {
                    rie = itemElementList[i] as ReportItemElement;
                    if (rie != null)
                    {
                        //设置ParItemList
                        SetParItemListByItem(rre.ParItemList, rie);
                        //通过item设置备注
                        SetRemarkFlagByItem(rre, rie);
                        //item是否转换为kv
                        if (ItemConvert2KV(rie, kvList))
                        {
                            itemElementList.RemoveAt(i);
                        }
                        //是否删除
                        if (ItemIsDelete(rie))
                        {
                            itemElementList.RemoveAt(i);
                        }
                    }
                    else
                    {
                        itemElementList.RemoveAt(i);
                    }
                }
            }
            return true;
        }
        protected virtual bool OperateItem(ReportItemElement rie)
        {
            //删除
            if (ItemIsDelete(rie))
            {
                return false;
            }
            //不删除，执行处理代码
            if (rie.ItemNo == 50004360 || rie.ItemNo == 50004370)
            {
                if (rie.RefRange != null)
                {
                    rie.RefRange = rie.RefRange.Replace(";", SystemInfo.NewLine);
                }
            }
            return true;
        }
        #endregion

        #region item项转换成KV项
        private bool ItemConvert2KV(ReportItemElement rie, List<ILisReportElement> kvList)
        {
            bool result = false;
            ReportKVElement rkv = null;
            if (kvList!=null&&kvList.Count == 0)
            {
                kvList.Add(new ReportKVElement());
            }
            //
            rkv = kvList[0] as ReportKVElement;
            if (rkv == null)
            {
                return false;
            }
            switch (rie.ItemNo)
            {
                case 90009288:     //血常规项目c8
                case 90009289:      //c9
                case 90009290:     //c10
                case 90009291:     //c11
                case 90009292:     //c12
                case 90009293:    //c13
                case 90009294:    //c14
                case 90009300:    //c0  
                case 90009295:    //c15
                case 90009296:    //c16
                case 90009297:   //c17
                case 90009301:   //c1
                    rkv.Name = "ManTable";
                    AddItem2KVTable(rie, rkv.KVTable);
                    result = true;
                    break;
                case 90008528:    //染色体
                case 90008797:
                case 90008798:
                case 90008799:
                    rkv.Name = "RanTable";
                    AddItem2KVTable(rie, rkv.KVTable);
                    result = true;
                    break;
                default:
                    break;
            }
            return result;
        }
        private void AddItem2KVTable(ReportItemElement item, Hashtable kvTable)
        {
            kvTable.Add(item.ItemCName, item.ItemResult);
        }
        #endregion

        #region item转换成custom
        private bool ItemConvert2Custom(ReportItemElement rie, List<ILisReportElement> customList)
        {
            bool result = false;
            switch (rie.ItemNo)
            {
                case 90009288:     //血常规项目c8
                case 90009289:      //c9
                case 90009290:     //c10
                case 90009291:     //c11
                case 90009292:     //c12
                case 90009293:    //c13
                case 90009294:    //c14
                case 90009300:    //c0  
                case 90009295:    //c15
                case 90009296:    //c16
                case 90009297:   //c17
                case 90009301:   //c1
                    result = true;
                    AddItem2CustomList(rie, customList, "ManCustom");
                    break;
                case 90008528:    //染色体
                case 90008797:
                case 90008798:
                case 90008799:
                    result = true;
                    AddItem2CustomList(rie, customList, "RanCustom");
                    break;
                default:
                    break;
            }
            return result;
        }
        private void AddItem2CustomList(ReportItemElement item, List<ILisReportElement> customElementList, string name)
        {
            string propertyName;
            if (customElementList.Count == 0)
            {
                customElementList.Add(new ReportCustomElement());
            }
            ReportCustomElement rce = customElementList[0] as ReportCustomElement;
            if (rce != null)
            {
                rce.Name = name;
                propertyName = GetCustomPropertyName(item.ItemNo);
                SetCustomProperty(propertyName, item.ItemResult, rce);
            }
        }
        private void SetCustomProperty(string propertyName, object value, ReportCustomElement rce)
        {
            try
            {
                PropertyInfo pro = rce.GetType().GetProperty(propertyName);
                if (pro != null)
                {
                    pro.SetValue(rce, value, null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string GetCustomPropertyName(int itemNo)
        {
            int m = itemNo % ReportCustomElement.COLUMN_COUNT;
            return "Column" + m;
        }
        #endregion

        #region 是否删除
        private bool ItemIsDelete(ReportItemElement rie)
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

        #region
        private void SetRemarkFlagByItem(ReportReportElement rre,ReportItemElement rie)
        {
            //
        }
        private void SetParItemListByItem(List<int> parItemList, ReportItemElement rie)
        {
            if (!parItemList.Contains(rie.ParItemNo))
            {
                parItemList.Add(rie.ParItemNo);
            }
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
