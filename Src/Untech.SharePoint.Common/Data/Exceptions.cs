using System;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;
using System.Runtime.Serialization;

namespace Untech.SharePoint.Common.Data
{
	/// <summary>
	/// Represents errors that occurs during <see cref="MetaField"/> mapping.
	/// </summary>
	[Serializable]
	public class DataMappingException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DataMappingException"/> class with the specified <see cref="MetaField"/>
		/// and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">Message of the exception.</param>
		/// <param name="innerException">The exception that is the cause of this exception.</param>
		public DataMappingException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <inheritdoc />
		protected DataMappingException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}

	/// <summary>
	/// Represents errors when <see cref="MetaField"/> wasn't found or loaded.
	/// </summary>
	[Serializable]
	public class FieldNotFoundException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FieldNotFoundException"/> class with the specified <see cref="MetaField"/>.
		/// </summary>
		/// <param name="field">Meta field that wasn't found or loaded.</param>
		public FieldNotFoundException(MetaField field)
			: base(GetMessage(field))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldNotFoundException"/> class with the specified <see cref="MetaField"/>
		/// and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="field">Meta field that wasn't found or loaded.</param>
		/// <param name="innerException">The exception that is the cause of this exception.</param>
		public FieldNotFoundException(MetaField field, Exception innerException)
			: base(GetMessage(field), innerException)
		{
		}

		/// <inheritdoc />
		protected FieldNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		private static string GetMessage(MetaField field)
		{
			Guard.CheckNotNull(nameof(field), field);

			return $"Unable to find field by internal name ${field.InternalName} in list ${field.ContentType.List.Url} that located at SP site ${field.ContentType.List.Context.Url}";
		}
	}

	/// <summary>
	/// Represents errors when <see cref="MetaContentType"/> wasn't found or loaded.
	/// </summary>
	[Serializable]
	public class ContentTypeNotFoundException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ContentTypeNotFoundException"/> class with the specified <see cref="MetaContentType"/>.
		/// </summary>
		/// <param name="contentType">Meta content type that wasn't found or loaded.</param>
		public ContentTypeNotFoundException(MetaContentType contentType)
			: base(GetMessage(contentType))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ContentTypeNotFoundException"/> class with the specified <see cref="MetaContentType"/>
		/// and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="contentType">Meta content type that wasn't found or loaded.</param>
		/// <param name="innerException">The exception that is the cause of this exception.</param>
		public ContentTypeNotFoundException(MetaContentType contentType, Exception innerException)
			: base(GetMessage(contentType), innerException)
		{
		}

		/// <inheritdoc />
		protected ContentTypeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		private static string GetMessage(MetaContentType contentType)
		{
			Guard.CheckNotNull(nameof(contentType), contentType);

			return $"Unable to find or load content type ${contentType.Id} in list ${contentType.List.Url} that located at SP site ${contentType.List.Context.Url}.";
		}
	}

	/// <summary>
	/// Represents errors when <see cref="MetaList"/> wasn't found or loaded.
	/// </summary>
	[Serializable]
	public class ListNotFoundException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ListNotFoundException"/> class with the specified <see cref="MetaList"/>.
		/// </summary>
		/// <param name="list">Meta list that wasn't found or loaded.</param>
		public ListNotFoundException(MetaList list)
			: base(GetMessage(list))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ListNotFoundException"/> class with the specified <see cref="MetaList"/>
		/// and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="list">Meta list that wasn't found or loaded.</param>
		/// <param name="innerException">The exception that is the cause of this exception.</param>
		public ListNotFoundException(MetaList list, Exception innerException)
			: base(GetMessage(list), innerException)
		{
		}

		/// <inheritdoc />
		protected ListNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		private static string GetMessage(MetaList list)
		{
			Guard.CheckNotNull(nameof(list), list);

			return $"Unable to find or load list by URL ${list.Url} that located in SP site ${list.Context.Url}";
		}
	}
}