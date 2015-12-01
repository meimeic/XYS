using System;
using System.Collections;
using System.Xml;

using XYS.Lis.Fill;
using XYS.Lis.Handler;
using XYS.Lis.Util;
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

        private static readonly string FILL_REF_TAG = "fill-ref";
        private static readonly string HANDLER_REF_STACK_TAG = "handler-ref-stack";
        private static readonly string HANDLER_REF_TAG = "handler-ref";

        private static readonly string FILL_STACK_TAG = "fill-stack";
        private static readonly string FILL_TAG = "fill";
        private static readonly string HANDLER_STACK_TAG = "handler-stack";
        private static readonly string HANDLER_TAG = "handler";
      
        private static readonly string NAME_ATTR = "name";
        private static readonly string TYPE_ATTR = "type";
        private static readonly string REF_ATTR = "ref";

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
                        ReportReport.Debug(declaringType, "XmlHierarchyConfigurator:Begin Configrue ReporterStrategyMap by <" + REPORTER_STRATEGY_STACK_TAG+"> element");
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
                        ReportReport.Debug(declaringType, "XmlHierarchyConfigurator:End Configrue ReporterStrategyMap");
                    }
                    else if (currentElement.LocalName == REPORTER_STRATEGY_TAG)
                    {
                        ParseStrategy(currentElement);
                    }
                    else if (currentElement.LocalName == FILL_STACK_TAG)
                    {
                        this.m_hierarchy.ClearFiller();
                        //
                        ReportReport.Debug(declaringType, "XmlHierarchyConfigurator:Begin Configrue ReporterFillMap by <" + FILL_STACK_TAG + "> element");
                        ParseFillStack(currentElement);
                        ReportReport.Debug(declaringType, "XmlHierarchyConfigurator:End Configrue ReporterFillMap");
                    }
                    else if (currentElement.LocalName == HANDLER_STACK_TAG)
                    {
                        this.m_hierarchy.ClearHandler();
                        //
                        ReportReport.Debug(declaringType, "XmlHierarchyConfigurator:Begin Configrue ReporterHandlerMap by <" + HANDLER_STACK_TAG + "> element");
                        ParseHandlerStack(currentElement);
                        ReportReport.Debug(declaringType, "XmlHierarchyConfigurator:End Configrue ReporterHandlerMap");
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
                        //参数处理
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
                    strategy.AddHandler(handlerName);
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
    }
}
