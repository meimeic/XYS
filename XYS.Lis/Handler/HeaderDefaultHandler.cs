using System;
using System.Collections.Generic;

using XYS.Lis.Core;
using XYS.Model;
using XYS.Lis.Model;
using XYS.Lis.Util;
namespace XYS.Lis.Handler
{
    /// <summary>
    /// 通用处理，删除各个集合中错误类型对象，以及各种元素的附加处理
    /// </summary>
    public class HeaderDefaultHandler : ReportHandlerSkeleton
    {
        #region
        public static readonly string m_defaultHandlerName = "HeaderDefaultHandler";
        #endregion

        #region
        public HeaderDefaultHandler()
            : this(m_defaultHandlerName)
        {
        }
        public HeaderDefaultHandler(string handlerName)
            : base(handlerName)
        {
        }
        #endregion

        #region 实现父类抽象方法
        public override HandlerResult ReportOptions(ILisReportElement reportElement)
        {
            switch (reportElement.ElementTag)
            {
                case ReportElementTag.ReportElement:
                    ReportReportElement rre = reportElement as ReportReportElement;
                    if (rre == null)
                    {
                        return HandlerResult.Fail;
                    }
                    else
                    {
                         OperateReport(rre);
                         return HandlerResult.Continue;
                    }
                case ReportElementTag.ExamElement:
                    ReportExamElement exam = reportElement as ReportExamElement;
                    if (exam == null)
                    {
                        return HandlerResult.Fail;
                    }
                    else
                    {
                        OperateExamInfo(exam);
                        return HandlerResult.Continue;
                    }
                case ReportElementTag.PatientElement:
                    ReportPatientElement patient = reportElement as ReportPatientElement;
                    if (patient == null)
                    {
                        return HandlerResult.Fail;
                    }
                    else
                    {
                        OperatePatient(patient);
                        return HandlerResult.Continue;
                    }
                case ReportElementTag.ItemElement:
                    ReportItemElement commonItem = reportElement as ReportItemElement;
                    if (commonItem == null)
                    {
                        return HandlerResult.Fail;
                    }
                    else
                    {
                        OperateCommonItem(commonItem);
                        return HandlerResult.Continue;
                    }
                case ReportElementTag.GraphElement:
                    ReportGraphElement graphItem = reportElement as ReportGraphElement;
                    if (graphItem == null)
                    {
                        return HandlerResult.Fail;
                    }
                    else
                    {
                        OperateGraphItem(graphItem);
                        return HandlerResult.Continue;
                    }
                case ReportElementTag.CustomElement:
                    ReportCustomElement customItem = reportElement as ReportCustomElement;
                    if (customItem == null)
                    {
                        return HandlerResult.Fail;
                    }
                    else
                    {
                        OperateCustomItem(customItem);
                        return HandlerResult.Continue;
                    }
                default:
                    return HandlerResult.Continue;
            }
        }

        public override HandlerResult ReportOptions(List<ILisReportElement> reportElementList, ReportElementTag elementTag)
        {
            HandlerResult rs = HandlerResult.Fail;
            switch (elementTag)
            {
                case ReportElementTag.ReportElement:
                    OperateReportList(reportElementList);
                    if (reportElementList.Count > 0)
                    {
                        rs= HandlerResult.Continue;
                    }
                    else
                    {
                        rs= HandlerResult.Fail;
                    }
                    break;
                case ReportElementTag.ExamElement:
                    OperateExamList(reportElementList);
                    rs = HandlerResult.Continue;
                    break;
                case ReportElementTag.PatientElement:
                    OperatePatientList(reportElementList);
                    rs = HandlerResult.Continue;
                    break;
                case ReportElementTag.ItemElement:
                    OperateCommonItemList(reportElementList, null);
                    rs = HandlerResult.Continue;
                    break;
                case ReportElementTag.GraphElement:
                    OperateGraphItemList(reportElementList);
                    rs = HandlerResult.Continue;
                    break;
                case ReportElementTag.CustomElement:
                    OperateCustomItemList(reportElementList);
                    rs = HandlerResult.Continue;
                    break;
                default:
                    return HandlerResult.Fail;
            }
            return rs;
        }
        #endregion



