using System;

namespace Untech.SharePoint.Core.Data
{
	internal class DataModelPropertyInfo
	{
		public string PropertyOrFieldName { get; set; }

		public Type PropertyOrFieldType { get; set; }

		public string SpFieldInternalName { get; set; }

		public Type CustomConverterType { get; set; }

		public object DefaultValue { get; set; }
	}
}