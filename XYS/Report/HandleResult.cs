using System;
namespace XYS.Report
{
    /// <summary>
    /// 执行状态 -1:失败，0:继续，1:成功
    /// 执行失败时 type记录失败模块,message 记录失败原因，reportkey 记录失败的主键
    /// </summary>
    public class HandleResult
    {
        #region 私有字段
        private int m_resultCode;
        private Type m_handleType;
        private string m_message;
        #endregion

        #region 构造函数
        public HandleResult()
            : this(0, "the result is undefined!")
        {
        }
        public HandleResult(int code, string message)
            : this(code, null, message)
        {
        }
        public HandleResult(int code, Type type, string message)
        {
            this.m_resultCode = code;
            this.m_handleType = type;
            this.m_message = message;
        }
        #endregion

        #region 实例属性
        public int ResultCode
        {
            get { return this.m_resultCode; }
            set { this.m_resultCode = value; }
        }
        public Type HandleType
        {
            get { return this.m_handleType; }
            set { this.m_handleType = value; }
        }
        public string Message
        {
            get { return this.m_message; }
            set { this.m_message = value; }
        }
        #endregion
    }
}