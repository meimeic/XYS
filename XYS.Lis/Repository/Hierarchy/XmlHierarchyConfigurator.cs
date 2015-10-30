using System;
using System.Collections;
using System.Xml;

using XYS.Model;
using XYS.Lis.Model;
using XYS.Lis.Core;
using XYS.Lis.Util;
using XYS.Lis.Fill;
using XYS.Lis.Handler;
using XYS.Lis.Export;
using XYS.Lis.DAL;
using XYS.Lis.Repository;
namespace XYS.Lis.Repository.Hierarchy
{
    public class XmlHierarchyConfigurator
    {
        #region
        private static Type declaringType = typeof(XmlHierarchyConfigurator);
        private Hierarchy m_hierarchy;
        private Hashtable m_reporterBag;
        #endregion

        #region Private Constants

        // String constants used while parsing the XML data
        private static readonly string CONFIGURATION_TAG = "lis-report";
        private static readonly string REPORTER_STRATEGY_STACK_TAG = "reporter-strategy-stack";
        private static readonly string REPORTER_STRATEGY_TAG = "reporter-strategy";
        //private static readonly string APPENDER_TAG = "appender";
        private static readonly string FILL_REF_TAG = "fill-ref";
        private static readonly string EXPORT_REF_TAG = "export-ref";
        private static readonly string HANDLER_REF_STACK_TAG = "handler-ref-stack";
        private static readonly string HANDLER_REF_TAG = "handler-ref";

        private static readonly string FILL_STACK_TAG = "fill-stack";
        private static readonly string FILL_TAG = "fill";
        private static readonly string HANDLER_STACK_TAG = "handler-stack";
        private static readonly string HANDLER_TAG = "handler";
        private static readonly string EXPORT_STACK_TAG = "export-stack";
        private static readonly string EXPORT_TAG = "export";
        //private static readonly string EXPORTS_TAG = "exports";
      
        private static readonly string NAME_ATTR = "name";
        private static readonly string TYPE_ATTR = "type";
        private static readonly string REF_ATTR = "ref";
        //private static readonly string PRINT_MODEL_NO_ATTR = "printModelNo";
        //private static readonly string ORDER_NO_ATTR = "orderNo";
        //private static readonly string REPORTER_NAME_ATTR = "reporterName";
        //private static readonly string IMAGE_FLAG_ATTR = "imageFlag";
        //private static readonly string IMAGE_PATH_ATTR = "imagePath";
        //private static readonly string MODEL_PATH_ATTR = "modelPath";
        //private static readonly string VALUE_ATTR = "value";

        #endregion

