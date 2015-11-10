using System.Collections;
using System.Collections.Generic;

using XYS.Lis.Core;
using XYS.Model;
using XYS.Lis.Model;
using XYS.Lis.Util;
namespace XYS.Lis.Handler
{
    public class ReportPrintModelHandler : ReportHandlerSkeleton
    {
        #region
        private static readonly string m_defaultHandlerName = "ReportPrintModelHandler";
        private Hashtable m_parItem2PrintModel;
        private Hashtable m_section2PrintModel;
        #endregion

        public ReportPrintModelHandler()
            : this(m_defaultHandlerName)
        { }
        public ReportPrintModelHandler(string handlerName)
            : base(handlerName)
        {
            this.m_parItem2PrintModel = new Hashtable();
            this.m_section2PrintModel = new Hashtable();
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
            else
            {
                return HandlerResult.Continue;
            }
        }
        //public override HandlerResult ReportOptions(List<ILisReportElement> reportElementList, ReportElementTag elementTag)
        //{
        //    if (elementTag == ReportElementTag.ReportElement)
        //    {
        //        OperateReportList(reportElementList);
        //        return HandlerResult.Continue;
        //    }
        //    else
        //    {
        //        return HandlerResult.Continue;
        //    }
        //}
        public override HandlerResult ReportOptions(Hashtable reportElementTable, ReportElementTag elementTag)
        {
            if (elementTag == ReportElementTag.ReportElement)
            {
                OperateReportTable(reportElementTable);
                return HandlerResult.Continue;
            }
            else
            {
                return HandlerResult.Continue;
            }
        }
        #endregion

        #region 实现抽象方法
        protected override void OperateReport(ReportReportElement rre)
        {
            ReportExamElement ree;
            //if (rre.ExamList.Count > 0)
            //{
            //    ReportExamElement examElement = rre.ExamList[0] as ReportExamElement;
            //    if (examElement != null)
            //    {
            //        //按照检验大项设置
            //        SetPrintModelNoByParItem(rre);
            //        if (rre.PrintModelNo <= 0)
            //        {
            //            SetPrintModelNoBySectionNo(rre, examElement.SectionNo);
            //        }
            //    }
            //}
            ree = rre.ItemTable[ReportElementTag.ExamElement] as ReportExamElement;
            if (ree != null)
            {
                //按照检验大项设置
                SetPrintModelNoByParItem(rre);
                if (rre.PrintModelNo <= 0)
                {
                    SetPrintModelNoBySectionNo(rre, ree.SectionNo);
                }
            }
        }
        #endregion

        //protected virtual int SetPrintModelNo(ReportReportElement rre)
        //{
        //    //-1 失败 0 继续 1 成功
        //    int flag=-1;
        //    if(rre.ExamList.Count>0)
        //    {
        //        ReportExamElement examElement = rre.ExamList[0] as ReportExamElement;
        //        if (examElement != null)
        //        {
        //            //按照检验大项设置
        //            SetPrintModelNoByParItem(rre);
        //            if (rre.PrintModelNo <= 0)
        //            {
        //                SetPrintModelNoBySectionNo(rre, examElement.SectionNo);
        //            }
        //        }
        //    }
        //    return flag;
        //}
        protected virtual void SetPrintModelNoBySectionNo(ReportReportElement rre,int sectionNo)
        {
            rre.PrintModelNo = this.GetPrintModelNoBySectionNo(sectionNo);
            //  //对于一个小组存在多个报告模板，根据检验大项进行报告模板匹配
            //switch (sectionNo)
            //{
            //    //细胞化学
            //    case 3:
            //    //临检
            //    case 28:
            //    //分子遗传小组
            //    case 11:
            //        SetPrintModelNoByParItem(rre);
            //        if (rre.PrintModelNo <= 0)
            //        {
            //            //默认模板号
            //            rre.PrintModelNo = PDFModel.GetPrintModelNoBySection(sectionNo);
            //        }
            //        break;
            //    //细胞形态
            //    case 39:
            //        break;
            //    //分子生物
            //    case 6:
            //        break;
            //    //组织配型
            //    case 45:
            //        break;
            //    //流式细胞
            //    case 10:
            //        break;
            //    default:
            //        rre.PrintModelNo = PDFModel.GetPrintModelNoBySection(sectionNo);
            //        break;
            //}
        }
        //通过paritemno 设置打印模板
        protected virtual void SetPrintModelNoByParItem(ReportReportElement rre)
        {
            List<int> printModelNoList = new List<int>();
            foreach (int item in rre.ParItemList)
            {
                printModelNoList.Add(this.GetPrintModelNoByParItemNo(item));
            }
            rre.PrintModelNo = GetMax(printModelNoList);
        }
        protected int GetPrintModelNoByParItemNo(int parItemNo)
        {
            if (this.m_parItem2PrintModel.Count == 0)
            {
                this.InitParItem2PrintModelTable();
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
        protected int GetPrintModelNoBySectionNo(int sectionNo)
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
            int result=-1;
            if(source.Count>0)
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
        #region
        private void InitParItem2PrintModelTable()
        {
            LisMap.InitParItem2PrintModelTable(this.m_parItem2PrintModel);
        }
        private void InitSection2PrintModelTable()
        {
            LisMap.InitSection2PrintModelTable(this.m_section2PrintModel);
        }
        #endregion
    }
}
