using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;
using XYS.Report;
using XYS.Model.Lab;

using XYS.Lis.Report.Utils;
using XYS.Lis.Report.Persistent;
namespace XYS.Lis.Report.Handler
{
    public class InfoHandle : HandleSkeleton
    {
        #region 构造函数
        public InfoHandle(LabReportDAL dal)
            : base(dal)
        {
        }
        #endregion

        #region 重写父类抽象方法
        protected override bool InnerHandle(IFillElement element, IReportKey RK)
        {
            LOG.Info("reportinfo 内部处理");
            bool result = false;
            InfoElement info = element as InfoElement;
            if (info != null)
            {
                //设置报告ID
                info.ReportID = RK.ID;
                //修改备注内容
                if (info.SectionNo == 11)
                {
                    if (info.Memo != null)
                    {
                        info.Memo = info.Memo.Replace(";", SystemInfo.NewLine);
                    }
                }
                //清理身份证号信息
                if (info.CID != null)
                {
                    info.CID = info.CID.Trim();
                }
                //设置检验者、审核者签名url
                info.CheckerUrl = PUser.GetUserUrl(info.Checker);
                info.TechnicianUrl = PUser.GetUserUrl(info.Technician);

                //
                info.CheckerImage = PUser.GetUserImage(info.Checker);
                info.TechnicianImage = PUser.GetUserImage(info.Technician);
                result = true;
            }
            return result;
        }
        protected override bool InnerHandle(List<IFillElement> elements, IReportKey RK)
        {
            if (IsExist(elements))
            {
                LOG.Info("reportinfo 列表内部处理");
                bool result = false;
                for (int i = elements.Count - 1; i >= 0; i--)
                {
                    result = this.InnerHandle(elements[i], RK);
                    if (!result)
                    {
                        elements.RemoveAt(i);
                    }
                }
            }
            return true;
        }
        #endregion
    }
}