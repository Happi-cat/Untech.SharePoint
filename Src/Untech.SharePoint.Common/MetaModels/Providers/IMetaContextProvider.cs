namespace Untech.SharePoint.MetaModels.Providers
{
	/// <summary>
	/// Represents interface of <see cref="MetaContext"/> provider.
	/// </summary>
	public interface IMetaContextProvider
	{
		/// <summary>
		/// Returns instance of <see cref="MetaContext"/>.
		/// </summary>
		/// <returns>New instance of <see cref="MetaContext"/></returns>
		MetaContext GetMetaContext();
	}
}