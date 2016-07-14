using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;
using XYS.Report;
using XYS.Lis.Report.Util;
using XYS.Lis.Report.Model;
namespace XYS.Lis.Report
{
    delegate void HandleErrorHandler(LabReport report);
    delegate void HandleSuccessHandler(LabReport report);
    public class HandleService
    {
        #region 私有静态子段
        private static readonly Hashtable Type2HandleMap;
        private static readonly Hashtable Section2FillTypeMap;
        #endregion

        #region 私有事件
        private event HandleErrorHandler m_handleErrorEvent;
        private event HandleSuccessHandler m_handleCompleteEvent;
        #endregion

        #region 构造函数
        static HandleService()
        {
            Type2HandleMap = new Hashtable(5);
            Section2FillTypeMap = new Hashtable(20);
        }
        public HandleService()
        {
        }
        #endregion

        #region 事件属性
        internal event HandleErrorHandler HandleErrorEvent
        {
            add { this.m_handleErrorEvent += value; }
            remove { this.m_handleErrorEvent -= value; }
        }
        internal event HandleSuccessHandler HandleCompleteEvent
        {
            add { this.m_handleCompleteEvent += value; }
            remove { this.m_handleCompleteEvent -= value; }
        }
        #endregion

        #region 公共方法
        public void HandleReport(LabReport report)
        {
            bool result = false;
            IHandle handle = null;
            ReportPK RK = report.ReportPK;
            List<Type> lt = GetFillTypes(RK);
            List<IFillElement> elements = null;
            if (lt != null && lt.Count > 0)
            {
                foreach (Type type in lt)
                {
                    handle = GetHandle(type);
                    if (handle != null)
                    {
                        elements = new List<IFillElement>(10);
                        result = handle.InitElement(elements, RK, type);
                        if (!result)
                        {
                            break;
                        }
                        report.ItemTable.Add(type.Name, elements);
                    }
                }
            }
            if (result)
            {
                OnSuccess(report);
            }
            else
            {
                OnError(report);
            }
        }
        #endregion

        #region 触发事件
        protected void OnError(LabReport report)
        {
            HandleErrorHandler handler = this.m_handleErrorEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        protected void OnSuccess(LabReport report)
        {
            HandleSuccessHandler handler = this.m_handleCompleteEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        #endregion

        #region 辅助方法
        protected List<Type> GetFillTypes(ReportPK RK)
        {
            return Section2FillTypeMap[RK.SectionNo] as List<Type>;
        }
        protected IHandle GetHandle(Type type)
        {
            return Type2HandleMap[type] as IHandle;
        }

        private static void InitFillTable()
        {
            lock (Section2FillTypeMap)
            {
                ConfigManager.InitSection2FillElementTable(Section2FillTypeMap);
            }
        }
        private static void InitHandleTable()
        {
            lock (Type2HandleMap)
            {
                ConfigManager.InitType2HandleTable(Type2HandleMap);
            }
        }
        #endregion
    }
}
