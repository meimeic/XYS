using System;
namespace XYS.Report
{
    /// <summary>
    /// 执行状态 -1:失败，0:继续，1:成功
    /// 执行失败时 type记录失败模块,message 记录失败原因
    /// </summary>
    public class HandleResult
    {
        #region 私有字段
        private int m_resultCode;
        private Type m_handleType;
        private Exception m_exception;
        #endregion

        #region 构造函数
        public HandleResult()
        {
        }
        public HandleResult(int code, Type type = null, Exception ex = null)
        {
            this.m_resultCode = code;
            this.m_handleType = type;
            this.m_exception = ex;
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
        public Exception Exception
        {
            get { return this.m_exception; }
            set { this.m_exception = value; }
        }
        #endregion

        #region 
        public void Clear()
        {
            this.m_resultCode = 0;
            this.m_handleType = null;
            this.m_exception = null;
        }
        #endregion
    }
}