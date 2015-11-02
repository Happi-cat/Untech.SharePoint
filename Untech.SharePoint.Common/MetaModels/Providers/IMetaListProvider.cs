namespace Untech.SharePoint.Common.MetaModels.Providers
{
	/// <summary>
	/// Represents interface of <see cref="MetaList"/> provider.
	/// </summary>
	public interface IMetaListProvider
	{
		/// <summary>
		/// Returns instance of <see cref="MetaList"/>.
		/// </summary>
		/// <param name="parent">Parent <see cref="MetaContext"/>.</param>
		/// <returns>New instance of <see cref="MetaList"/></returns>
		MetaList GetMetaList(MetaContext parent);
	}
}