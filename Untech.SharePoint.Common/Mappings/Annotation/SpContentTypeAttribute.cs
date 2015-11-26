using System;
using Untech.SharePoint.Common.CodeAnnotations;

namespace Untech.SharePoint.Common.Mappings.Annotation
{
	/// <summary>
	/// Class attribute that used for describing SP ContentType.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	[PublicAPI]
	public class SpContentTypeAttribute : Attribute
	{
		/// <summary>
		/// Gets or sets content type id.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets content type title.
		/// </summary>
		public string Name { get; set; }
	}
}