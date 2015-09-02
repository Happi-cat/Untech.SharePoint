using System;

namespace Untech.SharePoint.Common.AnnotationMapping
{
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class SpListAttribute : Attribute
	{
		public SpListAttribute(string listTile)
		{
			ListTitle = listTile;
		}

		public string ListTitle { get; private set; }
	}
}
