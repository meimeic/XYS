using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Globalization;
using System.Configuration;

namespace XYS.Util
{
    public sealed class SystemInfo
    {
        #region 私有常量
        private readonly static string DEFAULT_NULL_TEXT = "(null)";
        private readonly static string DEFAULT_NOT_AVAILABLE_TEXT = "NOT AVAILABLE";
        #endregion

        #region 私有实例构造函数
        private SystemInfo()
        {
        }
        #endregion Private Instance Constructors

        #region 静态构造函数
        static SystemInfo()
        {
            string nullText = DEFAULT_NULL_TEXT;
            string notAvailableText = DEFAULT_NOT_AVAILABLE_TEXT;
#if !NETCF
            // Look for XYS.NullText in AppSettings
            string nullTextAppSettingsKey = SystemInfo.GetAppSetting("XYS.NullText");
            if (nullTextAppSettingsKey != null && nullTextAppSettingsKey.Length > 0)
            {
                ConsoleInfo.Debug(declaringType, "Initializing NullText value to [" + nullTextAppSettingsKey + "].");
                nullText = nullTextAppSettingsKey;
            }
            // Look for XYS.NotAvailableText in AppSettings
            string notAvailableTextAppSettingsKey = SystemInfo.GetAppSetting("XYS.NotAvailableText");
            if (notAvailableTextAppSettingsKey != null && notAvailableTextAppSettingsKey.Length > 0)
            {
                ConsoleInfo.Debug(declaringType, "Initializing NotAvailableText value to [" + notAvailableTextAppSettingsKey + "].");
                notAvailableText = notAvailableTextAppSettingsKey;
            }
#endif
            s_notAvailableText = notAvailableText;
            s_nullText = nullText;
        }
        #endregion

        #region 公共静态属性
        //获取回车换行符
        public static string NewLine
        {
            get
            {
#if NETCF
				return "\r\n";
#else
                return System.Environment.NewLine;
#endif
            }
        }
        //获取应用基本路径
        public static string ApplicationBaseDirectory
        {
            get
            {
#if NETCF
				return System.IO.Path.GetDirectoryName(SystemInfo.EntryAssemblyLocation) + System.IO.Path.DirectorySeparatorChar;
#else
                return AppDomain.CurrentDomain.BaseDirectory;
#endif
            }
        }
        //获取程序默认配置文件
        public static string ConfigurationFileLocation
        {
            get
            {
#if NETCF
				return SystemInfo.EntryAssemblyLocation+".config";
#else
                return System.AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
#endif
            }
        }
        //获取程序集位置
        public static string EntryAssemblyLocation
        {
            get
            {
#if NETCF
				return SystemInfo.NativeEntryAssemblyLocation;
#else
                return System.Reflection.Assembly.GetEntryAssembly().Location;
#endif
            }
        }
        //获取当前线程id
        public static int CurrentThreadId
        {
            get
            {
#if NETCF_1_0
				return System.Threading.Thread.CurrentThread.GetHashCode();
#elif NET_2_0 || NETCF_2_0 || MONO_2_0
				return System.Threading.Thread.CurrentThread.ManagedThreadId;
#else
                //return AppDomain.GetCurrentThreadId();
                return System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif
            }
        }
        //获取主机名
        public static string HostName
        {
            get
            {
                if (s_hostName == null)
                {
                    // Get the DNS host name of the current machine
                    try
                    {
                        // Lookup the host name
                        s_hostName = System.Net.Dns.GetHostName();
                    }
                    catch (System.Net.Sockets.SocketException)
                    {
                        ConsoleInfo.Debug(declaringType, "Socket exception occurred while getting the dns hostname. Error Ignored.");
                    }
                    catch (System.Security.SecurityException)
                    {
                        // We may get a security exception looking up the hostname
                        // You must have Unrestricted DnsPermission to access resource
                        ConsoleInfo.Debug(declaringType, "Security exception occurred while getting the dns hostname. Error Ignored.");
                    }
                    catch (Exception ex)
                    {
                        ConsoleInfo.Debug(declaringType, "Some other exception occurred while getting the dns hostname. Error Ignored.", ex);
                    }
                    // Get the NETBIOS machine name of the current machine
                    //获取当前主机的 netbios名称
                    if (s_hostName == null || s_hostName.Length == 0)
                    {
                        try
                        {
#if (!SSCLI && !NETCF)
                            s_hostName = Environment.MachineName;
#endif
                        }
                        catch (InvalidOperationException)
                        {
                        }
                        catch (System.Security.SecurityException)
                        {
                            // We may get a security exception looking up the machine name
                            // You must have Unrestricted EnvironmentPermission to access resource
                        }
                    }

                    // Couldn't find a value
                    if (s_hostName == null || s_hostName.Length == 0)
                    {
                        s_hostName = s_notAvailableText;
                        ConsoleInfo.Debug(declaringType, "Could not determine the hostname. Error Ignored. Empty host name will be used");
                    }
                }
                return s_hostName;
            }
        }
        //获取应用程序域的友好名称
        public static string ApplicationFriendlyName
        {
            get
            {
                if (s_appFriendlyName == null)
                {
                    try
                    {
#if !NETCF
                        s_appFriendlyName = AppDomain.CurrentDomain.FriendlyName;
#endif
                    }
                    catch (System.Security.SecurityException)
                    {
                        // This security exception will occur if the caller does not have 
                        // some undefined set of SecurityPermission flags.
                        ConsoleInfo.Debug(declaringType, "Security exception while trying to get current domain friendly name. Error Ignored.");
                    }
                    if (string.IsNullOrEmpty(s_appFriendlyName))
                    {
                        try
                        {
                            string assemblyLocation = SystemInfo.EntryAssemblyLocation;
                            s_appFriendlyName = Path.GetFileName(assemblyLocation);
                        }
                        catch (System.Security.SecurityException)
                        {
                            // Caller needs path discovery permission
                        }
                    }
                    //找不到友好名称
                    if (string.IsNullOrEmpty(s_appFriendlyName))
                    {
                        s_appFriendlyName = s_notAvailableText;
                    }
                }
                return s_appFriendlyName;
            }
        }
        //获取当前时间
        public static DateTime ProcessStartTime
        {
            get { return s_processStartTime; }
        }

