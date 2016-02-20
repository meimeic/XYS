using System;
using System.Collections;

using XYS.Lis.Core;
namespace XYS.Lis.Core
{
    public class ElementTypeMap
    {
        #region 字段
        private Hashtable m_mapName2ElementType;
        #endregion

        #region 构造函数
        public ElementTypeMap()
        {
            this.m_mapName2ElementType = new Hashtable(3);
        }
        #endregion

        #region 属性
        public int Count
        {
            get { return this.m_mapName2ElementType.Count; }
        }
        public ElementType this[string name]
        {
            get
            {
                lock (this)
                {
                    return this.m_mapName2ElementType[name] as ElementType;
                }
            }
        }
        public ElementTypeCollection AllElementTypes
        {
            get
            {
                lock (this)
                {
                    return new ElementTypeCollection(this.m_mapName2ElementType.Values);
                }
            }
        }
        #endregion
        
        #region 方法
        public void Add(ElementType elementType)
        {
            if (elementType == null)
            {
                throw new ArgumentNullException("elementType");
            }
            lock (this)
            {
                this.m_mapName2ElementType[elementType.Name] = elementType;
            }
        }
        public void Clear()
        {
            this.m_mapName2ElementType.Clear();
        }
        #endregion
    }
}
