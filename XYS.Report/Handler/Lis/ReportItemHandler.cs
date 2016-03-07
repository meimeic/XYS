using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;
using XYS.Model;
using XYS.Report.Model.Lis;
namespace XYS.Report.Handler.Lis
{
    public class ReportItemHandler : ReportHandlerSkeleton
    {
        #region
        private static readonly string m_defaultHandlerName = "ReportItemHandler";
        private readonly Hashtable m_convertItemMap;
        #endregion

        #region 构造函数
        public ReportItemHandler()
            : this(m_defaultHandlerName)
        {
        }
        public ReportItemHandler(string handlerName)
            : base(handlerName)
        {
            this.m_convertItemMap = new Hashtable(16);
        }
        #endregion

        #region 实现父类抽象方法
        protected override bool OperateReport(ReportReportElement report)
        {
            //报告级操作
            return OperateItem(report);
        }
        protected override bool OperateElement(IReportElement element)
        {
            //元素级操作
            ReportItemElement rie = element as ReportItemElement;
            if (rie != null)
            {
                //删除此项
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
            return false;
        }
        #endregion

        #region item内部处理逻辑
        protected virtual bool OperateItem(ReportReportElement rre)
        {
            //item 处理
            ReportItemElement rie=null;
            List<IReportElement> itemElementList = rre.GetReportItem(typeof(ReportItemElement).Name);
            ReportKVElement kve = GetKVElement(rre.SectionNo);
            for (int i = itemElementList.Count - 1; i >= 0; i--)
            {
                rie = itemElementList[i] as ReportItemElement;
                if (rie == null)
                {
                    itemElementList.RemoveAt(i);
                    continue;
                }

                //设置ParItemList 检验大项集合
                SetParItemListByItem(rre.ParItemList, rie);

                //通过item设置备注
                SetRemarkFlagByItem(rre, rie);
                
                //item是否转换为kv
                if (IsConvert2KVElement(rie, kve))
                {
                    itemElementList.RemoveAt(i);
                    continue;
                }
                //是否删除
                if (!OperateElement(rie))
                {
                    itemElementList.RemoveAt(i);
                    continue;
                }
            }
            if (kve != null)
            {
                List<IReportElement> kveList = rre.GetReportItem(typeof(ReportKVElement).Name);
                kveList.Add(kve);
            }
            return true;
        }
        #endregion

        #region item项转换成KV项
        private void Convert2KVElement(ReportReportElement rre)
        {

        }
        private bool IsConvert2KVElement(ReportItemElement rie, ReportKVElement rkve)
        {
            if (this.m_convertItemMap.Count == 0)
            {
                InitItem2CustomMap();
            }
            string key = this.m_convertItemMap[rie.ItemNo] as string;
            if (!string.IsNullOrEmpty(key))
            {
                //抛出异常
                if (rkve == null)
                {
                    throw new ArgumentNullException("");
                }
                rkve.Add(key, rie.ItemResult);
                return true;
            }
            return false;
        }
        //获取kv 若所在小组不需要kve 则返回null
        private ReportKVElement GetKVElement(int sectionNo)
        {
            ReportKVElement kve = null; 
            switch (sectionNo)
            {
                case 2:
                case 27:
                    kve = new ReportKVElement();
                    kve.Name = "ManTable";
                    break;
                case 11:
                    kve = new ReportKVElement();
                    kve.Name = "RanTable";
                    break;
                default:
                    break;
            }
            return kve;
        }
        #endregion

        #region 是否删除检验项
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

        #region 通过项设置 检验大项集合 备注标记
        private void SetRemarkFlagByItem(ReportReportElement rre,ReportItemElement rie)
        {
            //
            if (rie.ItemNo == 123)
            {
                rre.RemarkFlag = 2;
            }
        }
        private void SetParItemListByItem(List<int> parItemList, ReportItemElement rie)
        {
            if (!parItemList.Contains(rie.ParItemNo))
            {
                parItemList.Add(rie.ParItemNo);
            }
        }
        #endregion

        #region 调用的方法
        private void InitItem2CustomMap()
        {
            this.m_convertItemMap.Clear();
            lock (this.m_convertItemMap)
            {
                this.m_convertItemMap.Add(90009288, "Column0");
                this.m_convertItemMap.Add(90009289, "Column1");
                this.m_convertItemMap.Add(90009290, "Column2");
                this.m_convertItemMap.Add(90009291, "Column3");
                this.m_convertItemMap.Add(90009292, "Column4");
                this.m_convertItemMap.Add(90009293, "Column5");
                this.m_convertItemMap.Add(90009294, "Column6");
                this.m_convertItemMap.Add(90009295, "Column7");
                this.m_convertItemMap.Add(90009296, "Column8");
                this.m_convertItemMap.Add(90009297, "Column9");
                this.m_convertItemMap.Add(90009300, "Column10");
                this.m_convertItemMap.Add(90009301, "Column11");

                this.m_convertItemMap.Add(90008528, "Column8");
                this.m_convertItemMap.Add(90008797, "Column9");
                this.m_convertItemMap.Add(90008798, "Column10");
                this.m_convertItemMap.Add(90008799, "Column11");
            }
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
