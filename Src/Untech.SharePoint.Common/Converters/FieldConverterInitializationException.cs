using System;
using System.Runtime.Serialization;
using Untech.SharePoint.MetaModels;

namespace Untech.SharePoint.Converters
{
	/// <summary>
	/// Represents errors that occurs during <see cref="IFieldConverter"/> initialization.
	/// </summary>
	[Serializable]
	public class FieldConverterInitializationException : FieldConverterException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FieldConverterInitializationException"/> class with the specified <see cref="IFieldConverter"/> type
		/// and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="converterType">Field converter type that is the cause of this error.</param>
		/// <param name="field">Field that is the cause of this exception.</param>
		/// <param name="innerException">The exception that is the cause of this exception.</param>
		public FieldConverterInitializationException(Type converterType, MetaField field, Exception innerException)
			: base(GetMessage(converterType, field), innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldConverterInitializationException"/> class with the specified <paramref name="message"/>.
		/// </summary>
		/// <param name="message">Message that describes error.</param>
		public FieldConverterInitializationException(string message) : base(message)
		{
		}

		/// <inheritdoc/>
		protected FieldConverterInitializationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		private static string GetMessage(Type converterType, MetaField field)
		{
			var fieldMember = field?.Member;
			return $"Field converter '{converterType}' cannot be initialized for field '{fieldMember}'";
		}
	}
}