        #region
        public XmlHierarchyConfigurator(Hierarchy hierarchy)
        {
            this.m_hierarchy = hierarchy;
            this.m_reporterBag = new Hashtable();
        }
        #endregion
        public void Configure(XmlElement element)
        {
            if (element == null || m_hierarchy == null)
            {
                return;
            }
            string rootElementName = element.LocalName;
            if (rootElementName != CONFIGURATION_TAG)
            {
                //
                ReportReport.Error(declaringType, "XmlHierarchyConfigurator:Xml element is - not a <" + CONFIGURATION_TAG + "> element.");
                return;
            }
            foreach (XmlNode currentNode in element.ChildNodes)
            {
                if (currentNode.NodeType == XmlNodeType.Element)
                {
                    XmlElement currentElement = (XmlElement)currentNode;
                    //策略节点
                    if (currentElement.LocalName == REPORTER_STRATEGY_STACK_TAG)
                    {
                        this.m_hierarchy.ClearStrategy();
                        ReportReport.Debug(declaringType, "XmlHierarchyConfigurator:Begin Configrue ReporterStrategyMap by " + REPORTER_STRATEGY_STACK_TAG+" element");
                        foreach (XmlNode node in currentElement.ChildNodes)
                        {
                            if (node.NodeType == XmlNodeType.Element)
                            {
                                XmlElement tempElement = (XmlElement)node;
                                if (tempElement.LocalName == REPORTER_STRATEGY_TAG)
                                {
                                    ParseStrategy(tempElement);
                                }
                            }
                        }
                        ReportReport.Debug(declaringType, "XmlHierarchyConfigurator:End Configrue ReporterStrategyMap by " + REPORTER_STRATEGY_STACK_TAG + " element");
                    }
                    else if (currentElement.LocalName == REPORTER_STRATEGY_TAG)
                    {
                        ParseStrategy(currentElement);
                    }
                    else if (currentElement.LocalName == FILL_STACK_TAG)
                    {
                        this.m_hierarchy.ClearFiller();
                        //
                        ReportReport.Debug(declaringType, "XmlHierarchyConfigurator:Begin Configrue ReporterFillMap by " + FILL_STACK_TAG + " element");
                        ParseFillStack(currentElement);
                        ReportReport.Debug(declaringType, "XmlHierarchyConfigurator:End Configrue ReporterFillMap by " + FILL_STACK_TAG + " element");
                    }
                    else if (currentElement.LocalName == HANDLER_STACK_TAG)
                    {
                        this.m_hierarchy.ClearHandler();
                        //
                        ReportReport.Debug(declaringType, "XmlHierarchyConfigurator:Begin Configrue ReporterHandlerMap by " + HANDLER_STACK_TAG + " element");
                        ParseHandlerStack(currentElement);
                        ReportReport.Debug(declaringType, "XmlHierarchyConfigurator:End Configrue ReporterHandlerMap by " + HANDLER_STACK_TAG + " element");
                    }
                    else if (currentElement.LocalName == EXPORT_STACK_TAG)
                    {
                        this.m_hierarchy.ClearExport();
                        ReportReport.Debug(declaringType, "XmlHierarchyConfigurator:Begin Configrue ReporterExportMap by " + EXPORT_STACK_TAG + " element");
                        ParseExportStack(currentElement);
                        ReportReport.Debug(declaringType, "XmlHierarchyConfigurator:End Configrue ReporterExportMap by " + EXPORT_STACK_TAG + " element");
                    }
                    else
                    {
                        // Read the param tags and set properties on the hierarchy
                       // SetParameter(currentElement, m_hierarchy);
                    }
                }
            }
        }
        protected void ParseStrategy(XmlElement reporterElement)
        {
            string strategyName = reporterElement.GetAttribute(NAME_ATTR);
            ReportReport.Debug(declaringType, "XmlHierarchyConfigurator:Loading Strategy [" + strategyName + "]");
            ReporterStrategy strategy = new ReporterStrategy(strategyName);
            ParseChildrenOfStrategyElement(reporterElement, strategy);
            this.m_hierarchy.AddStrategy(strategy);
            ReportReport.Debug(declaringType, "XmlHierarchyConfigurator:Loaded Strategy [" + strategyName + "]");
        }
        protected void ParseChildrenOfStrategyElement(XmlElement element, ReporterStrategy strategy)
        {
            foreach (XmlNode currentNode in element.ChildNodes)
            {
                if (currentNode.NodeType == XmlNodeType.Element)
                {
                    XmlElement currentElement = (XmlElement)currentNode;
                    if (currentElement.LocalName == FILL_REF_TAG)
                    {
                        //fill-ref
                        string fillName = currentElement.GetAttribute(REF_ATTR);
                        if (fillName != null && !fillName.Equals(""))
                        {
                            strategy.FillerName = fillName;
                        }
                    }
                    else if (currentElement.LocalName == EXPORT_REF_TAG)
                    {
                        //export-ref
                        string exportName = currentElement.GetAttribute(REF_ATTR);
                        if (exportName != null && !exportName.Equals(""))
                        {
                            strategy.ExportName = exportName;
                        }
                    }
                    else if(currentElement.LocalName==HANDLER_REF_TAG)
                    {
                        //handler-ref
                        PraseHandlerRef(currentElement,strategy);
                    }
                    else if (currentElement.LocalName == HANDLER_REF_STACK_TAG)
                    {
                        //handler-ref-stack
                        foreach (XmlNode node in currentElement.ChildNodes)
                        {
                            if (node.NodeType == XmlNodeType.Element)
                            {
                                PraseHandlerRef((XmlElement)node, strategy);
                            }
                        }
                    }
                    else
                    {
                    }
                }
            }
        }
        protected void PraseHandlerRef(XmlElement element, ReporterStrategy strategy)
        {
            if (element.LocalName == HANDLER_REF_TAG)
            {
                string handlerName = element.GetAttribute(REF_ATTR);
                if (handlerName != null && !handlerName.Equals(""))
                {
                    if (!strategy.HandlerList.Contains(handlerName))
                    {
                        strategy.HandlerList.Add(handlerName);
                    }
                }
            }
        }
           
