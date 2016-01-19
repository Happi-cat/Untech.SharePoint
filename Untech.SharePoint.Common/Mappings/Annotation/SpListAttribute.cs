using System;
using Untech.SharePoint.Common.CodeAnnotations;

namespace Untech.SharePoint.Common.Mappings.Annotation
{
	/// <summary>
	/// When applied to property, specifies member that should be mapped to SP list.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	[PublicAPI]
	public sealed class SpListAttribute : Attribute
	{
		/// <summary>
		/// Initializes new instance of the <see cref="SpListAttribute"/>
		/// </summary>
		public SpListAttribute() { }

		/// <summary>
		/// Initializes new instance of the <see cref="SpListAttribute"/>
		/// </summary>
		public SpListAttribute(string title)
		{
			Title = title;
		}

		/// <summary>
		/// Gets or sets SP list title.
		/// </summary>
		public string Title { get; set; }
	}
}
