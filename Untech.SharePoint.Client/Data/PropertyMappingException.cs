using System;

namespace Untech.SharePoint.Client.Data
{
	public class MemberMappingException : Exception
	{
		public MemberMappingException(string propertyName, string internalName, Exception innerException)
			: base(GetMessage(propertyName, internalName), innerException)
		{
			MemberName = propertyName;
			SpFieldInternalName = internalName;
		}

		internal MemberMappingException(IMetaDataMember member, Exception innerException)
			: base(GetMessage(member), innerException)
		{

		}

		public string MemberName { get; private set; }

		public string SpFieldInternalName { get; private set; }

		private static string GetMessage(IMetaDataMember member)
		{
			var message = GetMessage(member.Name, member.SpFieldInternalName);

			message = message + string.Format("Property or field type: {0}.", member.Type);

			return message;
		}

		private static string GetMessage(string propertyName, string internalName)
		{
			return string.Format("Unable to map property or field {0} from/to SP field {1}.", propertyName, internalName);
		}
	}
}
