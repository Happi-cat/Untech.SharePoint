using System;

namespace Untech.SharePoint.Client.AttributedMapping
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class SpFieldAttribute : Attribute
	{
		public string InternalName { get; set; }

		public Type CustomConverterType { get; set; }
	}
}