using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using XYS.Util;
using XYS.Common;
using XYS.Report.Lis.Util;
using XYS.Report.Lis.Model;
using XYS.Report.Lis.Persistent;
namespace XYS.Report.Lis.Handler
{
    public class ReportFillHandle : ReportHandleSkeleton
    {
        #region 静态常量
        private static readonly Hashtable Section2FillTypeMap;
        #endregion

        #region 只读字段
        private readonly ReportDAL m_reportDAL;
        #endregion

        #region 构造函数
        static ReportFillHandle()
        {
            Section2FillTypeMap = new Hashtable(20);
            ReportFillHandle.InitFillElementTable();
        }
        public ReportFillHandle()
            : base()
        {
            this.m_reportDAL = new ReportDAL();
        }
        #endregion

        #region 实例属性
        protected ReportDAL ReportDAL
        {
            get { return this.m_reportDAL; }
        }
        #endregion

        #region 实现父类抽象方法
        public override void ReportHandle(ReportReportElement report)
        {
            LOG.Info("填充报告处理开始");
            if (report.ReportPK.SectionNo == 45)
            {
                this.FillAndMerge(report);
            }
            else
            {
                this.Fill(report);
            }
            this.OnFill(report);
        }
        private void Fill(ReportReportElement report)
        {
            ReportPK PK = report.ReportPK;
            //填充报告元素
            FillReportElement(report.ReportInfo, PK, report.HandleResult);
            if (report.HandleResult.ResultCode < 0)
            {
                return;
            }
            //填充子项
            List<Type> availableElementList = GetAvailableFillElements(PK);
            if (availableElementList != null && availableElementList.Count > 0)
            {
                List<IFillElement> tempList = null;
                foreach (Type type in availableElementList)
                {
                    tempList = report.GetReportItem(type);
                    FillSubElements(tempList, PK, type, report.HandleResult);
                    if (report.HandleResult.ResultCode < 0)
                    {
                        return;
                    }
                }
            }
        }
        private void FillAndMerge(ReportReportElement report)
        {
            string sql = null;
            ReportDAL lisDAL = new ReportDAL();
            ReportPKDAL keyDAL = new ReportPKDAL();
            List<ReportPK> PKList = new List<ReportPK>(5);

            keyDAL.InitReportKey(report.ReportPK, PKList);

            Type type = null;
            bool formConfig = false;
            List<IFillElement> tempList = null;
            foreach (ReportPK key in PKList)
            {
                if (!formConfig)
                {
                    sql = GenderSql(report.ReportInfo, key);
                    try
                    {
                        lisDAL.Fill(report.ReportInfo, sql);
                        formConfig = true;
                        report.ReportPK = key;
                        this.SetHandlerResult(report.HandleResult, 20);
                    }
                    catch (Exception ex)
                    {
                        this.SetHandlerResult(report.HandleResult, -21,"", this.GetType(), ex);
                        return;
                    }
                }

                //
                type = typeof(ReportItemElement);
                tempList = report.GetReportItem(type);
                sql = GenderSql(type, key);
                try
                {
                    lisDAL.FillList(tempList, type, sql);
                    this.SetHandlerResult(report.HandleResult, 30);
                }
                catch (Exception ex)
                {
                    this.SetHandlerResult(report.HandleResult, -31,"", this.GetType(), ex);
                    return;
                }

                //
                type = typeof(ReportCustomElement);
            }
        }
        #endregion

