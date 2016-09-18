using Untech.SharePoint.Common.CodeAnnotations;

namespace Untech.SharePoint.Common.MetaModels.Providers
{
	/// <summary>
	/// Represents interface of <see cref="MetaField"/> provider.
	/// </summary>
	public interface IMetaFieldProvider
	{
		/// <summary>
		/// Returns instance of <see cref="MetaField"/>.
		/// </summary>
		/// <param name="parent">Parent <see cref="MetaContentType"/>.</param>
		/// <returns>New instance of <see cref="MetaField"/></returns>
		MetaField GetMetaField([NotNull]MetaContentType parent);
	}
}
