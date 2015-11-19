using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using XYS.Model;
using XYS.Lis.Core;
using XYS.Lis.Model;
using XYS.Lis.Util;
namespace XYS.Lis.Handler
{
    public class ReportReportHandler : ReportHandlerSkeleton
    {
        #region 变量
        private static readonly string m_defaultHandlerName = "ReportReportHandler";
        private Hashtable m_section2Order;
        private Hashtable m_parItem2Order;
        private Hashtable m_parItem2PrintModel;
        private Hashtable m_section2PrintModel;
        #endregion

        #region 构造函数
        public ReportReportHandler()
            : this(m_defaultHandlerName)
        {
        }
        public ReportReportHandler(string handlerName)
            : base(handlerName)
        {
            this.m_section2Order = new Hashtable(20);
            this.m_section2PrintModel = new Hashtable(20);
            this.m_parItem2Order = new Hashtable(30);
            this.m_parItem2PrintModel = new Hashtable(30);
        }
        #endregion

        #region 实现父类继承接口的虚方法
        public override HandlerResult ReportOptions(ILisReportElement reportElement)
        {
            ReportReportElement rre;
            bool result = IsElement(reportElement, reportElement.ElementTag);
            if (result)
            {
                if (reportElement.ElementTag == ReportElementTag.ReportElement)
                {
                    rre = reportElement as ReportReportElement;
                    OperateReport(rre);
                }
                else
                {
                    OperateSubElement(reportElement,reportElement.ElementTag);
                }
                return HandlerResult.Continue;
            }
            else
            {
                return HandlerResult.Fail;
            }
        }
        public override HandlerResult ReportOptions(List<ILisReportElement> reportElementList, ReportElementTag elementTag)
        {
            if (elementTag != ReportElementTag.NoneElement)
            {
                OperateElementList(reportElementList, elementTag);
                return HandlerResult.Continue;
            }
            else
            {
                return HandlerResult.Fail;
            }
        }
        #endregion

        #region 实现父类虚方法
        protected override void OperateElement(ILisReportElement element, ReportElementTag elementTag)
        {
            ReportReportElement rre;
            if (elementTag == ReportElementTag.ReportElement)
            {
                rre = element as ReportReportElement;
                OperateReport(rre);
            }
            else
            {
                OperateSubElement(element, elementTag);
            }
        }
        #endregion

        #region
        protected virtual void OperateReport(ReportReportElement rre)
        {
            OperateInfoList(rre);
            //处理item元素
            OperateItemList(rre);

            //设置排序号
            SetReportOrder(rre);
            //设置模板号
            SetReportModel(rre);
            //remark
            SetRemark(rre);
        }
        protected virtual void OperateSubElement(ILisReportElement element, ReportElementTag elementTag)
        {
            switch (elementTag)
            {
                case ReportElementTag.InfoElement:
                    ReportInfoElement rie1 = element as ReportInfoElement;
                    OperateInfo(rie1);
                    break;
                case ReportElementTag.ItemElement:
                    ReportItemElement rie = element as ReportItemElement;
                    OperateItem(rie);
                    break;
                case ReportElementTag.GraphElement:
                    ReportGraphElement rge = element as ReportGraphElement;
                    OperateGraph(rge);
                    break;
                case ReportElementTag.CustomElement:
                    ReportCustomElement rce = element as ReportCustomElement;
                    OperateCustom(rce);
                    break;
                default:
                    break;
            }
        }
        #endregion
        
        #region
        protected virtual void OperateInfoList(ReportReportElement rre)
        {
            ReportInfoElement rie;
            if (rre.SectionNo == 10)
            {
                List<ILisReportElement> infoList = rre.GetReportItem(ReportElementTag.InfoElement);
                if (infoList.Count > 0)
                {
                    rie = infoList[0] as ReportInfoElement;
                    OperateInfo(rie);
                }
            }
        }
        protected virtual void OperateItemList(ReportReportElement rre)
        {
            ReportItemElement rie;
            List<ILisReportElement> itemList = rre.GetReportItem(ReportElementTag.ItemElement);
            if (itemList.Count > 0)
            {
                for (int i = itemList.Count - 1; i >= 0; i--)
                {
                    rie = itemList[i] as ReportItemElement;
                    if (IsRemoveBySecret(rie.SecretGrade))
                    {
                        itemList.RemoveAt(i);
                    }
                    else
                    {
                        OperateSubElement(rie, ReportElementTag.ItemElement);
                    }
                }
            }
        }
        protected virtual void SetRemark(ReportReportElement rre)
        {
            if (rre.RemarkFlag && rre.ClinicType == ClinicType.clinic)
            {
                rre.Remark = "带*为天津市临床检测中心认定的相互认可检验项目";
            }
        }
        #endregion
        
        #region
        private void OperateInfo(ReportInfoElement rie)
        {
            //检验信息处理
            if (rie.SectionNo == 10)
            {
                if (rie.FormMemo != null)
                {
                    rie.FormMemo = rie.FormMemo.Replace(";", SystemInfo.NewLine);
                }
            }
        }
        private void OperatePatient(ReportPatientElement rpe)
        {
            //
        }
        private void OperateItem(ReportItemElement rie)
        {
            //检验项通用处理
            if (rie.ItemNo == 50004360 || rie.ItemNo == 50004370)
            {
                rie.RefRange = rie.RefRange.Replace(";", SystemInfo.NewLine);
            }
        }
        private void OperateGraph(ReportGraphElement rgie)
        {
            //
        }
        private void OperateCustom(ReportCustomElement customItem)
        {
            //通用处理
        }
        #endregion

