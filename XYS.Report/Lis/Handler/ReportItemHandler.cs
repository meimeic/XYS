﻿using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public class ReportItemHandler : ReportHandlerSkeleton
    {
        #region 构造函数
        public ReportItemHandler()
            : base()
        {
        }
        #endregion

        #region 实现父类抽象方法
        protected override void OperateReport(ReportReportElement report, HandlerResult result)
        {
            //报告级操作
            ReportItemElement rie = null;
            List<AbstractSubFillElement> itemList = report.GetReportItem(typeof(ReportItemElement));
            if (IsExist(itemList))
            {
                foreach (AbstractSubFillElement item in itemList)
                {
                    rie = item as ReportItemElement;
                    //检验项处理
                    if (!OperateItem(rie))
                    {
                        continue;
                    }
                    //设置检验大项集合
                    if (!report.SuperItemList.Contains(rie.ParItemNo))
                    {
                        report.SuperItemList.Add(rie.ParItemNo);
                    }
                    //将合法检验项添加到输出集合中
                    report.ReportItemCollection.Add(rie);
                }
            }
            this.SetHandlerResult(result, 0, "handle reportitem successfully and continue!");
            return;
        }
        #endregion

        #region 检验项处理逻辑
        protected bool OperateItem(ReportItemElement rie)
        {
            if (rie == null)
            {
                return false;
            }
            //判断检验项是否合法
            if (IsLegal(rie))
            {
                return false;
            }
            //合法，检验项处理操作
            if (rie.ItemNo == 50004360 || rie.ItemNo == 50004370)
            {
                if (rie.RefRange != null)
                {
                    rie.RefRange = rie.RefRange.Replace(";", SystemInfo.NewLine);
                }
            }
            return true;
        }
        //检验项是否合法
        private bool IsLegal(ReportItemElement rie)
        {
            return IsLegalBySecret(rie.SecretGrade);
        }
        //根据保密等级判断检验项是否合法
        protected bool IsLegalBySecret(int secretGrade)
        {
            if (secretGrade > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}