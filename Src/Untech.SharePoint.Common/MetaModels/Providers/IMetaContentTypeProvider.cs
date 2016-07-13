namespace Untech.SharePoint.Common.MetaModels.Providers
{
	/// <summary>
	/// Represents interface of <see cref="MetaContentType"/> provider.
	/// </summary>
	public interface IMetaContentTypeProvider
	{
		/// <summary>
		/// Returns instance of <see cref="MetaContentType"/>.
		/// </summary>
		/// <param name="parent">Parent <see cref="MetaList"/>.</param>
		/// <returns>New instance of <see cref="MetaContentType"/></returns>
		MetaContentType GetMetaContentType(MetaList parent);
	}
}