        #region
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
        
        #region 排序号设置相关方法
        protected virtual void SetReportOrder(ReportReportElement rre)
        {
            SetReportOrderNoByParItem(rre);
            if (rre.OrderNo <= 0)
            {
                SetReportOrderNoBySection(rre);
            }
        }
        protected virtual void SetReportOrderNoBySampleType(ReportReportElement rre)
        {
        }
        protected virtual void SetReportOrderNoBySection(ReportReportElement rre)
        {
            int preOrder = this.GetOrderNoBySectionNo(rre.SectionNo);
            if (preOrder > 1000)
            {
                rre.OrderNo = preOrder;
            }
            else if (preOrder > 0 && preOrder <= 1000)
            {
                int maxParItem = MaxOrder(rre.ParItemList);
                if (maxParItem > 0)
                {
                    int sufOrder = maxParItem % 10000;
                    rre.OrderNo = preOrder * 10000 + sufOrder;
                }
                else
                {
                    rre.OrderNo = preOrder * 10000;
                }
            }
            else
            {
                rre.OrderNo = 0;
            }
        }
        protected virtual void SetReportOrderNoByParItem(ReportReportElement rre)
        {
            List<int> orderedParItemList = this.GetOrderedParItemList();
            rre.OrderNo = Intersection(rre.ParItemList, orderedParItemList);
        }
        protected int GetOrderNoByParItemNo(int parItemNo)
        {
            if (this.m_parItem2Order.Count == 0)
            {
                this.InitParItem2OrderTable();
            }
            object orderNo = this.m_parItem2Order[parItemNo];
            if (orderNo == null)
            {
                return 0;
            }
            else
            {
                return (int)orderNo;
            }
        }
        protected int GetOrderNoBySectionNo(int sectionNo)
        {
            if (this.m_section2Order.Count == 0)
            {
                this.InitSection2OrderTable();
            }
            object orderNo = this.m_section2Order[sectionNo];
            if (orderNo == null)
            {
                return 0;
            }
            else
            {
                return (int)orderNo;
            }
        }
        protected List<int> GetOrderedParItemList()
        {
            List<int> result = new List<int>();
            if (this.m_parItem2Order.Count == 0)
            {
                this.InitParItem2OrderTable();
            }
            int temp;
            foreach (object c in this.m_parItem2Order.Keys)
            {
                try
                {
                    temp = Convert.ToInt32(c);
                    result.Add(temp);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return result;
        }
        protected int Intersection(List<int> first, List<int> second)
        {
            int re = 0;
            int temp;
            IEnumerable<int> result = first.Intersect<int>(second);
            foreach (int i in result)
            {
                temp = this.GetOrderNoByParItemNo(i);
                if (temp > re)
                {
                    re = temp;
                }
            }
            return re;
        }
        protected int MaxOrder(List<int> l)
        {
            int result = -1;
            foreach (int order in l)
            {
                if (order > result)
                {
                    result = order;
                }
            }
            return result;
        }
        #endregion

        #region 模板号设置相关方法
        protected virtual void SetReportModel(ReportReportElement rre)
        {
            SetReportModelNoByParItem(rre);
            if (rre.PrintModelNo <= 0)
            {
                SetReportModelNoBySectionNo(rre);
            }
        }
        protected virtual void SetReportModelNoBySectionNo(ReportReportElement rre)
        {
            rre.PrintModelNo = this.GetReportModelNoBySectionNo(rre.SectionNo);
        }
        protected virtual void SetReportModelNoByParItem(ReportReportElement rre)
        {
            List<int> printModelNoList = new List<int>(5);
            foreach (int item in rre.ParItemList)
            {
                printModelNoList.Add(this.GetReportModelNoByParItemNo(item));
            }
            rre.PrintModelNo = GetMax(printModelNoList);
        }
        protected int GetReportModelNoByParItemNo(int parItemNo)
        {
            if (this.m_parItem2PrintModel.Count == 0)
            {
                this.InitParItem2ReportModelTable();
            }
            object modelNo = this.m_parItem2PrintModel[parItemNo];
            if (modelNo == null)
            {
                return -1;
            }
            else
            {
                return (int)modelNo;
            }
        }
        protected int GetReportModelNoBySectionNo(int sectionNo)
        {
            if (this.m_section2PrintModel.Count == 0)
            {
                this.InitSection2PrintModelTable();
            }
            object modelNo = this.m_section2PrintModel[sectionNo];
            if (modelNo == null)
            {
                return -1;
            }
            else
            {
                return (int)modelNo;
            }
        }
        protected int GetMax(List<int> source)
        {
            int result = -1;
            if (source.Count > 0)
            {
                result = source[0];
                foreach (int i in source)
                {
                    if (i > result)
                    {
                        result = i;
                    }
                }
            }
            return result;
        }
        #endregion

        #region  内部实例方法
        private void InitParItem2ReportModelTable()
        {
            LisMap.InitParItem2ReportModelTable(this.m_parItem2PrintModel);
        }
        private void InitSection2PrintModelTable()
        {
            LisMap.InitSection2PrintModelTable(this.m_section2PrintModel);
        }
        private void InitSection2OrderTable()
        {
            LisMap.InitSection2OrderNoTable(this.m_section2Order);
        }
        private void InitParItem2OrderTable()
        {
            LisMap.InitParItem2OrderNoTable(this.m_parItem2Order);
        }

        #endregion
    }
}
