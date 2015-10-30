using System;

namespace Untech.SharePoint.Common.Converters
{
	/// <summary>
	/// Represents errors that occurs due to invalid state or usage of <see cref="IFieldConverter"/> instance.
	/// </summary>
	public class InvalidFieldConverterException : FieldConverterException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidFieldConverterException"/> class with the specified <see cref="IFieldConverter"/> type.
		/// </summary>
		/// <param name="converterType">Field converter type that is the cause of this error.</param>
		public InvalidFieldConverterException(Type converterType)
			: base(GetMessage(converterType))
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidFieldConverterException"/> class with the specified <see cref="IFieldConverter"/> type
		/// and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="converterType">Field converter type that is the cause of this error.</param>
		/// <param name="innerException">The exception that is the casue of this exception.</param>
		public InvalidFieldConverterException(Type converterType, Exception innerException)
			: base(GetMessage(converterType), innerException)
		{

		}

		private static string GetMessage(Type converterType)
		{
			return string.Format("Invalid FieldRef converter '{0}'", converterType);
		}
	}
}