using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;
using XYS.Report;
using XYS.Lis.Report.Model;
namespace XYS.Lis.Report.Handler
{
    public class ReportItemHandle : ReportHandleSkeleton
    {
        #region 构造函数
        public ReportItemHandle()
            : base()
        {
        }
        #endregion

        #region 实现父类抽象方法
        protected override void InnerHandle(ReportReportElement report)
        {
            LOG.Info("报告常规项处理");
            //报告级操作
            ReportItemElement rie = null;
            List<IFillElement> itemList = report.GetReportItem(typeof(ReportItemElement));
            if (IsExist(itemList))
            {
                foreach (IFillElement item in itemList)
                {
                    rie = item as ReportItemElement;
                    //检验项处理
                    if (!OperateItem(rie))
                    {
                        continue;
                    }
                    rie.ReportID = report.ReportID;
                    //将合法检验项添加到输出集合中
                    report.ReportItemCollection.Add(rie);
                }
            }
            this.SetHandlerResult(report.HandleResult, 30, "处理ReportItem集合成功", typeof(ReportItemHandle));
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
            int secretGrade = rie.SecretGrade;
            //根据保密等级判断检验项是否合法
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

        protected override bool HandleElement(IFillElement element, ReportPK RK)
        {
            bool result = false;
            ReportItemElement item = element as ReportItemElement;
            if (item != null)
            {
                item.ReportID = RK.ID;
                result = OperateItem(item);
            }
            return result;
        }

        protected override bool HandleElement(List<IFillElement> elements, ReportPK RK)
        {
            bool result = false;
            if (IsExist(elements))
            {
 
            }
        }
    }
}