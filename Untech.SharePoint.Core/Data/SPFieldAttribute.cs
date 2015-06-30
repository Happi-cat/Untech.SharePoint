using System;

namespace Untech.SharePoint.Core.Data
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class SpFieldAttribute : Attribute
	{
		public string InternalName { get; set; }

		public Type CustomConverterType { get; set; }
	}
}