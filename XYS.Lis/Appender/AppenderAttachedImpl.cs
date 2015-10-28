//using System;
//using XYS.Lis.Core;
//namespace XYS.Lis.Appender
//{
//   public class AppenderAttachedImpl:IAppenderAttachable
//   {
//       #region 私有实例字段
//       private AppenderCollection m_appenderList;
//       private IAppender[] m_appenderArray;
//       #endregion
       
//       #region 公共构造函数
//       public AppenderAttachedImpl()
//       { }
//       #endregion
       
//       #region 实现IAppenderAttachable接口
//       public void AddAppender(IAppender appender)
//        {
//            if (appender == null)
//            {
//                throw new ArgumentNullException("appender");
//            }
//            if (this.m_appenderList == null)
//            {
//                this.m_appenderList = new AppenderCollection(1);
//            }
//            if (!this.m_appenderList.Contains(appender))
//            {
//                this.m_appenderList.Add(appender);
//            }
//        }

//        public AppenderCollection Appenders
//        {
//            get 
//            {
//                if (this.m_appenderList == null)
//                {
//                    return AppenderCollection.EmptyCollection;
//                }
//                else
//                {
//                    return AppenderCollection.ReadOnly(this.m_appenderList);
//                }
//            }
//        }

//        public IAppender GetAppender(string name)
//        {
//            if (this.m_appenderList != null && name != null)
//            {
//                foreach(IAppender appender in this.m_appenderList)
//                {
//                    if (appender.AppenderName == name)
//                    {
//                        return appender;
//                    }
//                }
//            }
//            return null;
//        }

//        public void RemoveAllAppenders()
//        {
//            if (this.m_appenderList != null)
//            {
//                foreach (IAppender appender in this.m_appenderList)
//                {
//                    try
//                    {
//                        appender.Close();
//                    }
//                    catch(Exception ex)
//                    {
//                    }
//                }
//                this.m_appenderList = null;
//                this.m_appenderArray = null;
//            }
//        }

//        public IAppender RemoveAppender(IAppender appender)
//        {
//            if (appender != null && this.m_appenderList != null)
//            {
//                this.m_appenderList.Remove(appender);
//                if (this.m_appenderList.Count == 0)
//                {
//                    this.m_appenderList = null;
//                }
//                this.m_appenderArray = null;
//            }
//            return appender;
//        }

//        public IAppender RemoveAppender(string name)
//        {
//            return RemoveAppender(GetAppender(name));
//        }
//       #endregion

//       #region 公共实例方法
//        public int AppendLoopOnAppenders(ILisReportElement reportElement)
//        {
//            if (reportElement == null)
//            {
//                throw new ArgumentNullException("reportElement");
//            }
//            // m_appenderList is null when empty
//            if (m_appenderList == null)
//            {
//                return 0;
//            }
//            if (m_appenderArray == null)
//            {
//                m_appenderArray = m_appenderList.ToArray();
//            }
//            foreach (IAppender appender in m_appenderArray)
//            {
//                try
//                {
//                    appender.DoAppend(reportElement);
//                }
//                catch (Exception ex)
//                {
//                   // LogLog.Error(declaringType, "Failed to append to appender [" + appender.Name + "]", ex);
//                }
//            }
//            return m_appenderList.Count;
//        }
//       #endregion
//   }
//}
