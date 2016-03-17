using System;
using System.Xml;
using System.Collections;

using XYS.Util;

using XYS.Report.Lis.Core;
using XYS.Report.Lis.Filler;
using XYS.Report.Lis.Handler;
namespace XYS.Report.Lis.Repository
{
    public class XmlHierarchyConfigurator
    {
        #region 字段
        private Hashtable m_reporterBag;
        private Hierarchy m_repository;
        private static Type declaringType = typeof(XmlHierarchyConfigurator);
        #endregion

        #region 私有常量
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

        #region 构造函数
        public XmlHierarchyConfigurator(Hierarchy repository)
        {
            this.m_repository = repository;
            this.m_reporterBag = new Hashtable();
        }
        #endregion

        #region 方法
        public void Configure(XmlElement element)
        {
            if (element == null || m_repository == null)
            {
                return;
            }
            string rootElementName = element.LocalName;
            if (rootElementName != CONFIGURATION_TAG)
            {
                //
                ConsoleInfo.Error(declaringType, "Xml element is - not a <" + CONFIGURATION_TAG + "> element.");
                return;
            }
            foreach (XmlNode currentNode in element.ChildNodes)
            {
                if (currentNode.NodeType == XmlNodeType.Element)
                {
                    XmlElement currentElement = (XmlElement)currentNode;
                    //策略节点集合
                    if (currentElement.LocalName == REPORTER_STRATEGY_STACK_TAG)
                    {
                        this.m_repository.ClearStrategy();
                        ConsoleInfo.Debug(declaringType, "Begin Configrue ReporterStrategyMap by <" + REPORTER_STRATEGY_STACK_TAG + "> element");
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
                        ConsoleInfo.Debug(declaringType, "End Configrue ReporterStrategyMap");
                    }
                    //策略节点
                    else if (currentElement.LocalName == REPORTER_STRATEGY_TAG)
                    {
                        ParseStrategy(currentElement);
                    }
                    //填充集合
                    else if (currentElement.LocalName == FILL_STACK_TAG)
                    {
                        this.m_repository.ClearFiller();
                        ConsoleInfo.Debug(declaringType, "Begin Configrue ReporterFillMap by <" + FILL_STACK_TAG + "> element");
                        ParseFillStack(currentElement);
                        ConsoleInfo.Debug(declaringType, "End Configrue ReporterFillMap");
                    }
                    //处理集合
                    else if (currentElement.LocalName == HANDLER_STACK_TAG)
                    {
                        this.m_repository.ClearHandler();
                        ConsoleInfo.Debug(declaringType, "Begin Configrue ReporterHandlerMap by <" + HANDLER_STACK_TAG + "> element");
                        ParseHandlerStack(currentElement);
                        ConsoleInfo.Debug(declaringType, "End Configrue ReporterHandlerMap");
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
            ConsoleInfo.Debug(declaringType, "Loading Strategy [" + strategyName + "]");
            ReporterStrategy strategy = new ReporterStrategy(strategyName);
            ParseChildrenOfStrategyElement(reporterElement, strategy);
            this.m_repository.AddStrategy(strategy);
            ConsoleInfo.Debug(declaringType, "Loaded Strategy [" + strategyName + "]");
        }
        protected void ParseChildrenOfStrategyElement(XmlElement element, ReporterStrategy strategy)
        {
            this.m_repository.ClearStrategy();
            foreach (XmlNode currentNode in element.ChildNodes)
            {
                if (currentNode.NodeType == XmlNodeType.Element)
                {
                    XmlElement currentElement = (XmlElement)currentNode;
                    if (currentElement.LocalName == FILL_REF_TAG)
                    {
                        //fill-ref
                        string fillName = currentElement.GetAttribute(REF_ATTR);
                        if (!string.IsNullOrEmpty(fillName))
                        {
                            strategy.FillerName = fillName;
                        }
                    }
                    else if (currentElement.LocalName == HANDLER_REF_TAG)
                    {
                        //handler-ref
                        PraseHandlerRef(currentElement, strategy);
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
            ConsoleInfo.Debug(declaringType, "Configuring Filler [" + fillName + "]");
            object[] param = new object[] { fillName };
            string typeName = fillElement.GetAttribute(TYPE_ATTR);
            try
            {
                IReportFiller filler = (IReportFiller)Activator.CreateInstance(SystemInfo.GetTypeFromString(typeName, true, true), param);
                if (filler != null)
                {
                    this.m_repository.AddFiller(filler);
                    ConsoleInfo.Debug(declaringType, "Configured Filler [" + fillName + "]");
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
            ConsoleInfo.Debug(declaringType, "Configuring Handler [" + handlerName + "]");
            object[] param = new object[] { handlerName };
            string typeName = handlerElement.GetAttribute(TYPE_ATTR);
            try
            {
                IReportHandler handler = (IReportHandler)Activator.CreateInstance(SystemInfo.GetTypeFromString(typeName, true, true), param);
                if (handler != null)
                {
                    this.m_repository.AddHandler(handler);
                    ConsoleInfo.Debug(declaringType, "Configured Handler [" + handlerName + "]");
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        #endregion
    }
}
