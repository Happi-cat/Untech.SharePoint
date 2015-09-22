using System;

namespace Untech.SharePoint.Common.Mappings.Annotation
{
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class SpListAttribute : Attribute
	{
		public string Title { get; set; }
	}
}
