using System;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Utils;

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
		/// <param name="url">The site-relative URL at which the list was placed.</param>
		public SpListAttribute([NotNull]string url)
		{
			Guard.CheckNotNull(nameof(url), url);

			Url = url;
		}

		/// <summary>
		/// Gets or sets the site-relative URL at which the list was placed.
		/// </summary>
		public string Url { get; private set; }
	}
}
