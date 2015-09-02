using System;

namespace Untech.SharePoint.Common.AnnotationMapping
{
	[AttributeUsage(AttributeTargets.Class)]
	public class SpContentTypeAttribute : Attribute
	{
		public string ContentTypeId { get; set; }
	}
}