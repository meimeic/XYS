using System.Collections;

using XYS.DAL;
namespace XYS.Report.Lis.Util
{
    public class LisPUser
    {
        #region 私有静态字段
        private static readonly Hashtable SIGN_IMAGE_TABLE = new Hashtable();
        #endregion

        #region 公共静态方法
        public static byte[] GetSignImage(string name)
        {
            if (SIGN_IMAGE_TABLE.Count == 0)
            {
                InitImageTable();
            }
            return (byte[])SIGN_IMAGE_TABLE[name];
        }
        #endregion

        #region 私有静态方法
        /// <summary>
        /// 初始化签名表
        /// </summary>
        private static void InitImageTable()
        {
            CommonDAL.GetSignImage(SIGN_IMAGE_TABLE);
        }
        #endregion
    }
}