        public static string NullText
        {
            get { return s_nullText; }
            set { s_nullText = value; }
        }
        public static string NotAvailableText
        {
            get { return s_notAvailableText; }
            set { s_notAvailableText = value; }
        }
        #endregion

        #region 公共静态方法

        //获取特定程序集的位置信息
        public static string AssemblyLocationInfo(Assembly myAssembly)
        {
#if NETCF
			return "Not supported on Microsoft .NET Compact Framework";
#else
            if (myAssembly.GlobalAssemblyCache)
            {
                return "Global Assembly Cache";
            }
            else
            {
                try
                {
#if NET_4_0
					if (myAssembly.IsDynamic)
					{
						return "Dynamic Assembly";
					}
#else
                    if (myAssembly is System.Reflection.Emit.AssemblyBuilder)
                    {
                        return "Dynamic Assembly";
                    }
                    else if (myAssembly.GetType().FullName == "System.Reflection.Emit.InternalAssemblyBuilder")
                    {
                        return "Dynamic Assembly";
                    }
#endif
                    else
                    {
                        // This call requires FileIOPermission for access to the path
                        // if we don't have permission then we just ignore it and
                        // carry on.
                        return myAssembly.Location;
                    }
                }
                catch (NotSupportedException)
                {
                    // The location information may be unavailable for dynamic assemblies and a NotSupportedException
                    // is thrown in those cases. See: http://msdn.microsoft.com/de-de/library/system.reflection.assembly.location.aspx
                    return "Dynamic Assembly";
                }
                catch (TargetInvocationException ex)
                {
                    return "Location Detect Failed (" + ex.Message + ")";
                }
                catch (ArgumentException ex)
                {
                    return "Location Detect Failed (" + ex.Message + ")";
                }
                catch (System.Security.SecurityException)
                {
                    return "Location Permission Denied";
                }
            }
#endif
        }
        //获取指定类型的包含程序集的全名
        public static string AssemblyQualifiedName(Type type)
        {
            return type.FullName + ", " + type.Assembly.FullName;
        }
        //程序集的简称
        public static string AssemblyShortName(Assembly myAssembly)
        {
            string name = myAssembly.FullName;
            int offset = name.IndexOf(',');
            if (offset > 0)
            {
                name = name.Substring(0, offset);
            }
            return name.Trim();
            // TODO: Do we need to unescape the assembly name string? 
            // Doc says '\' is an escape char but has this already been 
            // done by the string loader?
        }
        //程序集文件名称
        public static string AssemblyFileName(Assembly myAssembly)
        {
#if NETCF
			// This is not very good because it assumes that only
			// the entry assembly can be an EXE. In fact multiple
			// EXEs can be loaded in to a process.

			string assemblyShortName = SystemInfo.AssemblyShortName(myAssembly);
			string entryAssemblyShortName = System.IO.Path.GetFileNameWithoutExtension(SystemInfo.EntryAssemblyLocation);

			if (string.Compare(assemblyShortName, entryAssemblyShortName, true) == 0)
			{
				// assembly is entry assembly
				return assemblyShortName + ".exe";
			}
			else
			{
				// assembly is not entry assembly
				return assemblyShortName + ".dll";
			}
#else
            return System.IO.Path.GetFileName(myAssembly.Location);
#endif
        }

