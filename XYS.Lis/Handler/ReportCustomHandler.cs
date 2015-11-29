using System;
using System.Reflection;
using System.Collections.Generic;

using XYS.Model;
using XYS.Lis.Core;
using XYS.Lis.Model;

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
        protected override bool OperateElement(ILisReportElement element, ReportElementTag elementTag)
        {
            if (elementTag == ReportElementTag.ReportElement)
            {
                ReportReportElement rre = element as ReportReportElement;
                return OperateCustomList(rre);
            }
            if (elementTag == ReportElementTag.CustomElement)
            {
                ReportCustomElement rce = element as ReportCustomElement;
                return OperateCustom(rce);
            }
            return true;
        }
        #endregion
        //可以添加一些custom项的内部处理逻辑
        #region
        protected virtual bool OperateCustomList(ReportReportElement rre)
        {
            if (rre.SectionNo == 45)
            {
                List<ILisReportElement> customList = rre.GetReportItem(ReportElementTag.CustomElement);
                List<ReportCustomElement> tempList = new List<ReportCustomElement>(15);
                if (customList.Count > 0)
                {
                    ReportCustomElement rec;
                    for (int i = customList.Count - 1; i >= 0; i--)
                    {
                        rec = customList[i] as ReportCustomElement;
                        if (rec != null)
                        {
                            tempList.Add(rec);
                        }
                        customList.RemoveAt(i);
                    }
                    //
                    List<ReportCustomElement> searchList = new List<ReportCustomElement>(5);
                    MergeCustomList(tempList, customList, searchList);
                }
            }
            return true;
        }
        protected virtual bool OperateCustom(ReportCustomElement rce)
        {
            return true;
        }
        #endregion

        #region
        protected virtual void MergeCustomList(List<ReportCustomElement> customList,List<ILisReportElement> resultList, List<ReportCustomElement> searchList)
        {
            string sampleNo;
            if (customList.Count > 0)
            {
                sampleNo = customList[customList.Count-1].Column0;
                searchList.Clear();
                FindAll(sampleNo, customList, searchList);
                if (searchList.Count > 1)
                {
                    MergeCommonList(searchList);
                }
                if (searchList.Count > 0)
                {
                    resultList.Add(searchList[0]);
                }
                MergeCustomList(customList, resultList, searchList);
            }
        }
        private void MergeCommonList(List<ReportCustomElement> commonList)
        {
            ReportCustomElement rce;
            ReportCustomElement target = commonList[0];
            Type type = typeof(ReportCustomElement);
            for (int i = 1; i < commonList.Count; i++)
            {
                rce = commonList[i];
                if (rce.Column4 != null && !rce.Column4.Equals(""))
                {
                    SetCustomProperty(target, rce.Column4);
                }
                if (rce.Column5 != null && !rce.Column5.Equals(""))
                {
                    string[] values = rce.Column5.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length > 0)
                    {
                        for (int j = 0; j < values.Length; j++)
                        {
                            SetCustomProperty(target, values[j]);
                        }
                    }
                }
            }
        }
        private void SetCustomProperty(ReportCustomElement rce, object value)
        {
            PropertyInfo pro;
            int index = rce.ColumnIndex;
            string propertyName = "Column" + index;
            try
            {
                pro = rce.GetType().GetProperty(propertyName);
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
        private string GetCustomPropertyName(int index)
        {
            int m = index % ReportCustomElement.COLUMN_COUNT;
            return "Column" + m;
        }
        private void FindAll(string sampleNo, List<ReportCustomElement> customList, List<ReportCustomElement> searchList)
        {
            for (int i = customList.Count - 1; i >= 0; i--)
            {
                if (customList[i].Column0.Equals(sampleNo))
                {
                    searchList.Add(customList[i]);
                    customList.RemoveAt(i);
                }
            }
        }
        #endregion
    }
}
