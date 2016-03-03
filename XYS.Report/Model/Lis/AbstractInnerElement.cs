﻿using XYS.Report.Core;
namespace XYS.Report.Model.Lis
{
    public class AbstractInnerElement : ILisReportElement
    {
        #region 字段
        private readonly ReportElementTag m_elementTag;
        #endregion

        #region 构造函数
        protected AbstractInnerElement()
        {
            this.m_elementTag = ReportElementTag.Temp;
        }
        #endregion

        #region 实现接口
        public ReportElementTag ElementTag
        {
            get { return this.m_elementTag; }
        }
        #endregion
    }
}