        public static Type GetTypeFromString(Type relativeType, string typeName, bool throwOnError, bool ignoreCase)
        {
            return GetTypeFromString(relativeType.Assembly, typeName, throwOnError, ignoreCase);
        }
        public static Type GetTypeFromString(string typeName, bool throwOnError, bool ignoreCase)
        {
            return GetTypeFromString(Assembly.GetCallingAssembly(), typeName, throwOnError, ignoreCase);
        }
        public static Type GetTypeFromString(Assembly relativeAssembly, string typeName, bool throwOnError, bool ignoreCase)
        {
            // typeName 不包含程序集名称
            if (typeName.IndexOf(',') == -1)
            {
                ConsoleInfo.Debug(declaringType, "Loading type [" + typeName + "] from assembly [" + relativeAssembly.FullName + "]");
#if NETCF
				return relativeAssembly.GetType(typeName, throwOnError);
#else
                // 尝试从调用者所在程序集加载类型
                Type type = relativeAssembly.GetType(typeName, false, ignoreCase);
                if (type != null)
                {
                    // Found type in relative assembly
                    ConsoleInfo.Debug(declaringType, "Loaded type [" + typeName + "] from assembly [" + relativeAssembly.FullName + "]");
                    return type;
                }
                Assembly[] loadedAssemblies = null;
                try
                {
                    //获取加载的程序集
                    loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                }
                catch (System.Security.SecurityException)
                {
                    // Insufficient permissions to get the list of loaded assemblies
                }
                if (loadedAssemblies != null)
                {
                    //从加载的程序集中加载类型
                    foreach (Assembly assembly in loadedAssemblies)
                    {
                        type = assembly.GetType(typeName, false, ignoreCase);
                        if (type != null)
                        {
                            // Found type in loaded assembly
                            ConsoleInfo.Debug(declaringType, "Loaded type [" + typeName + "] from assembly [" + assembly.FullName + "] by searching loaded assemblies.");
                            return type;
                        }
                    }
                }
                // Didn't find the type
                if (throwOnError)
                {
                    throw new TypeLoadException("SystemInfo:Could not load type [" + typeName + "]. Tried assembly [" + relativeAssembly.FullName + "] and all loaded assemblies");
                }
                return null;
#endif
            }
            else
            {
                //包含程序集名称
                ConsoleInfo.Debug(declaringType, "Loading type [" + typeName + "] from global Type");

#if NETCF
				// In NETCF 2 and 3 arg versions seem to behave differently
				// https://issues.apache.org/jira/browse/LOG4NET-113
				return Type.GetType(typeName, throwOnError);
#else
                return Type.GetType(typeName, throwOnError, ignoreCase);
#endif
            }
        }
        public static Guid NewGuid()
        {
#if NETCF_1_0
			return PocketGuid.NewGuid();
#else
            return Guid.NewGuid();
#endif
        }
        public static ArgumentOutOfRangeException CreateArgumentOutOfRangeException(string parameterName, object actualValue, string message)
        {
#if NETCF_1_0
			return new ArgumentOutOfRangeException(message + " [param=" + parameterName + "] [value=" + actualValue + "]");
#elif NETCF_2_0
			return new ArgumentOutOfRangeException(parameterName, message + " [value=" + actualValue + "]");
#else
            return new ArgumentOutOfRangeException(parameterName, actualValue, message);
#endif
        }

