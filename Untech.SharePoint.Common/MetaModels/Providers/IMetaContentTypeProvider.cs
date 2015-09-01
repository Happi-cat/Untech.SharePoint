namespace Untech.SharePoint.Common.MetaModels.Providers
{
	public interface IMetaContentTypeProvider
	{
		MetaContentType GetMetaContentType(MetaList parent);
	}
}