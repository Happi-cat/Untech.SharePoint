using System;

namespace Untech.SharePoint.Common.Mappings.Annotation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class SpFieldAttribute : Attribute
	{
		public string Name { get; set; }

		public string FieldType { get; set; }

		public Type CustomConverterType { get; set; }
	}
}