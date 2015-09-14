using System;

namespace Untech.SharePoint.Common.AnnotationMapping
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class SpFieldAttribute : Attribute
	{
		public string Name { get; set; }

		public string FieldType { get; set; }

		public Type CustomConverterType { get; set; }
	}
}