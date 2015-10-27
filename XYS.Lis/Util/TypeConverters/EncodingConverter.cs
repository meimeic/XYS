using System;
using System.Text;

namespace XYS.Lis.Util.TypeConverters
{
    internal class EncodingConverter : IConvertFrom 
	{
		#region Implementation of IConvertFrom

		public bool CanConvertFrom(Type sourceType) 
		{
			return (sourceType == typeof(string));
		}

		public object ConvertFrom(object source) 
		{
			string str = source as string;
			if (str != null) 
			{
				return Encoding.GetEncoding(str);
			}
			throw ConversionNotSupportedException.Create(typeof(Encoding), source);
		}

		#endregion
	}
}