        #region
        protected override void OperateReportList(List<ILisReportElement> reportList)
        {
            if (reportList.Count > 0)
            {
                //HandlerResult rs = HandlerResult.Fail;
                for (int i = reportList.Count - 1; i >= 0; i--)
                {
                    ReportReportElement rre = reportList[i] as ReportReportElement;
                    if (rre == null)
                    {
                        reportList.RemoveAt(i);
                    }
                    else
                    {
                        OperateReport(rre);
                        //rs = GetMax(rs, OperateReport(rre));
                    }
                }
              //  return rs;
            }
            //else
            //{
            //    return HandlerResult.Fail;
            //}
        }
        protected override void OperateReport(ReportReportElement rre)
        {
            OperateExamList(rre.ExamList);
            OperatePatientList(rre.PatientList);
            OperateCommonItemList(rre.CommonItemList, rre.ParItemList);
            OperateGraphItemList(rre.GraphItemList);
            OperateCustomItemList(rre.CustomItemList);
            //if (rre.CommonItemList.Count > 0 && rre.ExamList.Count > 0 && rre.PatientList.Count > 0)
            //{
            //    return HandlerResult.Continue;
            //}
            //else
            //{
            //    return HandlerResult.Fail;
            //}
        }
        //protected virtual HandlerResult OperateReporter(ReportReportElement rre)
        //{

        //}
        
        protected virtual void OperateExamList(List<ILisReportElement> examList)
        {
            ReportExamElement examItem;
            if (examList.Count > 0)
            {
                for (int i = examList.Count - 1; i >= 0; i--)
                {
                    examItem = examList[i] as ReportExamElement;
                    if (examItem == null)
                    {
                        examList.RemoveAt(i);
                    }
                    else
                    {
                        OperateExamInfo(examItem);
                    }
                }
            }
        }
        protected virtual void OperateExamInfo(ReportExamElement ree)
        {
            //检验信息处理

        }
        
        protected virtual void OperatePatientList(List<ILisReportElement> patientList)
        {
            ReportPatientElement patientItem;
            if (patientList.Count > 0)
            {
                for (int i = patientList.Count - 1; i >= 0; i--)
                {
                    patientItem = patientList[i] as ReportPatientElement;
                    if (patientItem == null)
                    {
                        patientList.RemoveAt(i);
                    }
                    else
                    {
                        OperatePatient(patientItem);
                    }
                }
            }
        }
        protected virtual void OperatePatient(ReportPatientElement rpe)
        {
            //
        }
        
        protected virtual void OperateCommonItemList(List<ILisReportElement> commonItemList, List<int> parItemNos)
        {
            ReportItemElement commonItem;
            if (commonItemList.Count > 0)
            {
                for (int i = commonItemList.Count - 1; i >= 0; i--)
                {
                    commonItem = commonItemList[i] as ReportItemElement;
                    if (commonItem == null)
                    {
                        commonItemList.RemoveAt(i);
                    }
                    else if (IsRemoveByItemNo(commonItem.ItemNo))
                    {
                        commonItemList.RemoveAt(i);
                    }
                    else
                    {
                        if (parItemNos!=null&&!parItemNos.Contains(commonItem.ParItemNo))
                        {
                            parItemNos.Add(commonItem.ParItemNo);
                        }
                        //检验项通用处理
                        OperateCommonItem(commonItem);
                    }
                }
            }
        }
        protected virtual void OperateCommonItem(ReportItemElement rcie)
        {
            //检验项通用处理

        }

        protected virtual void OperateGraphItemList(List<ILisReportElement> graphItemList)
        {
            ReportGraphElement graphItem;
            if (graphItemList.Count > 0)
            {
                for (int i = graphItemList.Count - 1; i >= 0; i--)
                {
                    graphItem = graphItemList[i] as ReportGraphElement;
                    if (graphItem == null)
                    {
                        graphItemList.RemoveAt(i);
                    }
                    else
                    {
                        OperateGraphItem(graphItem);
                    }
                }
            }
        }
        protected virtual void OperateGraphItem(ReportGraphElement rgie)
        {
            //
        }
        
        protected virtual void OperateCustomItemList(List<ILisReportElement> customItemList)
        {
            ReportCustomElement customItem;
            if (customItemList.Count > 0)
            {
                for (int i = customItemList.Count - 1; i >= 0; i--)
                {
                    customItem = customItemList[i] as ReportCustomElement;
                    if (customItem == null)
                    {
                        customItemList.RemoveAt(i);
                    }
                    else
                    {
                        OperateCustomItem(customItem);
                    }
                }
            }
        }
        protected virtual void OperateCustomItem(ReportCustomElement customItem)
        {
            //自定义项处理

        }
        protected bool IsRemoveByItemNo(int itemNo)
        {
            return TestItem.GetHideItemNo.Contains(itemNo);
        }

        protected HandlerResult GetMin(HandlerResult rs1, HandlerResult rs2)
        {
            return (int)rs1 < (int)rs2 ? rs1 : rs2;
        }
        protected HandlerResult GetMax(HandlerResult rs1, HandlerResult rs2)
        {
            return (int)rs1 > (int)rs2 ? rs1 : rs2;
        }
        #endregion
    }
}
