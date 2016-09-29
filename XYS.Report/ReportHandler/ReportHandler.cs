using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using XYS.Report.Attributes;
using XYS.Report.Entities;
using XYS.Report.Persistent;
namespace XYS.Report.ReportHandler
{
    public interface IReportHandler<TDB, TTS>
        where TDB : IDBReport
        where TTS : ITSReport
    {

        TDB DBReport { get; set; }
        TTS TSReport { get; set; }
        IReportDAO ReportDAO { get; set; }
        void OnExecuting();
        void Execute();
        void OnExecuted();
    }
    public abstract class ReportHandler<TDB, TTS> : IReportHandler<TDB, TTS>
        where TDB : IDBReport
        where TTS : ITSReport
    {
        public virtual IReportDAO ReportDAO { get; set; }
        public virtual TDB DBReport { get; set; }
        public virtual TTS TSReport { get; set; }

        public virtual void OnExecuting()
        {
        }

        public abstract void Execute();

        public virtual void OnExecuted()
        {
            
        }

        protected abstract bool Convert();
        protected virtual bool FillItem()
        {
            if (DBReport != null)
            {
                IReportPK RK = DBReport.RK;
                if (!RK.Configured)
                {
                    return false;
                }
                foreach (Type type in DBReport.FillTypes)
                {
                    List<IDBReportItem> ItemList = DBReport.ItemCollection(type);
                    string sql = GenderSql(type, RK);
                    this.ReportDAO.Fill(ItemList, type, sql);
                }
                return true;
            }
            return false;
        }
        protected virtual string GenderSql(Type type, IReportPK RK)
        {
            return GenderPreSql(type) + RK.KeyWhere();
        }
        protected virtual string GenderPreSql(Type type)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select ");
            PropertyInfo[] props = type.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (IsDBColumn(prop))
                {
                    sb.Append(prop.Name);
                    sb.Append(',');
                }
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(" from ");
            sb.Append(type.Name);
            sb.Append(" ");
            return sb.ToString();
        }
        protected bool IsDBColumn(PropertyInfo prop)
        {
            if (prop != null)
            {
                try
                {
                    object[] attrs = prop.GetCustomAttributes(typeof(DBColumnAttribute), true);
                    if (attrs != null && attrs.Length > 0)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }
    }
}
