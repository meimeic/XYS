﻿using System;
using System.Collections.Generic;

using XYS.Model;
using XYS.Lis.Core;
using XYS.Lis.Model;
using XYS.Lis.Util;

namespace XYS.Lis.Handler
{
    public class ReportInfoHandler : ReportHandlerSkeleton
    {
        #region 静态变量
        public static readonly string m_defaultHandlerName = "ReportInfoHandler";
        #endregion

          #region 构造函数
        public ReportInfoHandler()
            : this(m_defaultHandlerName)
        {
        }
        public ReportInfoHandler(string handlerName)
            : base(handlerName)
        {
        }
        #endregion

        #region 实现父类抽象方法
        protected override bool OperateElement(ILisReportElement element, ReportElementTag elementTag)
        {
            if (elementTag == ReportElementTag.ReportElement)
            {
                ReportReportElement rre = element as ReportReportElement;
                return OperateInfoList(rre);
            }
            if (elementTag == ReportElementTag.InfoElement)
            {
                ReportInfoElement rie = element as ReportInfoElement;
                return OperateInfo(rie);
            }
            return true;
        }
        #endregion

        #region info项的内部处理逻辑
        protected virtual bool OperateInfoList(ReportReportElement rre)
        {
            //info 处理
            ReportInfoElement rie;
            List<ILisReportElement> infoList = rre.GetReportItem(ReportElementTag.InfoElement);
            OperateElementList(infoList, ReportElementTag.InfoElement);
            if (infoList.Count > 0)
            {
                rie = infoList[0] as ReportInfoElement;
                rre.SectionNo = rie.SectionNo;
                rre.ParItemName = rie.ParItemName;
                rre.ReceiveDateTime = rie.ReceiveDateTime;
                rre.CollectDateTime = rie.CollectDateTime;
                rre.InceptDateTime = rie.InceptDateTime;
                rre.TestDateTime = rie.TestDateTime;
                rre.CheckDateTime = rie.CheckDateTime;
                rre.SecondeCheckDateTime = rie.SecondeCheckDateTime;
                rre.ClinicType = rie.ClinicType;
                //设置签名图片
                if (rie.Checker != null && !rie.Checker.Equals(""))
                {
                    rre.CheckerImage = LisPUser.GetSignImage(rie.Checker);
                }
                if (rie.Technician != null && !rie.Technician.Equals(""))
                {
                    rre.TechnicianImage = LisPUser.GetSignImage(rie.Technician);
                }
                return true;
            }
            return false;
        }
        protected virtual bool OperateInfo(ReportInfoElement rie)
        {
            //此处可以添加判断是否删除代码
            if (rie.SectionNo == 10)
            {
                if (rie.FormMemo != null)
                {
                    rie.FormMemo = rie.FormMemo.Replace(";", SystemInfo.NewLine);
                }
            }
            return true;
        }
        #endregion
        
        #region
        private void SetRemarkFlagByInfo(ReportReportElement rre, ReportInfoElement rie)
        {
            //
        }
        #endregion
    }
}