        public static bool TryParse(string s, out int val)
        {
#if NETCF
			val = 0;
			try
			{
				val = int.Parse(s, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture);
				return true;
			}
			catch
			{
			}
			return false;
#else
            // 初始化 out 参数
            val = 0;
            try
            {
                double doubleVal;
                if (Double.TryParse(s, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out doubleVal))
                {
                    val = Convert.ToInt32(doubleVal);
                    return true;
                }
            }
            catch
            {
                // Ignore exception, just return false
            }
            return false;
#endif
        }
        public static bool TryParse(string s, out long val)
        {
#if NETCF
			val = 0;
			try
			{
				val = long.Parse(s, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture);
				return true;
			}
			catch
			{
			}
			return false;
#else
            // 初始化 out 参数
            val = 0;
            try
            {
                double doubleVal;
                if (Double.TryParse(s, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out doubleVal))
                {
                    val = Convert.ToInt64(doubleVal);
                    return true;
                }
            }
            catch
            {
                // Ignore exception, just return false
            }
            return false;
#endif
        }
        public static bool TryParse(string s, out short val)
        {
#if NETCF
			val = 0;
			try
			{
				val = short.Parse(s, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture);
				return true;
			}
			catch
			{
			}

			return false;
#else
            // 初始化 out 参数
            val = 0;
            try
            {
                double doubleVal;
                if (Double.TryParse(s, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out doubleVal))
                {
                    val = Convert.ToInt16(doubleVal);
                    return true;
                }
            }
            catch
            {
                // Ignore exception, just return false
            }
            return false;
#endif
        }
        public static bool ToBoolean(string argValue, bool defaultValue)
        {
            if (argValue != null && argValue.Length > 0)
            {
                try
                {
                    return bool.Parse(argValue);
                }
                catch (Exception e)
                {
                    ConsoleInfo.Error(declaringType, "[" + argValue + "] is not in proper bool", e);
                }
            }
            return defaultValue;
        }
        public static string GetAppSetting(string key)
        {
            try
            {
#if NETCF
				// Configuration APIs are not suported under the Compact Framework
#elif NET_2_0
				return ConfigurationManager.AppSettings[key];
#else
                return ConfigurationManager.AppSettings[key];
#endif
            }
            catch (Exception ex)
            {
                // If an exception is thrown here then it looks like the config file does not parse correctly.
                ConsoleInfo.Error(declaringType, "Exception while reading ConfigurationSettings. Check your .config file is well formed XML.", ex);
            }
            return null;
        }
        public static string ConvertToFullPath(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            string baseDirectory = "";
            try
            {
                string applicationBaseDirectory = SystemInfo.ApplicationBaseDirectory;
                if (applicationBaseDirectory != null)
                {
                    // applicationBaseDirectory may be a URI not a local file path
                    Uri applicationBaseDirectoryUri = new Uri(applicationBaseDirectory);
                    if (applicationBaseDirectoryUri.IsFile)
                    {
                        baseDirectory = applicationBaseDirectoryUri.LocalPath;
                    }
                }
            }
            catch
            {
                // Ignore URI exceptions & SecurityExceptions from SystemInfo.ApplicationBaseDirectory
            }
            if (baseDirectory != null && baseDirectory.Length > 0)
            {
                // Note that Path.Combine will return the second path if it is rooted
                return Path.GetFullPath(Path.Combine(baseDirectory, path));
            }
            return Path.GetFullPath(path);
        }
        //public static string GetNormalImageFilePath()
        //{
        //    try
        //    {
        //        string applicationBaseDirectory = SystemInfo.ApplicationBaseDirectory;
        //        string filePath = Path.Combine(applicationBaseDirectory, "normal");
        //        if (!Directory.Exists(filePath))
        //        {
        //            Directory.CreateDirectory(filePath);
        //        }
        //        return filePath;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public static string GetPrintModelFilePath()
        //{
        //    try
        //    {
        //        string applicationBaseDirectory = SystemInfo.ApplicationBaseDirectory;
        //        string filePath = Path.Combine(applicationBaseDirectory, "model");
        //        if (!Directory.Exists(filePath))
        //        {
        //            Directory.CreateDirectory(filePath);
        //        }
        //        return filePath;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public static byte[] ReadImageFile(string filePath)
        //{
        //    try
        //    {
        //        FileStream fs = File.OpenRead(filePath); //OpenRead
        //        int filelength = 0;
        //        filelength = (int)fs.Length; //获得文件长度 
        //        Byte[] image = new Byte[filelength]; //建立一个字节数组 
        //        fs.Read(image, 0, filelength); //按字节流读取 
        //        return image;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public static bool IsFileExist(string filePath)
        {
            return File.Exists(filePath);
        }
        public static bool IsType(Type type1, Type type2)
        {
            return type1.Equals(type2);
        }
        public static bool DeleteFile(string filePath)
        {
            if (IsFileExist(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    return true;
                }
                catch (Exception ex)
                {
                    ConsoleInfo.Warn(declaringType, "deleting the " + filePath + " file ocurre exception:" + ex.Message);
                }
            }
            return false;
        }
        public static string GetFileFullName(string filePath, string fileName)
        {
            return Path.Combine(filePath, fileName);
        }
        public static string FormatDateTime(DateTime dt)
        {
            return FormatDateTime(dt, "yyyy-MM-dd HH:mm", "");
        }
        public static string FormatDateTime(DateTime dt, string formatter, string emptyLabel)
        {
            string result = null;
            if (dt == DateTime.MinValue)
            {
                result = emptyLabel;
            }
            else
            {
                try
                {
                    result = dt.ToString(formatter);
                }
                catch (FormatException fe)
                {
                    ConsoleInfo.Warn(declaringType, "formatting datetime occuring exception", fe);
                }
            }
            return result;
        }
        public static Hashtable CreateCaseInsensitiveHashtable(int capacity)
        {
            if (capacity > 0)
            {
                return new Hashtable(capacity, new MyCaseInsensitiveComparer());
            }
            return new Hashtable(new MyCaseInsensitiveComparer());
        }
        #endregion

