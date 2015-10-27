using System;
using System.Runtime.Serialization;

namespace XYS.Lis.Util.TypeConverters
{
	[Serializable]
	public class ConversionNotSupportedException : ApplicationException 
	{
		#region Public Instance Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="ConversionNotSupportedException" /> class.
		/// </para>
		/// </remarks>
		public ConversionNotSupportedException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">A message to include with the exception.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="ConversionNotSupportedException" /> class
		/// with the specified message.
		/// </para>
		/// </remarks>
		public ConversionNotSupportedException(String message) : base(message) 
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">A message to include with the exception.</param>
		/// <param name="innerException">A nested exception to include.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="ConversionNotSupportedException" /> class
		/// with the specified message and inner exception.
		/// </para>
		/// </remarks>
		public ConversionNotSupportedException(String message, Exception innerException) : base(message, innerException) 
		{
		}

		#endregion Public Instance Constructors

		#region Protected Instance Constructors

#if !NETCF
		/// <summary>
		/// Serialization constructor
		/// </summary>
		/// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="ConversionNotSupportedException" /> class 
		/// with serialized data.
		/// </para>
		/// </remarks>
		protected ConversionNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context) 
		{
		}
#endif

		#endregion Protected Instance Constructors

		#region  Public Static Methods

		/// <summary>
		/// Creates a new instance of the <see cref="ConversionNotSupportedException" /> class.
		/// </summary>
		/// <param name="destinationType">The conversion destination type.</param>
		/// <param name="sourceValue">The value to convert.</param>
		/// <returns>An instance of the <see cref="ConversionNotSupportedException" />.</returns>
		/// <remarks>
		/// <para>
		/// Creates a new instance of the <see cref="ConversionNotSupportedException" /> class.
		/// </para>
		/// </remarks>
		public static ConversionNotSupportedException Create(Type destinationType, object sourceValue)
		{
			return Create(destinationType, sourceValue, null);
		}

		/// <summary>
		/// Creates a new instance of the <see cref="ConversionNotSupportedException" /> class.
		/// </summary>
		/// <param name="destinationType">The conversion destination type.</param>
		/// <param name="sourceValue">The value to convert.</param>
		/// <param name="innerException">A nested exception to include.</param>
		/// <returns>An instance of the <see cref="ConversionNotSupportedException" />.</returns>
		/// <remarks>
		/// <para>
		/// Creates a new instance of the <see cref="ConversionNotSupportedException" /> class.
		/// </para>
		/// </remarks>
		public static ConversionNotSupportedException Create(Type destinationType, object sourceValue, Exception innerException)
		{
			if (sourceValue == null)
			{
				return new ConversionNotSupportedException("Cannot convert value [null] to type ["+destinationType+"]", innerException);
			}
			else
			{
				return new ConversionNotSupportedException("Cannot convert from type ["+sourceValue.GetType()+"] value ["+sourceValue+"] to type ["+destinationType+"]", innerException);
			}
		}

		#endregion  Public Static Methods
	}
}
