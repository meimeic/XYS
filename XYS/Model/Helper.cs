using System; 
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace XYS.Model
{
    public class Helper
    {
        public static byte[] SerializeObject(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                byte[] read = new byte[ms.Length];
                ms.Read(read, 0, read.Length);
                return read;
            }
        }
        public static object DeserializeObject(byte[] bytes)
        {
            object obj = null;
            if (bytes == null)
            {
                return obj;
            }
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                ms.Position = 0;
                BinaryFormatter formatter = new BinaryFormatter();
                obj = formatter.Deserialize(ms);
                return obj;
            }
        }
    }
}