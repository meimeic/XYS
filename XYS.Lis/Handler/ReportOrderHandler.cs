//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;

//using XYS.Model;
//using XYS.Lis.Model;
//using XYS.Lis.Core;
//using XYS.Lis.Util;
//namespace XYS.Lis.Handler
//{
//    public class ReportOrderHandler:ReportHandlerSkeleton
//    {
//        #region 变量
//        private static readonly string m_defaultHandlerName = "ReportOrderHandler";
//        private Hashtable m_section2Order;
//        private Hashtable m_parItem2Order;
//        #endregion

//        #region 构造函数
//        public ReportOrderHandler()
//            : this(m_defaultHandlerName)
//        {
//        }
//        public ReportOrderHandler(string handlerName)
//            : base(handlerName)
//        {
//            this.m_section2Order = new Hashtable();
//            this.m_parItem2Order = new Hashtable();
//        }
//        #endregion
        
//        #region 实现父类继承接口的抽象方法
//        public override HandlerResult ReportOptions(ILisReportElement reportElement)
//        {
//            IsElementAndOperate(reportElement, reportElement.ElementTag);
//            return HandlerResult.Continue;
//        }
//        public override HandlerResult ReportOptions(List<ILisReportElement> reportElementList, ReportElementTag elementTag)
//        {
//            if (elementTag == ReportElementTag.ReportElement)
//            {
//                OperateElementList(reportElementList, elementTag);
//                return HandlerResult.Continue;
//            }
//            else
//            {
//                return HandlerResult.Continue;
//            }
//        }
//        #endregion
        
//        #region 实现父类的抽象方法
//        protected override void OperateReport(ReportReportElement rre)
//        {
//            ReportExamElement ree;
//            List<ILisReportElement> examElementList = rre.GetReportItem(ReportElementTag.ExamElement);
//            if (examElementList.Count > 0)
//            {
//                ree = examElementList[0] as ReportExamElement;
//                if (ree != null)
//                {
//                    //按照检验大项设置
//                    SetReportOrderNoByParItem(rre);
//                    if (rre.OrderNo <= 0)
//                    {
//                        SetReportOrderNoBySection(rre, ree.SectionNo);
//                    }
//                }
//            }
//        }
//        protected override bool IsElementAndOperate(ILisReportElement reportElement, ReportElementTag elementTag)
//        {
//            bool result = false;
//            ReportReportElement rre;
//            if (elementTag == ReportElementTag.ReportElement)
//            {
//                rre = reportElement as ReportReportElement;
//                if (rre != null)
//                {
//                    result = true;
//                    OperateReport(rre);
//                }
//            }
//            return result;
//        }
//        #endregion

//        #region
//        protected virtual void SetReportOrderNoBySampleType(ReportReportElement rre, int sampleTypeNo)
//        {

//        }
//        protected virtual void SetReportOrderNoBySection(ReportReportElement rre, int sectionNo)
//        {
//            int preOrder = this.GetOrderNoBySectionNo(sectionNo);
//            if (preOrder > 1000)
//            {
//                rre.OrderNo = preOrder;
//            }
//            else if (preOrder > 0&&preOrder<=1000)
//            {
//                int maxParItem = MaxOrder(rre.ParItemList);
//                if (maxParItem > 0)
//                {
//                    int sufOrder = maxParItem % 10000;
//                    rre.OrderNo = preOrder * 10000 + sufOrder;
//                }
//                else
//                {
//                    rre.OrderNo = preOrder * 10000;
//                }
//            }
//            else
//            {
//                rre.OrderNo = 0;
//            }
//        }
//        protected virtual void SetReportOrderNoByParItem(ReportReportElement rre)
//        {
//            List<int> orderedParItemList = this.GetOrderedParItemList();
//            rre.OrderNo = Intersection(rre.ParItemList, orderedParItemList);
//        }
//        //取有排序号的检验项与报告检验项的交集，并取得对应最大的排序号 
//        protected int GetOrderNoByParItemNo(int parItemNo)
//        {
//            if (this.m_parItem2Order.Count == 0)
//            {
//                this.InitParItem2OrderTable();
//            }
//            object orderNo = this.m_parItem2Order[parItemNo];
//            if (orderNo == null)
//            {
//                return 0;
//            }
//            else
//            {
//                return (int)orderNo;
//            }
//        }
//        protected int GetOrderNoBySectionNo(int sectionNo)
//        {
//            if (this.m_section2Order.Count == 0)
//            {
//                this.InitSection2OrderTable();
//            }
//            object orderNo = this.m_section2Order[sectionNo];
//            if (orderNo == null)
//            {
//                return 0;
//            }
//            else
//            {
//                return (int)orderNo;
//            }
//        }
//        protected List<int> GetOrderedParItemList()
//        {
//            List<int> result=new List<int>();
//            if (this.m_parItem2Order.Count == 0)
//            {
//                this.InitParItem2OrderTable();
//            }
//            int temp;
//            foreach (object c in this.m_parItem2Order.Keys)
//            {
//                try
//                {
//                    temp = Convert.ToInt32(c);
//                    result.Add(temp);
//                }
//                catch (Exception ex)
//                {
//                    continue;
//                }
//            }
//            return result;
//        }
//        protected int Intersection(List<int> first, List<int> second)
//        {
//            int re = 0;
//            int temp;
//            IEnumerable<int> result = first.Intersect<int>(second);
//            foreach (int i in result)
//            {
//                temp = this.GetOrderNoByParItemNo(i);
//                if (temp > re)
//                {
//                    re = temp;
//                }
//            }
//            return re;
//        }
//        protected int MaxOrder(List<int> l)
//        {
//            int result = -1;
//            foreach (int order in l)
//            {
//                if (order > result)
//                {
//                    result = order;
//                }
//            }
//            return result;
//        }
//        #endregion

//        #region  内部实例方法
//        private void InitSection2OrderTable()
//        {
//            LisMap.InitSection2OrderNoTable(this.m_section2Order);
//        }
//        private void InitParItem2OrderTable()
//        {
//            LisMap.InitParItem2OrderNoTable(this.m_parItem2Order);
//        }
//        #endregion

//    }
//}
