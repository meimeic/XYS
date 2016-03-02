using System;
using System.Reflection;
using System.Collections.Generic;

using XYS.Lis.Model;
using XYS.Lis.Core;
namespace XYS.Lis.Handler
{
    public class ReportCustomHandler : ReportHandlerSkeleton
    {
        #region
        private static readonly string m_defaultHandlerName = "ReportCustomHandler";
        #endregion

        #region 构造函数
        public ReportCustomHandler()
            : this(m_defaultHandlerName)
        {
        }
        public ReportCustomHandler(string handlerName)
            : base(handlerName)
        {
        }
        #endregion

        #region 实现父类抽象方法
        protected override bool OperateElement(ILisReportElement element)
        {
            ReportCustomElement rce = element as ReportCustomElement;
            if (rce != null)
            {
                return true;
            }
            return false;
        }
        protected override bool OperateReport(ReportReportElement report)
        {
            return OperateCustom(report);
        }
        #endregion
        //可以添加一些custom项的内部处理逻辑
        #region
        protected virtual bool OperateCustom(ReportReportElement rre)
        {
            //if (rre.SectionNo == 45)
            //{
            //    List<IReportElement> customList = rre.GetReportItem(typeof(ReportCustomElement).Name);
            //    List<ReportCustomElement> tempList = new List<ReportCustomElement>(15);
            //    if (customList.Count > 0)
            //    {
            //        ReportCustomElement rec;
            //        for (int i = customList.Count - 1; i >= 0; i--)
            //        {
            //            rec = customList[i] as ReportCustomElement;
            //            if (rec != null)
            //            {
            //                tempList.Add(rec);
            //            }
            //            customList.RemoveAt(i);
            //        }
            //        //
            //        List<ReportCustomElement> searchList = new List<ReportCustomElement>(5);
            //        MergeCustomList(tempList, customList, searchList);
            //    }
            //}
            OperateElementList(rre.GetReportItem(typeof(ReportCustomElement).Name));
            return true;
        }
        #endregion

        #region 合并自定义项
        //protected virtual void MergeCustomList(List<ReportCustomElement> customList, List<ILisReportElement> resultList, List<ReportCustomElement> searchList)
        //{
        //    string sampleNo;
        //    ReportCustomElement targeCustom = null;
        //    if (customList.Count > 0)
        //    {
        //        sampleNo = customList[customList.Count - 1].Column0;
        //        searchList.Clear();
        //        FindAll(sampleNo, customList, searchList);
        //        if (searchList.Count > 1)
        //        {
        //            targeCustom = new ReportCustomElement();
        //            MergeCommonList(searchList, targeCustom);
        //        }
        //        else
        //        {
        //            if (searchList.Count == 1)
        //            {
        //                targeCustom = searchList[0];
        //            }
        //        }
        //        if (targeCustom != null)
        //        {
        //            resultList.Add(targeCustom);
        //        }
        //        MergeCustomList(customList, resultList, searchList);
        //    }
        //}
        //private void MergeCommonList(List<ReportCustomElement> commonList, ReportCustomElement target)
        //{
        //    ReportCustomElement rce;
        //    //相同数据处理
        //    target.Column0 = commonList[0].Column0;
        //    target.Column1 = commonList[0].Column1;
        //    target.Column2 = commonList[0].Column2;
        //    target.Column3 = commonList[0].Column3;
        //    //不同数据处理
        //    Type type = typeof(ReportCustomElement);
        //    for (int i = 0; i < commonList.Count; i++)
        //    {
        //        rce = commonList[i];
        //        if (rce.Column4 != null && !rce.Column4.Equals(""))
        //        {
        //            SetCustomProperty(target, rce.Column4);
        //        }
        //        if (rce.Column5 != null && !rce.Column5.Equals(""))
        //        {
        //            string[] values = rce.Column5.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        //            if (values.Length > 0)
        //            {
        //                for (int j = 0; j < values.Length; j++)
        //                {
        //                    SetCustomProperty(target, values[j]);
        //                }
        //            }
        //        }
        //    }
        //}
        //private void SetCustomProperty(ReportCustomElement rce, object value)
        //{
        //    PropertyInfo pro;
        //    int index = rce.ColumnIndex;
        //    string propertyName = "Column" + index;
        //    try
        //    {
        //        pro = rce.GetType().GetProperty(propertyName);
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
        //private string GetCustomPropertyName(int index)
        //{
        //    int m = index % ReportCustomElement.COLUMN_COUNT;
        //    return "Column" + m;
        //}
        //private void FindAll(string sampleNo, List<ReportCustomElement> customList, List<ReportCustomElement> searchList)
        //{
        //    for (int i = customList.Count - 1; i >= 0; i--)
        //    {
        //        if (customList[i].Column0.Equals(sampleNo))
        //        {
        //            searchList.Add(customList[i]);
        //            customList.RemoveAt(i);
        //        }
        //    }
        //}
        #endregion
    }
}
