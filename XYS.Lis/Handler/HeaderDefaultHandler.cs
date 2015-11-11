using System;
using System.Collections;
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
        #region 变量
        public static readonly string m_defaultHandlerName = "HeaderDefaultHandler";
        #endregion

        #region 构造函数
        public HeaderDefaultHandler()
            : this(m_defaultHandlerName)
        {
        }
        public HeaderDefaultHandler(string handlerName)
            : base(handlerName)
        {
        }
        #endregion

        #region 实现父类继承接口的抽象方法

        public override HandlerResult ReportOptions(ILisReportElement reportElement)
        {
            bool result = IsElementAndOperate(reportElement, reportElement.ElementTag);
            if (result)
            {
                return HandlerResult.Continue;
            }
            else
            {
                return HandlerResult.Fail;
            }
        }
        public override HandlerResult ReportOptions(List<ILisReportElement> reportElementList, ReportElementTag elementTag)
        {
            OperateElementList(reportElementList, elementTag);
            if (reportElementList.Count > 0)
            {
                return HandlerResult.Continue;
            }
            else
            {
                return HandlerResult.Fail;
            }
            //HandlerResult rs = HandlerResult.Fail;
            //switch (elementTag)
            //{
            //    case ReportElementTag.ReportElement:
            //        OperateReportList(reportElementList);
            //        rs = HandlerResult.Continue;
            //        break;
            //    case ReportElementTag.ExamElement:
            //        OperateExamList(reportElementList);
            //        rs = HandlerResult.Continue;
            //        break;
            //    case ReportElementTag.PatientElement:
            //        OperatePatientTable(elementTag, reportElementTable);
            //        rs = HandlerResult.Continue;
            //        break;
            //    case ReportElementTag.ItemElement:
            //        OperateItemTable(elementTag, reportElementTable);
            //        rs = HandlerResult.Continue;
            //        break;
            //    case ReportElementTag.GraphElement:
            //        OperateGraphTable(elementTag, reportElementTable);
            //        rs = HandlerResult.Continue;
            //        break;
            //    case ReportElementTag.CustomElement:
            //        OperateCustomTable(elementTag, reportElementTable);
            //        rs = HandlerResult.Continue;
            //        break;
            //    default:
            //        return HandlerResult.Fail;
            //}
            //return rs;
        }
        #endregion

        #region 实现父类受保护的抽象方法
        protected override void OperateReport(ReportReportElement rre)
        {
            ReportElementTag elementTag;
            List<ILisReportElement> reportElementList;
            foreach (DictionaryEntry de in rre.ReportItemTable)
            {
                if (de.Key != null)
                {
                    try
                    {
                        elementTag = (ReportElementTag)de.Key;
                        reportElementList = de.Value as List<ILisReportElement>;
                        if (reportElementList != null)
                        {
                            OperateElementList(reportElementList, elementTag);
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }
            //OperateExamList(rre.ExamList);
            //OperatePatientList(rre.PatientList);
            //OperateCommonItemList(rre.CommonItemList, rre.ParItemList);
            //OperateGraphItemList(rre.GraphItemList);
            //OperateCustomItemList(rre.CustomItemList);
            //if (rre.CommonItemList.Count > 0 && rre.ExamList.Count > 0 && rre.PatientList.Count > 0)
            //{
            //    return HandlerResult.Continue;
            //}
            //else
            //{
            //    return HandlerResult.Fail;
            //}
        }
        protected override bool IsElementAndOperate(ILisReportElement reportElement, ReportElementTag elementTag)
        {
            bool result = false;
            switch (elementTag)
            {
                case ReportElementTag.ReportElement:
                    result = reportElement is ReportReportElement;
                    if (result)
                    {
                        ReportReportElement report = reportElement as ReportReportElement;
                        OperateReport(report);
                    }
                    break;
                case ReportElementTag.ExamElement:
                    result = reportElement is ReportExamElement;
                    if (result)
                    {
                        ReportExamElement exam = reportElement as ReportExamElement;
                        OperateExam(exam);
                    }
                    break;
                case ReportElementTag.PatientElement:
                    result = reportElement is ReportPatientElement;
                    if (result)
                    {
                        ReportPatientElement patient = reportElement as ReportPatientElement;
                        OperatePatient(patient);
                    }
                    break;
                case ReportElementTag.ItemElement:
                    result = reportElement is ReportItemElement;
                    if (result)
                    {
                        ReportItemElement item = reportElement as ReportItemElement;
                        OperateItem(item);
                    }
                    break;
                case ReportElementTag.GraphElement:
                    result = reportElement is ReportGraphElement;
                    if (result)
                    {
                        ReportGraphElement graph = reportElement as ReportGraphElement;
                        OperateGraph(graph);
                    }
                    break;
                case ReportElementTag.CustomElement:
                    result = reportElement is ReportCustomElement;
                    if (result)
                    {
                        ReportCustomElement custom = reportElement as ReportCustomElement;
                        OperateCustom(custom);
                    }
                    break;
                case ReportElementTag.NoneElement:
                    result = false;
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }
        #endregion

        #region

        protected virtual void OperateExam(ReportExamElement ree)
        {
            //检验信息处理

        }

        protected virtual void OperatePatient(ReportPatientElement rpe)
        {
            //
        }

        protected virtual void OperateItem(ReportItemElement rcie)
        {
            //检验项通用处理

        }

        protected virtual void OperateGraph(ReportGraphElement rgie)
        {
            //
        }

        protected virtual void OperateCustom(ReportCustomElement customItem)
        {
            //自定义项处理

        }

        #endregion

        #region
        //public override HandlerResult ReportOptions(Hashtable reportElementTable, ReportElementTag elementTag)
        //{
        //    HandlerResult rs = HandlerResult.Fail;
        //    switch (elementTag)
        //    {
        //        case ReportElementTag.ReportElement:
        //            OperateReportTable(reportElementTable);
        //            rs = HandlerResult.Continue;
        //            break;
        //        case ReportElementTag.ExamElement:
        //            OperateExamTable(elementTag, reportElementTable);
        //            rs = HandlerResult.Continue;
        //            break;
        //        case ReportElementTag.PatientElement:
        //            OperatePatientTable(elementTag, reportElementTable);
        //            rs = HandlerResult.Continue;
        //            break;
        //        case ReportElementTag.ItemElement:
        //            OperateItemTable(elementTag, reportElementTable);
        //            rs = HandlerResult.Continue;
        //            break;
        //        case ReportElementTag.GraphElement:
        //            OperateGraphTable(elementTag, reportElementTable);
        //            rs = HandlerResult.Continue;
        //            break;
        //        case ReportElementTag.CustomElement:
        //            OperateCustomTable(elementTag, reportElementTable);
        //            rs = HandlerResult.Continue;
        //            break;
        //        default:
        //            return HandlerResult.Fail;
        //    }
        //    return rs;
        //}
        //protected override void OperateReportTable(Hashtable reportTable)
        //{
        //    FilterItem(ReportElementTag.ReportElement, reportTable);
        //    ReportReportElement rre;
        //    foreach(DictionaryEntry de in reportTable)
        //    {
        //        rre = de.Value as ReportReportElement;
        //        if (rre != null)
        //        {
        //            OperateReport(rre);
        //        }
        //    }
        //    //if (reportTable.Count > 0)
        //    //{
        //    //    //HandlerResult rs = HandlerResult.Fail;
        //    //    for (int i = reportTable.Count - 1; i >= 0; i--)
        //    //    {
        //    //        ReportReportElement rre = reportTable[i] as ReportReportElement;
        //    //        if (rre == null)
        //    //        {
        //    //            reportTable.RemoveAt(i);
        //    //        }
        //    //        else
        //    //        {
        //    //            OperateReport(rre);
        //    //            //rs = GetMax(rs, OperateReport(rre));
        //    //        }
        //    //    }
        //    //  //  return rs;
        //    //}
        //    ////else
        //    ////{
        //    ////    return HandlerResult.Fail;
        //    ////}
        //}
        //protected virtual void OperateReportItemTable(ReportElementTag elementTag, Hashtable table)
        //{
        //    switch (elementTag)
        //    {
        //        case ReportElementTag.ExamElement:
        //            OperateExamTable(elementTag, table);
        //            break;
        //        case ReportElementTag.PatientElement:
        //            OperatePatientTable(elementTag, table);
        //            break;
        //        case ReportElementTag.ItemElement:
        //            OperateItemTable(elementTag, table);
        //            break;
        //        case ReportElementTag.GraphElement:
        //            OperateGraphTable(elementTag, table);
        //            break;
        //        case ReportElementTag.CustomElement:
        //            OperateCustomTable(elementTag, table);
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //protected virtual void OperateExamList(List<ILisReportElement> examList)
        //{
        //    ReportExamElement examItem;
        //    if (examList.Count > 0)
        //    {
        //        for (int i = examList.Count - 1; i >= 0; i--)
        //        {
        //            examItem = examList[i] as ReportExamElement;
        //            if (examItem == null)
        //            {
        //                examList.RemoveAt(i);
        //            }
        //            else
        //            {
        //                OperateExam(examItem);
        //            }
        //        }
        //    }
        //}
        //protected virtual void OperateExamTable(ReportElementTag elementTag, Hashtable table)
        //{
        //    ReportExamElement ree;
        //    FilterItem(elementTag, table);
        //    foreach (DictionaryEntry de in table)
        //    {
        //        ree = de.Value as ReportExamElement;
        //        if (ree != null)
        //        {
        //            OperateExam(ree);
        //        }
        //    }
        //}

        //protected virtual void OperatePatientList(List<ILisReportElement> patientList)
        //{
        //    ReportPatientElement patientItem;
        //    if (patientList.Count > 0)
        //    {
        //        for (int i = patientList.Count - 1; i >= 0; i--)
        //        {
        //            patientItem = patientList[i] as ReportPatientElement;
        //            if (patientItem == null)
        //            {
        //                patientList.RemoveAt(i);
        //            }
        //            else
        //            {
        //                OperatePatient(patientItem);
        //            }
        //        }
        //    }
        //}
        //protected virtual void OperatePatientTable(ReportElementTag elementTag, Hashtable table)
        //{
        //    ReportPatientElement rpe;
        //    FilterItem(elementTag, table);
        //    foreach (DictionaryEntry de in table)
        //    {
        //        rpe = de.Value as ReportPatientElement;
        //        if (rpe != null)
        //        {
        //            OperatePatient(rpe);
        //        }
        //    }
        //}

        //protected virtual void OperateItemList(List<ILisReportElement> commonItemList, List<int> parItemNos)
        //{
        //    ReportItemElement commonItem;
        //    if (commonItemList.Count > 0)
        //    {
        //        for (int i = commonItemList.Count - 1; i >= 0; i--)
        //        {
        //            commonItem = commonItemList[i] as ReportItemElement;
        //            if (commonItem == null)
        //            {
        //                commonItemList.RemoveAt(i);
        //            }
        //            else if (IsRemoveByItemNo(commonItem.ItemNo))
        //            {
        //                commonItemList.RemoveAt(i);
        //            }
        //            else
        //            {
        //                if (parItemNos != null && !parItemNos.Contains(commonItem.ParItemNo))
        //                {
        //                    parItemNos.Add(commonItem.ParItemNo);
        //                }
        //                //检验项通用处理
        //                OperateItem(commonItem);
        //            }
        //        }
        //    }
        //}
        //protected virtual void OperateItemTable(ReportElementTag elementTag, Hashtable table)
        //{
        //    ReportItemElement rie;
        //    FilterItem(elementTag, table);
        //    foreach (DictionaryEntry de in table)
        //    {
        //        rie = de.Value as ReportItemElement;
        //        if (rie != null)
        //        {
        //            OperateItem(rie);
        //        }
        //    }
        //}

        //protected virtual void OperateGraphList(List<ILisReportElement> graphItemList)
        //{
        //    ReportGraphElement graphItem;
        //    if (graphItemList.Count > 0)
        //    {
        //        for (int i = graphItemList.Count - 1; i >= 0; i--)
        //        {
        //            graphItem = graphItemList[i] as ReportGraphElement;
        //            if (graphItem == null)
        //            {
        //                graphItemList.RemoveAt(i);
        //            }
        //            else
        //            {
        //                OperateGraph(graphItem);
        //            }
        //        }
        //    }
        //}
        //protected virtual void OperateGraphTable(ReportElementTag elementTag, Hashtable table)
        //{
        //    ReportGraphElement rge;
        //    FilterItem(elementTag, table);
        //    foreach (DictionaryEntry de in table)
        //    {
        //        rge = de.Value as ReportGraphElement;
        //        if (rge != null)
        //        {
        //            OperateGraph(rge);
        //        }
        //    }
        //}

        //protected virtual void OperateCustomList(List<ILisReportElement> customItemList)
        //{
        //    ReportCustomElement customItem;
        //    if (customItemList.Count > 0)
        //    {
        //        for (int i = customItemList.Count - 1; i >= 0; i--)
        //        {
        //            customItem = customItemList[i] as ReportCustomElement;
        //            if (customItem == null)
        //            {
        //                customItemList.RemoveAt(i);
        //            }
        //            else
        //            {
        //                OperateCustom(customItem);
        //            }
        //        }
        //    }
        //}
        //protected virtual void OperateCustomTable(ReportElementTag elementTag, Hashtable table)
        //{
        //    ReportCustomElement rce;
        //    FilterItem(elementTag, table);
        //    foreach (DictionaryEntry de in table)
        //    {
        //        rce = de.Value as ReportCustomElement;
        //        if (rce != null)
        //        {
        //            OperateCustom(rce);
        //        }
        //    }
        //}

        //protected bool IsRemoveByItemNo(int itemNo)
        //{
        //    return TestItem.GetHideItemNo.Contains(itemNo);
        //}
        //protected void FilterItem(ReportElementTag elementTag, Hashtable table)
        //{
        //    ArrayList removeKey = new ArrayList();
        //    foreach (DictionaryEntry de in table)
        //    {
        //        if (!IsElement(de.Value, elementTag))
        //        {
        //            if (de.Key != null)
        //            {
        //                removeKey.Add(de.Key);
        //            }
        //        }
        //    }
        //    foreach (object obj in removeKey)
        //    {
        //        table.Remove(obj);
        //    }
        //}
        //private bool IsElement(object obj, ReportElementTag elementTag)
        //{
        //    switch (elementTag)
        //    {
        //        case ReportElementTag.ReportElement:
        //            return obj is ReportReportElement;
        //        case ReportElementTag.ExamElement:
        //            return obj is ReportExamElement;
        //        case ReportElementTag.PatientElement:
        //            return obj is ReportPatientElement;
        //        case ReportElementTag.ItemElement:
        //            return obj is ReportItemElement;
        //        case ReportElementTag.GraphElement:
        //            return obj is ReportGraphElement;
        //        case ReportElementTag.CustomElement:
        //            return obj is ReportCustomElement;
        //        default:
        //            return true;
        //    }
        //}
        //protected HandlerResult GetMin(HandlerResult rs1, HandlerResult rs2)
        //{
        //    return (int)rs1 < (int)rs2 ? rs1 : rs2;
        //}
        //protected HandlerResult GetMax(HandlerResult rs1, HandlerResult rs2)
        //{
        //    return (int)rs1 > (int)rs2 ? rs1 : rs2;
        //}
        #endregion
    }
}
