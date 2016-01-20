using System;

namespace Untech.SharePoint.Common.Data
{
	/// <summary>
	/// Describes different options for <see cref="ISpList{T}"/> configuration.
	/// </summary>
	[Flags]
	public enum SpListOptions
	{
		/// <summary>
		/// Default options
		/// </summary>
		Default = 0,
		/// <summary>
		/// Don't filter by content type Id for SP list.
		/// </summary>
		NoFilteringByContentType = 0x01,
	}
}