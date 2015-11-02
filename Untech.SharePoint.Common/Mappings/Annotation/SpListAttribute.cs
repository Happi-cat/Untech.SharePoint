using System;

namespace Untech.SharePoint.Common.Mappings.Annotation
{
	/// <summary>
	/// When applied to property, specifies member that should be mapped to SP list.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class SpListAttribute : Attribute
	{
		/// <summary>
		/// Gets or sets SP list title.
		/// </summary>
		public string Title { get; set; }
	}
}
