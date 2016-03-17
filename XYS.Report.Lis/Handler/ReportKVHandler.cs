//using System;
//using System.Reflection;
//using System.Collections;
//using System.Collections.Generic;

//using XYS.Report.Lis.Core;
//using XYS.Report.Lis.Model;
//namespace XYS.Report.Lis.Handler
//{
//    public class ReportKVHandler : ReportHandlerSkeleton
//    {
//        #region 静态变量
//        public static readonly string m_defaultHandlerName = "ReportKVHandler";
//        #endregion

//        #region 构造函数
//        public ReportKVHandler()
//            : this(m_defaultHandlerName)
//        {
//        }
//        public ReportKVHandler(string handlerName)
//            : base(handlerName)
//        {
//        }
//        #endregion

//        #region 实现父类抽象方法
//        protected override bool OperateElement(ILisReportElement element)
//        {
//            return true;
//        }
//        protected override bool OperateReport(ReportReportElement report)
//        {
//            ReportKVElement kv = null;
//            ReportCustomElement custom = null;
//            List<ILisReportElement> kvList = report.GetReportItem(typeof(ReportKVElement));
//            if (IsExist(kvList))
//            {
//                List<ILisReportElement> customList = report.GetReportItem(typeof(ReportCustomElement));
//                foreach (ILisReportElement element in kvList)
//                {
//                    kv = element as ReportKVElement;
//                    if (kv != null)
//                    {
//                        custom = new ReportCustomElement();
//                        ConvertKV2Custom(kv, custom);
//                        customList.Add(custom);
//                    }
//                }
//            }
//            report.RemoveReportItem(typeof(ReportKVElement));
//            return true;
//        }
//        #endregion

//        #region
//        private void ConvertKV2Custom(ReportKVElement kv, ReportCustomElement custom)
//        {
//            custom.Name = kv.Name;
//            string propertyName = null;
//            PropertyInfo customProperty = null;
//            foreach (DictionaryEntry de in kv.KVTable)
//            {
//                propertyName = de.Key as string;
//                if (!string.IsNullOrEmpty(propertyName))
//                {
//                    customProperty = custom.GetType().GetProperty(propertyName);
//                    if (customProperty != null)
//                    {
//                        SetCustomProp(customProperty, custom, de.Value);
//                    }
//                }
//            }
//        }
//        private void SetCustomProp(PropertyInfo ep, ReportCustomElement custom, object value)
//        {
//            ep.SetValue(custom, value, null);
//        }
//        #endregion
//    }
//}
