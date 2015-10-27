//using System;
//using XYS.Lis.Core;
//namespace XYS.Lis.Model
//{
//   public class LisReportPKElement:ILisPKElement
//   {
//       #region 私有静态字段
//       private static readonly string ELEMENTNAME = "LIS_PK_ELEMENT";
//       private static readonly long ELEMENTVALUE = 100001L;
//       #endregion

//       #region 私有字段
//       private object m_pk;
//       private Type m_declareType;
//       #endregion

//       #region 公共构造方法
//       public LisReportPKElement()
//       {
//           this.m_pk = new object();
//       }
//       public LisReportPKElement(object pk)
//       {
//           this.m_pk = pk;
//       }
//       #endregion
//       public string PKElementName
//        {
//            get { return ELEMENTNAME; }
//        }

//        public long PKElementValue
//        {
//            get { return ELEMENTVALUE; }
//        }

//        public Type PKType
//        {
//            get { return this.m_pk.GetType(); }
//        }
//        public Type DeclaredType
//        {
//            get { return this.m_declareType; }
//            set { this.m_declareType = value; }
//        }
//        public object PK
//        {
//            get { return this.m_pk; }
//            set { this.m_pk = value; }
//        }
//    }
//}
