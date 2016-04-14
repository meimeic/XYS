using System;
namespace XYS.Report
{
    /// <summary>
    /// 执行状态 -1:失败，0:继续，1:成功
    /// 执行失败时 type记录失败模块,message 记录失败原因，reportkey 记录失败的主键
    /// </summary>
    public class HandlerResult
    {
        #region 私有字段
        private int m_code;
        private Type m_type;
        private string m_message;
        private IReportKey m_reportKey;
        #endregion

        #region 构造函数
        public HandlerResult()
            : this(0, "continue")
        {
        }
        public HandlerResult(int code, string message)
            : this(code, null, message)
        {
        }
        public HandlerResult(int code, Type type, string message)
            : this(code, type, message, null)
        {
        }
        public HandlerResult(int code, Type type, string message, IReportKey key)
        {
            this.m_type = type;
            this.m_code = code;
            this.m_message = message;
            this.m_reportKey = key;
        }
        #endregion

        #region 实例属性
        public int Code
        {
            get { return this.m_code; }
            set { this.m_code = value; }
        }
        public Type FinalType
        {
            get { return this.m_type; }
            set { this.m_type = value; }
        }
        public string Message
        {
            get { return this.m_message; }
            set { this.m_message = value; }
        }
        public IReportKey ReportKey
        {
            get { return this.m_reportKey; }
            set { this.m_reportKey = value; }
        }
        #endregion
    }
}