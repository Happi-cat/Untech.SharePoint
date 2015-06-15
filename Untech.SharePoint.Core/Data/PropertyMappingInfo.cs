using System;

namespace Untech.SharePoint.Core.Data
{
	internal class PropertyMappingInfo
	{
		public string PropertyOrFieldName { get; set; }

		public Type PropertyOrFieldType { get; set; }

		public string SPFieldInternalName { get; set; }

		public Type CustomConverterType { get; set; }
	}
}