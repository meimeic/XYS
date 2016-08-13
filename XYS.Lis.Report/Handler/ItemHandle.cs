using System;
using System.Collections.Generic;

using XYS.Util;
using XYS.Report;
using XYS.Model.Lab;
using XYS.Lis.Report.Persistent;
namespace XYS.Lis.Report.Handler
{
    public class ItemHandle : HandleSkeleton
    {
        #region 构造函数
        public ItemHandle(LabReportDAL dal)
            : base(dal)
        {
        }
        #endregion

        #region 实现父类抽象方法
        protected override bool InnerHandle(IFillElement element, IReportKey RK)
        {
            bool result = false;
            ItemElement item = element as ItemElement;
            if (item != null)
            {
                item.ReportID = RK.ID;
                result = this.OperateItem(item);
            }
            return result;
        }
        protected override bool InnerHandle(List<IFillElement> elements, IReportKey RK)
        {
            if (IsExist(elements))
            {
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

        #region 检验项处理逻辑
        private bool OperateItem(ItemElement rie)
        {
            if (rie == null)
            {
                return false;
            }
            //判断检验项是否合法
            if (!IsLegal(rie))
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
        private bool IsLegal(ItemElement rie)
        {
            int secretGrade = rie.SecretGrade;
            //根据保密等级判断检验项是否合法
            if (secretGrade > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
    }
}