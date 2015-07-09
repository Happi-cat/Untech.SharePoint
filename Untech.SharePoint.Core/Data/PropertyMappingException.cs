using System;

namespace Untech.SharePoint.Core.Data
{
	public class PropertyMappingException : Exception
	{
		public PropertyMappingException(string propertyName, string internalName, Exception innerException) 
			: base(GetMessage(propertyName, internalName), innerException)
		{
			PropertyName = propertyName;
			InternalName = internalName;
		}

		internal PropertyMappingException(MetaProperty info, Exception innerException)
			: base(GetMessage(info), innerException)
		{
			
		}

		public string PropertyName { get; private set; }

		public string InternalName { get; private set; }

		private static string GetMessage(MetaProperty info)
		{
			var message = GetMessage(info.MemberName, info.SpFieldInternalName);

			message = message + string.Format("Property or field type: {0}.", info.MemberType.FullName);

			if (info.CustomConverterType != null)
			{
				message = message + string.Format("Custom converter type: {0}.", info.CustomConverterType.FullName);
			}

			if (info.DefaultValue != null)
			{
				message = message + string.Format("Default value type: {0}.", info.DefaultValue.GetType().FullName);
			}

			return message;
		}

		private static string GetMessage(string propertyName, string internalName)
		{
			return string.Format("Unable to map property or field {0} from/to SP field {1}.", propertyName, internalName);
		}
	}
}
