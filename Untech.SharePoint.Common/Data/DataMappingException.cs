using System;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Data
{
	/// <summary>
	/// Represents errors that occurs during <see cref="MetaField"/> mapping.
	/// </summary>
	public class DataMappingException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DataMappingException"/> class with the specified <see cref="MetaField"/>.
		/// </summary>
		/// <param name="field">The meta field that wasn't mapped correctly.</param>
		public DataMappingException(MetaField field)
			: base(GetMessage(field))
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataMappingException"/> class with the specified <see cref="MetaField"/>
		/// and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="field">The meta field that wasn't mapped correctly.</param>
		/// <param name="innerException">The exception that is the casue of this exception.</param>
		public DataMappingException(MetaField field, Exception innerException)
			: base(GetMessage(field), innerException)
		{

		}

		private static string GetMessage(MetaField field)
		{
			var message = GetMessage(field.MemberName, field.InternalName);

			message = message + string.Format("Property or field type: {0}.", field.MemberType);

			return message;
		}

		private static string GetMessage(string propertyName, string internalName)
		{
			return string.Format("Unable to map property or field {0} from/to SP field {1}.", propertyName, internalName);
		}
	}
}