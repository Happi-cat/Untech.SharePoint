using System;

namespace Untech.SharePoint.Common.Mappings.Annotation
{
	[AttributeUsage(AttributeTargets.Class)]
	public class SpContentTypeAttribute : Attribute
	{
		public string Id { get; set; }

		public string Name { get; set; }
	}
}