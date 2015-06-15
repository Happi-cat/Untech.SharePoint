using System;

namespace Untech.SharePoint.Core.Data
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class SPFieldAttribute : Attribute
	{
		public SPFieldAttribute(string internalName)
		{
			InternalName = internalName;
		}

		public string InternalName { get; private set; }

		public Type CustomConverterType { get; set; }
	}
}