using System;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Data
{
	public class DataMappingException : Exception
	{
		public DataMappingException(MetaField field)
			: base(GetMessage(field))
		{
			
		}

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