using System;

namespace Untech.SharePoint.Common.Converters
{
	/// <summary>
	/// Represents errors that occurs with <see cref="IFieldConverter"/>.
	/// </summary>
	public class FieldConverterException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FieldConverterException"/> with specified message.
		/// </summary>
		/// <param name="message">Message of the exception.</param>
		public FieldConverterException(string message)
			: base(message)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldConverterException"/> with specified message and inner exception.
		/// </summary>
		/// <param name="message">Message of the exception.</param>
		/// <param name="innerException">Inner exception</param>
		public FieldConverterException(string message, Exception innerException)
			: base(message, innerException)
		{

		}
	}
}
