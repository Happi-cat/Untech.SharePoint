using System;

namespace Untech.SharePoint.Core.Data
{
	public class DataMapperException : Exception
	{
		public DataMapperException(string propertyName, string internalName, Exception innerException) 
			: base(GetMessage(propertyName, internalName), innerException)
		{
			PropertyName = propertyName;
			InternalName = internalName;
		}

		internal DataMapperException(PropertyMappingInfo mappingInfo, Exception innerException)
			: base(GetMessage(mappingInfo), innerException)
		{
			
		}

		public string PropertyName { get; private set; }

		public string InternalName { get; private set; }

		private static string GetMessage(PropertyMappingInfo mappingInfo)
		{
			var message = GetMessage(mappingInfo.PropertyOrFieldName, mappingInfo.SPFieldInternalName);

			message = message + string.Format("Property or field type: {0}.", mappingInfo.PropertyOrFieldType.FullName);

			if (mappingInfo.CustomConverterType != null)
			{
				message = message + string.Format("Custom converter type: {0}.", mappingInfo.CustomConverterType.FullName);
			}

			return message;
		}

		private static string GetMessage(string propertyName, string internalName)
		{
			return string.Format("Unable to map property or field {0} from/to SP field {1}.", propertyName, internalName);
		}
	}
}
