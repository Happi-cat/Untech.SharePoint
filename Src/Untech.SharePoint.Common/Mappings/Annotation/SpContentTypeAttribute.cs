using System;
using Untech.SharePoint.CodeAnnotations;

namespace Untech.SharePoint.Mappings.Annotation
{
	/// <summary>
	/// Class attribute that used for describing SP ContentType.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	[PublicAPI]
	public class SpContentTypeAttribute : Attribute
	{
		/// <summary>
		/// Initializes new instance of the <see cref="SpContentTypeAttribute"/>
		/// </summary>
		public SpContentTypeAttribute()
		{
		}

		/// <summary>
		/// Initializes new instance of the <see cref="SpContentTypeAttribute"/>
		/// </summary>
		/// <param name="id">Content type id.</param>
		public SpContentTypeAttribute(string id)
		{
			Id = id;
		}

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