        #region 填充数据
        private void FillReportElement(IFillElement report, ReportPK PK, HandleResult result)
        {
            string sql = GenderSql(report, PK);
            LOG.Info("生成报告主元素的SQL语句:" + sql);
            try
            {
                LOG.Info("报告主元素数据填充");
                this.ReportDAL.Fill(report, sql);
            }
            catch (Exception ex)
            {
                this.SetHandlerResult(result, -10, "报告主元素填充异常", this.GetType(), ex);
            }
        }
        private void FillSubElements(List<IFillElement> subElementList, ReportPK PK, Type type, HandleResult result)
        {
            string sql = GenderSql(type, PK);
            LOG.Info("生成报告子元素" + type.Name + "的SQL语句:" + sql);
            try
            {
                LOG.Info("报告子元素" + type.Name + "集合填充");
                this.ReportDAL.FillList(subElementList, type, sql);
            }
            catch (Exception ex)
            {
                this.SetHandlerResult(result, -11, "报告子元素" + type.Name + "集合填充异常", this.GetType(), ex);
            }
        }
        #endregion

        #region 生成sql语句
        protected string GenderSql(IFillElement element, ReportPK PK)
        {
            return GenderSql(element.GetType(), PK);
        }
        protected string GenderSql(Type type, ReportPK PK)
        {
            return GenderPreSQL(type) + GenderWhere(PK);
        }
        protected string GenderPreSQL(Type type)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select ");
            PropertyInfo[] props = type.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (IsColumn(prop))
                {
                    sb.Append(prop.Name);
                    sb.Append(',');
                }
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(" from ");
            sb.Append(type.Name);
            return sb.ToString();
        }
        protected string GenderWhere(ReportPK PK)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" where ");
            sb.Append("receivedate='");
            sb.Append(PK.ReceiveDate.ToString("yyyy-MM-dd"));
            sb.Append("' and sectionno=");
            sb.Append(PK.SectionNo);
            sb.Append(" and testtypeno=");
            sb.Append(PK.TestTypeNo);
            sb.Append(" and sampleno='");
            sb.Append(PK.SampleNo);
            sb.Append("'");
            return sb.ToString();

            //foreach (KeyColumn key in PK.KeySet)
            //{
            //    if (key.Value.GetType().Equals(typeof(System.Int32)))
            //    {
            //        sb.Append(key.Name);
            //        sb.Append("=");
            //        sb.Append(key.Value.ToString());
            //    }
            //    else if (key.Value.GetType().Equals(typeof(System.DateTime)))
            //    {
            //        sb.Append(key.Name);
            //        sb.Append("='");
            //        sb.Append(((DateTime)(key.Value)).ToString("yyyy-MM-dd"));
            //        sb.Append("'");
            //    }
            //    else
            //    {
            //        sb.Append(key.Name);
            //        sb.Append("='");
            //        sb.Append(key.Value.ToString());
            //        sb.Append("'");
            //    }
            //    sb.Append(" and ");
            //}
            //sb.Remove(sb.Length - 5, 5);
            //return sb.ToString();
        }
        private bool IsColumn(PropertyInfo prop)
        {
            if (prop != null)
            {
                object[] attrs = prop.GetCustomAttributes(typeof(ColumnAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 辅助方法
        private static void InitFillElementTable()
        {
            lock (Section2FillTypeMap)
            {
                ConfigManager.InitSection2FillElementTable(Section2FillTypeMap);
            }
        }
        protected static List<Type> GetAvailableFillElements(ReportPK RK)
        {
            return Section2FillTypeMap[RK.SectionNo] as List<Type>;
        }
        //protected static List<Type> GetAvailableFillElements(LisReportPK RK)
        //{
        //    int sectionNo = -1;
        //    foreach (KeyColumn key in RK.KeySet)
        //    {
        //        if (key.Name.Equals("sectionno"))
        //        {
        //            sectionNo = (int)key.Value;
        //            break;
        //        }
        //    }
        //    if (sectionNo == -1)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return Section2FillTypeMap[sectionNo] as List<Type>;
        //    }
        //}
        #endregion

        #region 触发事件
        private void OnFill(ReportReportElement report)
        {
            if (report.HandleResult.ResultCode < 0)
            {
                this.OnHandleError(report);
            }
            else
            {
                this.OnHandleSuccess(report);
            }
        }
        #endregion
    }
}