        protected void ParseFillStack(XmlElement fillStackHandler)
        {
            foreach (XmlNode currentNode in fillStackHandler)
            {
                if (currentNode.NodeType == XmlNodeType.Element)
                {
                    XmlElement currentElement = (XmlElement)currentNode;
                    if (currentElement.LocalName == FILL_TAG)
                    {
                        ParseFill(currentElement);
                    }
                }
            }
        }
        protected void ParseFill(XmlElement fillElement)
        {
            string fillName = fillElement.GetAttribute(NAME_ATTR);
            ReportReport.Debug(declaringType, "XmlHierarchyConfigurator:Configure Filler [" + fillName + "]");
            object[] param = new object[] { fillName };
            string typeName = fillElement.GetAttribute(TYPE_ATTR);
            try
            {
                IReportFiller filler = (IReportFiller)Activator.CreateInstance(SystemInfo.GetTypeFromString(typeName, true, true), param);
                if (filler != null)
                {
                    this.m_hierarchy.AddFiller(filler);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        protected void ParseHandlerStack(XmlElement handlerStackElement)
        {
            foreach (XmlNode currentNode in handlerStackElement.ChildNodes)
            {
                if (currentNode.NodeType == XmlNodeType.Element)
                {
                    XmlElement currentElement = (XmlElement)currentNode;
                    if (currentElement.LocalName == HANDLER_TAG)
                    {
                        ParseHandler(currentElement);
                    }
                }
            }
        }
        protected void ParseHandler(XmlElement handlerElement)
        {
            string handlerName = handlerElement.GetAttribute(NAME_ATTR);
            ReportReport.Debug(declaringType, "XmlHierarchyConfigurator:Configure Handler [" + handlerName + "]");
            object[] param = new object[] { handlerName };
            string typeName = handlerElement.GetAttribute(TYPE_ATTR);
            try
            {
                IReportHandler handler = (IReportHandler)Activator.CreateInstance(SystemInfo.GetTypeFromString(typeName, true, true), param);
                if (handler != null)
                {
                    this.m_hierarchy.AddHandler(handler);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        protected void ParseExportStack(XmlElement exportsElement)
        {
            foreach (XmlNode currentNode in exportsElement.ChildNodes)
            {
                if (currentNode.NodeType == XmlNodeType.Element)
                {
                    XmlElement currentElement = (XmlElement)currentNode;
                    if (currentElement.LocalName == EXPORT_TAG)
                    {
                        ParseExport(currentElement);
                    }
                }
            }
        }
        protected void ParseExport(XmlElement exportElement)
        {
            string exportName = exportElement.GetAttribute(NAME_ATTR);
            ReportReport.Debug(declaringType, "XmlHierarchyConfigurator:Configure Export [" + exportName + "]");
            object[] param = new object[] { exportName };
            string typeName = exportElement.GetAttribute(TYPE_ATTR);
            try
            {
                IReportExport reportExport = (IReportExport)Activator.CreateInstance(SystemInfo.GetTypeFromString(typeName, true, true), param);
                if (reportExport != null)
                {
                    this.m_hierarchy.AddExport(reportExport);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        
        //protected void PraseReportElements(XmlElement reportsElement)
        //{
        //    foreach (XmlNode currentNode in reportsElement.ChildNodes)
        //    {
        //        if (currentNode.NodeType == XmlNodeType.Element)
        //        {
        //            XmlElement currentElement = (XmlElement)currentNode;
        //            if (currentElement.LocalName == REPORT_ELEMENT_TAG)
        //            {
        //                PraseReportElement(reporter,currentElement);
        //            }
        //        }
        //    }
        //}
        //protected void PraseReportElement(Reporter reporter,XmlElement reportElement)
        //{
        //    string val = reportElement.GetAttribute(VALUE_ATTR);
        //    string typeName = reportElement.GetAttribute(TYPE_ATTR);
        //    int elementValue;
        //    bool flag = SystemInfo.TryParse(val,out elementValue);
        //    ReportElementTag elementTag = ReportElementTag.NoneElement;
        //    if (flag)
        //    {
        //        elementTag = (ReportElementTag)elementValue;
        //    }
        //    Type elementType = SystemInfo.GetTypeFromString(typeName, true, true);
        //    if (elementTag != ReportElementTag.NoneElement && elementType != null)
        //    {
        //        reporter.AddElementType(new ReportElementType(elementTag, elementType));
        //    }
        //}
        //protected void ParseFill(Reporter reporter, XmlElement fillElement)
        //{
        //    string typeName = fillElement.GetAttribute(TYPE_ATTR);
        //    try
        //    {
        //        ILisReportDAL reportDAL = (ILisReportDAL)Activator.CreateInstance(SystemInfo.GetTypeFromString(typeName, true, true));
        //        if (reportDAL != null)
        //        {
        //            //设置参数
        //            reporter.ReporterDAL = reportDAL;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return;
        //    }
        //}
        //protected IAppender ParseAppender(XmlElement appenderElement)
        //{
        //    string appenderName = appenderElement.GetAttribute(NAME_ATTR);
        //    string typeName = appenderElement.GetAttribute(TYPE_ATTR);
        //    try
        //    {
        //        IAppender appender = (IAppender)Activator.CreateInstance(SystemInfo.GetTypeFromString(typeName, true, true));
        //        appender.AppenderName = appenderName;
        //        appender.ClearExport();
        //        appender.ClearHandler();
        //        foreach (XmlNode currentNode in appenderElement.ChildNodes)
        //        {
        //            if (currentNode.NodeType == XmlNodeType.Element)
        //            {
        //                XmlElement currentElement = (XmlElement)currentNode;
        //                if (currentElement.LocalName == HANDLER_STACK_TAG)
        //                {
        //                    ParseHandlerStack(appender, currentElement);
        //                }
        //                else if (currentElement.LocalName == HANDLER_TAG)
        //                {
        //                    ParseHandler(appender, currentElement);
        //                }
        //                else if (currentElement.LocalName == EXPORTS_TAG)
        //                {
        //                    ParseExportStack(appender, currentElement);
        //                }
        //                else if (currentElement.LocalName == EXPORT_TAG)
        //                {
        //                    ParseExport(appender, currentElement);
        //                }
        //                else
        //                {
        //                    //
        //                }
        //            }
        //        }
        //        return appender;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }
}
