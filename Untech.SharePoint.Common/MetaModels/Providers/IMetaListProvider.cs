namespace Untech.SharePoint.Common.MetaModels.Providers
{
	public interface IMetaListProvider
	{
		MetaList GetMetaList(MetaContext parent);
	}
}