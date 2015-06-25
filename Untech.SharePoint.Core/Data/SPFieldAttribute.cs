using System;

namespace Untech.SharePoint.Core.Data
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class SPFieldAttribute : Attribute
	{
		public string InternalName { get; set; }

		public Type CustomConverterType { get; set; }
	}
}