        #region 私有静态方法
#if NETCF
		private static string NativeEntryAssemblyLocation 
		{
			get 
			{
				StringBuilder moduleName = null;

				IntPtr moduleHandle = GetModuleHandle(IntPtr.Zero);

				if (moduleHandle != IntPtr.Zero) 
				{
					moduleName = new StringBuilder(255);
					if (GetModuleFileName(moduleHandle, moduleName,	moduleName.Capacity) == 0) 
					{
						throw new NotSupportedException(NativeError.GetLastError().ToString());
					}
				} 
				else 
				{
					throw new NotSupportedException(NativeError.GetLastError().ToString());
				}

				return moduleName.ToString();
			}
		}

		[DllImport("CoreDll.dll", SetLastError=true, CharSet=CharSet.Unicode)]
		private static extern IntPtr GetModuleHandle(IntPtr ModuleName);

		[DllImport("CoreDll.dll", SetLastError=true, CharSet=CharSet.Unicode)]
		private static extern Int32 GetModuleFileName(
			IntPtr hModule,
			StringBuilder ModuleName,
			Int32 cch);

#endif
        #endregion

        #region 公共静态字段
        public static readonly Type[] EmptyTypes = new Type[0];
        #endregion

        #region 私有静态字段
        private readonly static Type declaringType = typeof(SystemInfo);
        private static string s_hostName;
        private static string s_appFriendlyName;
        private static string s_nullText;
        private static string s_notAvailableText;
        private static DateTime s_processStartTime = DateTime.Now;
        #endregion

