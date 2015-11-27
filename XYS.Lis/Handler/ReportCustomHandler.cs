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
                List<ReportCustomElement> tempList = new List<ReportCustomElement>(30);
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
                        else
                        {
                            customList.RemoveAt(i);
                        }
                    }
                    //
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
        protected virtual void MergeCustomList(int count,List<ReportCustomElement> customList,List<ILisReportElement> resultList)
        {
            string sampleNo;
            if (customList.Count > 0)
            {
                sampleNo = customList[count-1].Column0;
                List<ReportCustomElement> searchList = new List<ReportCustomElement>(10);
                FindAll(sampleNo, customList, searchList);

            }
        }
        private void MergeCommonList(List<ReportCustomElement> commonList)
        {

            int index;
            ReportCustomElement rce;
            ReportCustomElement target = commonList[0];
            Type type = typeof(ReportCustomElement);
            string pn1,pn2;
            PropertyInfo p1, p2;
            for (int i = 1; i < commonList.Count; i++)
            {
                rce = commonList[i];
                index = target.ColumnIndex;
                p1=type.GetProperty()
            }
        }
        private void SetCustomProperty(string propertyName, ReportCustomElement rce, string propertyName1, ReportCustomElement rce1)
        {
            try
            {
                PropertyInfo pro = rce.GetType().GetProperty(propertyName);
                if (pro != null)
                {
                    PropertyInfo pro1=
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
