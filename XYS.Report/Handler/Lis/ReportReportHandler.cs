using System;

using XYS.Model;
using XYS.Report.Model;
using XYS.Report.Core;
namespace XYS.Report.Handler
{
    public class ReportReportHandler : ReportHandlerSkeleton
    {
        #region 变量
        private static readonly string m_defaultHandlerName = "ReportReportHandler";
        #endregion

        #region 构造函数
        public ReportReportHandler()
            : this(m_defaultHandlerName)
        {
        }
        public ReportReportHandler(string handlerName)
            : base(handlerName)
        {
        }
        #endregion

        #region 实现父类虚方法
        protected override bool OperateElement(ILisReportElement element)
        {
            return true;
        }
        #endregion

        #region
        protected override bool OperateReport(ReportReportElement rre)
        {
            //设置备注
            SetRemark(rre);
            return true;
        }
        #endregion

        #region 备注设置
        protected virtual void SetRemark(ReportReportElement rre)
        {
            if (rre.RemarkFlag == 2 && rre.ReportPatient.ClinicType == ClinicType.clinic)
            {
                rre.Remark = "带*为天津市临床检测中心认定的相互认可检验项目";
            }
        }
        #endregion

        //#region 排序号设置
        //protected virtual void SetReportOrder(ReportReportElement rre)
        //{
        //    SetReportOrderNoByParItem(rre);
        //    if (rre.OrderNo <= 0)
        //    {
        //        SetReportOrderNoBySection(rre);
        //    }
        //}
        //protected virtual void SetReportOrderNoBySampleType(ReportReportElement rre)
        //{
        //}
        //protected virtual void SetReportOrderNoBySection(ReportReportElement rre)
        //{
        //    int preOrder = this.GetOrderNoBySectionNo(rre.SectionNo);
        //    if (preOrder > 1000)
        //    {
        //        rre.OrderNo = preOrder;
        //    }
        //    else if (preOrder > 0 && preOrder <= 1000)
        //    {
        //        int maxParItem = MaxOrder(rre.ParItemList);
        //        if (maxParItem > 0)
        //        {
        //            int sufOrder = maxParItem % 10000;
        //            rre.OrderNo = preOrder * 10000 + sufOrder;
        //        }
        //        else
        //        {
        //            rre.OrderNo = preOrder * 10000;
        //        }
        //    }
        //    else
        //    {
        //        rre.OrderNo = 0;
        //    }
        //}
        //protected virtual void SetReportOrderNoByParItem(ReportReportElement rre)
        //{
        //    List<int> orderedParItemList = this.GetOrderedParItemList();
        //    rre.OrderNo = Intersection(rre.ParItemList, orderedParItemList);
        //}
        //protected int GetOrderNoByParItemNo(int parItemNo)
        //{
        //    if (this.m_parItem2Order.Count == 0)
        //    {
        //        this.InitParItem2OrderTable();
        //    }
        //    object orderNo = this.m_parItem2Order[parItemNo];
        //    if (orderNo == null)
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        return (int)orderNo;
        //    }
        //}
        //protected int GetOrderNoBySectionNo(int sectionNo)
        //{
        //    if (this.m_section2Order.Count == 0)
        //    {
        //        this.InitSection2OrderTable();
        //    }
        //    object orderNo = this.m_section2Order[sectionNo];
        //    if (orderNo == null)
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        return (int)orderNo;
        //    }
        //}
        //protected List<int> GetOrderedParItemList()
        //{
        //    List<int> result = new List<int>();
        //    if (this.m_parItem2Order.Count == 0)
        //    {
        //        this.InitParItem2OrderTable();
        //    }
        //    int temp;
        //    foreach (object c in this.m_parItem2Order.Keys)
        //    {
        //        try
        //        {
        //            temp = Convert.ToInt32(c);
        //            result.Add(temp);
        //        }
        //        catch (Exception ex)
        //        {
        //            continue;
        //        }
        //    }
        //    return result;
        //}
        //protected int Intersection(List<int> first, List<int> second)
        //{
        //    int re = 0;
        //    int temp;
        //    IEnumerable<int> result = first.Intersect<int>(second);
        //    foreach (int i in result)
        //    {
        //        temp = this.GetOrderNoByParItemNo(i);
        //        if (temp > re)
        //        {
        //            re = temp;
        //        }
        //    }
        //    return re;
        //}
        //protected int MaxOrder(List<int> l)
        //{
        //    int result = -1;
        //    foreach (int order in l)
        //    {
        //        if (order > result)
        //        {
        //            result = order;
        //        }
        //    }
        //    return result;
        //}
        //#endregion

        //#region 模板号设置
        //protected virtual void SetReportModel(ReportReportElement rre)
        //{
        //    SetReportModelNoByParItem(rre);
        //    if (rre.PrintModelNo <= 0)
        //    {
        //        SetReportModelNoBySectionNo(rre);
        //    }
        //}
        //protected virtual void SetReportModelNoBySectionNo(ReportReportElement rre)
        //{
        //    rre.PrintModelNo = this.GetReportModelNoBySectionNo(rre.SectionNo);
        //}
        //protected virtual void SetReportModelNoByParItem(ReportReportElement rre)
        //{
        //    List<int> printModelNoList = new List<int>(5);
        //    foreach (int item in rre.ParItemList)
        //    {
        //        printModelNoList.Add(this.GetReportModelNoByParItemNo(item));
        //    }
        //    rre.PrintModelNo = GetMax(printModelNoList);
        //}
        //protected int GetReportModelNoByParItemNo(int parItemNo)
        //{
        //    if (this.m_parItem2PrintModel.Count == 0)
        //    {
        //        this.InitParItem2ReportModelTable();
        //    }
        //    object modelNo = this.m_parItem2PrintModel[parItemNo];
        //    if (modelNo == null)
        //    {
        //        return -1;
        //    }
        //    else
        //    {
        //        return (int)modelNo;
        //    }
        //}
        //protected int GetReportModelNoBySectionNo(int sectionNo)
        //{
        //    if (this.m_section2PrintModel.Count == 0)
        //    {
        //        this.InitSection2PrintModelTable();
        //    }
        //    object modelNo = this.m_section2PrintModel[sectionNo];
        //    if (modelNo == null)
        //    {
        //        return -1;
        //    }
        //    else
        //    {
        //        return (int)modelNo;
        //    }
        //}
        //protected int GetMax(List<int> source)
        //{
        //    int result = -1;
        //    if (source.Count > 0)
        //    {
        //        result = source[0];
        //        foreach (int i in source)
        //        {
        //            if (i > result)
        //            {
        //                result = i;
        //            }
        //        }
        //    }
        //    return result;
        //}
        //#endregion

        //#region  内部实例方法
        //private void InitParItem2ReportModelTable()
        //{
        //    LisMap.InitParItem2ReportModelTable(this.m_parItem2PrintModel);
        //}
        //private void InitSection2PrintModelTable()
        //{
        //    LisMap.InitSection2PrintModelTable(this.m_section2PrintModel);
        //}
        //private void InitSection2OrderTable()
        //{
        //    LisMap.InitSection2OrderNoTable(this.m_section2Order);
        //}
        //private void InitParItem2OrderTable()
        //{
        //    LisMap.InitParItem2OrderNoTable(this.m_parItem2Order);
        //}

        //#endregion
    }
}