        #region Hashtable 帮助类
        public class MyCaseInsensitiveComparer : IEqualityComparer
        {
            private CaseInsensitiveComparer caseInsensitiveComparer;
            public MyCaseInsensitiveComparer()
            {
                this.caseInsensitiveComparer = CaseInsensitiveComparer.DefaultInvariant;
            }
            public MyCaseInsensitiveComparer(CultureInfo culture)
            {
                this.caseInsensitiveComparer = new CaseInsensitiveComparer(culture);
            }
            public bool Equals(object x, object y)
            {
                if (this.caseInsensitiveComparer.Compare(x, y) == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            public int GetHashCode(object obj)
            {
                return obj.ToString().ToLower().GetHashCode();
            }
        }
        #endregion

        #region Compact Framework Helper Classes
#if NETCF_1_0
		/// <summary>
		/// Generate GUIDs on the .NET Compact Framework.
		/// </summary>
		public class PocketGuid
		{
			// guid variant types
			private enum GuidVariant
			{
				ReservedNCS = 0x00,
				Standard = 0x02,
				ReservedMicrosoft = 0x06,
				ReservedFuture = 0x07
			}

			// guid version types
			private enum GuidVersion
			{
				TimeBased = 0x01,
				Reserved = 0x02,
				NameBased = 0x03,
				Random = 0x04
			}

			// constants that are used in the class
			private class Const
			{
				// number of bytes in guid
				public const int ByteArraySize = 16;

				// multiplex variant info
				public const int VariantByte = 8;
				public const int VariantByteMask = 0x3f;
				public const int VariantByteShift = 6;

				// multiplex version info
				public const int VersionByte = 7;
				public const int VersionByteMask = 0x0f;
				public const int VersionByteShift = 4;
			}

			// imports for the crypto api functions
			private class WinApi
			{
				public const uint PROV_RSA_FULL = 1;
				public const uint CRYPT_VERIFYCONTEXT = 0xf0000000;

				[DllImport("CoreDll.dll")] 
				public static extern bool CryptAcquireContext(
					ref IntPtr phProv, string pszContainer, string pszProvider,
					uint dwProvType, uint dwFlags);

				[DllImport("CoreDll.dll")] 
				public static extern bool CryptReleaseContext( 
					IntPtr hProv, uint dwFlags);

				[DllImport("CoreDll.dll")] 
				public static extern bool CryptGenRandom(
					IntPtr hProv, int dwLen, byte[] pbBuffer);
			}

			// all static methods
			private PocketGuid()
			{
			}

			/// <summary>
			/// Return a new System.Guid object.
			/// </summary>
			public static Guid NewGuid()
			{
				IntPtr hCryptProv = IntPtr.Zero;
				Guid guid = Guid.Empty;

				try
				{
					// holds random bits for guid
					byte[] bits = new byte[Const.ByteArraySize];

					// get crypto provider handle
					if (!WinApi.CryptAcquireContext(ref hCryptProv, null, null, 
						WinApi.PROV_RSA_FULL, WinApi.CRYPT_VERIFYCONTEXT))
					{
						throw new SystemException(
							"Failed to acquire cryptography handle.");
					}

					// generate a 128 bit (16 byte) cryptographically random number
					if (!WinApi.CryptGenRandom(hCryptProv, bits.Length, bits))
					{
						throw new SystemException(
							"Failed to generate cryptography random bytes.");
					}

					// set the variant
					bits[Const.VariantByte] &= Const.VariantByteMask;
					bits[Const.VariantByte] |= 
						((int)GuidVariant.Standard << Const.VariantByteShift);

					// set the version
					bits[Const.VersionByte] &= Const.VersionByteMask;
					bits[Const.VersionByte] |= 
						((int)GuidVersion.Random << Const.VersionByteShift);

					// create the new System.Guid object
					guid = new Guid(bits);
				}
				finally
				{
					// release the crypto provider handle
					if (hCryptProv != IntPtr.Zero)
						WinApi.CryptReleaseContext(hCryptProv, 0);
				}

				return guid;
			}
		}
#endif
        #endregion Compact Framework Helper Classes
    }
}
