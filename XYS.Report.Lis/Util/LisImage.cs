using System;
using System.IO;

using XYS.Util;
namespace XYS.Report.Util
{
    public class LisImage
    {
        public LisImage()
        {
        }
        public static string GetImageFilePath()
        {
            try
            {
                string applicationBaseDirectory = SystemInfo.ApplicationBaseDirectory;
                string filePath = Path.Combine(applicationBaseDirectory, "normal");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                return filePath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static byte[] ReadImageFile(string fileFullName)
        {
            try
            {
                FileStream fs = File.OpenRead(fileFullName); //OpenRead
                int filelength = 0;
                filelength = (int)fs.Length; //获得文件长度 
                Byte[] image = new Byte[filelength]; //建立一个字节数组 
                fs.Read(image, 0, filelength); //按字节流读取 
                return image;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
